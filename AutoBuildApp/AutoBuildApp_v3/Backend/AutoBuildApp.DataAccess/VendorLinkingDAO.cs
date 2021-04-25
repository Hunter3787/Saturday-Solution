using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Models.WebCrawler;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace AutoBuildApp.DataAccess
{
    public class VendorLinkingDAO
    {
        private string _connectionString;
        public VendorLinkingDAO(string connectionString)
        {
            Console.WriteLine("conn = " + connectionString);
            _connectionString = connectionString;
        }

        public List<AddProductDTO> GetProductsByFilter(GetProductByFilterDTO product, int f)
        {
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string username = _threadPrinciple.Identity.Name;
            //string username = "new egg";
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


                        //SqlParameter param = adapter.InsertCommand
                        //.Parameters
                        //.AddWithValue("@TYPEBUDGET", pair);


                        //command.Transaction = connection.BeginTransaction();
                        //command.Connection = connection;
                        //command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                        //command.CommandType = CommandType.Text;
                        //String sql = "select vp.productName, vp.vendorImageURL, vp.productStatus, v.vendorName, vp.VendorLinkURL, p.modelNumber, vp.productPrice from ((vendor_product_junction as vp " +
                        //                "inner join products as p on vp.productID = p.productID) " +
                        //                "inner join vendorclub as v on vp.vendorID = v.vendorID)" +
                        //                "where vp.vendorID = (select userCredID from userCredentials where username = @USERNAME)";
                        //String sql = "select vp.productStatus from vendor_product_junction as vp";

                        //string sp_GetAllProductsByVendor = "GetAllProductsByVendor";

                        //adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                        //adapter.InsertCommand.CommandText = sp_GetAllProductsByVendor;
                        //adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;
                        //adapter.InsertCommand.Parameters.Add("@MOTHERBOARD", SqlDbType.Bit).Value = product.FilteredListOfProducts["motherboard"];
                        //adapter.InsertCommand.Parameters.Add("@CPU", SqlDbType.Bit).Value = product.FilteredListOfProducts["cpu"];
                        //adapter.InsertCommand.Parameters.Add("@GPU", SqlDbType.Bit).Value = product.FilteredListOfProducts["gpu"];
                        //adapter.InsertCommand.Parameters.Add("@CASE", SqlDbType.Bit).Value = product.FilteredListOfProducts["case"];
                        //adapter.InsertCommand.Parameters.Add("@PSU", SqlDbType.Bit).Value = product.FilteredListOfProducts["power supply"];
                        //adapter.InsertCommand.Parameters.Add("@RAM", SqlDbType.Bit).Value = product.FilteredListOfProducts["ram"];
                        //adapter.InsertCommand.Parameters.Add("@SSD", SqlDbType.Bit).Value = product.FilteredListOfProducts["ssd"];
                        //adapter.InsertCommand.Parameters.Add("@HD", SqlDbType.Bit).Value = product.FilteredListOfProducts["hd"];
                        //adapter.InsertCommand.Parameters.Add("@ORDER", SqlDbType.VarChar).Value = product.PriceOrder;


                        //adapter.InsertCommand.Parameters.AddWithValue("@USERNAME", username);

                        using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                        {
                            Console.WriteLine("hey");

                            while (reader.Read())
                            {
                                Console.WriteLine("hey2");

                                AddProductDTO productInfo = new AddProductDTO();
                                productInfo.Name = (string)reader["productName"];
                                productInfo.ImageUrl = (string)reader["vendorImageURL"];
                                productInfo.Availability = (bool)reader["productStatus"];
                                productInfo.Company = (string)reader["vendorName"];
                                productInfo.Url = (string)reader["VendorLinkURL"];
                                productInfo.ModelNumber = (string)reader["modelNumber"];
                                productInfo.Price = Decimal.ToDouble((decimal)reader["productPrice"]);

                                allProductsByVendor.Add(productInfo);
                            }
                        }

                        transaction.Commit();
                        Console.WriteLine("donef2");
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                    }
                }
            }

            return allProductsByVendor;
        }

        public List<AddProductDTO> GetProductsByFilter(GetProductByFilterDTO product)
        {
            Console.WriteLine("here");
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string username = _threadPrinciple.Identity.Name;
            List<AddProductDTO> allProductsByVendor = new List<AddProductDTO>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        //command.Transaction = connection.BeginTransaction();
                        //command.Connection = connection;
                        //command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                        //command.CommandType = CommandType.Text;
                        //String sql = "select vp.productName, vp.vendorImageURL, vp.productStatus, v.vendorName, vp.VendorLinkURL, p.modelNumber, vp.productPrice from ((vendor_product_junction as vp " +
                        //                "inner join products as p on vp.productID = p.productID) " +
                        //                "inner join vendorclub as v on vp.vendorID = v.vendorID)" +
                        //                "where vp.vendorID = (select userCredID from userCredentials where username = @USERNAME)";
                        //String sql = "select vp.productStatus from vendor_product_junction as vp";

                        string sp_GetAllProductsByVendor = "GetAllProductsByVendor";
                        adapter.InsertCommand = new SqlCommand(sp_GetAllProductsByVendor, connection, transaction);
                        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                        adapter.InsertCommand.CommandText = sp_GetAllProductsByVendor;
                        adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;
                        adapter.InsertCommand.Parameters.Add("@MOTHERBOARD", SqlDbType.Bit).Value = product.FilteredListOfProducts["motherboard"];
                        adapter.InsertCommand.Parameters.Add("@CPU", SqlDbType.Bit).Value = product.FilteredListOfProducts["cpu"];
                        adapter.InsertCommand.Parameters.Add("@GPU", SqlDbType.Bit).Value = product.FilteredListOfProducts["gpu"];
                        adapter.InsertCommand.Parameters.Add("@CASE", SqlDbType.Bit).Value = product.FilteredListOfProducts["case"];
                        adapter.InsertCommand.Parameters.Add("@PSU", SqlDbType.Bit).Value = product.FilteredListOfProducts["power supply"];
                        adapter.InsertCommand.Parameters.Add("@RAM", SqlDbType.Bit).Value = product.FilteredListOfProducts["ram"];
                        adapter.InsertCommand.Parameters.Add("@SSD", SqlDbType.Bit).Value = product.FilteredListOfProducts["ssd"];
                        adapter.InsertCommand.Parameters.Add("@HD", SqlDbType.Bit).Value = product.FilteredListOfProducts["hd"];
                        adapter.InsertCommand.Parameters.Add("@ORDER", SqlDbType.VarChar).Value = product.PriceOrder;

                        //adapter.InsertCommand.Parameters.AddWithValue("@USERNAME", username);

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
                                productInfo.Price = Decimal.ToDouble((decimal)reader["productPrice"]);

                                allProductsByVendor.Add(productInfo);
                            }
                        }

                        transaction.Commit();
                        Console.WriteLine("donef2");
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                    }
                }
            }
            return allProductsByVendor;
        }

        public bool AddProductToVendorListOfProducts(AddProductDTO product)
        {
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
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = product.Company.ToLower();
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTNAME", SqlDbType.VarChar).Value = product.Name;
                        adapter.InsertCommand.Parameters.Add("@VENDORIMAGEURL", SqlDbType.VarChar).Value = product.ImageUrl;
                        adapter.InsertCommand.Parameters.Add("@VENDORLINKURL", SqlDbType.VarChar).Value = product.Url;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTSTATUS", SqlDbType.VarChar).Value = product.Availability;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTPRICE", SqlDbType.VarChar).Value = product.Price;

                        adapter.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();
                        Console.WriteLine("donef");
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool EditProductInVendorListOfProducts(AddProductDTO product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "update vendor_product_junction set productName = @PRODUCTNAME, vendorImageUrl = @VENDORIMAGEURL, vendorlinkurl = @VENDORLINKURL, productStatus = @PRODUCTSTATUS, productPrice = @PRODUCTPRICE where productID =" +
                            "(select productID from products where modelNumber = @MODELNUMBER) and vendorID = (select vendorID from vendorclub where vendorName = @VENDORNAME)";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@PRODUCTNAME", SqlDbType.VarChar).Value = product.Name;
                        adapter.InsertCommand.Parameters.Add("@VENDORIMAGEURL", SqlDbType.VarChar).Value = product.ImageUrl;
                        adapter.InsertCommand.Parameters.Add("@VENDORLINKURL", SqlDbType.VarChar).Value = product.Url;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTSTATUS", SqlDbType.VarChar).Value = product.Availability;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTPRICE", SqlDbType.VarChar).Value = product.Price;
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = product.Company.ToLower();

                        //adapter.InsertCommand.Parameters.Add("@RATING", SqlDbType.VarChar).Value = product.TotalRating;
                        //adapter.InsertCommand.Parameters.Add("@REVIEWS", SqlDbType.VarChar).Value = product.TotalNumberOfReviews;


                        adapter.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();
                        Console.WriteLine("donef2");
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool DeleteProductFromVendorListOfProducts(AddProductDTO product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "delete from Vendor_product_junction where vendorID = (select vendorId from vendorClub where vendorName =@VENDORNAME) " +
                            "and (select productID from products where modelNumber = @MODELNUMBER)";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = product.Company.ToLower();
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;

                        adapter.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();
                        Console.WriteLine("donef2");
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        //public List<AddProductDTO> GetAllProductsByVendor(string companyName)
        //{
        //    ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
        //    string username = _threadPrinciple.Identity.Name;
        //    Console.WriteLine("username = " + username);
        //    List<AddProductDTO> allProductsByVendor = new List<AddProductDTO>();
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        using (SqlTransaction transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {
        //                SqlDataAdapter adapter = new SqlDataAdapter();
        //                //command.Transaction = connection.BeginTransaction();
        //                //command.Connection = connection;
        //                //command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
        //                //command.CommandType = CommandType.Text;
        //                //String sql = "select vp.productName, vp.vendorImageURL, vp.productStatus, v.vendorName, vp.VendorLinkURL, p.modelNumber, vp.productPrice from ((vendor_product_junction as vp " +
        //                //                "inner join products as p on vp.productID = p.productID) " +
        //                //                "inner join vendorclub as v on vp.vendorID = v.vendorID)" +
        //                //                "where vp.vendorID = (select userCredID from userCredentials where username = @USERNAME)";
        //                //String sql = "select vp.productStatus from vendor_product_junction as vp";

        //                string sp_GetAllProductsByVendor = "GetAllProductsByVendor";
        //                adapter.InsertCommand = new SqlCommand(sp_GetAllProductsByVendor, connection, transaction);
        //                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
        //                adapter.InsertCommand.CommandText = sp_GetAllProductsByVendor;
        //                adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

        //                //adapter.InsertCommand.Parameters.AddWithValue("@USERNAME", username);

        //                using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        AddProductDTO productInfo = new AddProductDTO();
        //                        productInfo.Name = (string)reader["productName"];
        //                        productInfo.ImageUrl = (string)reader["vendorImageURL"];
        //                        productInfo.Availability = (bool)reader["productStatus"];
        //                        productInfo.Company = (string)reader["vendorName"];
        //                        productInfo.Url = (string)reader["VendorLinkURL"];
        //                        productInfo.ModelNumber = (string)reader["modelNumber"];
        //                        productInfo.Price = Decimal.ToDouble((decimal)reader["productPrice"]);

        //                        allProductsByVendor.Add(productInfo);
        //                    }
        //                }

        //                transaction.Commit();
        //                Console.WriteLine("donef2");
        //            }
        //            catch (SqlException ex)
        //            {
        //                Console.WriteLine("wrong");
        //                transaction.Rollback();
        //            }
        //        }
        //    }
        //    return allProductsByVendor;
        //} 

        public List<AddProductDTO> GetAllProductsByVendor(string companyName)
        {
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string username = _threadPrinciple.Identity.Name;
            Console.WriteLine("username = " + username);
            List<AddProductDTO> allProductsByVendor = new List<AddProductDTO>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        //command.Transaction = connection.BeginTransaction();
                        //command.Connection = connection;
                        //command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                        //command.CommandType = CommandType.Text;
                        //String sql = "select vp.productName, vp.vendorImageURL, vp.productStatus, v.vendorName, vp.VendorLinkURL, p.modelNumber, vp.productPrice from ((vendor_product_junction as vp " +
                        //                "inner join products as p on vp.productID = p.productID) " +
                        //                "inner join vendorclub as v on vp.vendorID = v.vendorID)" +
                        //                "where vp.vendorID = (select userCredID from userCredentials where username = @USERNAME)";
                        //String sql = "select vp.productStatus from vendor_product_junction as vp";

                        string sp_GetAllProductsByVendor = "nickProc";
                        adapter.InsertCommand = new SqlCommand(sp_GetAllProductsByVendor, connection, transaction);
                        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                        adapter.InsertCommand.CommandText = sp_GetAllProductsByVendor;
                        //adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

                        //adapter.InsertCommand.Parameters.AddWithValue("@USERNAME", username);

                        using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                        {
                            int i = 0;
                            while (reader.Read())
                            {
                                Console.WriteLine((string)reader.GetName(i++));

                                //while()
                                //AddProductDTO productInfo = new AddProductDTO();
                                //productInfo.Name = (string)reader["productName"];
                                //productInfo.ImageUrl = (string)reader["vendorImageURL"];
                                //productInfo.Availability = (bool)reader["productStatus"];
                                //productInfo.Company = (string)reader["vendorName"];
                                //productInfo.Url = (string)reader["VendorLinkURL"];
                                //productInfo.ModelNumber = (string)reader["modelNumber"];
                                //productInfo.Price = Decimal.ToDouble((decimal)reader["productPrice"]);

                                //allProductsByVendor.Add(productInfo);
                            }

                            reader.NextResult();
                            i = 0;
                            while(reader.Read())
                            {
                                Console.WriteLine("hello");
                                Console.WriteLine((string)reader.GetName(i++));
                            }

                        }

                        transaction.Commit();
                        Console.WriteLine("donef2");
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                    }
                }
            }
            return allProductsByVendor;
        }
    }
}
