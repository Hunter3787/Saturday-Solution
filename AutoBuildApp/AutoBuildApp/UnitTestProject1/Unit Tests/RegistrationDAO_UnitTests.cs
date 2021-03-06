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
    public class RegistrationDAO_UnitTests
    {
        const string CONNECTION_STRING = "Data Source=DESKTOP-LG4QVLU;Initial Catalog=DB;Integrated Security=True";
        private static SqlConnection connection;


        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            connection = new SqlConnection(CONNECTION_STRING);
            connection.Open();
        }

        [DataTestMethod]
        [DataRow("crk", "Connor", "Kobel", "crkobel@verizon.net", "Taco12345")]
        [DataRow("crkobel123456789000000000000", "Connor", "Kobel", "crkobel@verizon.net", "Taco12345")]
        [DataRow("crkobel", "", "Kobel", "crkobel@verizon.net", "Taco12345")]
        [DataRow("crkobel", "Connor", "", "crkobel@verizon.net", "Taco12345")]
        [DataRow("ckobel", "Connor", "Kobel", "crkobelverizon.net", "Taco12345")]
        [DataRow("crkobel", "Connor", "Kobel", "crkobel@verizonnet", "Taco12345")]
        [DataRow("crkobel", "Connor", "Kobel", "", "Taco12345")]
        [DataRow("crkobel", "Connor", "Kobel", "crkobel@verizon.net", "Tacoooooooooo")]
        [DataRow("crkobel", "Connor", "Kobel", "crkobel@verizon.net", "Taco12")]
        [DataRow("crkobel", "Connor", "Kobel", "crkobel@verizon.net", "taco12345")]
        [DataRow("crkobel", "Connor", "Kobel", "crkobel@verizon.net", "TACO12345")]
        public void Registration_Invalid_Input(string username, string firstName, string lastName, string email, string password)
        {

            //Arrange
            RegistrationManager manager = new RegistrationManager(CONNECTION_STRING);

            UserAccount account = new UserAccount(username, firstName, lastName, email, "basic", password, "03-06-2021");

            //Act
            String result = manager.RegisterUser(account);

            //Assert
            Assert.AreEqual("Invalid Input!", result);
        }
    }
}