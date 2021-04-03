using AutoBuildApp.Security.Models;
using AutoBuildApp.Services.Auth_Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AutoBuildApp.Services.Tests
{
    public class AuthorizationServiceTest
    {

        public static IList<Claims> ClaimsRequiredForUnregistered =
            ClaimsPerRoles.claimsPerUnregistered;

        AuthorizationService authorizationService;

        public static UserPrinciple userPrincipleCustom = new UserPrinciple();


        private static IEnumerable<object[]> getClaimsRequired()
        {
            Thread.CurrentPrincipal = userPrincipleCustom;

            return new List<object[]>()
            {
                 new object[]{ ClaimsRequiredForUnregistered,true}

            };
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(getClaimsRequired), DynamicDataSourceType.Method)]
        public void checkPermissions_ReturnsBool
            (IList<Claims> claimsRequired, bool isAuthorizedExpected)
        {
            authorizationService = new AuthorizationService(claimsRequired);

            bool isAuthorizedActual =
                authorizationService.checkPermissions();

            Assert.AreEqual(isAuthorizedExpected, isAuthorizedActual);
        }



    }
}
