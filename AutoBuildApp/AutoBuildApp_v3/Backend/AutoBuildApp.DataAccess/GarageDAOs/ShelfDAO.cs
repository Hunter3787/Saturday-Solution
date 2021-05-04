using System;
using System.Collections.Generic;
using System.Data;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models;
using Microsoft.Data.SqlClient;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Models.Enumerations;

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
                    command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
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
                    command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
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
                    command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
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
                    command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
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
                    command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
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

        public List<Shelf> GetAllShelvesByUser(string user)
        {
            List<Shelf> shelves = new List<Shelf>();

            try
            {
                IsNotNullOrEmpty(user);
            }
            catch (ArgumentNullException)
            {

            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    try
                    {
                        string GetAllShelvesByUserQuery = "SELECT S.nameOfShelf, SPS.quantity, SPS.itemIndex , P.productType, P.modelNumber, P.manufacturerName " +
                            "FROM Shelves S " +
                            "LEFT JOIN Save_Product_Shelf SPS ON S.shelfID = SPS.shelfID " +
                            "LEFT JOIN Products P ON P.productId = SPS.productID WHERE userID = 2";
                        command.Transaction = connection.BeginTransaction();
                        command.Connection = connection;
                        command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
                        command.CommandType = CommandType.Text;
                        command.CommandText = GetAllShelvesByUserQuery;
                        
                        // Add parameter for userID from username.

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string currentShelf = (string)reader[ShelfTableCollumns.SHELF_NAME];
                                Shelf shelf = new Shelf();

                                shelf.ShelfName = currentShelf;

                                var hasMore = true;
                                while (hasMore
                                    && (string)reader[ShelfTableCollumns.SHELF_NAME] == currentShelf
                                    && (reader[SaveProductTableCollumns.SAVED_PRODUCT_INDEX] != DBNull.Value
                                    || reader[SaveProductTableCollumns.SAVED_PRODUCT_QUANTITY] != DBNull.Value)
                                    )
                                {
                                    Component component = new Component()
                                    {
                                        ModelNumber = (string) reader[ProductTableColumns.PRODUCT_COLUMN_MODEL],
                                       // ProductType = (ProductType) Enum.Parse(typeof(ProductType),(string)reader[ProductTableColumns.PRODUCT_COLUMN_TYPE]),
                                        ManufacturerName = (string) reader[ProductTableColumns.PRODUCT_COLUMN_MANUFACTURER],
                                        Quantity = (int) reader[SaveProductTableCollumns.SAVED_PRODUCT_QUANTITY]
                                    };

                                    int itemIndex = (int) reader[SaveProductTableCollumns.SAVED_PRODUCT_INDEX];
                                    shelf.ComponentList.Insert(itemIndex , component);

                                    hasMore = reader.Read();
                                }

                                shelves.Add(shelf);
                            }
                        }

                    }
                    catch (SqlException)
                    {
                        
                    }
                }
            }

            return shelves;
        }

        public Shelf GetSingleShelfByName(int id)
        {
            Shelf shelf = new Shelf();
            



            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    string insertRequest = "INSERT INTO ";
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = insertRequest;

                }
            }



            return shelf;
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
                    command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
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
