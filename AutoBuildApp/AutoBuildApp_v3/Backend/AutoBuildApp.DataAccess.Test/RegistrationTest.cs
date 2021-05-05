using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace AutoBuildApp.DataAccess.Test
{

    [TestClass]
    public class RegistrationTest
    {

        // created a connection manager to access the connection strings in 
        // 1) the app settings .json file
        ConnectionManager conString = ConnectionManager.connectionManager;
        // 2) passing in the name I assigned my connection string 
        static string connection = ConnectionManager
            .connectionManager
            .GetConnectionStringByName("MyConnection");


        static ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
        private static IEnumerable<object[]> getPermissionsData()
        {

            IClaims basic = claimsFactory.GetClaims(RoleEnumType.BasicRole);



            return new List<object[]>()
            {
                // here I instatiate objects that carry the secret key, the JWTHeader object and the JWTPayload object
               new object[]{basic.Claims()}
            };

        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(getPermissionsData), DynamicDataSourceType.Method)]
        public void UpdateUserPermisions_RowsAffected(IEnumerable<Claim> claims)
        {

            RegistrationDAO regDAO = new RegistrationDAO(connection);
            int result = 0;


            //result = regDAO.updatePermissions(claims);

            Assert.AreEqual(1, result);
        }














    }
}
