using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Models.WebCrawler;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Threading;
using AutoBuildApp.Models;
using AutoBuildApp.Models.Enumerations;

namespace AutoBuildApp.Models
{
    public class VendorLinkingDAO
    {
        private string _connectionString;
        public VendorLinkingDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SystemCodeWithCollection<ConcurrentDictionary<string, HashSet<string>>> PopulateVendorsProducts()
        {
            ConcurrentDictionary<string, HashSet<string>> vendorsProducts = new ConcurrentDictionary<string, HashSet<string>>();
            SystemCodeWithCollection<ConcurrentDictionary<string, HashSet<string>>> response = new SystemCodeWithCollection<ConcurrentDictionary<string, HashSet<string>>>();

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

                                // Add entry to dictionary. 
                                if (!vendorsProducts.ContainsKey(VendorName))
                                {
                                    vendorsProducts.TryAdd(VendorName, new HashSet<string>());
                                }

                                // Then, add the model number to the set (dictionary's value)
                                if (!vendorsProducts[VendorName].Contains(ModelNumber))
                                {
                                    vendorsProducts[VendorName].Add(ModelNumber);
                                }
                            }
                        }

                        transaction.Commit();

                        response.Code = AutoBuildSystemCodes.Success;
                        response.GenericCollection = vendorsProducts;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        response.GenericCollection = null;
                        response.Code = SqlExceptionHandler.GetCode(ex.Number);
                    }
                }
            }

            return response;
        }

        public SystemCodeWithCollection<List<string>> GetAllModelNumbers()
        {
            string company = "new egg";
            List<string> allModelNumbers = new List<string>();
            SystemCodeWithCollection<List<string>> response = new SystemCodeWithCollection<List<string>>();
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
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = company;

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
                        response.GenericCollection = allModelNumbers;
                    }
                    catch (SqlException ex)
                    {
                        response.Code = SqlExceptionHandler.GetCode(ex.Number);
                        response.GenericCollection = null;
                    }
                }
            }
            return response;
        }

        public SystemCodeWithCollection<List<AddProductDTO>> GetVendorProductsByFilter(GetProductByFilterDTO product)
        {
            SystemCodeWithCollection<List<AddProductDTO>> response = new SystemCodeWithCollection<List<AddProductDTO>>();
            List<AddProductDTO> allProductsByFilter = new List<AddProductDTO>();

            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string username = _threadPrinciple.Identity.Name;
            Console.WriteLine("username - " + username);
            List<AddProductDTO> allProductsByVendor = new List<AddProductDTO>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();

                        string sp_GetAllProductsByVendor = "GetAllProductsByVendor2";
                        adapter.InsertCommand = new SqlCommand(sp_GetAllProductsByVendor, connection, transaction);
                        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                        adapter.InsertCommand.CommandText = sp_GetAllProductsByVendor;

                        DataTable pair = new DataTable();

                        DataColumn column = new DataColumn();
                        column.ColumnName = "productType";
                        column.DataType = typeof(string);
                        pair.Columns.Add(column);

                        column = new DataColumn();

                        column.ColumnName = "FilterOn";
                        column.DataType = typeof(bool);
                        pair.Columns.Add(column);

                        DataRow row;
                        foreach (var filter in product.FilteredListOfProducts)
                        {
                            row = pair.NewRow();
                            row["productType"] = filter.Key;
                            row["FilterOn"] = filter.Value;
                            pair.Rows.Add(row);
                        }

                        var param = new SqlParameter[3];
                        param[0] = adapter.InsertCommand.Parameters.AddWithValue("@Filterlist", pair);
                        param[1] = adapter.InsertCommand.Parameters.AddWithValue("@USERNAME", username);
                        param[2] = adapter.InsertCommand.Parameters.AddWithValue("@ORDER", product.PriceOrder);


                        using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AddProductDTO productInfo = new AddProductDTO();
                                productInfo.Name = (string)reader["productName"];
                                productInfo.ImageUrl = (string)reader["vendorImageURL"];
                                productInfo.Availability = (bool)reader["productStatus"];
                                productInfo.Company = (string)reader["vendorName"];
                                productInfo.Url = (string)reader["VendorLinkURL"];
                                productInfo.ModelNumber = (string)reader["modelNumber"];
                                productInfo.Price = Decimal.ToDouble(reader["productPrice"] == DBNull.Value ? 0 : (decimal)reader["productPrice"]);

                                allProductsByVendor.Add(productInfo);
                            }
                        }

                        transaction.Commit();

                        response.Code = AutoBuildSystemCodes.Success;
                        response.GenericCollection = allProductsByVendor;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        response.Code = SqlExceptionHandler.GetCode(ex.Number);
                        response.GenericCollection = null;

                    }
                }
            }

            return response;
        }

        public AutoBuildSystemCodes AddProductToVendorListOfProducts(AddProductDTO product)
        {
            string company = "new egg";
            //if(!VendorsProducts.ContainsKey(company))
            //{
            //    HashSet<string> HashSet = new HashSet<string>();
            //    VendorsProducts.TryAdd(company, HashSet);
            //}

            //if(VendorsProducts[company].Contains(product.ModelNumber))
            //{
            //    Console.WriteLine("can't add. that model number already exists");

            //    return false;
            //}

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "insert into vendor_product_junction(vendorID, productID, productName, vendorImageUrl, vendorLinkURL, productStatus, productPrice)Values" +
                            "((select vendorID from vendorClub where vendorName = @VENDORNAME), (select productID from products where modelNumber = @MODELNUMBER), @PRODUCTNAME, @VENDORIMAGEURL, @VENDORLINKURL, @PRODUCTSTATUS, @PRODUCTPRICE)";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = company;
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTNAME", SqlDbType.VarChar).Value = product.Name;
                        adapter.InsertCommand.Parameters.Add("@VENDORIMAGEURL", SqlDbType.VarChar).Value = product.ImageUrl;
                        adapter.InsertCommand.Parameters.Add("@VENDORLINKURL", SqlDbType.VarChar).Value = product.Url;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTSTATUS", SqlDbType.VarChar).Value = product.Availability;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTPRICE", SqlDbType.VarChar).Value = product.Price;

                        var y = adapter.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();

                        //VendorsProducts[company].Add(product.ModelNumber);

                        return AutoBuildSystemCodes.Success;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        return SqlExceptionHandler.GetCode(ex.Number);
                    }
                }
            }
        }

        public AutoBuildSystemCodes EditProductInVendorListOfProducts(AddProductDTO product)
        {
            string company = "new egg";
            //if (!VendorsProducts[company].Contains(product.ModelNumber))
            //{
            //    Console.WriteLine("can't edit. that model number doesn't exist");

            //    return false;
            //}
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "update vendor_product_junction set productName = @PRODUCTNAME, vendorImageUrl = @VENDORIMAGEURL, vendorLinkUrl = @VENDORLINKURL, productStatus = @PRODUCTSTATUS, productPrice = @PRODUCTPRICE where productID =" +
                        "(select productID from products where modelNumber = @MODELNUMBER) and vendorID = (select vendorID from vendorclub where vendorName = @VENDORNAME)";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@PRODUCTNAME", SqlDbType.VarChar).Value = product.Name;
                        adapter.InsertCommand.Parameters.Add("@VENDORIMAGEURL", SqlDbType.VarChar).Value = product.ImageUrl;
                        adapter.InsertCommand.Parameters.Add("@VENDORLINKURL", SqlDbType.VarChar).Value = product.Url;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTSTATUS", SqlDbType.VarChar).Value = product.Availability;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTPRICE", SqlDbType.Decimal).Value = product.Price;
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = company;// product.Company.ToLower();

                        adapter.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();

                        return AutoBuildSystemCodes.Success;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        return SqlExceptionHandler.GetCode(ex.Number);

                    }
                }
            }
        }

        public AutoBuildSystemCodes DeleteProductFromVendorList(string modelNumber)
        {
            string company = "new egg";
            Console.WriteLine("mode num = " + modelNumber);
            //if (!VendorsProducts["new egg"].Contains(modelNumber))
            //{
            //    Console.WriteLine("can't delete. that model number doesn't exist");
            //    return false;
            //}

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
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = company;
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = modelNumber;

                        adapter.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();

                        return AutoBuildSystemCodes.Success;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        return SqlExceptionHandler.GetCode(ex.Number);
                    }
                }
            }
        }
    }
}
