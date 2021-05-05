using System;
using System.Collections.Generic;
using System.Data;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models;
using Microsoft.Data.SqlClient;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.DataTransferObjects;

namespace AutoBuildApp.DataAccess
{
    public class ShelfDAO
    {
        private readonly string _connectionString;
        private readonly string _getAllShelvesByUserQuery =
            "SELECT S.nameOfShelf, SPS.quantity, SPS.itemIndex , P.productType, P.modelNumber, P.manufacturerName " +
            "FROM Shelves S " +
            "LEFT JOIN Save_Product_Shelf SPS ON S.shelfID = SPS.shelfID " +
            "LEFT JOIN Products P ON P.productId = SPS.productID WHERE userID = 2";
        private readonly string _getShelfByNameAndUser =
            "SELECT S.nameOfShelf, SPS.quantity, SPS.itemIndex , P.productType, P.modelNumber, P.manufacturerName " +
            "FROM Shelves S " +
            "LEFT JOIN Save_Product_Shelf SPS ON S.shelfID = SPS.shelfID " +
            "LEFT JOIN Products P ON P.productId = SPS.productID " +
            "WHERE userID = 2 AND nameOfShelf = 'tacobell'";
        private readonly string _getProductByModel =
            "SELECT productType, manufacturerName, modelNumber, productSpecs, productSpecsValue " +
            "FROM Products P " +
            "INNER JOIN Products_Specs PS ON p.productID = ps.productID " +
            "WHERE p.modelNumber = '100-100000071BOX';";
        private readonly string _updateShelf = "";
        private readonly string _removeProduct = "";
        private readonly string _addProduct = "";
        private readonly string _deleteShelf =
            "DELETE FROM Shelves " +
            "WHERE nameOfShelf = 'TestShelf' " +
            "AND userID = (SELECT UA.userID " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = 'Zeina');";
        private readonly string _insertShelf =
            "INSERT INTO Shelves (userID, nameOfShelf) " +
            "VALUES( " +
            "(SELECT UA.userID " +
            "FROM UserAccounts UA " +
            "INNER JOIN MappingHash MH ON UA.userID = MH.userID " +
            "INNER JOIN UserCredentials UC ON MH.userHashID = UC.userHashID " +
            "WHERE username = 'Zeina') , 'TestShelf');";


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
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = _insertShelf;

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

        public bool DeleteShelf(string shelfName, string shelfID)
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
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = _deleteShelf;



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

