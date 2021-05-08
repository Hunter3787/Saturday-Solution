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
    public class AnalyticsManagerTests
    {

        // getting the manager 
        ConnectionManager conString = ConnectionManager.connectionManager;

        private static IEnumerable<object[]> NullPossibleCases()
        {
            string expectedExceptionParamName = "NULL OBJECT PROVIDED";
            AnalyticsManager AnalyticsManagerNull = null;
            AnalyticsManager AnalyticsManagerNULLParam = new AnalyticsManager(null);
            AnalyticsManager AnalyticsManagerString = new AnalyticsManager("    ");

            return new List<object[]>()
            {
               new object[]{ AnalyticsManagerNull, expectedExceptionParamName},
               new object[]{ AnalyticsManagerNULLParam, expectedExceptionParamName},
               new object[]{ AnalyticsManagerString, expectedExceptionParamName }
            };
        }

        /// <summary>
        /// AnalyticsManager_Null_Object_ReturnNullException check
        /// to see how the manager handles null calls
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="expectedParamName"></param>
        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(NullPossibleCases), DynamicDataSourceType.Method)]
        public void AnalyticsManager_Null_Object_ReturnNullException(AnalyticsManager obj, string expectedParamName)
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


        private static IEnumerable<object[]> ManagerResponseForAuthorizationData()
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



            AnalyticsDataDTO expectedAuthorizedDTO = new AnalyticsDataDTO()
            {
             SuccessFlag = true,
             Result = "Successful Data retrieval", };
            AnalyticsDataDTO expectedNotAuthorizedDTO = new AnalyticsDataDTO()
            {
                Result = AuthorizationResultType.NotAuthorized.ToString(),
                SuccessFlag = false, };
            AnalyticsDataDTO expectedInValidGraphType = new AnalyticsDataDTO()
            {
                Result = "InValid Graph Specified",
                SuccessFlag = true, };
            return new List<object[]>()
            {
               new object[]{ new ClaimsPrincipal(adminClaimsIdentity), expectedAuthorizedDTO, 1},
               new object[]{ new ClaimsPrincipal(basicClaimsIdentity), expectedNotAuthorizedDTO, 1},
               new object[]{ new ClaimsPrincipal(adminClaimsIdentity), expectedInValidGraphType, 788},

            };

        }
        /// <summary>
        /// this is an intergration test:
        /// </summary>
        /// <param name="principalGenerated"></param>
        /// <param name="expectedResponse"></param>
        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(ManagerResponseForAuthorizationData), DynamicDataSourceType.Method)]
        public void GetGraphData_PermissioinsRequired_DtoReturned
       (ClaimsPrincipal principalGenerated, AnalyticsDataDTO expectedDataDTO, int graphType)
        {
            Thread.CurrentPrincipal = principalGenerated;
            string connection = conString.GetConnectionStringByName("MyConnection");

            AnalyticsManager uadManager = new AnalyticsManager(connection);
            AnalyticsDataDTO actualDTO = new AnalyticsDataDTO();

            actualDTO = uadManager.GetChartData(graphType);

            string expected = $"{actualDTO.Result}";
            string actual = $"{expectedDataDTO.Result}";

            Console.WriteLine( $" {actualDTO.ToString()} ");
            Assert.AreEqual(expected, actual);


        }

    }
}
