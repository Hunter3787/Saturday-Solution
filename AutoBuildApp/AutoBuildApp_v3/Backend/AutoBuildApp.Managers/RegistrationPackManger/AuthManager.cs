using AutoBuildApp.DataAccess;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Security.Models;
using AutoBuildApp.Services.Auth_Services;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

namespace AutoBuildApp.Managers.Registration_PackManger
{
    public class AuthManager
    {
        // this is to access the principleUser on the thread to update its identity and 
        // principle after they are authenticated 


        private AuthenticationService _authenticationService;
        // private AuthDAO _authDAO;


        private LoginDAO _loginDAO;

        public AuthManager(string _cnnctString)
        {
            //Console.WriteLine($"connection string passed: { _cnnctString} ");
             //_authDAO = new AuthDAO(_cnnctString);
            //authenticationService = new AuthenticationService(_authDAO);

            _loginDAO = new LoginDAO(_cnnctString);
            _authenticationService = new AuthenticationService(_loginDAO);


        }

        public string AuthenticateUser(UserCredentials userCredentials)
        {
            var _CRAuth = _authenticationService.AuthenticateUser(userCredentials);
            if (_CRAuth.isAuthenticated)
            {
                //COMMON RESPONSE All THE WAYYY - WHAT I HAVE IS :  THATS BAD -
                return _CRAuth.JWTString;
                // VONG WOULD ALWAYS OVERRIDE IT -> DONT LEAVE IT TO CHANCE !!!! FIX ITTTTT
            }
            else
            {
                //return "Authentication Failed, Username or Password Incorrect";
                return _CRAuth.ResponseString;
            }




        }

    }
}