        public bool AddComponent(string modelNumber, string shelfName, string username)
        {



            bool success = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = _addProduct;



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

        public bool RemoveComponent(int index, string shelfName, string username)
        {
            bool success = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = _removeProduct;



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

        public SystemCodeWithCollection<bool> UpdateShelf(string oldName, string newName, string user)
        {
            SystemCodeWithCollection<bool> output = new SystemCodeWithCollection<bool>()
            {
                GenericCollection = false
            };

            try
            {
                IsNotNullOrEmpty(oldName);
                IsNotNullOrEmpty(newName);
                IsNotNullOrEmpty(user);
            }
            catch (ArgumentNullException)
            {
                output.Code = AutoBuildSystemCodes.ArguementNull;
                return output;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = _updateShelf;



                    var rowsAdded = command.ExecuteNonQuery();
                    if (rowsAdded == 1)
                    {
                        command.Transaction.Commit();
                        output.GenericCollection = true;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Get all Shelves from the Database using the collections with code
        /// class.
        /// </summary>
        /// <param name="user">User name</param>
        /// <returns></returns>
        public SystemCodeWithCollection<List<Shelf>> GetAllShelvesByUser(string user)
        {
            SystemCodeWithCollection<List<Shelf>> output = new SystemCodeWithCollection<List<Shelf>>();
            output.GenericCollection = new List<Shelf>();
            var shelves = output.GenericCollection;

            try
            {
                IsNotNullOrEmpty(user);
            }
            catch (ArgumentNullException)
            {
                output.Code = AutoBuildSystemCodes.ArguementNull;
                return output;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    try
                    {
                        command.Transaction = connection.BeginTransaction();
                        command.Connection = connection;
                        command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
                        command.CommandType = CommandType.Text;
                        command.CommandText = _getAllShelvesByUserQuery;
                        
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
                                    Component component = new Component();
                                    PopulateComponent(component, reader);

                                    int itemIndex = (int) reader[SaveProductTableCollumns.SAVED_PRODUCT_INDEX];
                                    shelf.ComponentList.Insert(itemIndex , component);

                                    hasMore = reader.Read();
                                }

                                shelves.Add(shelf);
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
            }

            output.Code = AutoBuildSystemCodes.Success;
            return output;
        }

        /// <summary>
        /// Get single shelf
        /// </summary>
        /// <param name="shelfName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public SystemCodeWithCollection<Shelf> GetShelfByName(string shelfName, string user)
        {
            SystemCodeWithCollection<Shelf> output = new SystemCodeWithCollection<Shelf>();
            output.GenericCollection = new Shelf();
            var shelf = output.GenericCollection;

            try
            {
                IsNotNullOrEmpty(shelfName);
                IsNotNullOrEmpty(user);
            }
            catch (ArgumentNullException)
            {
                output.Code = AutoBuildSystemCodes.ArguementNull;
                return output;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    try
                    {
                        command.Transaction = connection.BeginTransaction();
                        command.Connection = connection;
                        command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
                        command.CommandType = CommandType.Text;
                        command.CommandText = _getShelfByNameAndUser;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                shelf.ShelfName = (string)reader[ShelfTableCollumns.SHELF_NAME];

                                Component component = new Component();
                                PopulateComponent(component, reader);

                                int itemIndex = (int)reader[SaveProductTableCollumns.SAVED_PRODUCT_INDEX];
                                shelf.ComponentList.Insert(itemIndex, component);
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
            }

            output.Code = AutoBuildSystemCodes.Success;
            return output;
        }

        public SystemCodeWithCollection<Component> GetComponentByModel(string model)
        {
            SystemCodeWithCollection<Component> output = new SystemCodeWithCollection<Component>();
            ProductFactory productFactory = new ProductFactory();
            var component = output.GenericCollection;
            
            try
            {
                IsNotNullOrEmpty(model);
            }
            catch (ArgumentNullException)
            {
                output.Code = AutoBuildSystemCodes.ArguementNull;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    try
                    {
                        command.Transaction = connection.BeginTransaction();
                        command.Connection = connection;
                        command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
                        command.CommandType = CommandType.Text;
                        command.CommandText = _getProductByModel;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            var productType = (ProductType)Enum.Parse(typeof(ProductType), (string)reader[ProductTableColumns.PRODUCT_COLUMN_TYPE]);
                            component = productFactory.CreateComponent(productType);
                            component.ProductType = productType;

                            PopulateComponent(component, reader);

                            // TODO: Need to get product specs.
                         
                        }
                    }
                    catch (System.ComponentModel.InvalidEnumArgumentException)
                    {
                        output.Code = AutoBuildSystemCodes.ProductCreationFailed;
                        return output;
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
            }

            output.Code = AutoBuildSystemCodes.Success;
            return output;
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
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        private void ComponentSwitch(Component component, SqlDataReader reader)
        {
            // TODO
            switch (component.ProductType)
            {
                case ProductType.CPU:
                    
                    break;
                case ProductType.Case:
                    break;
                case ProductType.HDD:
                    break;
                case ProductType.SSD:
                    break;
                case ProductType.RAM:
                    break;
                case ProductType.PSU:
                    break;
                case ProductType.GPU:
                    break;
                case ProductType.Fan:
                    break;
                case ProductType.Cooler:
                    break;
                default:
                    break;
            }

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
            if(toCheck is null)
            {
                throw new ArgumentNullException(nameof(toCheck));
            }
        }
        #endregion
        #endregion
    }
}
