using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Project.DataAccess;
using Project.DataAccess.Entities;
using Project.Manager;
using Project.Security.Models;
using Project.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Project.Client
{
    class Program
    {
        static void Main(string[] args)
        {

            UserCredentials userCredentials = new UserCredentials();


            Console.WriteLine($"JSON OF USER CREDENTIALS    {JsonSerializer.Serialize(userCredentials)} ");



          Console.WriteLine($"checking      { string.IsNullOrEmpty(" ") && string.IsNullOrWhiteSpace(" ") }");
/*
            Console.WriteLine("ASSUMING START OF THE APPLICATION: ");

            Console.WriteLine("mimicing the Business Object");
            
            IList<Claims> claimsRequired = new List<Claims>()
            {
                new Claims("Read Only","AutoBuild")
            };

            UserPrinciple userPrincipleCustom = new UserPrinciple();

            // setting this principle to the thread:

            Thread.CurrentPrincipal = userPrincipleCustom;


            ClaimsPerRoles ADMIN = ClaimsPerRoles.AdminUserClaims;

            // instantiate the Authorization service:

            AuthorizationService authorizationService
                = new AuthorizationService(claimsRequired);



            int c = 2;
            Console.WriteLine($"count permissions : {c}" +
                $"/n {userPrincipleCustom.getPermissions()}");

            Console.WriteLine($"The check permissions returned:\n " +
                $"{ authorizationService.checkPermissions()}\n");

            */



            AuthDAO authDAO1 = new AuthDAO();

            // instantiating the SQL connecttion 
            string sqlCon = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            Console.WriteLine("This is the connection string name:ConnectionString1");

            Console.WriteLine(" SQLCON  " + sqlCon);
            SqlConnection con = new SqlConnection(sqlCon);


            //("Data Source=localhost;Initial Catalog=DB;Integrated Security=True" 

            CommonReponseAuth ExpectedReponseNULL =
                new CommonReponseAuth();
            ExpectedReponseNULL.SuccessBool = false;
            ExpectedReponseNULL.FailureString = "NULL EXCEPTION";

            CommonReponseAuth commonResponseActual = new CommonReponseAuth();
            //Act:
            commonResponseActual = (CommonReponseAuth)authDAO1.CheckConnection(null);

            Console.WriteLine($"PART 1 NULL Common response expected: {commonResponseActual.ToString() } \n" +
                $"   common repsonse returned: { ExpectedReponseNULL.ToString() } " +
                $"\n\nEQUALS METHOD  { commonResponseActual.Equals(ExpectedReponseNULL )}");


            CommonReponseAuth ExpectedReponseVALID =
                new CommonReponseAuth();
            ExpectedReponseVALID.connectionState = true;
            Console.WriteLine($"\n ExpectedReponseVALID : {ExpectedReponseVALID.ToString() } \n");

          commonResponseActual = (CommonReponseAuth)authDAO1.CheckConnection(con);


            Console.WriteLine($" PART 2 VALID \nCommon response expected: {commonResponseActual.ToString() } \n" +
                $"   common repsonse returned: { ExpectedReponseVALID.ToString() } " +
                $"\n\nEQUALS METHOD  { commonResponseActual.Equals(ExpectedReponseVALID)}");



            Console.WriteLine($"---------------------------------------");

           UserCredentials credential1 = new UserCredentials("Zeina", "PassHash"); // exists
            UserCredentials credential2 = new UserCredentials("SamJ", "PassHash"); // exists

            CommonReponseAuth _CRAuthIsAuthenticated = new CommonReponseAuth();
            _CRAuthIsAuthenticated.SuccessString = "User Exists";
            _CRAuthIsAuthenticated.IsUserExists = true;
            _CRAuthIsAuthenticated.connectionState = true;

            CommonReponseAuth _CRAuthNotAuthenticated = new CommonReponseAuth();
            _CRAuthNotAuthenticated.FailureString = "User not found";
            _CRAuthNotAuthenticated.IsUserExists = false;
            _CRAuthNotAuthenticated.connectionState = true;

            AuthDAO _authDAO = new AuthDAO("Data Source=localhost;Initial Catalog=DB;Integrated Security=True");

            // commonResponseActual = (CommonReponseAuth)_authDAO.RetrieveUserInformation(credential1);


            // Console.WriteLine($" PART 1 VALIDate user \nCommon response expected: {_CRAuthIsAuthenticated.ToString() } \n" +
            //$"   common repsonse returned: { commonResponseActual.ToString() } " +
            //$"\n\nEQUALS METHOD  { commonResponseActual.Equals(_CRAuthIsAuthenticated)}");




            Console.WriteLine($"-------------- AUTHENTICATION CHECK -------------------");

            //UserPrinciple userPrincipleCustom = new UserPrinciple();

            // setting this principle to the thread:

            //Thread.CurrentPrincipal = userPrincipleCustom;

            AuthenticationService authenticationService = new AuthenticationService(_authDAO);


            _CRAuthIsAuthenticated.SuccessBool = true;
            _CRAuthIsAuthenticated.isAuthenticated = true;


            _CRAuthNotAuthenticated.isAuthenticated = false;
            _CRAuthNotAuthenticated.SuccessBool = false;

            commonResponseActual  = (CommonReponseAuth)authenticationService.AuthenticateUser(credential1);

         
            Console.WriteLine($" PART 1 VALIDate user AUTH \nCommon response expected: {_CRAuthIsAuthenticated.ToString() } \n" +
        $"   common repsonse returned AUTH: { commonResponseActual.ToString() } " +
        $"\n\nEQUALS METHOD  { commonResponseActual.Equals(_CRAuthIsAuthenticated)}");





            //UserIdentity customIdentity = new UserIdentity
            //{
            //    AuthenticationType = "Autobuild Authentication",
            //    IsAuthenticated = false,
            //    Name = "Unregistered"

            //};

            //UserPrinciple customPrinciple = new UserPrinciple(customIdentity)
            //{
            //    Permissions = claimsRequired
            //};
            //testing test2 = new testing();
            //test2.checkcustomPrinciple();






            IList<Claim> claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Andras")
                , new Claim(ClaimTypes.Country, "Sweden")
                , new Claim(ClaimTypes.Gender, "M")
                , new Claim(ClaimTypes.Surname, "Nemes")
                , new Claim(ClaimTypes.Email, "hello@me.com")
                , new Claim(ClaimTypes.Role, "IT")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claim, "autobuild", ClaimTypes.Email, ClaimTypes.Role);
            //Console.WriteLine(claimsIdentity.IsAuthenticated);
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            Thread.CurrentPrincipal = principal;

            testing test = new testing();










            #region this is for retrieve permission tests
            //////CommonReponseAuth CRAuth = new CommonReponseAuth();
            //////UserCredentials credential1 = new UserCredentials("Zeina", "PassHash");
            //////UserCredentials credential2 = new UserCredentials("SamJ", "PassHash");
            //////UserCredentials credential3 = new UserCredentials("Isabel", "PassHash");
            //////UserCredentials credential4 = new UserCredentials("Yasolen", "PassHash");

            //////GetConnectionStringByName("Connection");
            //////AuthDAO _authDAO = new AuthDAO(GetConnectionStringByName("Connection"));
            //////Console.WriteLine("connection state: " + _authDAO.tryConnection());

            //////AuthenticationService _AuthService = new AuthenticationService(_authDAO);

            //////var commonResponse = (CommonReponseAuth)_authDAO.RetrieveUserPermissions(credential3);

            //////CommonReponseAuth _CRAuthNotAuthenticated = new CommonReponseAuth();
            //////_CRAuthNotAuthenticated.FailureString = "User not found";
            //////_CRAuthNotAuthenticated.IsUserExists = false;
            //////_CRAuthNotAuthenticated.connectionState = true;


            //////Console.WriteLine($"\n\nUSER 3 ACTUAL OUTCOME:\n {commonResponse.ToString()}\n");

            //////Console.WriteLine($"Expected: {_CRAuthNotAuthenticated.ToString()}");
            //////Console.WriteLine($"USER 3 CHECK in main !!!: {string.Equals(commonResponse.ToString(), _CRAuthNotAuthenticated.ToString())}");

            //////AuthDAO authDAO =  new AuthDAO(GetConnectionStringByName("Connection"));


            //////CommonReponseAuth commonResponseActual = new CommonReponseAuth();

            //////commonResponseActual = (CommonReponseAuth)authDAO.CheckConnection(null);
            //////Console.WriteLine(commonResponseActual.ToString());

            //////SqlConnection con = new SqlConnection(GetConnectionStringByName("Connection"));
            //////commonResponseActual  = (CommonReponseAuth)authDAO.tryConnection();

            //////Console.WriteLine(commonResponseActual.ToString());


            //////commonResponseActual = (CommonReponseAuth)authDAO.CheckConnection(con);
            // Console.WriteLine(commonResponseActual.ToString());

            #endregion
            #region testing the signature 

            IList<Claims> claimPlaceHolder = new List<Claims>
            {
                new Claims("none", "no scope")
            };

        JWTHeader _headerDefault = new JWTHeader();

            //JWTPayload _payloadPlaceHolder = new JWTPayload
            //    ("Autobuild User", "Autobuild", "US",
            //    "Email",
            //    DateTimeOffset.UtcNow.AddDays(7),
            //    DateTimeOffset.UtcNow.AddDays(7),
            //    DateTimeOffset.UtcNow)
            //{
            //    UserCLaims = new List<Claims>
            //    {
            //        new Claims("none", "no scope")
            //    }

            //};

            JWTPayload _payloadOne = new JWTPayload
                ("Autobuild User", "Autobuild", "US",
                "Email", (long)1617759065, (long)1617759065, (long)1617154265)
            {
                UserCLaims = new List<Claims>
            {
                new Claims("none", "no scope")
            }

            };


        JWTPayload _payloadEmpty = new JWTPayload();


        JWT JWT = new JWT("Secret", _payloadEmpty, _headerDefault);
            JWT.GenerateJWTSignature();
            Console.WriteLine(JWT.ToString());


        #endregion


        // checking the for jwt if it is valid:


        //Console.WriteLine($"validating the JWT token PART:");

        //string JWTValidToken =
        //    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJBdXRvYnVpbGQiLCJzdWIiOiJBdXRvYnVpbGQgVXNlciIsImF1ZCI6IlVTIiwiaWF0IjoxNjE3MDY4MjkwLCJleHAiOjE2MTc2NzMwOTAsIm5iZiI6MTYxNzY3MzA5MCwiVXNlcm5hbWUiOiJaZWluYSIsIlVzZXJDTGFpbXMiOlt7IlBlcm1pc3Npb24iOiJCTE9DSyIsInNjb3BlT2ZQZXJtaXNzaW9ucyI6IkFOWSBVU0VSIn0seyJQZXJtaXNzaW9uIjoiQ1JFQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiQU5ZIFVTRVIifSx7IlBlcm1pc3Npb24iOiJDUkVBVEUiLCJzY29wZU9mUGVybWlzc2lvbnMiOiJQUk9EVUNUIn0seyJQZXJtaXNzaW9uIjoiREVMRVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiQU5ZIFVTRVJTIn0seyJQZXJtaXNzaW9uIjoiVVBEQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiQU5ZIFVTRVJTIn0seyJQZXJtaXNzaW9uIjoiVVBEQVRFIiwic2NvcGVPZlBlcm1pc3Npb25zIjoiUFJPRFVDVFMifV19.FJXd9LOb793ERughSZP16kPjzWxkhV9Us_gblqB5yhU";
        //// will do the nnecessary checks:
        //JWTValidator validateJWTToken = new JWTValidator(JWTValidToken);


        //bool IsValidJWT = validateJWTToken.IsValidJWT();
        //Console.WriteLine($"JWT validator result : {IsValidJWT}");




        //-------------------


            // AuthorizationService _authorizationService = new AuthorizationService(claimsRequired);

           

            //ClaimsPrincipal currentClaimsPrincipal = 
            //   Thread.CurrentPrincipal as ClaimsPrincipal;
            //Claim nameClaim = currentClaimsPrincipal.FindFirst(ClaimTypes.Name);
            //Console.WriteLine(nameClaim.Value);

            //foreach (ClaimsIdentity ci in currentClaimsPrincipal.Identities)
            //{
            //    Console.WriteLine(ci.Name);
            //}

        }
        static string GetConnectionStringByName(string name)
        {
            string retVal = null;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            // If found, return the connection string.
            if (settings != null)
                retVal = settings.ConnectionString;
            Console.WriteLine("This is the connection string: " + retVal);

            return retVal;
        }


    }
}

