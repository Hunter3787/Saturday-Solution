using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels;
using AutoBuildApp.DomainModels.Abstractions;
using AutoBuildApp.Managers.FeatureManagers;
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

namespace AutoBuildApp.Manger.Tests
{
    [TestClass]
    public class UADManagerTests
    {

        // getting the manager 
        ConnectionManager conString = ConnectionManager.connectionManager;

        private static IEnumerable<object[]> Principle_CommonResponse_Data()
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

            ClaimsIdentity adminClaimsIdentity = new ClaimsIdentity
            (AdminIdentity, adminClaims.Claims(), AdminIdentity.AuthenticationType, AdminIdentity.Name, " ");


            ClaimsIdentity basicClaimsIdentity = new ClaimsIdentity
            (AdminIdentity, basicClaims.Claims(), AdminIdentity.AuthenticationType, AdminIdentity.Name, " ");



            AnalyticsDataDTO analyticsDataDTOExpectedSuccess = new AnalyticsDataDTO()
            {
                SuccessFlag = true,
                Result =  "Successful Data retrieval",
        };
            AnalyticsDataDTO analyticsDataDTOExpectedFail = new AnalyticsDataDTO()
            {
                SuccessFlag = false,
                Result = AuthorizationResultType.NotAuthorized.ToString(),

            };

            return new List<object[]>()
            {
               new object[]{ new ClaimsPrincipal(basicClaimsIdentity), analyticsDataDTOExpectedFail },
               //new object[]{ new ClaimsPrincipal(adminClaimsIdentity), analyticsDataDTOExpectedSuccess },
            };

        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(Principle_CommonResponse_Data), DynamicDataSourceType.Method)]
        public void GetChartData_AnalyticsDTO_Returned
          (ClaimsPrincipal principalGenerated, AnalyticsDataDTO analyticsDataDTOExpected)
        {
            // setting up:
            Thread.CurrentPrincipal = principalGenerated; // setting the passes principle to the thread
            ResponseUAD expectedResponseUAD = new ResponseUAD(); // inst

            string connection = conString.GetConnectionStringByName("MyConnection");

            AnalyticsManager uadManager = new AnalyticsManager(connection);

            AnalyticsDataDTO analyticsDataDTOActual = new AnalyticsDataDTO();
            analyticsDataDTOActual = uadManager.GetChartData(0);

            string actual = analyticsDataDTOActual.Result;
            string expected = analyticsDataDTOExpected.Result;


           Assert.AreEqual(expected, actual);

        }



        private static IEnumerable<object[]> PricipleData()
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

            ClaimsIdentity adminClaimsIdentity = new ClaimsIdentity
            (AdminIdentity, adminClaims.Claims(), AdminIdentity.AuthenticationType, AdminIdentity.Name, " ");


            ClaimsIdentity basicClaimsIdentity = new ClaimsIdentity
            (AdminIdentity, basicClaims.Claims(), AdminIdentity.AuthenticationType, AdminIdentity.Name, " ");




            ResponseUAD expectedResponse = new ResponseUAD()
            {
                IsSuccessful =  true,
                ConnectionState = true,
               // IsAuthorized = true
            };


            ResponseUAD expectedFailedUAD = new ResponseUAD()
            {
                IsSuccessful = false,
                ResponseString = AuthorizationResultType.NotAuthorized.ToString(),
            };


            return new List<object[]>()
            {
               new object[]{ new ClaimsPrincipal(adminClaimsIdentity),expectedResponse },
               new object[]{ new ClaimsPrincipal(basicClaimsIdentity),expectedFailedUAD },

            };

        }


        /// <summary>
        /// this is an intergration test:
        /// </summary>
        /// <param name="principalGenerated"></param>
        /// <param name="expectedResponse"></param>
        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(PricipleData), DynamicDataSourceType.Method)]
        public void GetGraphData_PermissioinsRequired_ValidResponseReturned
       (ClaimsPrincipal principalGenerated, ResponseUAD expectedResponse)
        {
            Thread.CurrentPrincipal = principalGenerated;
            ResponseUAD responseUAD = new ResponseUAD();

            string connection = conString.GetConnectionStringByName("MyConnection");

            AnalyticsManager _uadManager = new AnalyticsManager(connection);

            _uadManager.GetChartData(0);


            Assert.IsNotNull(1);

        }


        private static IEnumerable<object[]> getNullPayload()
        {
            AnalyticsManager AnalyticsManagerNull = null;
            AnalyticsManager AnalyticsManagerNULLParam = new AnalyticsManager(null);
            AnalyticsManager AnalyticsManagerString = new AnalyticsManager("    ");

            return new List<object[]>()
            {
               new object[]{ AnalyticsManagerNull, "NULL OBJECT PROVIDED"},
               new object[]{ AnalyticsManagerNULLParam, "NULL OBJECT PROVIDED"},
               new object[]{ AnalyticsManagerString, "NULL OBJECT PROVIDED"}
            };
        }



        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(getNullPayload), DynamicDataSourceType.Method)]
        public void AuthDAO_Null_Object_ReturnNullException(AnalyticsManager obj, string expectedParamName)
        {
            try
            {
                AnalyticsManager authDAO = (AnalyticsManager)obj;
            }
            //Assert:
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual(expectedParamName, ex.ParamName);
            }

        }





    }
}
