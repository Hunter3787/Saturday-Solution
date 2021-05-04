using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Models.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess.Test
{

    [TestClass]
    public class RecommendationDaoTest
    {

        ConnectionManager conString = ConnectionManager.connectionManager;
        // 2) passing in the name I assigned my connection string 
        private RecommendationDAO _recommendationDAO;
        public RecommendationDaoTest()
        {
            string connection = conString.GetConnectionStringByName("MyConnection");
            _recommendationDAO = new RecommendationDAO(connection);
        }


        private static IEnumerable<object[]> FORMETHOD()
        {
            return new List<object[]>()
            {
               new object[]{},
            };

        }

        [TestMethod]
        [DataTestMethod]
        //[DynamicData(nameof(getPermissionsData), DynamicDataSourceType.Method)]
        public void METHOD()
        {
            // have a rollback
            var result = _recommendationDAO.GetComponentDictionary(null);
            Assert.IsNull(result);
        }





    }
}
