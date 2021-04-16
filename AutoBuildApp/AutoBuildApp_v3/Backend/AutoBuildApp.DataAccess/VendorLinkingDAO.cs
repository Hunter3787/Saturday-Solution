using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Models.WebCrawler;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class VendorLinkingDAO
    {
        public bool AddProductToVendorListOfProducts(AddProductDTO product)
        {
            using (SqlConnection connection = new SqlConnection("Server = localhost; Database = DB; Trusted_Connection = True;"))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "insert into vendor_product_junction(vendorName, modelNumber, vendorImageUrl, vendorLinkURL, productStatus, productPrice)Values" +
                            "(@VENDORNAME, @MODELNUMBER, @VENDORIMAGEURL, @VENDORLINKURL, @PRODUCTSTATUS, @PRODUCTPRICE)";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = product.Company.ToLower();
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
                        adapter.InsertCommand.Parameters.Add("@VENDORIMAGEURL", SqlDbType.VarChar).Value = product.ImageUrl;
                        adapter.InsertCommand.Parameters.Add("@VENDORLINKURL", SqlDbType.VarChar).Value = product.Url;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTSTATUS", SqlDbType.VarChar).Value = product.Availability ? "AVAILABLE" : "NOT AVAILABLE";
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
            using (SqlConnection connection = new SqlConnection("Server = localhost; Database = DB; Trusted_Connection = True;"))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "update vendor_product_junction set vendorImageUrl = @VENDORIMAGEURL, vendorlinkurl = @VENDORLINKURL, productStatus = @PRODUCTSTATUS, productPrice = @PRODUCTPRICE where modelNumber =" +
                            "@MODELNUMBER and vendorName = @VENDORNAME)";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@VENDORIMAGEURL", SqlDbType.VarChar).Value = product.ImageUrl;
                        adapter.InsertCommand.Parameters.Add("@VENDORLINKURL", SqlDbType.VarChar).Value = product.Url;
                        adapter.InsertCommand.Parameters.Add("@PRODUCTSTATUS", SqlDbType.VarChar).Value = product.Availability ? "AVAILABLE" : "NOT AVAILABLE";
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
            using (SqlConnection connection = new SqlConnection("Server = localhost; Database = DB; Trusted_Connection = True;"))
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

        public List<AddProductDTO> GetAllProductsByVendor(string companyName)
        {
            List<AddProductDTO> allProductsByVendor = new List<AddProductDTO>();
            Console.WriteLine("here");
            using (SqlConnection connection = new SqlConnection("Server = localhost; Database = DB; Trusted_Connection = True;"))
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
                        String sql = "select vp.vendorImageURL, vp.productStatus, v.vendorName, vp.VendorLinkURL, p.modelNumber, vp.productPrice from ((vendor_product_junction as vp " +
                                        "inner join products as p on vp.modelNumber = p.modelNumber) " +
                                        "inner join vendorclub as v on vp.vendorName = v.vendorName)" +
                                        "where vp.vendorName = @VENDORNAME";
                        //String sql = "select vp.productStatus from vendor_product_junction as vp";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@VENDORNAME", SqlDbType.VarChar).Value = companyName.ToLower();

                        using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AddProductDTO productInfo = new AddProductDTO();
                                productInfo.ImageUrl = (string)reader["vendorImageURL"];
                                productInfo.Availability = (string)reader["productStatus"] == "AVAILABLE" ? true : false;
                                productInfo.Company = (string)reader["vendorName"];
                                productInfo.Url = (string)reader["VendorLinkURL"];
                                productInfo.ModelNumber = (string)reader["modelNumber"];
                                productInfo.Price = (string)reader["productPrice"];

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
    }
}
