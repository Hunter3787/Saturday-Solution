using AutoBuildApp.Models;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using AutoBuildApp.BusinessLayer;
using Microsoft.Data.SqlClient;
using System.Data;
using BC = BCrypt.Net.BCrypt;

namespace UnitTests.Integration_Tests
{
    [TestClass]
    public class LoginDAO_IntegrationTests
    {
        const string CONNECTION_STRING = "Data Source=DESKTOP-LG4QVLU;Initial Catalog=DB;Integrated Security=True";
        private static SqlConnection connection;


        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            connection = new SqlConnection(CONNECTION_STRING);
            connection.Open();
        }

        [TestMethod]
        public void Login_Successful_Login()
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                //Arrange
                RegistrationManager manager1 = new RegistrationManager(CONNECTION_STRING);
                LoginManager manager = new LoginManager(CONNECTION_STRING);
                SqlDataAdapter adapter = new SqlDataAdapter();

                UserAccount account = new UserAccount("crkobel", "Connor", "Kobel", "crkobel@verizon.net", "basic", "Taco12345", "03-06-2021");
                int rows = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    manager1.RegisterUser(account);
                    String result = manager.DoesUserExist(account);
                    String sql = "SELECT COUNT (UserName) FROM userAccounts WHERE Username = @USERNAME";


                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = account.UserName;
                    rows = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());

                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(1, rows);
                    Assert.AreEqual("Logged in", result);
                }


                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    String sql = "DELETE from userAccounts where username = @USERNAME;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = account.UserName;
                    adapter.InsertCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }


        [TestMethod]
        public void Login_Failed_Login()
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING))
            {
                //Arrange
                //RegistrationManager manager1 = new RegistrationManager(CONNECTION_STRING);
                LoginManager manager = new LoginManager(CONNECTION_STRING);
                SqlDataAdapter adapter = new SqlDataAdapter();

                UserAccount account = new UserAccount("crkobel", "Connor", "Kobel", "crkobel@verizon.net", "basic", "Taco12345", "03-06-2021");
                int rows = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    //manager1.RegisterUser(account);
                    String result = manager.DoesUserExist(account);
                    String sql = "SELECT COUNT (UserName) FROM userAccounts WHERE Username = @USERNAME";


                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = account.UserName;
                    rows = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());

                    transaction.Commit();

                    //Assert
                    //Assert.AreEqual(1, rows);
                    Assert.AreEqual("Failed to login", result);
                }


                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    String sql = "DELETE from userAccounts where username = @USERNAME;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = account.UserName;
                    adapter.InsertCommand.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
        }
    }
}