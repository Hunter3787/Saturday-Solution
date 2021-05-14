using AutoBuildApp.Models.Builds;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Products;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

/**
* This Data Access Object will handle the database
* operations with regard to Builds, such as "add", "remove", "delete".
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.DataAccess
{
    public class BuildDAO
    {
        private readonly string _connectionString;

        public BuildDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool InsertBuild(Build build, string buildName, string user)
        {
            
            return false;
        }

        public bool DeleteBuild(string buildName, string username)
        {
            return false;
        }

        public bool ModifyBuild(Build newBuild, Build oldBuild, string oldName, string username)
        {
            return false;
        }

        /// <summary>
        /// Copy build from one table to another to effectively
        /// create a non-user related build.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="buildName"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public SystemCodeWithObject<bool> PublishBuild(string title, string buildName, string username)
        {
            SystemCodeWithObject<bool> output = new SystemCodeWithObject<bool>()
            {
                GenericObject = false
            };
            try
            {
                IsNotNullOrEmpty(buildName);
                IsNotNullOrEmpty(username);
            }
            catch (ArgumentNullException)
            {
                output.Code = AutoBuildSystemCodes.ArguementNull;
                return output;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        InitializeSqlCommand(command, connection, AutoBuildSqlQueries.GET_BUILD_BY_NAME_AND_USER);
                        command.Parameters.AddWithValue("@TITLE", title);
                        command.Parameters.AddWithValue("@BUILDNAME", buildName);
                        command.Parameters.AddWithValue("@USERNAME", username);

                        var rowsAdded = command.ExecuteNonQuery();
                        if (rowsAdded > 0)
                        {
                            command.Transaction.Commit();
                            output.GenericObject = true;
                            output.Code = AutoBuildSystemCodes.Success;
                        }
                    }
                }
                catch (ArgumentException)
                {
                    output.Code = AutoBuildSystemCodes.FailedParse;
                    return output;
                }
                catch (SqlException ex)
                {
                    output.Code = SqlExceptionHandler.GetCode(ex.Number);
                    return output;
                }
            }

            
            return output;
        }

        /// <summary>
        /// Get a build 
        /// </summary>
        /// <param name="buildName"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public SystemCodeWithObject<Build> GetBuild(string buildName, string username)
        {
            SystemCodeWithObject<Build> output = new SystemCodeWithObject<Build>();
            output.GenericObject = new Build();

            try
            {
                IsNotNullOrEmpty(buildName);
                IsNotNullOrEmpty(username);
            }
            catch (ArgumentNullException)
            {
                output.Code = AutoBuildSystemCodes.ArguementNull;
                return output;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        InitializeSqlCommand(command, connection, AutoBuildSqlQueries.GET_BUILD_BY_NAME_AND_USER);
                        command.Parameters.AddWithValue("@BUILDNAME",buildName);
                        command.Parameters.AddWithValue("@USERNAME",username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read() && reader[ProductTableColumns.PRODUCT_COLUMN_TYPE] != DBNull.Value)
                            {
                                var temp = new Component();

                                PopulateComponent(temp, reader);
                                PopulateBuild(output.GenericObject, temp);
                            }
                        }
                    }
                }
                catch (ArgumentException)
                {
                    output.Code = AutoBuildSystemCodes.FailedParse;
                    return output;
                }
                catch (SqlException ex)
                {
                    output.Code = SqlExceptionHandler.GetCode(ex.Number);
                    return output;
                }
            }

            output.Code = AutoBuildSystemCodes.Success;
            return output;
        }

        public List<Build> GetListOfBuilds(string username)
        {
            List<Build> outputList = new List<Build>();

            return outputList;
        }

        #region Private Methods
        /// <summary>
        /// Populates component using SqlDataReader.
        /// </summary>
        /// <param name="toPopulate"></param>
        /// <param name="reader"></param>
        private void PopulateComponent(Component toPopulate, SqlDataReader reader)
        {
            toPopulate.ProductType = (ProductType)Enum.Parse(typeof(ProductType), (string)reader[ProductTableColumns.PRODUCT_COLUMN_TYPE]);
            toPopulate.ModelNumber = (string)reader[ProductTableColumns.PRODUCT_COLUMN_MODEL];
            toPopulate.ManufacturerName = (string)reader[ProductTableColumns.PRODUCT_COLUMN_MANUFACTURER];
            toPopulate.Quantity = (int)reader[SaveProductTableCollumns.SAVED_PRODUCT_QUANTITY];
        }

        /// <summary>
        /// Helper method to populate a build with a
        /// component if it does not already have that component slot filled.
        /// </summary>
        /// <param name="build"></param>
        /// <param name="part"></param>
        private void PopulateBuild(Build build, Component part)
        {
            switch (part.ProductType)
            {
                case ProductType.Case:
                    if(build.Case.ModelNumber == null)
                    {
                        build.Case = (ComputerCase)part;
                    }
                    break;

                case ProductType.Motherboard:
                    if(build.Mobo.ModelNumber == null)
                    {
                        build.Mobo = (Motherboard)part;
                    }
                    break;

                case ProductType.PSU:
                    if (build.Psu.ModelNumber == null)
                    {
                        build.Psu = (PowerSupplyUnit)part;
                    }
                    break;

                case ProductType.RAM:
                    if(build.Ram.ModelNumber == null)
                    {
                        build.Ram = (RAM)part;
                    }
                    break;

                case ProductType.CPU:
                    if(build.Cpu.ModelNumber == null)
                    {
                        build.Cpu = (CentralProcUnit)part;
                    }
                    break;

                case ProductType.GPU:
                    if(build.Gpu.ModelNumber == null)
                    {
                        build.Gpu = (GraphicsProcUnit)part;
                    }
                    break;

                case ProductType.SSD:
                case ProductType.HDD:
                    build.HardDrives.Add((HardDrive)part);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Helper method to initialize the
        /// connection and begin the transaction
        /// with the passed SqlQuery.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <param name="queryString"></param>
        private void InitializeSqlCommand(SqlCommand command, SqlConnection connection, string queryString)
        {
            command.Transaction = connection.BeginTransaction();
            command.Connection = connection;
            command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
            command.CommandType = CommandType.Text;
            command.CommandText = queryString;

        }

        #region Guard Methods
        /// <summary>
        /// Throws exception if string is null or empty.
        /// </summary>
        /// <param name="toCheck"></param>
        private void IsNotNullOrEmpty(string toCheck)
        {
            if (string.IsNullOrEmpty(toCheck))
            {
                throw new ArgumentNullException(nameof(toCheck));
            }
        }

        /// <summary>
        /// Throws exception if object is null.
        /// </summary>
        /// <param name="toCheck"></param>
        private void IsNotNull(object toCheck)
        {
            if (toCheck is null)
            {
                throw new ArgumentNullException(nameof(toCheck));
            }
        }
        #endregion
        #endregion
    }
}
