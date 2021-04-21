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
    public class UmDaoTest
    {
        // attributes for initaliz and remove 


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

            IClaims basic = claimsFactory.GetClaims(RoleEnumType.BASIC_ROLE);
            IClaims admin = claimsFactory.GetClaims(RoleEnumType.BASIC_ADMIN);




            return new List<object[]>()
            {
                // here I instatiate objects that carry the secret key, the JWTHeader object and the JWTPayload object
               new object[]{basic.Claims()}, 
               //new object[]{admin.Claims()}
            };

        }

        [TestMethod]
        [DataTestMethod]
        //[DynamicData(nameof(getPermissionsData), DynamicDataSourceType.Method)]
        public void UpdateUserPermisions_RowsAffected()
        {

            IClaims basic = claimsFactory.GetClaims(RoleEnumType.BASIC_ROLE);
            Console.WriteLine(connection);
            string username = "NEW_USERNAME1";
            UserManagementDAO UMDAO = new UserManagementDAO(connection);
            string result = "";


            result = UMDAO.ChangePermissionsDB(username, basic.Claims());

            Assert.AreEqual("permissions have been successfully updated", result);

            // have a rollback
        }

    }
}