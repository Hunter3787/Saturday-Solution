﻿using AutoBuildApp.Models.VendorLinking;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security;

namespace AutoBuildApp.DataAccess
{
    /// <summary>
    /// This class is responsible for communicating with the database
    /// </summary>
    public class VendorLinkingDAO
    {
        private List<string> _allowedRoles;
        private string _connectionString;

        /// <summary>
        /// Establishes the connection with the connection string that is passed through and sets the allowed roles
        /// </summary>
        /// <param name="connectionString">sql database string to be able to connect to database.</param>
        public VendorLinkingDAO(string connectionString)
        {
            _connectionString = connectionString;
            _allowedRoles = new List<string>()
            {
                RoleEnumType.SystemAdmin,
                RoleEnumType.VendorRole
            };
        }

        /// <summary>
        /// This method retrieves data from the DB by getting all vendors products and saving it in a cache
        /// </summary>
        /// <returns>returns a system code with a concurrent dictionary of string and concurrent dictionary of string and byte</returns>
        public virtual SystemCodeWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> PopulateVendorsProducts()
        {
            // Initialize the system code response object with the concurrent dictionary as the generic collection
            SystemCodeWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> response = new SystemCodeWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>>();

            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                response.Code = AutoBuildSystemCodes.Unauthorized;
                return response;
            }

            // Get the current principal on the thread
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Initialize concurrent dictionary
            ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> vendorsProducts = new ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();

                        string sql = "select * from Vendor_Product_Junction vpj inner join vendorclub v on vpj.vendorID = v.vendorID inner join products p on p.productID = vpj.productID";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);

                        using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string VendorName = (string)reader["vendorName"];
                                string ModelNumber = (string)reader["modelNumber"];

                                // This will loop through all vendor's products. So, a vendor will show up multiple times. If it doesn't exist, we add it to the dictionary
                                if (!vendorsProducts.ContainsKey(VendorName))
                                {
                                    // Add entry to dictionary and initialize with a dictionary
                                    vendorsProducts.TryAdd(VendorName, new ConcurrentDictionary<string, byte>());
                                }

