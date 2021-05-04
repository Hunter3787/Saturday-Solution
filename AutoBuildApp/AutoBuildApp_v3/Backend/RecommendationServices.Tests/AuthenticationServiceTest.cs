using AutoBuildApp.Models;
using AutoBuildApp.Models.Entities;
using AutoBuildApp.Services.Auth_Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Services.Tests
{
    [TestClass]
    public class AuthenticationServiceTest
    {

        // setting this principle to the thread:

        private static AuthDAO _authDAO = new AuthDAO("Data Source=localhost;Initial Catalog=DB;Integrated Security=True");

        //private static AuthenticationService authenticationService = new AuthenticationService(_authDAO);

        private static IEnumerable<object[]> getNullObjects()
        {
            AuthDAO authDAONull = null;

            return new List<object[]>()
            {
                // here I instatiate objects that carry the secret key, the JWTHeader object and the JWTPayload object
                new object[]{authDAONull, "NULL OBJECT PROVIDED"}
            };
        }

        private static IEnumerable<object[]> getUserCredentialsData()
        {
            // creating users credentials within the system without needing to go to the DB
            UserCredentials credential1 = new UserCredentials("Zeina", "PassHash"); //user exists
            UserCredentials credential2 = new UserCredentials("SamJ", "PassHash"); // user exists
            UserCredentials credential3 = new UserCredentials("Isabel", "PassHash"); // user does not exist 
            UserCredentials credential4 = new UserCredentials("Yasolen", "PassHash"); // user does not exist

            CommonReponseAuth _CRAuthIsAuthenticated = new CommonReponseAuth();
            _CRAuthIsAuthenticated.ResponseString = "User Exists";
            _CRAuthIsAuthenticated.IsUserExists = true;
            _CRAuthIsAuthenticated.connectionState = true;

            _CRAuthIsAuthenticated.ResponseBool = true;
            _CRAuthIsAuthenticated.isAuthenticated = true;

            CommonReponseAuth _CRAuthNotAuthenticated = new CommonReponseAuth();
            _CRAuthNotAuthenticated.ResponseString = "User not found";
            _CRAuthNotAuthenticated.IsUserExists = false;
            _CRAuthNotAuthenticated.connectionState = true;

            _CRAuthNotAuthenticated.ResponseBool = false;
            _CRAuthNotAuthenticated.isAuthenticated = false;



            return new List<object[]>()
            {
                // here I instatiate objects that carry the secret key, the JWTHeader object and the JWTPayload object
                new object[]{credential1,_CRAuthIsAuthenticated},
                new object[]{credential2,_CRAuthIsAuthenticated},
                new object[]{credential3,_CRAuthNotAuthenticated},
                new object[]{credential4,_CRAuthNotAuthenticated}


            };
        }


        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(getUserCredentialsData), DynamicDataSourceType.Method)]
        public void AuthenticationUser_ReturnUSerAuthenticated(UserCredentials user, CommonReponseAuth commonResponseExpected)
        {


            CommonReponseAuth _CRAuth;
        //_CRAuth = authenticationService.AuthenticateUser(user);

            //Assert.AreEqual(commonResponseExpected.Equals(_CRAuth), true);

        }



        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(getNullObjects), DynamicDataSourceType.Method)]
        public void AuthDAONull_Object_ReturnNullException(AuthDAO obj, string expectedParamName)
        {
            try
            {
                //AuthenticationService authenticationService = new AuthenticationService(obj);
            }

            //Assert:
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual(expectedParamName, ex.ParamName);
            }

        }





        private static IEnumerable<object[]> User_Jwt_data()
        {
            // creating users credentials within the system without needing to go to the DB
            UserCredentials credential1 = new UserCredentials("Zeina", "PassHash"); //user exists
            UserCredentials credential2 = new UserCredentials("SamJ", "PassHash"); // user exists
            UserCredentials credential3 = new UserCredentials("Isabel", "PassHash"); // user does not exist 
            UserCredentials credential4 = new UserCredentials("Yasolen", "PassHash"); // user does not exist

            return new List<object[]>()
            {   new object[]{credential1, true},
                new object[]{credential2, true},
                new object[]{credential3, false},
                new object[]{credential4, false}
            };
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(User_Jwt_data), DynamicDataSourceType.Method)]
        public void Authentication_generatingJWT_UserExistCases(UserCredentials user, bool expectedJWTgeneratedBool)
        {

            CommonReponseAuth _CRAuth;
            //_CRAuth = authenticationService.AuthenticateUser(user);
            //bool JWTExists = !(string.IsNullOrWhiteSpace(_CRAuth.JWTString));
            //Assert.AreEqual(expectedJWTgeneratedBool, JWTExists);

        }




    }
}
