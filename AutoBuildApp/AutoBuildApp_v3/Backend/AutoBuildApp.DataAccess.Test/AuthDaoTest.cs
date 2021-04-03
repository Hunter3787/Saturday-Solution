using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoBuildApp.DataAccess.Entities;

namespace AutoBuildApp.DataAccess.Test
{
    [TestClass]
    public class AuthDaoTest
    {
        private static AuthDAO _authDAO = new AuthDAO("Data Source=localhost;Initial Catalog=DB;Integrated Security=True");
        //this is the valid connection string
        /// this is the common respone object for authentication
        private static CommonReponseAuth _CRAuth = new CommonReponseAuth();


        private static IEnumerable<object[]> getcheckConnectionData()
        {
            AuthDAO authDAO = new AuthDAO(); // testing connection state true:
            SqlConnection conValid = new SqlConnection("Data Source=localhost;Initial Catalog=DB;Integrated Security=True");



            CommonReponseAuth ExpectedReponseVALID =
                new CommonReponseAuth();
            ExpectedReponseVALID.connectionState = true;


            CommonReponseAuth ExpectedReponseNULL =
                new CommonReponseAuth();
            ExpectedReponseNULL.SuccessBool = false;
            ExpectedReponseNULL.FailureString = "NULL EXCEPTION";


            return new List<object[]>()
            {
                // here I instatiate objects that carry the secret key, the JWTHeader object and the JWTPayload object
               new object[]{ authDAO ,conValid, ExpectedReponseVALID },
               new object[]{ authDAO , null, ExpectedReponseNULL}
            };

        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(getcheckConnectionData), DynamicDataSourceType.Method)]
        public void AuthDAO_checkConnection_CheckConnection
            (AuthDAO authDAO, SqlConnection sqlConnection, CommonReponseAuth ExpectedReponseAuth)
        {

            _CRAuth = (CommonReponseAuth)authDAO.CheckConnection(sqlConnection);
            bool actualBool = _CRAuth.Equals(ExpectedReponseAuth);
            //Assert:
            Assert.AreEqual(true, actualBool);

        }

        private static IEnumerable<object[]> getUserCredentialsData()
        {
            // creating users credentials within the system without needing to go to the DB
            UserCredentials credential1 = new UserCredentials("Zeina", "PassHash"); // exists
            UserCredentials credential2 = new UserCredentials("SamJ", "PassHash"); // exists
            UserCredentials credential3 = new UserCredentials("Isabel", "PassHash"); // does not exist
            UserCredentials credential4 = new UserCredentials("Yasolen", "PassHash"); // does not exist

            CommonReponseAuth _CRAuthIsAuthenticated = new CommonReponseAuth();
            _CRAuthIsAuthenticated.SuccessString = "User Exists";
            _CRAuthIsAuthenticated.IsUserExists = true;
            _CRAuthIsAuthenticated.connectionState = true;

            CommonReponseAuth _CRAuthNotAuthenticated = new CommonReponseAuth();
            _CRAuthNotAuthenticated.FailureString = "User not found";
            _CRAuthNotAuthenticated.IsUserExists = false;
            _CRAuthNotAuthenticated.connectionState = true;

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
        public void AuthDAO_RetrieveUserInformation_ReturnCorrectCommonResponse(UserCredentials user, CommonReponseAuth commonResponseExpected)
        {
            _CRAuth = (CommonReponseAuth)_authDAO.RetrieveUserInformation(user);

            Assert.AreEqual(commonResponseExpected.Equals(_CRAuth), true);

        }


        private static IEnumerable<object[]> getNullPayload()
        {
            AuthDAO authDAONull = null;
            AuthDAO authDAONULLParam = new AuthDAO(null);
            AuthDAO authDAOEmptyString = new AuthDAO("    ");
            return new List<object[]>()
            {
               new object[]{authDAONull, "NULL OBJECT PROVIDED"},
               new object[]{ authDAONULLParam , "NULL OBJECT PROVIDED"},
               new object[]{ authDAONULLParam , "NULL OBJECT PROVIDED"}
            };
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(getNullPayload), DynamicDataSourceType.Method)]
        public void AuthDAO_Null_Object_ReturnNullException(AuthDAO obj, string expectedParamName)
        {
            try
            {
                AuthDAO authDAO = (AuthDAO)obj;
            }
            //Assert:
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual(expectedParamName, ex.ParamName);
            }

        }



        



    }
}
