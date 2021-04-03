using AutoBuildApp.Models;
using AutoBuildApp.Models.Web_Crawler;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class WebCrawlerDAO
    {
        private string connectionString;
        public WebCrawlerDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void PostProductToDatabase(Product product)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        String sql = "insert into products(modelNumber, productType, productName, manufacturerName)Values(@MODELNUMBER, @PRODTYPE, @PRODNAME, @MANUFACTURERNAME)";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = product.ModelNumber;
                        adapter.InsertCommand.Parameters.Add("@PRODTYPE", SqlDbType.VarChar).Value = product.ProductType;
                        adapter.InsertCommand.Parameters.Add("@PRODNAME", SqlDbType.VarChar).Value = product.Name;
                        adapter.InsertCommand.Parameters.Add("@MANUFACTURERNAME", SqlDbType.VarChar).Value = product.ManufacturerName;

                        adapter.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();
                        Console.WriteLine("done");

                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("wrong");
                        transaction.Rollback();
                    }
                }
            }
        }

        public void PostSpecsOfProductsToDatabase()
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //try
                    //{
                    //    SqlDataAdapter adapter = new SqlDataAdapter();
                    //    String sql = "INSERT INTO userAccounts(username, email, firstName, lastName, roley, registrationDate, passwordHash ) VALUES(@USERNAME,@EMAIL, @FIRSTNAME, @LASTNAME, @ROLEY, @REG, @PASSWORD);";

                    //    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    //    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = user.UserName;
                    //    adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.UserEmail;
                    //    adapter.InsertCommand.Parameters.Add("@FIRSTNAME", SqlDbType.VarChar).Value = user.FirstName;
                    //    adapter.InsertCommand.Parameters.Add("@LASTNAME", SqlDbType.VarChar).Value = user.LastName;
                    //    adapter.InsertCommand.Parameters.Add("@ROLEY", SqlDbType.VarChar).Value = user.role;
                    //    adapter.InsertCommand.Parameters.Add("@PASSWORD", SqlDbType.VarChar).Value = user.passHash;
                    //    adapter.InsertCommand.Parameters.Add("@REG", SqlDbType.Date).Value = user.registrationDate;
                    //    adapter.InsertCommand.ExecuteNonQuery();

                    //    transaction.Commit();
                    //    Console.WriteLine("done");

                    //} catch (SqlException ex)
                    //{
                    //    Console.WriteLine("wrong");
                    //    transaction.Rollback();
                    //}
                }
            }
        }

        public void PostToVendorProductsTable()
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //try
                    //{
                    //    SqlDataAdapter adapter = new SqlDataAdapter();
                    //    String sql = "INSERT INTO userAccounts(username, email, firstName, lastName, roley, registrationDate, passwordHash ) VALUES(@USERNAME,@EMAIL, @FIRSTNAME, @LASTNAME, @ROLEY, @REG, @PASSWORD);";

                    //    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    //    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = user.UserName;
                    //    adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.UserEmail;
                    //    adapter.InsertCommand.Parameters.Add("@FIRSTNAME", SqlDbType.VarChar).Value = user.FirstName;
                    //    adapter.InsertCommand.Parameters.Add("@LASTNAME", SqlDbType.VarChar).Value = user.LastName;
                    //    adapter.InsertCommand.Parameters.Add("@ROLEY", SqlDbType.VarChar).Value = user.role;
                    //    adapter.InsertCommand.Parameters.Add("@PASSWORD", SqlDbType.VarChar).Value = user.passHash;
                    //    adapter.InsertCommand.Parameters.Add("@REG", SqlDbType.Date).Value = user.registrationDate;
                    //    adapter.InsertCommand.ExecuteNonQuery();

                    //    transaction.Commit();
                    //    Console.WriteLine("done");

                    //} catch (SqlException ex)
                    //{
                    //    Console.WriteLine("wrong");
                    //    transaction.Rollback();
                    //}
                }
            }
        }
    }
}
