using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Models;
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


        ConnectionManager conString = ConnectionManager.connectionManager;
        
        // 2) passing in the name I assigned my connection string 
        private static IEnumerable<object[]> FORMETHOD()
        {
            return new List<object[]>()
            {
               new object[]{},
            };

        }



        private static IEnumerable<object[]> NonAdminPricipleData()
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
               //new object[]{ new ClaimsPrincipal(adminClaimsIdentity),expectedSuccessUAD },
               new object[]{ new ClaimsPrincipal(basicClaimsIdentity) },
            };

        }



        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(NonAdminPricipleData), DynamicDataSourceType.Method)]
        public void GetAllChartData_NOT_AUTHORIZED_Returned
          (ClaimsPrincipal principalGenerated)
        {
            Thread.CurrentPrincipal = principalGenerated;
            ResponseUAD responseUAD = new ResponseUAD();

            string connection = conString.GetConnectionStringByName("MyConnection");

            UADManager _uadManager = new UADManager(connection);

           // string actual = _uadManager.GetAllChartData();
           
            string expected = AuthorizationResultType.NOT_AUTHORIZED.ToString();

           // Assert.AreEqual(expected, actual);

        }



        private static IEnumerable<object[]> AdminPricipleData()
        {
            UserIdentity AdminIdentity = new UserIdentity
            {
                Name = "ADMIN USER",
                IsAuthenticated = true,
                AuthenticationType = "JWT"
            };

            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaims adminClaims = claimsFactory.GetClaims(RoleEnumType.SYSTEM_ADMIN);

            ClaimsIdentity adminClaimsIdentity = new ClaimsIdentity
            (AdminIdentity, adminClaims.Claims(), AdminIdentity.AuthenticationType, AdminIdentity.Name, " ");

            ResponseUAD expectedResponse = new ResponseUAD()
            {
                ResponseBool =  true,
                ConnectionState = true,
                IsAuthorized = true
            };
            return new List<object[]>()
            {
               new object[]{ new ClaimsPrincipal(adminClaimsIdentity),expectedResponse }
            };

        }



        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(AdminPricipleData), DynamicDataSourceType.Method)]
        public void GetAllChartData_IList_Returned
       (ClaimsPrincipal principalGenerated, ResponseUAD expectedResponse)
        {
            Thread.CurrentPrincipal = principalGenerated;
            ResponseUAD responseUAD = new ResponseUAD();

            string connection = conString.GetConnectionStringByName("MyConnection");

            UADManager _uadManager = new UADManager(connection);

            var result = _uadManager.GetAllChartData();


            Assert.IsNotNull(result);

        }












    }
}
