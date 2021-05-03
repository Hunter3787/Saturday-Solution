using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;

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


        private static IEnumerable<object[]> GetAllAnalytics_Data()
        {
            UserIdentity AdminIdentity = new UserIdentity
            {
                Name = "ADMIN USER",
                IsAuthenticated = true,
                AuthenticationType = "JWT"
            };

            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaims adminClaims = claimsFactory.GetClaims(RoleEnumType.SYSTEM_ADMIN);
            IClaims basicClaims = claimsFactory.GetClaims(RoleEnumType.BASIC_ROLE);

            ClaimsIdentity adminClaimsIdentity = new ClaimsIdentity
            (AdminIdentity, adminClaims.Claims(), AdminIdentity.AuthenticationType, AdminIdentity.Name, " ");


            ClaimsIdentity basicClaimsIdentity = new ClaimsIdentity
            (AdminIdentity, basicClaims.Claims(), AdminIdentity.AuthenticationType, AdminIdentity.Name, " ");

            ResponseUAD expectedSuccessUAD = new ResponseUAD()
            {
                ResponseBool = true,
                ConnectionState = true,
                IsAuthorized = true
            };
            ResponseUAD expectedFailedUAD = new ResponseUAD()
            {
                ResponseBool = false,
                IsAuthorized = false,
            };
            return new List<object[]>()
            {
               new object[]{ new ClaimsPrincipal(adminClaimsIdentity),expectedSuccessUAD },
               new object[]{ new ClaimsPrincipal(basicClaimsIdentity),expectedFailedUAD },
            };

        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(GetAllAnalytics_Data), DynamicDataSourceType.Method)]
        public void GetAllAnalytics_ResponseReturned
            (ClaimsPrincipal principalGenerated, ResponseUAD expectedUAD)
        {
           Thread.CurrentPrincipal = principalGenerated;
            ResponseUAD responseUAD = new ResponseUAD();
            
            string connection = conString.GetConnectionStringByName("MyConnection");
            responseUAD = _uadDAO.GetAllAnalytics();
            //Console.WriteLine(expectedUAD.ToString());
            //Console.WriteLine(responseUAD.ToString());
            Assert.AreEqual(expectedUAD.ToString(), responseUAD.ToString());

        }


    }

}
