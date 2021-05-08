using System;
using System.Collections.Generic;
using System.Data;
using AutoBuildApp.Models;
using Microsoft.Data.SqlClient;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security;


/**
 * Data Access class that handles sql database queries
 * and requests, returning AutoBuild specific elements,
 * such as the Shelf object.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.DataAccess
{
    public class ShelfDAO
    {
        private readonly string _connectionString;
        private readonly List<string> _approvedRoles;

        public ShelfDAO(string connectionString)
        {
            _approvedRoles = new List<string>()
            {
                RoleEnumType.BasicRole,
                RoleEnumType.DelegateAdmin,
                RoleEnumType.VendorRole,
                RoleEnumType.SystemAdmin
            };
            _connectionString = connectionString;
        }

        /// <summary>
        /// Create a new shelf in the user garage.
        /// </summary>
        /// <param name="shelfName">Name of the new shelf.</param>
        /// <param name="username">User associated with the new shelf.</param>
        /// <returns></returns>
        public SystemCodeWithObject<bool> InsertShelf(string shelfName, string username)
        {
            SystemCodeWithObject<bool> output = new SystemCodeWithObject<bool>()
            {
                GenericObject = false
            };

            try
            {
                IsNotNullOrEmpty(shelfName);
                IsNotNullOrEmpty(username);
                IsAuthorized(_approvedRoles);
            }
            catch (UnauthorizedAccessException)
            {
                output.Code = AutoBuildSystemCodes.Unauthorized;
                return output;
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
                        InitializeSqlCommand(command, connection, AutoBuildSqlQueries.INSERT_SHELF);
                        command.Parameters.AddWithValue("@SHELFNAME", shelfName);
                        command.Parameters.AddWithValue("@USERNAME", username);
                        command.Parameters.AddWithValue("@CREATEDAT", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@MODIFIEDAT", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        var rowsAdded = command.ExecuteNonQuery();
                        if (rowsAdded == 1)
                        {
                            command.Transaction.Commit();
                            output.Code = AutoBuildSystemCodes.Success;
                            output.GenericObject = true;
                        }
                        else
                        {
                            output.Code = AutoBuildSystemCodes.InsertFailed;
                        }
                    }
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
        /// Delete a shelf and all items from that shelf.
        /// </summary>
        /// <param name="shelfName">The shelf to be deleted.</param>
        /// <param name="username">The username associated with the shelf.</param>
        /// <returns></returns>
        public SystemCodeWithObject<bool> DeleteShelf(string shelfName, string username)
        {
            SystemCodeWithObject<bool> output = new SystemCodeWithObject<bool>()
            {
                GenericObject = false
            };

            try
            {
                IsNotNullOrEmpty(shelfName);
                IsNotNullOrEmpty(username);
                IsAuthorized(_approvedRoles);
            }
            catch (UnauthorizedAccessException)
            {
                output.Code = AutoBuildSystemCodes.Unauthorized;
                return output;
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
                        InitializeSqlCommand(command, connection, AutoBuildSqlQueries.DELETE_SHELF);
                        command.Parameters.AddWithValue("@SHELFNAME", shelfName);
                        command.Parameters.AddWithValue("@USERNAME", username);

                        var rowsAdded = command.ExecuteNonQuery();
                        if (rowsAdded == 1)
                        {
                            command.Transaction.Commit();
                            output.Code = AutoBuildSystemCodes.Success;
                            output.GenericObject = true;
                        }
                        else
                        {
                            output.Code = AutoBuildSystemCodes.DeleteFailed;
                        }
                    }
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
        /// Add a component to a specific shelf in the user garage.
        /// </summary>
        /// <param name="modelNumber">Item to be added.</param>
        /// <param name="quantity">THe amount to add.</param>
        /// <param name="shelfName">The name of the shelf.</param>
        /// <param name="username">The user associated with the shelf.</param>
        /// <returns></returns>
        public SystemCodeWithObject<bool> AddComponent(string modelNumber, int quantity, string shelfName, string username)
        {
            SystemCodeWithObject<bool> output = new SystemCodeWithObject<bool>()
            {
                GenericObject = false
            };

            try
            {
                IsNotNullOrEmpty(modelNumber);
                IsNotNullOrEmpty(shelfName);
                IsNotNullOrEmpty(username);
                IsNotNull(quantity);
                IsAuthorized(_approvedRoles);
            }
            catch (UnauthorizedAccessException)
            {
                output.Code = AutoBuildSystemCodes.Unauthorized;
                return output;
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
                        InitializeSqlCommand(command, connection, AutoBuildSqlQueries.ADD_COMPONENT);
                        command.Parameters.AddWithValue("@MODELNUMBER", modelNumber);
                        command.Parameters.AddWithValue("@QUANTITY", quantity);
                        command.Parameters.AddWithValue("@SHELFNAME", shelfName);
                        command.Parameters.AddWithValue("@USERNAME", username);

                        var rowsAdded = command.ExecuteNonQuery();
                        if (rowsAdded == 1)
                        {
                            command.Transaction.Commit();
                            output.Code = AutoBuildSystemCodes.Success;
                            output.GenericObject = true;
                        }
                        else
                        {
                            output.Code = AutoBuildSystemCodes.InsertFailed;
                        }
                    }
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
        /// Remove a component from a users shelf in the user garage.
        /// </summary>
        /// <param name="itemindex">Current index of the to be removed element.</param>
        /// <param name="shelfName">Name of the shelf the item is to be removed from.</param>
        /// <param name="username">The user associated with the shelf to remove from.</param>
        /// <returns></returns>
        public SystemCodeWithObject<bool> RemoveComponent(int itemindex, string shelfName, string username)
        {
            SystemCodeWithObject<bool> output = new SystemCodeWithObject<bool>()
            {
                GenericObject = false
            };

            try
            {
                IsNotNullOrEmpty(shelfName);
                IsNotNullOrEmpty(username);
                IsNotNull(itemindex);
                IsAuthorized(_approvedRoles);
            }
            catch (UnauthorizedAccessException)
            {
                output.Code = AutoBuildSystemCodes.Unauthorized;
                return output;
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
                        InitializeSqlCommand(command, connection, AutoBuildSqlQueries.REMOVE_COMPONENT);
                        command.Parameters.AddWithValue("@ITEMINDEX", itemindex);
                        command.Parameters.AddWithValue("@SHELFNAME", shelfName);
                        command.Parameters.AddWithValue("@USERNAME", username);

                        var rowsRemoved = command.ExecuteNonQuery();
                        if (rowsRemoved == 1)
                        {
                            command.Transaction.Commit();
                            output.Code = AutoBuildSystemCodes.Success;
                            output.GenericObject = true;
                        }
                        else
                        {
                            output.Code = AutoBuildSystemCodes.DeleteFailed;
                        }
                    }
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
        /// Update the count of a product on the shelf. 
        /// </summary>
        /// <param name="modelNumber">Product identifier.</param>
        /// <param name="quantity">The quantity to update to.</param> 
        /// <param name="shelfName">The name of the shelf that contains the item.</param>
        /// <param name="username">The user associated with the shelf to update.</param>
        /// <returns></returns>
        public SystemCodeWithObject<bool> UpdateQuantity(
            int itemIndex,
            int quantity,
            string shelfName,
            string username)
        {
            SystemCodeWithObject<bool> output = new SystemCodeWithObject<bool>()
            {
                GenericObject = false
            };

            try
            {
                IsNotNullOrEmpty(shelfName);
                IsNotNullOrEmpty(username);
                IsNotNull(quantity);
                IsNotNull(itemIndex);
                IsAuthorized(_approvedRoles);
            }
            catch (UnauthorizedAccessException)
            {
                output.Code = AutoBuildSystemCodes.Unauthorized;
                return output;
            }
            catch (ArgumentNullException)
            {
                output.Code = AutoBuildSystemCodes.ArguementNull;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        InitializeSqlCommand(command, connection, AutoBuildSqlQueries.UPDATE_QUANTITY);
                        command.Parameters.AddWithValue("@ITEMINDEX", itemIndex);
                        command.Parameters.AddWithValue("@QUANTITY", quantity);
                        command.Parameters.AddWithValue("@SHELFNAME", shelfName);
                        command.Parameters.AddWithValue("@USERNAME", username);

                        var rowsRemoved = command.ExecuteNonQuery();
                        if (rowsRemoved == 1)
                        {
                            command.Transaction.Commit();
                            output.Code = AutoBuildSystemCodes.Success;
                            output.GenericObject = true;
                        }
                        else
                        {
                            output.Code = AutoBuildSystemCodes.UpdateFailed;
                        }
                    }
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
        /// Update shelf name in the user garage.
        /// </summary>
        /// <param name="oldShelfName">Old name to be changed.</param>
        /// <param name="newShelfName">New name to change to.</param>
        /// <param name="username">The user's name the shelf is associated with.</param>
        /// <returns>True or false with AutoBuild system code.</returns>
        public SystemCodeWithObject<bool> UpdateShelfName(string oldShelfName, string newShelfName, string username)
        {
            SystemCodeWithObject<bool> output = new SystemCodeWithObject<bool>()
            {
                GenericObject = false
            };

            try
            {
                IsNotNullOrEmpty(oldShelfName);
                IsNotNullOrEmpty(newShelfName);
                IsNotNullOrEmpty(username);
                IsAuthorized(_approvedRoles);
            }
            catch (UnauthorizedAccessException)
            {
                output.Code = AutoBuildSystemCodes.Unauthorized;
                return output;
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
                        InitializeSqlCommand(command, connection, AutoBuildSqlQueries.UPDATE_SHELF_NAME);
                        command.Parameters.AddWithValue("@NEWSHELFNAME", newShelfName);
                        command.Parameters.AddWithValue("@OLDSHELFNAME", oldShelfName);
                        command.Parameters.AddWithValue("@USERNAME", username);

                        var rowsAdded = command.ExecuteNonQuery();
                        if (rowsAdded == 1)
                        {
                            command.Transaction.Commit();
                            output.GenericObject = true;
                            output.Code = AutoBuildSystemCodes.Success;
                        }
                        else
                        {
                            output.Code = AutoBuildSystemCodes.UpdateFailed;
                        }
                    }
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
        /// Updates the order of items on the shelf in the user garage.
        /// </summary>
        /// <param name="indexOrder">List carrying the previous order of the items in their new indices</param>
        /// <param name="shelfName">The name of the shelf being modified.</param>
        /// <param name="username">The user the shelf is associated with.</param>
        /// <returns>True or false with AutoBuild system code.</returns>
        public SystemCodeWithObject<bool> UpdateShelfOrder(
            List<int> indexOrder,
            string shelfName,
            string username)
        {
            SystemCodeWithObject<bool> output = new SystemCodeWithObject<bool>()
            {
                GenericObject = false

            };

            try
            {
                IsNotNull(indexOrder);
                IsNotNullOrEmpty(username);
                IsNotNullOrEmpty(shelfName);
                IsAuthorized(_approvedRoles);
            }
            catch (UnauthorizedAccessException)
            {
                output.Code = AutoBuildSystemCodes.Unauthorized;
                return output;
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

                        DataTable indicies = new DataTable();

                        InitializeSqlCommand(command, connection, AutoBuildSqlQueries.UPDATE_SHELF_ORDER);
                        command.Parameters.AddWithValue("@USERNAME", username);
                        command.Parameters.AddWithValue("@SHELFNAME", shelfName);


                        // TODO: What is the return from the query.
                        var rowsAdded = command.ExecuteNonQuery();
                        if (rowsAdded == 1)
                        {
                            command.Transaction.Commit();
                            output.GenericObject = true;
                            output.Code = AutoBuildSystemCodes.Success;
                        }
                        else
                        {
                            output.Code = AutoBuildSystemCodes.UpdateFailed;
                        }
                    }
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
        /// Get all Shelves from the Database using the collections with code
        /// class.
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns></returns>
        public SystemCodeWithObject<List<Shelf>> GetShelvesByUser(string username)
        {
            SystemCodeWithObject<List<Shelf>> output = new SystemCodeWithObject<List<Shelf>>();
            output.GenericObject = new List<Shelf>();
            var shelves = output.GenericObject;

            try
            {
                IsNotNullOrEmpty(username);
                IsAuthorized(_approvedRoles);
            }
            catch (UnauthorizedAccessException)
            {
                output.Code = AutoBuildSystemCodes.Unauthorized;
                return output;
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

                        InitializeSqlCommand(command, connection, AutoBuildSqlQueries.GET_ALL_SHELVES_BY_USERNAME);
                        command.Parameters.AddWithValue("@USERNAME", username);

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
                                    && reader[SaveProductTableCollumns.SAVED_PRODUCT_QUANTITY] != DBNull.Value
                                    && reader[ProductTableColumns.PRODUCT_COLUMN_TYPE] != DBNull.Value)
                                    )
                                {
                                    Component component = new Component();
                                    PopulateComponent(component, reader);
                                    shelf.ComponentList.Add(component);

                                    hasMore = reader.Read();
                                }
                                Console.WriteLine(shelf);
                                shelves.Add(shelf);
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

        /// <summary>
        /// Get a single shelf by user name and shelf name.
        /// </summary>
        /// <param name="shelfName"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public SystemCodeWithObject<Shelf> GetShelfByName(string shelfName, string username)
        {
            SystemCodeWithObject<Shelf> output = new SystemCodeWithObject<Shelf>();
            output.GenericObject = new Shelf();
            var shelf = output.GenericObject;

            try
            {
                IsNotNullOrEmpty(shelfName);
                IsNotNullOrEmpty(username);
                IsAuthorized(_approvedRoles);
            }
            catch (UnauthorizedAccessException)
            {
                output.Code = AutoBuildSystemCodes.Unauthorized;
                return output;
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
                        InitializeSqlCommand(command, connection, AutoBuildSqlQueries.GET_SHELF_BY_NAME_AND_USER);
                        command.Parameters.AddWithValue("@USERNAME", username);
                        command.Parameters.AddWithValue("@SHELFNAME", shelfName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read() && reader[ProductTableColumns.PRODUCT_COLUMN_TYPE] != DBNull.Value)
                            {
                                shelf.ShelfName = (string)reader[ShelfTableCollumns.SHELF_NAME];
                                Component component = new Component();
                                PopulateComponent(component, reader);
                                shelf.ComponentList.Add(component);
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

            if(output.GenericObject.ShelfName == null)
            {
                output.Code = AutoBuildSystemCodes.NoEntryFound;
                return output;
            }

            output.Code = AutoBuildSystemCodes.Success;
            return output;
        }

        // Out of scope
        //public SystemCodeWithObject<Component> GetComponentByModel(string model)
        //{
        //    SystemCodeWithObject<Component> output = new SystemCodeWithObject<Component>();
        //    ProductFactory productFactory = new ProductFactory();
        //    var component = output.GenericObject;

        //    try
        //    {
        //        IsNotNullOrEmpty(model);
        //        IsAuthorized(_approvedRoles);
        //    }
        //    catch (UnauthorizedAccessException)
        //    {
        //        output.Code = AutoBuildSystemCodes.Unauthorized;
        //        return output;
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        output.Code = AutoBuildSystemCodes.ArguementNull;
        //    }

        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand())
        //        {
        //            try
        //            {
        //                command.Transaction = connection.BeginTransaction();
        //                command.Connection = connection;
        //                command.CommandTimeout = TimeoutLengths.TIMEOUT_SHORT;
        //                command.CommandType = CommandType.Text;
        //                //command.CommandText = _getProductByModel;

        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    var productType = (ProductType)Enum.Parse(typeof(ProductType), (string)reader[ProductTableColumns.PRODUCT_COLUMN_TYPE]);
        //                    component = productFactory.CreateComponent(productType);
        //                    component.ProductType = productType;

        //                    PopulateComponent(component, reader);

        //                    // TODO: Need to get product specs.

        //                }
        //            }
        //            catch (System.ComponentModel.InvalidEnumArgumentException)
        //            {
        //                output.Code = AutoBuildSystemCodes.ProductCreationFailed;
        //                return output;
        //            }
        //            catch (ArgumentException)
        //            {
        //                output.Code = AutoBuildSystemCodes.FailedParse;
        //                return output;
        //            }
        //            catch (SqlException ex)
        //            {
        //                output.Code = SqlExceptionHandler.GetCode(ex.Number);
        //                return output;
        //            }
        //        }
        //    }

        //    output.Code = AutoBuildSystemCodes.Success;
        //    return output;
        //}

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

        // Out of scope
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="reader"></param>
        //private void ComponentSwitch(Component component, SqlDataReader reader)
        //{
        //    // TODO
        //    switch (component.ProductType)
        //    {
        //        case ProductType.CPU: 
        //            break;
        //        case ProductType.Case:
        //            break;
        //        case ProductType.HDD:
        //            break;
        //        case ProductType.SSD:
        //            break;
        //        case ProductType.RAM:
        //            break;
        //        case ProductType.PSU:
        //            break;
        //        case ProductType.GPU:
        //            break;
        //        case ProductType.Fan:
        //            break;
        //        case ProductType.Cooler:
        //            break;
        //        default:
        //            break;
        //    }

        //}

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

        /// <summary>
        /// Throws unauthorized access exception if
        /// the user should not be able to perform the operation.
        /// </summary>
        public void IsAuthorized(List<string> roles)
        {
            if (!AuthorizationCheck.IsAuthorized(roles))
            {
                throw new UnauthorizedAccessException();
            }
        }
        #endregion
        #endregion
    }
}
