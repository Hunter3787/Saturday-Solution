using AutoBuildApp.Models;
using AutoBuildApp.Models.WebCrawler;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class WebCrawlerDAO
    {
        private string _connectionString;
        public WebCrawlerDAO(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public ConcurrentDictionary<string, byte> GetAllVendors()
        {
            ConcurrentDictionary<string, byte> vendors = new ConcurrentDictionary<string, byte>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception) { }

                
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "select vendorName from vendorclub";
                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);

                        using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vendors.TryAdd((string)reader["vendorname"], 0);
                            }
                        }

                        transaction.Commit();
                    }
                    catch (SqlException )
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                    }
                }
            }
            return vendors;
        }

        public ConcurrentDictionary<string, byte> GetAllModelNumbers()
        {
            ConcurrentDictionary<string, byte> modelNumbers = new ConcurrentDictionary<string, byte>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "select modelNumber from products";
                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);

                        using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                modelNumbers.TryAdd((string)reader["modelNumber"], 0);
                            }
                        }

                        transaction.Commit();
                    }
                    catch (SqlException )
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                    }
                }
            }
            return modelNumbers;
        }
        public ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> GetAllVendorsProducts()
        {
            ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> vendorsProducts = new ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "select * from Vendor_Product_Junction vpj inner join vendorclub v on vpj.vendorID = v.vendorID inner join products p on vpj.productID = p.productID";
                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);

                        using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string vendorName = (string)reader["vendorName"];
                                string modelNumber = (string)reader["modelNumber"];

                                if(!vendorsProducts.ContainsKey(vendorName))
                                {
                                    vendorsProducts.TryAdd(vendorName, new ConcurrentDictionary<string, byte>());
                                }

                                vendorsProducts[vendorName].TryAdd(modelNumber, 0);
                            }
                        }

                        transaction.Commit();
                    }
                    catch (SqlException )
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                    }
                }
            }
            return vendorsProducts;
        }
        public bool ProductExists(string modelNumber)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //    Console.WriteLine(product.ModelNumber);
                        //    Console.WriteLine(product.ProductType);
                        //    Console.WriteLine(product.Name);
                        //    Console.WriteLine(product.ManufacturerName);

                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "select * from products where modelNumber = @MODELNUMBER";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = modelNumber;

                        int rowsReturned = adapter.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();
                        return rowsReturned == 1;
                    }
                    catch (SqlException )
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
        public bool PostProductToDatabase(Product product)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if(ProductExists(product.ModelNumber))
                        {
                            Console.WriteLine("already exists");
                            return false;
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "insert into products(productName, imageUrl, modelNumber, productType, manufacturerName)Values(@PRODUCTNAME, @IMAGEURL, @MODELNUMBER, @PRODTYPE, @MANUFACTURERNAME)";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@PRODUCTNAME", SqlDbType.VarChar).Value = product.Name;
                        adapter.InsertCommand.Parameters.Add("@IMAGEURL", SqlDbType.VarChar).Value = product.ImageUrl;
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
                        adapter.InsertCommand.Parameters.Add("@PRODTYPE", SqlDbType.VarChar).Value = product.ProductType;
                        adapter.InsertCommand.Parameters.Add("@MANUFACTURERNAME", SqlDbType.VarChar).Value = product.ManufacturerName;

                        adapter.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();
                        Console.WriteLine("done");

                    }
                    catch (SqlException )
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                    }
                }
            }
            return true;
        }

        public void PostSpecsOfProductsToDatabase(Product product)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "insert into products_specs(productID, productSpecs, productSpecsValue)Values((select productID from products where modelNumber = @MODELNUMBER), @SPECSKEY, @SPECSVALUE)";

                        foreach(var pair in product.Specs)
                        {

                            adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                            adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
                            adapter.InsertCommand.Parameters.Add("@SPECSKEY", SqlDbType.VarChar).Value = pair.Key;
                            adapter.InsertCommand.Parameters.Add("@SPECSVALUE", SqlDbType.VarChar).Value = pair.Value;
                            adapter.InsertCommand.ExecuteNonQuery();
                        }
                        transaction.Commit();

                        Console.WriteLine("done");

                    }
                    catch (SqlException)
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                    }
                }
            }
        }
        
        public bool AddVendor(string vendorName)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "insert into vendorClub(vendorName)Values(@VENDORNAME)";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = vendorName;

                        adapter.InsertCommand.ExecuteNonQuery();
                        
                        transaction.Commit();
                        return true;
                        Console.WriteLine("done");

                    }
                    catch (SqlException )
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
        //public bool VendorExists(string vendorName)
        //{
        //    using (SqlConnection connection = new SqlConnection(this.connectionString))
        //    {
        //        connection.Open();
        //        using (SqlTransaction transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {
        //                SqlDataAdapter adapter = new SqlDataAdapter();
        //                String sql = "insert into products_specs(productId, productSpecs, productSpecsValue)Values((select productID from products where modelNumber =@MODELNUMBER), @SPECSKEY, @SPECSVALUE)";

        //                foreach (var pair in product.Specs)
        //                {

        //                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
        //                    adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
        //                    adapter.InsertCommand.Parameters.Add("@SPECSKEY", SqlDbType.VarChar).Value = pair.Key;
        //                    adapter.InsertCommand.Parameters.Add("@SPECSVALUE", SqlDbType.VarChar).Value = pair.Value;
        //                    adapter.InsertCommand.ExecuteNonQuery();
        //                }
        //                transaction.Commit();

        //                Console.WriteLine("done");

        //            }
        //            catch (SqlException ex)
        //            {
        //                Console.WriteLine("wrong");
        //                transaction.Rollback();
        //            }
        //        }
        //    }
        //}
        public bool PostToVendorProductsTable(Product product)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "insert into vendor_product_junction(vendorID, productID, listingName, vendorImageUrl, vendorLinkURL, productStatus, productPrice, rating, reviews)Values(" +
                            "(select vendorID from vendorclub where vendorName = @VENDORNAME),(select productID from products where modelNumber = @MODELNUMBER), @LISTINGNAME, @VENDORIMAGEURL, " +
                            "@VENDORLINKURL, @PRODUCTSTATUS, @PRODUCTPRICE, @RATING, @REVIEWS)";

                        //double parsedPrice = (product.Price == null) ? DBNull. : Double.Parse(product.Price.Replace("$", "").Replace(",", ""));
                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@VENDORIMAGEURL", SqlDbType.VarChar).Value = product.ImageUrl;
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = product.Company;
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
                        adapter.InsertCommand.Parameters.Add("@LISTINGNAME", SqlDbType.VarChar).Value = product.Name;
                        adapter.InsertCommand.Parameters.Add("@VENDORLINKURL", SqlDbType.VarChar).Value = product.Url;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTSTATUS", SqlDbType.VarChar).Value = product.Availability ;
                        if(product.Price == null)
                        {
                            adapter.InsertCommand.Parameters.Add("@PRODUCTPRICE", SqlDbType.Decimal).Value = DBNull.Value;
                        }
                        else
                        {
                            adapter.InsertCommand.Parameters.Add("@PRODUCTPRICE", SqlDbType.Decimal).Value = Double.Parse(product.Price.Replace("$", "").Replace(",", ""));
                        }
                        adapter.InsertCommand.Parameters.Add("@RATING", SqlDbType.VarChar).Value = product.TotalRating;
                        adapter.InsertCommand.Parameters.Add("@REVIEWS", SqlDbType.VarChar).Value = product.TotalNumberOfReviews;

                        adapter.InsertCommand.ExecuteNonQuery();
                        
                        transaction.Commit();

                        return true;
                    }
                    catch (SqlException )
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();

                        return false;
                    }
                }
            }
        }

        public void PostToVendorProductReviewsTable(Product product)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "insert into Vendor_product_reviews_junction(vendorID, productID, reviewerName, reviewStarRating, reviewContent, reviewDate)VALUES(" +
                            "(select vendorID from vendorclub where vendorName = @VENDORNAME),(select productID from products where modelNumber = @MODELNUMBER)," +
                            " @REVIEWERNAME, @REVIEWSTARRATING, @REVIEWCONTENT, @REVIEWDATE)";

                        foreach (var review in product.Reviews)
                        {

                            adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                            adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = product.Company;
                            adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
                            adapter.InsertCommand.Parameters.Add("@REVIEWERNAME", SqlDbType.VarChar).Value = review.ReviewerName;
                            adapter.InsertCommand.Parameters.Add("@REVIEWSTARRATING", SqlDbType.VarChar).Value = review.StarRating;
                            adapter.InsertCommand.Parameters.Add("@REVIEWCONTENT", SqlDbType.VarChar).Value = review.Content.Length > 8000 ? review.Content.Substring(0, 8000) : review.Content;
                            adapter.InsertCommand.Parameters.Add("@REVIEWDATE", SqlDbType.VarChar).Value = review.Date;
                            adapter.InsertCommand.ExecuteNonQuery();
                        }
                        transaction.Commit();

                        Console.WriteLine("done");

                    }
                    catch (SqlException )
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
