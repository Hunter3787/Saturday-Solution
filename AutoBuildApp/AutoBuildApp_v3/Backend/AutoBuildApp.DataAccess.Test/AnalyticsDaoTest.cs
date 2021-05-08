using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;

namespace AutoBuildApp.DataAccess.Test
{
    [TestClass]
    public class AnalyticsDaoTest
    {
        // since this is a test for the DAO: will instantiate here

        ConnectionManager conString = ConnectionManager.connectionManager;
        // 2) passing in the name I assigned my connection string 
        private AnalyticsDAO _uadDAO;
        public AnalyticsDaoTest()
        {
            string connection = conString.GetConnectionStringByName("MyConnection");
             _uadDAO = new AnalyticsDAO(connection);
        }

        private static IEnumerable<object[]> getNullPayload()
        {
            AnalyticsDAO AnalyticsDAONull = null;
            AnalyticsDAO AnalyticsDAONULLParam = new AnalyticsDAO(null);
            AnalyticsDAO AnalyticsDAOEmptyString = new AnalyticsDAO("    ");

            return new List<object[]>()
            {
               new object[]{ AnalyticsDAONull, "NULL OBJECT PROVIDED"},
               new object[]{ AnalyticsDAONULLParam, "NULL OBJECT PROVIDED"},
               new object[]{ AnalyticsDAOEmptyString, "NULL OBJECT PROVIDED"}
            };
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(getNullPayload), DynamicDataSourceType.Method)]
        public void AuthDAO_Null_Object_ReturnNullException(AnalyticsDAO obj, string expectedParamName)
        {
            try
            {
                AnalyticsDAO authDAO = (AnalyticsDAO)obj;
            }
            //Assert:
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual(expectedParamName, ex.ParamName);
            }

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
            IClaims adminClaims = claimsFactory.GetClaims(RoleEnumType.SystemAdmin);
            IClaims basicClaims = claimsFactory.GetClaims(RoleEnumType.BasicRole);

            ClaimsIdentity adminClaimsIdentity =  new ClaimsIdentity
            (AdminIdentity, adminClaims.Claims(), AdminIdentity.AuthenticationType, AdminIdentity.Name, " ");


            ClaimsIdentity basicClaimsIdentity = new ClaimsIdentity
            (AdminIdentity, basicClaims.Claims(), AdminIdentity.AuthenticationType, AdminIdentity.Name, " ");



            ResponseUAD expectedSuccessUAD = new ResponseUAD()
            {
                IsSuccessful = true,
                ConnectionState = true,
            };
            ResponseUAD expectedFailedUAD = new ResponseUAD()
            {
                IsSuccessful = false,
                ResponseString = AuthorizationResultType.NotAuthorized.ToString(),
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
        public void GetAllAnalytics_ValidAndInvalidParmissions_ResponseReturned
            (ClaimsPrincipal principalGenerated, ResponseUAD expectedUAD)
        {



           Thread.CurrentPrincipal = principalGenerated;
            ResponseUAD responseUAD = new ResponseUAD();
            
            string connection = conString.GetConnectionStringByName("MyConnection");
            responseUAD = _uadDAO.GetGraphData(DBViews.none);


            //Console.WriteLine(expectedUAD.ToString());
            Console.WriteLine(responseUAD.ToString());

            Assert.AreEqual(expectedUAD.ToString(), responseUAD.ToString());

        }


    }

}
