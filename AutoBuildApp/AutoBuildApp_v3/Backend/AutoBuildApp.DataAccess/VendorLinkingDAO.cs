using AutoBuildApp.DataAccess.Abstractions;
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

namespace AutoBuildApp.DataAccess
{
    public class VendorLinkingDAO
    {
        private string _connectionString;
        public readonly ConcurrentDictionary<string, HashSet<string>> VendorsProducts;
        public VendorLinkingDAO(string connectionString)
        {
            _connectionString = connectionString;
            VendorsProducts = PopulateVendorsProducts();
            //PopulateAllProducts();
        }

        public ConcurrentDictionary<string, HashSet<string>> PopulateVendorsProducts()
        {
            ConcurrentDictionary<string, HashSet<string>> VendorsProducts = new ConcurrentDictionary<string, HashSet<string>>();
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
                                if (!VendorsProducts.ContainsKey(VendorName))
                                {
                                    VendorsProducts.TryAdd(VendorName, new HashSet<string>());
                                }

                                // Then, add the model number to the set (dictionary's value)
                                if (!VendorsProducts[VendorName].Contains(ModelNumber))
                                {
                                    VendorsProducts[VendorName].Add(ModelNumber);
                                }
                            }
                        }

                        transaction.Commit();

                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
            return VendorsProducts;
        }

        //public ConcurrentDictionary<string, byte> PopulateVendors()
        //{
        //    ConcurrentDictionary<string, byte> Vendors = new ConcurrentDictionary<string, byte>();
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        using (SqlTransaction transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {
        //                SqlDataAdapter adapter = new SqlDataAdapter();

        //                string sql = "select vendorName from vendorClub";
        //                adapter.InsertCommand = new SqlCommand(sql, connection, transaction);

        //                using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        string Vendor = (string)reader["vendorName"];
        //                        Vendors.TryAdd(Vendor, 0);
        //                    }
        //                }

        //                transaction.Commit();

        //            }
        //            catch (SqlException ex)
        //            {
        //                transaction.Rollback();
        //            }
        //        }
        //    }
        //    return Vendors;
        //}

        public List<string> GetAllModelNumbers()
        {
            List<string> allModelNumbers = new List<string>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();

                        string sql = "select modelNumber from products";
                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);

                        using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string modelNumber = (string)reader["modelNumber"];
                                allModelNumbers.Add(modelNumber);
                            }
                        }

                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
            }
            return allModelNumbers;
        }

        public List<AddProductDTO> GetProductsByFilter(GetProductByFilterDTO product, int f)
        {
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string username = _threadPrinciple.Identity.Name;
            Console.WriteLine("username - " + username);
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

        public CommonResponse AddProductToVendorListOfProducts(AddProductDTO product)
        {
            CommonResponse response = new CommonResponse();
            string company = "danny";
            if(!VendorsProducts.ContainsKey(company))
            {
                HashSet<string> HashSet = new HashSet<string>();
                VendorsProducts.TryAdd(company, HashSet);
            }

            if(VendorsProducts[company].Contains(product.ModelNumber))
            {
                Console.WriteLine("can't add. that model number already exists");

                return false;
            }

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
                        Console.WriteLine(y + " records changed!!!");
                        transaction.Commit();

                        VendorsProducts[company].Add(product.ModelNumber);

                        Console.WriteLine("done!!");

                        response.SuccessString = "Successfully added product to vendor list.";
                        response.SuccessBool = true;

                        return response;
                    }
                    catch (SqlException ex)
                    {

                        transaction.Rollback();

                        response.SuccessBool = false;

                        if(ex.Number == 2627)
                        {
                            response.SuccessString = "You already have this model number in your list of products.";
                        }
                        else if(ex.Number == -2)
                        {

                        }
                        else
                        {
                            response.SuccessString = "Failed to add product to vendor list.";
                        }

                        return response;
                    }
                }
            }
        }

        public bool EditProductInVendorListOfProducts(AddProductDTO product)
        {
            string company = "danny";
            if (!VendorsProducts[company].Contains(product.ModelNumber))
            {
                Console.WriteLine("can't edit. that model number doesn't exist");

                return false;
            }
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
                        //String sql = "update vendor_product_junction set productName = @productname where productID = (select productID from products where modelNumber = @modelnumber)" ;

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@PRODUCTNAME", SqlDbType.VarChar).Value = product.Name;
                        adapter.InsertCommand.Parameters.Add("@VENDORIMAGEURL", SqlDbType.VarChar).Value = product.ImageUrl;
                        adapter.InsertCommand.Parameters.Add("@VENDORLINKURL", SqlDbType.VarChar).Value = product.Url;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTSTATUS", SqlDbType.VarChar).Value = product.Availability;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTPRICE", SqlDbType.Decimal).Value = product.Price;
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = company;// product.Company.ToLower();

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

        public bool DeleteProductFromVendorList(string modelNumber)
        {
            Console.WriteLine("mode num = " + modelNumber);
            if (!VendorsProducts["new egg"].Contains(modelNumber))
            {
                Console.WriteLine("can't delete. that model number doesn't exist");
                return false;
            }

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
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = "new egg";
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = modelNumber;

                        var x = adapter.InsertCommand.ExecuteNonQuery();
                        Console.WriteLine(x + " rows affected");
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
                                try
                                {
                                    Console.WriteLine((string)reader.GetName(i++));
                                }
                                catch(Exception e)
                                {
                                    break;
                                    Console.WriteLine("ok");
                                }

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
