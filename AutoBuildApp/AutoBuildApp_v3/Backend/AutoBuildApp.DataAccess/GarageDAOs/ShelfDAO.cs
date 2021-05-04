using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AutoBuildApp.DataAccess
{
    public class ShelfDAO
    {
        private readonly string _connectionString;

        public ShelfDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool InsertShelf(string shelfID, string user)
        {

            try
            {
                IsNotNullOrEmpty(shelfID);
                IsNotNullOrEmpty(user);
            }
            catch (ArgumentNullException ex)
            {
                // TODO: return new CommonResponse()
            }

            bool success = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    string insertRequest = "INSERT INTO ";
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = DAOGlobals.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = insertRequest;

                    

                    var rowsAdded = command.ExecuteNonQuery();
                    if(rowsAdded == 1)
                    {
                        command.Transaction.Commit();
                        success = true;
                    }
                }
            }

            return success;
        }

        public bool DeleteShelf(string shelfID)
        {
            try
            {
                IsNotNullOrEmpty(shelfID);
            }
            catch(ArgumentNullException ex)
            {
                // TODO: return new CommonResponse();
            }

            bool success = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    string insertRequest = "INSERT INTO ";
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = DAOGlobals.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = insertRequest;



                    var rowsAdded = command.ExecuteNonQuery();
                    if (rowsAdded == 1)
                    {
                        command.Transaction.Commit();
                        success = true;
                    }
                }
            }

            return success;
        }

        public bool AddComponent(IComponent item)
        {



            bool success = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    string insertRequest = "INSERT INTO ";
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = DAOGlobals.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = insertRequest;



                    var rowsAdded = command.ExecuteNonQuery();
                    if (rowsAdded == 1)
                    {
                        command.Transaction.Commit();
                        success = true;
                    }
                }
            }

            return success;
        }

        public bool RemoveComponent(int id)
        {
            bool success = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    string insertRequest = "INSERT INTO ";
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = DAOGlobals.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = insertRequest;



                    var rowsAdded = command.ExecuteNonQuery();
                    if (rowsAdded == 1)
                    {
                        command.Transaction.Commit();
                        success = true;
                    }
                }
            }

            return success;
        }

        public bool UpdateShelf(string oldName, string newName, string user)
        {
            bool success = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    string insertRequest = "INSERT INTO ";
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = DAOGlobals.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = insertRequest;



                    var rowsAdded = command.ExecuteNonQuery();
                    if (rowsAdded == 1)
                    {
                        command.Transaction.Commit();
                        success = true;
                    }
                }
            }

            return success;
        }

        public List<IComponent> GetComponentsByShelf(int id)
        {
            List<IComponent> outputList = new List<IComponent>();



            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    string insertRequest = "INSERT INTO ";
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = DAOGlobals.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = insertRequest;

                }
            }



            return outputList;
        }

        public IComponent GetComponentByID(int id)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    string insertRequest = "INSERT INTO ";
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = DAOGlobals.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = insertRequest;


                }
            }

            return null;
        }

        #region Private Guard Methods
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
            if(toCheck is null)
            {
                throw new ArgumentNullException(nameof(toCheck));
            }
        }
        #endregion
    }
}