                                // Then, add the model number to the dictionary
                                if (!vendorsProducts[VendorName].ContainsKey(ModelNumber))
                                {
                                    vendorsProducts[VendorName].TryAdd(ModelNumber, 0);
                                }
                            }
                        }

                        transaction.Commit();

                        response.Code = AutoBuildSystemCodes.Success;
                        response.GenericObject = vendorsProducts;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        // Passes the exception number to a handler which returns an AutoBuildSystemCode
                        response.Code = SqlExceptionHandler.GetCode(ex.Number);
                        response.GenericObject = null;
                    }
                }
            }

            return response;
        }

        /// <summary>
        /// This method retrieves data from the DB by getting all model numbers
        /// </summary>
        /// <returns>returns a system code with a list of strings</returns>
        public virtual SystemCodeWithObject<List<string>> GetAllModelNumbers()
        {
            // Initialize the system code response object
            SystemCodeWithObject<List<string>> response = new SystemCodeWithObject<List<string>>();

            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                response.Code = AutoBuildSystemCodes.Unauthorized;
                response.GenericObject = null;

                return response;
            }

            // Get the current principal on the thread
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string vendor = _threadPrinciple.Identity.Name;
            List<string> allModelNumbers = new List<string>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        string sql = "select p.modelNumber from products p where p.modelNumber not in " +
                            "(select modelNumber from products p inner join vendor_product_junction vpj on p.productId = vpj.productID " +
                            "where vendorID = (select vendorID from vendorclub where vendorName = @VENDORNAME))";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = vendor;

                        using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string modelNumber = (string)reader["modelNumber"];
                                allModelNumbers.Add(modelNumber);
                            }
                        }

                        transaction.Commit();

                        response.Code = AutoBuildSystemCodes.Success;
                        response.GenericObject = allModelNumbers;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        // Passes the exception number to a handler which returns an AutoBuildSystemCode
                        response.Code = SqlExceptionHandler.GetCode(ex.Number);
                        response.GenericObject = null;
                    }
                }
            }
            return response;
        }

        /// <summary>
        /// This method will retrieve all records for a vendor from the DB based on the filters that were specified
        /// </summary>
        /// <param name="product">takes in the GetProductByFilterDTO which contains the filters and the order</param>
        /// <returns>returns a system code with a list of AddProductDTOs.</returns>
        public virtual SystemCodeWithObject<List<AddProductDTO>> GetVendorProductsByFilter(ProductByFilterDTO product)
        {
            // Initialize the system code response object
            SystemCodeWithObject<List<AddProductDTO>> response = new SystemCodeWithObject<List<AddProductDTO>>();

            // Product null check
            if (product == null)
            {
                response.Code = AutoBuildSystemCodes.NullValue;
                response.GenericObject = null;

                return response;
            }

            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                response.Code = AutoBuildSystemCodes.Unauthorized;
                response.GenericObject = null;

                return response;
            }

            // Get the current principal on the thread
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string vendor = _threadPrinciple.Identity.Name;

            List<AddProductDTO> allProductsByVendor = new List<AddProductDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();

                        string sp_GetAllProductsByVendor = "GetAllProductsByVendorAccount";
                        adapter.InsertCommand = new SqlCommand(sp_GetAllProductsByVendor, connection, transaction);
                        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                        adapter.InsertCommand.CommandText = sp_GetAllProductsByVendor;

                        // Creates a dynamic data table to be passed into the stored procedure
                        DataTable pair = new DataTable();

                        // Create columns in the table
                        DataColumn column = new DataColumn();
                        column.ColumnName = "productType";
                        column.DataType = typeof(string);
                        pair.Columns.Add(column);

                        column = new DataColumn();

                        column.ColumnName = "FilterOn";
                        column.DataType = typeof(bool);
                        pair.Columns.Add(column);

                        // Create all the rows
                        DataRow row;
                        foreach (var filter in product.FilteredListOfProducts)
                        {
                            row = pair.NewRow();
                            row["productType"] = filter.Key;
                            row["FilterOn"] = filter.Value;
                            pair.Rows.Add(row);
                        }

                        // Add the necessary parameters for the stored procedure
                        var param = new SqlParameter[3];
                        param[0] = adapter.InsertCommand.Parameters.AddWithValue("@Filterlist", pair);
                        param[1] = adapter.InsertCommand.Parameters.AddWithValue("@USERNAME", vendor);
                        param[2] = adapter.InsertCommand.Parameters.AddWithValue("@ORDER", product.PriceOrder);

                        // Execute the stored procedure and read each row and create an AddProductDTO
                        using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AddProductDTO productInfo = new AddProductDTO();
                                productInfo.Name = (string)reader["listingName"];
                                productInfo.ImageUrl = (string)reader["vendorImageURL"];
                                productInfo.Availability = (bool)reader["productStatus"];
                                productInfo.Company = (string)reader["vendorName"];
                                productInfo.Url = (string)reader["VendorLinkURL"];
                                productInfo.ModelNumber = (string)reader["modelNumber"];
                                productInfo.Price = Decimal.ToDouble(reader["productPrice"] == DBNull.Value ? 0 : (decimal)reader["productPrice"]);
                                productInfo.ProductType = (string)reader["ProductType"];

                                allProductsByVendor.Add(productInfo);
                            }
                        }

                        transaction.Commit();

                        response.Code = AutoBuildSystemCodes.Success;
                        response.GenericObject = allProductsByVendor;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        // Passes the exception number to a handler which returns an AutoBuildSystemCode
                        response.Code = SqlExceptionHandler.GetCode(ex.Number);
                        response.GenericObject = null;

                    }
                }
            }

            return response;
        }

        /// <summary>
        /// This method will add a product to the vendor's list of products
        /// </summary>
        /// <param name="product">takes in the AddProductDTO which contains the information of the product that needs to be added</param>
        /// <returns>returns a system code with a list of AddProductDTOs.</returns>
        public virtual SystemCodeWithObject<int> AddProductToVendorListOfProducts(AddProductDTO product)
        {
            // Initialize the system code response object
            SystemCodeWithObject<int> response = new SystemCodeWithObject<int>();

            // Product null check
            if (product == null)
            {
                response.Code = AutoBuildSystemCodes.NullValue;
                response.GenericObject = -1;

                return response;
            }

            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                response.Code = AutoBuildSystemCodes.Unauthorized;
                response.GenericObject = -1;

                return response;
            }

            // Get the current principal on the thread
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string vendor = _threadPrinciple.Identity.Name;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "insert into vendor_product_junction(vendorID, productID, listingName, vendorImageUrl, vendorLinkURL, productStatus, productPrice)Values" +
                            "((select vendorID from vendorClub where vendorName = @VENDORNAME), (select productID from products where modelNumber = @MODELNUMBER), @LISTINGNAME, @VENDORIMAGEURL, @VENDORLINKURL, @PRODUCTSTATUS, @PRODUCTPRICE)";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = vendor;
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
                        adapter.InsertCommand.Parameters.Add("@LISTINGNAME", SqlDbType.VarChar).Value = product.Name;
                        adapter.InsertCommand.Parameters.Add("@VENDORIMAGEURL", SqlDbType.VarChar).Value = product.ImageUrl;
                        adapter.InsertCommand.Parameters.Add("@VENDORLINKURL", SqlDbType.VarChar).Value = product.Url;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTSTATUS", SqlDbType.VarChar).Value = product.Availability;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTPRICE", SqlDbType.VarChar).Value = product.Price;

                        int rowsReturned = adapter.InsertCommand.ExecuteNonQuery();

                        transaction.Commit();

                        response.Code = AutoBuildSystemCodes.Success;
                        response.GenericObject = rowsReturned;

                        return response;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        // Passes the exception number to a handler which returns an AutoBuildSystemCode
                        response.Code = SqlExceptionHandler.GetCode(ex.Number);
                        response.GenericObject = -1;

                        return response;
                    }
                }
            }
        }

        /// <summary>
        /// This method will edit a product from the vendor's list of products
        /// </summary>
        /// <param name="product">takes in the AddProductDTO which contains the information of the product that needs to be added</param>
        /// <returns>returns a system code with a list of AddProductDTOs.</returns>
        public virtual SystemCodeWithObject<int> EditProductInVendorListOfProducts(AddProductDTO product)
        {
            // Initialize the system code response object
            SystemCodeWithObject<int> response = new SystemCodeWithObject<int>();

            // Product null check
            if (product == null)
            {
                response.Code = AutoBuildSystemCodes.NullValue;
                response.GenericObject = -1;

                return response;
            }

            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                response.Code = AutoBuildSystemCodes.Unauthorized;
                response.GenericObject = -1;

                return response;
            }

            // Get the current principal on the thread
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string vendor = _threadPrinciple.Identity.Name;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "update vendor_product_junction set listingName = @LISTINGNAME, vendorImageUrl = @VENDORIMAGEURL, vendorLinkUrl = @VENDORLINKURL, productStatus = @PRODUCTSTATUS, productPrice = @PRODUCTPRICE where productID =" +
                        "(select productID from products where modelNumber = @MODELNUMBER) and vendorID = (select vendorID from vendorclub where vendorName = @VENDORNAME)";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@LISTINGNAME", SqlDbType.VarChar).Value = product.Name;
                        adapter.InsertCommand.Parameters.Add("@VENDORIMAGEURL", SqlDbType.VarChar).Value = product.ImageUrl;
                        adapter.InsertCommand.Parameters.Add("@VENDORLINKURL", SqlDbType.VarChar).Value = product.Url;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTSTATUS", SqlDbType.Bit).Value = product.Availability;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTPRICE", SqlDbType.Decimal).Value = product.Price;
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = vendor;

                        int rowsReturned = adapter.InsertCommand.ExecuteNonQuery();

                        transaction.Commit();

                        response.Code = AutoBuildSystemCodes.Success;
                        response.GenericObject = rowsReturned;

                        return response;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        // Passes the exception number to a handler which returns an AutoBuildSystemCode
                        response.Code = SqlExceptionHandler.GetCode(ex.Number);
                        response.GenericObject = -1;

                        return response;

                    }
                }
            }
        }

        /// <summary>
        /// This method will delete a product from the vendor's list of products
        /// </summary>
        /// <param name="modelNumber">takes in the model number of the product to be deleted</param>
        /// <returns>returns a system code with a list of AddProductDTOs.</returns>
        public virtual SystemCodeWithObject<int> DeleteProductFromVendorList(string modelNumber)
        {
            // Initialize the system code response object
            SystemCodeWithObject<int> response = new SystemCodeWithObject<int>();

            // Product null check
            if (String.IsNullOrEmpty(modelNumber))
            {
                response.Code = AutoBuildSystemCodes.NullValue;
                response.GenericObject = -1;

                return response;
            }

            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                response.Code = AutoBuildSystemCodes.Unauthorized;
                response.GenericObject = -1;

                return response;
            }

            // Get the current principal on the thread
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string vendor = _threadPrinciple.Identity.Name;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "delete from Vendor_product_junction where vendorID = (select vendorId from vendorClub where vendorName =@VENDORNAME) " +
                            "and productID = (select productID from products where modelNumber = @MODELNUMBER)";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = vendor;
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = modelNumber;

                        int rowsReturned = adapter.InsertCommand.ExecuteNonQuery();

                        transaction.Commit();

                        response.Code = AutoBuildSystemCodes.Success;
                        response.GenericObject = rowsReturned;

                        return response;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        // Passes the exception number to a handler which returns an AutoBuildSystemCode
                        response.Code = SqlExceptionHandler.GetCode(ex.Number);
                        response.GenericObject = -1;

                        return response;
                    }
                }
            }
        }
    }
}
