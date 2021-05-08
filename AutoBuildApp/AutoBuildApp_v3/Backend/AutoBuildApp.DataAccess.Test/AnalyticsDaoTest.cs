using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Models.DataTransferObjects;
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
        //1) Calling connection manager for the connection string
        ConnectionManager _conString = ConnectionManager.connectionManager;
        // 2) declaring the dao 
        private AnalyticsDAO _analyticsDAO;

        #region Variables for thr principle on thread.
        private ClaimsPrincipal _principalGenerated;
        // Instantiating the claims factory to get the claims per a "role" passed
        #endregion


        public AnalyticsDaoTest()
        {
            // getting the connection string by given name
            // that lives in the appsetting 
            string connection = _conString.GetConnectionStringByName("MyConnection");
            _analyticsDAO = new AnalyticsDAO(connection);

            // Instantiating the user identity for the thread.
            UserIdentity adminIdentity = new UserIdentity
            {
                Name = "X USER",
                IsAuthenticated = true,
                AuthenticationType = "JWT"
            };
            ClaimsFactory _claimsFactory = new ConcreteClaimsFactory();
            IClaims adminClaims = _claimsFactory.GetClaims(RoleEnumType.SystemAdmin);

            // Instantiation of the admin identity and basic user identity 
            ClaimsIdentity adminClaimsIdentity = new ClaimsIdentity
            (adminIdentity, adminClaims.Claims(), adminIdentity.AuthenticationType, adminIdentity.Name, " ");
            _principalGenerated = new ClaimsPrincipal(adminClaimsIdentity);


        }

        /// <summary>
        /// The NullPossibleCases method accounts for all possible null values 
        /// that should be caught and handles
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<object[]> NullPossibleCases()
        {
            string expectedExceptionParamName = "NULL OBJECT PROVIDED";
            // in the case the DAO object is null
            AnalyticsDAO analyticsDAONull = null;
            // in case connection string parameter is null
            AnalyticsDAO analyticsDAONULLParam = new AnalyticsDAO(null);
            // in case connection string parameter is empty
            AnalyticsDAO analyticsDAOEmptyString = new AnalyticsDAO("    ");

            return new List<object[]>()
            {
               new object[]{ analyticsDAONull, expectedExceptionParamName},
               new object[]{ analyticsDAONULLParam, expectedExceptionParamName},
               new object[]{ analyticsDAOEmptyString, expectedExceptionParamName }
            };
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(NullPossibleCases), DynamicDataSourceType.Method)]
        public void AuthDAO_Null_Cases_ReturnNullException(AnalyticsDAO obj, string expectedParamName)
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


        [TestMethod]
        //unsuccessful data inputs
        [DataRow(6)]
        [DataRow(-1)]
        [DataRow(500)]
        [DataRow(-500)]
        public void GetGraphData_Null_Cases_ReturnNullException(int graphType)
        {

            Thread.CurrentPrincipal = _principalGenerated;
            /// here is the expected common response for authorizaed user
            ResponseUAD actualResponse = new ResponseUAD();

            actualResponse = _analyticsDAO.GetGraphData((DBViews)graphType);
            string expectedResponse = "InValid Graph Specified";

            Assert.AreEqual(expectedResponse, actualResponse.ResponseString);



        }



        private static IEnumerable<object[]> AuhtorizationCheckData()
        {
            // Instantiating the user identity for the thread.
            UserIdentity adminIdentity = new UserIdentity
            {
                Name = "X USER",
                IsAuthenticated = true,
                AuthenticationType = "JWT"
            };

            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaims adminClaims = claimsFactory.GetClaims(RoleEnumType.SystemAdmin);

            IClaims basicClaims = claimsFactory.GetClaims(RoleEnumType.BasicRole);

            // Instantiation of the admin identity and basic user identity 
            ClaimsIdentity adminClaimsIdentity = new ClaimsIdentity
            (adminIdentity, adminClaims.Claims(), adminIdentity.AuthenticationType, adminIdentity.Name, " ");

            ClaimsIdentity basicClaimsIdentity = new ClaimsIdentity
            (adminIdentity, basicClaims.Claims(), adminIdentity.AuthenticationType, adminIdentity.Name, " ");


            /// here is the expected common response for authorizaed user
            ResponseUAD expectedSuccessUAD = new ResponseUAD()
            {
                ResponseString = "Successful Data retrieval",
                IsSuccessful = true,
                ConnectionState = true,
            };

            /// here is the expected common response for UN authorizaed user
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
        [DynamicData(nameof(AuhtorizationCheckData), DynamicDataSourceType.Method)]
        public void GetAllAnalytics_ValidAndInvalidParmissions_ResponseReturned
            (ClaimsPrincipal principalGenerated, ResponseUAD expectedUAD)
        {
            Thread.CurrentPrincipal = principalGenerated;
            ResponseUAD responseUAD = new ResponseUAD();

            string connection = _conString.GetConnectionStringByName("MyConnection");
            responseUAD = _analyticsDAO.GetGraphData(DBViews.NumberOfAccountTypes);
            Console.WriteLine(responseUAD.ToString());

            Assert.AreEqual(expectedUAD.ToString(), responseUAD.ToString());

        }




        private static IEnumerable<object[]> FORMETHOD()
        {
            return new List<object[]>()
            {
               new object[]{},
            };

        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]

        //[DataRow(6)]
        //[DynamicData(nameof(getPermissionsData), DynamicDataSourceType.Method)]
        public void GetGraphData_ReturnGraphDataAsExpected(int graphType)
        {

            Thread.CurrentPrincipal = _principalGenerated;

            ResponseUAD actualResponse = new ResponseUAD();

            actualResponse = _analyticsDAO.GetGraphData((DBViews)graphType);

            IList<ChartData> GetChartDatas = new List<ChartData>();

            Assert.AreNotEqual(GetChartDatas.Count, actualResponse.GetChartDatas.Count);




        }

    }

}
