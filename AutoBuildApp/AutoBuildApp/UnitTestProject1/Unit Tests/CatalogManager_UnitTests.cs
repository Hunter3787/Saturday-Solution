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
    public class CatalogManager_UnitTests
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
        public void CatalogManager_SearchProduct_SearchSuccessful(string searchString)
        {
            //Arrange

            //Act

            //Assert
            Assert.AreEqual("Search Successful")
        }

        [TestMethod]
        public void CatalogManager_ApplyFilter_FilterApplied(string searchString)
        {
            //Arrange

            //Act

            //Assert
            Assert.AreEqual("Filter Applied")
        }

        [TestMethod]
        public void CatalogManager_SaveProductToUser_ProductSaved()
        {
            //Arrange

            //Act

            //Assert
            Assert.AreEqual("Product Saved to User")
        }

        [TestMethod]
        public void CatalogManager_GetProductDetails_ProductDetails()
        {
            //Arrange

            //Act

            //Assert
            Assert.AreEqual("Product Details")
        }
    }
}