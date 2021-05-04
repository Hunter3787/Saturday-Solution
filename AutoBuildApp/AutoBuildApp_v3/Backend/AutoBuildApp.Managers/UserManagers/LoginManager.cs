using System;
using AutoBuildApp.DataAccess;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Models.Users;
using AutoBuildApp.Services;
using AutoBuildApp.Services.Auth_Services;

namespace AutoBuildApp.Managers
{
    public class LoginManager
    {
        private LoginDAO _loginDAO;
        private AuthenticationService _authenticationService;

        public LoginManager(String CnnctString)
        {
            // establish a connection to DB

            _loginDAO = new LoginDAO(CnnctString);
            _authenticationService = new AuthenticationService(_loginDAO);
        }


        //public String LoginUser(string username, string password)
        //{
        //    return _LoginDAO.LoginInformation();
        //}

        public string LoginUser(UserCredentials userCredentials)
        {
            var _CRAuth = _authenticationService.AuthenticateUser(userCredentials);
            if (_CRAuth.isAuthenticated)
            {
                //COMMON RESPONSE ALL THE WAYYY - WHAT I HAVE IS :  THATS BAD -
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
