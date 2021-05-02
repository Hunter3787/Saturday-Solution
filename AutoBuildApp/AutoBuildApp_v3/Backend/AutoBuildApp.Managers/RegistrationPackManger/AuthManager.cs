//using AutoBuildApp.DataAccess;
//using AutoBuildApp.DataAccess.Entities;
//using AutoBuildApp.Security.Models;
//using AutoBuildApp.Services.Auth_Services;
//using System;
//using System.Security.Claims;
//using System.Security.Principal;
//using System.Threading;

//namespace AutoBuildApp.Managers.Registration_PackManger
//{
//    public class AuthManager
//    {
//        // this is to access the principleUser on the thread to update its identity and 
//        // principle after they are authenticated 


//        private AuthenticationService authenticationService;
//        private AuthDAO _authDAO;


//        public AuthManager(string _cnnctString)
//        {
//            Console.WriteLine($"connection string passed: { _cnnctString} ");
//            _authDAO = new AuthDAO(_cnnctString);
//            //authenticationService = new AuthenticationService(_authDAO);

//        }

//        public string AuthenticateUser(UserCredentials userCredentials)
//        {

//            //var _CRAuth = authenticationService.AuthenticateUser(userCredentials);
//            if (_CRAuth.isAuthenticated)
//            {
//                //COMMON RESPONSE ALL THE WAYYY - WHAT I HAVE IS :  THATS BAD -
//                return _CRAuth.JWTString;
//                // VONG WOULD ALWAYS OVERRIDE IT -> DONT LEAVE IT TO CHANCE !!!! FIX ITTTTT
//            }
//            else
//            {
//                //return "Authentication Failed, Username or Password Incorrect";
//                return _CRAuth.FailureString;
//            }



//        }

//    }
//}
