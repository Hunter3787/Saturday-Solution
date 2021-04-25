using AutoBuildApp.Api.HelperFunctions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess.Test
{
    [TestClass]
    public class UadDaoTest
    {
        // since this is a test for the DAO: will instantiate here

        ConnectionManager conString = ConnectionManager.connectionManager;
        // 2) passing in the name I assigned my connection string 
        private UadDAO _uadDAO;
        public UadDaoTest()
        {
            string connection = conString.GetConnectionStringByName("MyConnection");
             _uadDAO = new UadDAO(connection);
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
        }

        [TestMethod]
        [DataTestMethod]
        //[DynamicData(nameof(getPermissionsData), DynamicDataSourceType.Method)]
        public void GetAllAnalytics()
        {
            ResponseUAD _responseUAD = new ResponseUAD();

            string connection = conString.GetConnectionStringByName("MyConnection");
            _responseUAD = _uadDAO.GetAllAnalytics();

            var BarGraph1 = _responseUAD.GetNumAccountsPerRole;

            var BarGraph2 = _responseUAD.GetUsePerComponent;

            var LineGraph1 = _responseUAD.GetRegPerMontthByUserType;

            var BarGraph3 = _responseUAD.GetAvgSessDurPerRole;

            var LineGraph2 = _responseUAD.GetPageViewPerMonth;

            Assert.IsNotNull(LineGraph2);


            // have a rollback
        }
    }

}
