using Microsoft.IdentityModel.Tokens;
using Project.DataAccess;
using Project.DataAccess.Entities;
using Project.DomainModels;
using Project.Security.Models;
using Project.Services;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace Project.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            CommonReponseAuth CRAuth = new CommonReponseAuth();
            UserCredentials _userCredentials = new UserCredentials("Zeina", "PassHash");

            GetConnectionStringByName("Connection");
            AuthDAO _authDAO = new AuthDAO(GetConnectionStringByName("Connection"));
            Console.WriteLine("connection state: " + _authDAO.tryConnection());

            AuthenticationService _AuthService = new AuthenticationService(_authDAO);

            Console.WriteLine($"using business object in client: {_AuthService.AuthenticateUser(_userCredentials)}");


            // this is done 




            //-----------------------checking claims identity and principle identity to understand:

            // so what will we be doing is passing in a username and password and returning list of
            // permissiions for that user 
            // storing username into the identity 
            // and list of permissions into the principle object

            //this will be done during the authentication phase

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


        public static void JWTDemo()
        {

            //SimpleLogServiceForDemo simple = new SimpleLogServiceForDemo();
            //testingTheDAOfromBL dAOfromBL = new testingTheDAOfromBL();
            // dAOfromBL.getNumberOfUsers();
            //simple.Log("this is test on 03/11!!!!", Project.DataAccess.Models.LogLevel.Warning);
            //testing out the JWT generator:

            JWTHeader header = new JWTHeader();
            Console.WriteLine(header.ToString());

            string jsonString = JsonSerializer.Serialize(header);

            Console.WriteLine("Header\n" + jsonString);


            JWTPayload payload = new JWTPayload()
            {
                Iss = "25374",
                UserName = "zee",
                Aud = "zee",
                Subject = "Autobuild",

                iat = "23333",
                exp = "23456",
                nbf = "39485",

                UserCLaims = new UserPermissions[]
                {
                    new UserPermissions("create", "all"),
                    new UserPermissions("delete", "Any")
                }
            };


            jsonString = JsonSerializer.Serialize(payload);

            Console.WriteLine("PayLoad\n" + jsonString);

            JWT jwtGenerator = new JWT("test", payload, header);


            Console.WriteLine(jwtGenerator.GenerateJWTSignature());
            Console.WriteLine(jwtGenerator.ToString());

            Console.WriteLine("Base64URL Header: \n" + jwtGenerator.Base64Object(header));
            Console.WriteLine("Base64URL payload: \n" + jwtGenerator.Base64Object(payload));

            var handler = new JwtSecurityTokenHandler();
            

           // Console.WriteLine($"DECODED VALUE: {decodedValue}");

            ////    string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SneQiuAGUW9aTpxlNNbMkEoYNj7v4-Sw_5jl13-hosk";
            ////    token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.XbPfbIHMI6arZ3Y922BhjWgQzWXcXNrz0ogtVhfEd2o";
            ////    var handler = new JwtSecurityTokenHandler();


            ////    var decodedValue = handler.ReadJwtToken(token);
            //// ASCIIEncoding encoding = new ASCIIEncoding();

            ////string secret = "XbPfbIHMI6arZ3Y922BhjWgQzWXcXNrz0ogtVhfEd2o";



        }
    }
}

