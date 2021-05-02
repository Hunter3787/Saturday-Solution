using AutoBuildApp.DataAccess;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using AutoBuildApp.Services.Auth_Services;
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

                  new Claim( PermissionEnumType.READ_ONLY  , ScopeEnumType.AUTOBUILD),
                  new Claim( PermissionEnumType.DELETE     , ScopeEnumType.SELF),
                  new Claim( PermissionEnumType.UPDATE     , ScopeEnumType.SELF),
                  new Claim( PermissionEnumType.EDIT       , ScopeEnumType.SELF),
                  new Claim( PermissionEnumType.CREATE     , ScopeEnumType.REVIEWS),
                  new Claim( PermissionEnumType.DELETE     , ScopeEnumType.SELF_REVIEWS),
                  new Claim( PermissionEnumType.UPDATE     , ScopeEnumType.SELF_REVIEWS),


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
           bool AuthActual =  AuthorizationService.CheckPermissions(permissionsRequired);
           bool AuthExpected = true;

            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            UserCredentials credential1 = new UserCredentials("Zeina", "PassHash");
            CommonReponseAuth _CRAuth = new CommonReponseAuth();
            AuthDAO _authDAO = new AuthDAO("Data Source=localhost;Initial Catalog=DB;Integrated Security=True");
            AuthenticationService authenticationService = new AuthenticationService(_authDAO);
            _CRAuth = authenticationService.AuthenticateUser(credential1);
            IClaims basic = claimsFactory.GetClaims(RoleEnumType.BASIC_ROLE);

            AuthActual = AuthorizationService.CheckPermissions(basic.Claims());


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

            bool AuthActual = AuthorizationService.CheckPermissions(null);
            bool AuthExpected = false;

            Assert.AreEqual(AuthExpected, AuthActual);

        }



    }
}
