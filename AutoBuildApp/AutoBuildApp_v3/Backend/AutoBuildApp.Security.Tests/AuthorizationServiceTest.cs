using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;


namespace AutoBuildApp.Security.Tests
{
    /// <summary>
    /// the authorization service unit test
    /// here I will testing on the following:
    /// 1) setting the thread with a claimsprinicple and 
    /// testing the ones passed if they match 
    /// 
    /// </summary>
    [TestClass]
    public class AuthorizationServiceTest
    {

        private static IEnumerable<Claim> claims = new List<Claim>() {

                new Claim(PermissionEnumType.READ_ONLY,ScopeEnumType.AUTOBUILD),

            };

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(GetClaimsRequired), DynamicDataSourceType.Method)]
        public void CheckPermissions_ReturnsBool(IEnumerable<Claim> permissionsRequired)
        {
            /// setting up:
            // instantiating claimsIdentity and principle.
            ClaimsIdentity claimsIdentity;
            ClaimsPrincipal claimsPrincipal;
            UserIdentity _defaultuserIdentity = new UserIdentity();

            claimsIdentity = new ClaimsIdentity
                (claims, _defaultuserIdentity.AuthenticationType,
                _defaultuserIdentity.Name, " ");


            claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            Thread.CurrentPrincipal = claimsPrincipal;
            AuthorizationService.print();
           bool AuthActual =  AuthorizationService.checkPermissions(permissionsRequired);
           bool AuthExpected = true;


           Assert.AreEqual(AuthExpected, AuthActual);
        }

        private static IEnumerable<object[]> GetClaimsRequired()
        {
            

            return new List<object[]>()
            {
                new object[]{claims}
            };


        }
        [TestMethod]

        public void CheckNullPermissions_ReturnsBool()
        {

            bool AuthActual = AuthorizationService.checkPermissions(null);
            bool AuthExpected = false;

            Assert.AreEqual(AuthExpected, AuthActual);

        }



    }
}
