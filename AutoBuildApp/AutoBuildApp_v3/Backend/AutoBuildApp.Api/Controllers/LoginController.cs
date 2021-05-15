using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Managers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AutoBuildApp.Api.Controllers
{
    [EnableCors("CorsPolicy")] // applying the cors policy so that the client can
    // make a call the this controller 
    [ApiController]
    [Route("[controller]")] // the route
    public class LoginController : Controller
    {

        #region  Authentication Controller variables 

        private LoginManager _loginManager;
        private UserCredentials _userCredentials;
        #endregion


        public LoginController()
        {
            #region getting the connection string and passing to the loginmanager
            // created a connection manager to access the connection strings in 
            // 1) the app settings .json file
            ConnectionManager conString = ConnectionManager.connectionManager;
            // 2) passing in the name I assigned my connection string 
            string connection = conString.GetConnectionStringByName(ControllerGlobals.ADMIN_CREDENTIALS_CONNECTION);
            // Console.WriteLine($"connection string passed in controller: {connection} ");
            //3) connection string passed to the logIn manager 
            _loginManager = new LoginManager(connection);
            #endregion

        }
        [HttpPost]
        public ActionResult<AuthUserDTO> AuthenticateUser(UserCredentials userCredentials)
        {
            this._userCredentials = userCredentials;
            var JWTToken = _loginManager.LoginUser(_userCredentials);
            return Ok(JWTToken);
        }
    }
}
