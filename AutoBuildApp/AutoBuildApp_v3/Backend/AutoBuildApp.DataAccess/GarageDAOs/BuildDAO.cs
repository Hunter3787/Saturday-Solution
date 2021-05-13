using AutoBuildApp.Models;
using AutoBuildApp.Models.Builds;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
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
        /// <summary>
        /// the _approved roles define the set of roles
        /// that are need to be able to 
        /// access the BuildDOA class
        /// </summary>
        private readonly List<string> _approvedRoles = new List<string>()
        {
            RoleEnumType.BasicRole,
            RoleEnumType.DelegateAdmin,
            RoleEnumType.VendorRole,
            RoleEnumType.SystemAdmin
        };
        /// <summary>
        /// the build DAO constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public BuildDAO(string connectionString)
        {
            try
            {
                _connectionString = connectionString;
            }
            catch (ArgumentNullException)
            {
                if (connectionString == null)
                {
                    // can be changed per nick
                    var expectedParamName = "NULL OBJECT PROVIDED";
                    throw new ArgumentNullException(expectedParamName);
                }
            }

        }

        /// <summary>
        /// this method creates a PCBuild for a user account 
        /// in the database 
        /// </summary>
        /// <param name="build"></param>
        /// <param name="buildName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public CommonResponse InsertBuild(Build build, string buildName, string userName)
        {
            CommonResponse  response = new CommonResponse();
            //step one, Call Authorization check:

            // if (!AuthorizationCheck.IsAuthorized(_approvedRoles)) { return false; }

            // step two:
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    try
                    {
                        command.Transaction = conn.BeginTransaction();

                        #region SQL related
                        command.Connection = conn; // sets the connection of the command equal to the connection that has already been starte in the outer using block.

                        command.CommandTimeout = TimeSpan.FromSeconds(TimeoutLengths.TIMEOUT_SHORT).Seconds;
                        // 1) Create a Command, and set its CommandType property to StoredProcedure.
                        command.CommandType = CommandType.Text;
                        // 2) Set the CommandText to the name of the stored procedure.
                        command.CommandText = 
                            $"INSERT INTO PcBuilds(userID, buildName, createdAt, createdby, position, imagePath)" +
                            "VALUES((SELECT UI.userID FROM UserInfo UI WHERE username = @USERNAME)," +
                            "@BUILDNAME," +
                            "@DATETIME," +
                            "@USERNAME," +
                            "(SELECT MAX(position) + 1 FROM PcBuilds WHERE userID = " +
                            "(SELECT UI.userID FROM UserInfo UI WHERE username = @USERNAME))," +
                            "@IMAGEPATH);";

                        // 3) will be defining the parameters to be passed:
                        command.Parameters.AddWithValue("@BUILDNAME", buildName);
                        command.Parameters.AddWithValue("@USERNAME", userName);
                        command.Parameters.AddWithValue("@DATETIME", DateTime.Now);
                        command.Parameters.AddWithValue("@IMAGEPATH", "EMPTY");

                        #endregion SQL related
                        // checking if there are rows
                            var rowsAdded = command.ExecuteNonQuery();

                            // If the row that was added is not one then true
                            if (rowsAdded != 0)
                            {
                                response.ResponseString = ResponseStringGlobals.SUCCESSFUL_ADDITION;
                                response.IsSuccessful = true;
                                command.Transaction.Commit(); // sends the transaction to be commited at the database.
                                return response;
                            }

                    }
                    catch (SqlException e)
                    {

                        Console.WriteLine("SqlException.GetType: {0}", e.GetType());
                        Console.WriteLine("SqlException.Source: {0}", e.Source);
                        Console.WriteLine("SqlException.ErrorCode: {0}", e.ErrorCode);
                        Console.WriteLine("SqlException.Message: {0}", e.Message);
                        command.Transaction.Rollback();
                        if (!conn.State.Equals(ConnectionState.Open))
                        {
                            response.ResponseString = ResponseStringGlobals.CONNECTION_FAILED;
                            response.IsSuccessful = false;
                            return response;
                        }

                    }
                }
            }

            response.ResponseString = ResponseStringGlobals.FAILED_ADDITION;
            response.IsSuccessful = false;
            return response;
        }

        public CommonResponse DeleteBuild(string buildName, string username)
        {
            CommonResponse response = new CommonResponse();
            //step one, Call Authorization check:

            // if (!AuthorizationCheck.IsAuthorized(_approvedRoles)) { return false; }

            // step two:
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    try
                    {
                        command.Transaction = conn.BeginTransaction();

                        #region SQL related
                        command.Connection = conn; // sets the connection of the command equal to the connection that has already been starte in the outer using block.

                        command.CommandTimeout = TimeSpan.FromSeconds(TimeoutLengths.TIMEOUT_SHORT).Seconds;
                        // 1) Create a Command, and set its CommandType property to StoredProcedure.
                        command.CommandType = CommandType.Text;
                        // 2) Set the CommandText to the name of the stored procedure.
                        command.CommandText =
                            $" DELETE FROM PcBuilds WHERE PcBuilds.buildName = @BUILDNAME; ";
                        // 3) will be defining the parameters to be passed:
                        command.Parameters.AddWithValue("@BUILDNAME", buildName);

                        #endregion SQL related
                        // checking if there are rows
                        var rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"rows affected: {rowsAffected} ");
                        // If the row that was added is not one then true
                        if (rowsAffected == 0)
                        {
                            response.ResponseString = ResponseStringGlobals.FAILED_DELETION;
                            response.IsSuccessful = false;
                            return response;
                        }
                        response.ResponseString = ResponseStringGlobals.SUCCESSFUL_DELETION;
                        response.IsSuccessful = true;
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return response;

                    }
                    catch (SqlException e)
                    {

                        Console.WriteLine("SqlException.GetType: {0}", e.GetType());
                        Console.WriteLine("SqlException.Source: {0}", e.Source);
                        Console.WriteLine("SqlException.ErrorCode: {0}", e.ErrorCode);
                        Console.WriteLine("SqlException.Message: {0}", e.Message);
                        command.Transaction.Rollback();
                        if (!conn.State.Equals(ConnectionState.Open))
                        {
                            response.ResponseString = ResponseStringGlobals.CONNECTION_FAILED;
                            response.IsSuccessful = false;
                            return response;
                        }

                    }
                }
            }

            response.ResponseString = ResponseStringGlobals.FAILED_DELETION;
            response.IsSuccessful = false;
            return response;
        }




        public CommonResponse AddProductsToBuild(string buildName, string modleNumber, int quantity, string username)
        {
            CommonResponse response = new CommonResponse();
            //step one, Call Authorization check:

            // if (!AuthorizationCheck.IsAuthorized(_approvedRoles)) { return false; }

            // step two:
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    try
                    {
                        command.Transaction = conn.BeginTransaction();

                        #region SQL related
                        command.Connection = conn; // sets the connection of the command equal to the connection that has already been starte in the outer using block.

                        command.CommandTimeout = TimeSpan.FromSeconds(TimeoutLengths.TIMEOUT_SHORT).Seconds;
                        // 1) Create a Command, and set its CommandType property to StoredProcedure.
                        command.CommandType = CommandType.StoredProcedure;
                        // 2) Set the CommandText to the name of the stored procedure.
                        string SP_AddProductsToBuild = $"{nameof(AddProductsToBuild)}";
                        command.CommandText = SP_AddProductsToBuild;
                        // 3) will be defining the parameters to be passed:
                        command.Parameters.AddWithValue("@BUILDNAME", buildName);

                        command.Parameters.AddWithValue("@USERNAME",username );
                        command.Parameters.AddWithValue("@MODELNUMBER", modleNumber);
                        command.Parameters.AddWithValue("@QUANTITY", quantity);

                        #endregion SQL related
                        // checking if there are rows
                        var rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"rows affected: {rowsAffected} ");
                        // If the row that was added is not one then true
                        if (rowsAffected == 0)
                        {
                            response.ResponseString = ResponseStringGlobals.FAILED_ADDITION;
                            response.IsSuccessful = false;
                            return response;
                        }
                      response.ResponseString = ResponseStringGlobals.SUCCESSFUL_ADDITION;
                        response.IsSuccessful = true;
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return response;

                    }
                    catch (SqlException e)
                    {

                        Console.WriteLine("SqlException.GetType: {0}", e.GetType());
                        Console.WriteLine("SqlException.Source: {0}", e.Source);
                        Console.WriteLine("SqlException.ErrorCode: {0}", e.ErrorCode);
                        Console.WriteLine("SqlException.Message: {0}", e.Message);
                        command.Transaction.Rollback();
                        if (!conn.State.Equals(ConnectionState.Open))
                        {
                            response.ResponseString = ResponseStringGlobals.CONNECTION_FAILED;
                            response.IsSuccessful = false;
                            return response;
                        }

                    }
                }
            }

            response.ResponseString = ResponseStringGlobals.FAILED_ADDITION;
            response.IsSuccessful = false;
            return response;
        }




        public CommonResponse ModifyProductQuantityFromBuild(string buildName, string modleNumber, int quantity, string username)
        {
            CommonResponse response = new CommonResponse();
            //step one, Call Authorization check:

            // if (!AuthorizationCheck.IsAuthorized(_approvedRoles)) { return false; }

            // step two:
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    try
                    {
                        command.Transaction = conn.BeginTransaction();

                        #region SQL related
                        command.Connection = conn; // sets the connection of the command equal to the connection that has already been starte in the outer using block.

                        command.CommandTimeout = TimeSpan.FromSeconds(TimeoutLengths.TIMEOUT_SHORT).Seconds;
                        // 1) Create a Command, and set its CommandType property to StoredProcedure.
                        command.CommandType = CommandType.StoredProcedure;

                        string SP_ModifyProductQuantityFromBuild = $"{nameof(ModifyProductQuantityFromBuild)}";
                        // 2) Set the CommandText to the name of the stored procedure.
                        command.CommandText = SP_ModifyProductQuantityFromBuild;
                        // 3) will be defining the parameters to be passed:
                        command.Parameters.AddWithValue("@BUILDNAME", buildName);

                        command.Parameters.AddWithValue("@USERNAME", username);
                        command.Parameters.AddWithValue("@MODELNUMBER", modleNumber);
                        command.Parameters.AddWithValue("@QUANTITY", quantity);

                        #endregion SQL related
                        // checking if there are rows
                        var rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"rows affected: {rowsAffected} ");
                        // If the row that was added is not one then true
                        if (rowsAffected == 0)
                        {
                            response.ResponseString = ResponseStringGlobals.FAILED_MODIFICATION;
                            response.IsSuccessful = false;
                            return response;
                        }
                        response.ResponseString = ResponseStringGlobals.SUCCESSFUL_MODIFICATION;
                        response.IsSuccessful = true;
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return response;

                    }
                    catch (SqlException e)
                    {

                        Console.WriteLine("SqlException.GetType: {0}", e.GetType());
                        Console.WriteLine("SqlException.Source: {0}", e.Source);
                        Console.WriteLine("SqlException.ErrorCode: {0}", e.ErrorCode);
                        Console.WriteLine("SqlException.Message: {0}", e.Message);
                        command.Transaction.Rollback();
                        if (!conn.State.Equals(ConnectionState.Open))
                        {
                            response.ResponseString = ResponseStringGlobals.CONNECTION_FAILED;
                            response.IsSuccessful = false;
                            return response;
                        }

                    }
                }
            }

            response.ResponseString = ResponseStringGlobals.FAILED_ADDITION;
            response.IsSuccessful = false;
            return response;
        }


        // copy build -> MPS request cope

        // recommender adds full build -> list of model numbers for a  buildname, 



        public CommonResponse SaveBuildRecommended
            (IList<string> modelNumbers, string buildName, string username)
        {
            CommonResponse response = new CommonResponse();
            //step one, Call Authorization check:

            // if (!AuthorizationCheck.IsAuthorized(_approvedRoles)) { return false; }

            // step two:
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    try
                    {
                        command.Transaction = conn.BeginTransaction();

                        #region SQL related
                        command.Connection = conn; // sets the connection of the command equal to the connection that has already been starte in the outer using block.

                        command.CommandTimeout = TimeSpan.FromSeconds(TimeoutLengths.TIMEOUT_SHORT).Seconds;
                        // 1) Create a Command, and set its CommandType property to StoredProcedure.
                        command.CommandType = CommandType.StoredProcedure;

                        string SP_ModifyProductQuantityFromBuild = $"{nameof(SaveBuildRecommended)}";
                        // 2) Set the CommandText to the name of the stored procedure.
                        command.CommandText = SP_ModifyProductQuantityFromBuild;





                        // Creates a dynamic data table to be passed into the stored procedure
                        DataTable listOfModelNumbers = new DataTable();

                        // Create columns in the table
                        DataColumn column = new DataColumn();
                        column.ColumnName = "modelNumber";
                        column.DataType = typeof(string);
                        listOfModelNumbers.Columns.Add(column);
                        // Create all the rows
                        DataRow row;
                        foreach (var modelNumber in modelNumbers)
                        {
                            row = listOfModelNumbers.NewRow();
                            row["modelNumber"] = modelNumber;
                            listOfModelNumbers.Rows.Add(row);
                        }



                        // 3) will be defining the parameters to be passed:
                        command.Parameters.AddWithValue("@BUILDNAME", buildName);
                        command.Parameters.AddWithValue("@USERNAME", username);
                        command.Parameters.AddWithValue("@MODELS", listOfModelNumbers);
                        command.Parameters.AddWithValue("@BUILDNAME", buildName);

                        #endregion SQL related
                        // checking if there are rows
                        var rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"rows affected: {rowsAffected} ");
                        // If the row that was added is not one then true
                        if (rowsAffected == 0)
                        {
                            response.ResponseString = ResponseStringGlobals.FAILED_MODIFICATION;
                            response.IsSuccessful = false;
                            return response;
                        }
                        response.ResponseString = ResponseStringGlobals.SUCCESSFUL_MODIFICATION;
                        response.IsSuccessful = true;
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return response;

                    }
                    catch (SqlException e)
                    {

                        Console.WriteLine("SqlException.GetType: {0}", e.GetType());
                        Console.WriteLine("SqlException.Source: {0}", e.Source);
                        Console.WriteLine("SqlException.ErrorCode: {0}", e.ErrorCode);
                        Console.WriteLine("SqlException.Message: {0}", e.Message);
                        command.Transaction.Rollback();
                        if (!conn.State.Equals(ConnectionState.Open))
                        {
                            response.ResponseString = ResponseStringGlobals.CONNECTION_FAILED;
                            response.IsSuccessful = false;
                            return response;
                        }

                    }
                }
            }

            response.ResponseString = ResponseStringGlobals.FAILED_ADDITION;
            response.IsSuccessful = false;
            return response;
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
                    build.HardDrives.Add((IHardDrive)part);
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
