using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Managers.Registration_PackManger;
using AutoBuildApp.Security.Models;
using AutoBuildSecure.WebApi.HelperFunctions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

using System.Text.Json;
using System.Threading;


/// <summary>
/// Reference: see /AuthReference.
/// 
/// </summary>
namespace AutoBuildSecure.WebApi.Controllers
{ /// <summary>
  /// Web API as the name suggests, 
  /// is an API over the web which can be accessed using HTTP protocol.
  /// </summary>


    [EnableCors("CorsPolicy")] // applying the cors policy so that the client can
    // make a call the this controller 
    [ApiController]
    [Route("[controller]")] // the route
    public class AuthenticationController : ControllerBase
    {
        #region  Authentication Controller variables 

        private UserPrinciple _userPrinciple;
        private AuthManager _loginManager;
        private UserCredentials _userCredentials;


        #endregion


        public AuthenticationController()
        {
            #region Variable Instantiations 


            _userPrinciple = new UserPrinciple();
            // assign the principle to the thread.
            Thread.CurrentPrincipal = _userPrinciple;

            // created a connection manager to access the connection strings in 
            // 1) the app settings .json file
            ConnectionManager conString = ConnectionManager.connectionManager;
            // 2) passing in the name I assigned my connection string 
            string connection = conString.GetConnectionStringByName("MyConnection");
            // Console.WriteLine($"connection string passed in controller: {connection} ");
            //3) connection string passed to the logIn manager 
            _loginManager = new AuthManager(connection);

            #endregion

        }

        /// <summary>
        /// the 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            // retrieve the bearer token from the Authorization header 
            var accessTokenHeader = Request.Headers[HeaderNames.Authorization];

            // if there is no JWT key assigned to the incomming request
            // then assign a Default user principle object
            if (   string.IsNullOrEmpty(accessTokenHeader) 
                || string.IsNullOrWhiteSpace(accessTokenHeader))
            { 
                _userPrinciple = new UserPrinciple();
            }
            else
            {  
                string accessToken = accessTokenHeader;
                string[] parse = accessToken.Split(' ');
                /// parse and retrieve the access token from the header
                accessToken = parse[1];
                /// calling the JWT Validator to parse and return the
                /// object to be stored as a UserPrinciple
                JWTValidator validateAuthorizationHeader = new JWTValidator(accessToken);

                if (validateAuthorizationHeader.IsValidJWT())
                {
                    // if the checks pass for the JWT then extract the user information
                    _userPrinciple = validateAuthorizationHeader.ParseForUserPrinciple();
                    // and assign it to the current thread!

                }
            }

            // update the current priniciple to accomedate changes
            Thread.CurrentPrincipal = _userPrinciple;

            return 
                Ok($"The header extracted: {accessTokenHeader}" +
              $"\n\nCurrent Thread Priciple: {JsonSerializer.Serialize(_userPrinciple)}");
        }

        [HttpPost("{UserCred}")]
        public ActionResult<AuthUserDTO> AuthenticateUser(UserCredentials userCredentials)
        {
            this._userCredentials= userCredentials;
            var accessTokenHeader = Request.Headers[HeaderNames.Authorization];
            var JWTToken =  _loginManager.AuthenticateUser(_userCredentials);
            return Ok(JWTToken);
        }
    }
}
