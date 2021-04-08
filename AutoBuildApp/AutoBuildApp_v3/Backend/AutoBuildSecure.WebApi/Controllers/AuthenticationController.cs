using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Managers.Registration_PackManger;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using AutoBuildSecure.WebApi.HelperFunctions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Security.Claims;
using System.Security.Principal;
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

        private AuthManager _loginManager;
        private UserCredentials _userCredentials;
        private ClaimsIdentity _claimsIdentity;
        private ClaimsPrincipal _principal;
        #endregion

        public AuthenticationController()
        {

            // setting a default principle object t=for the thread.
            #region Instantiating the Claims principle
            IIdentity userIdentity = new UserIdentity();
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaimsFactory unregistered = claimsFactory.GetClaims(RoleEnumType.UNREGISTERED_ROLE);
            _claimsIdentity = new ClaimsIdentity(userIdentity, unregistered.Claims());
            Console.WriteLine($" Identity name: {_claimsIdentity.Name} {_claimsIdentity.IsAuthenticated} ");
            _principal = new ClaimsPrincipal(_claimsIdentity);

            // set it to th current thread
            Thread.CurrentPrincipal = _principal;
            #endregion


            #region getting the connection string and passing to the loginmanager
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
        public IActionResult GetJWTToken()
        {
            // retrieve the bearer token from the Authorization header 
            var accessTokenHeader = Request.Headers[HeaderNames.Authorization];

            // if there is no JWT key assigned to the incomming request
            // then assign a Default user principle object
            if (string.IsNullOrEmpty(accessTokenHeader)
                || string.IsNullOrWhiteSpace(accessTokenHeader))
            {
                ClaimsPrincipal principal = new ClaimsPrincipal(_claimsIdentity);
                // set it to th current thread
                Thread.CurrentPrincipal = principal;
                foreach (var clm in _principal.Claims)
                {
                    Console.WriteLine($" claim type: { clm.Type } claim value: {clm.Value} ");
                }

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
                    ClaimsPrincipal modified = validateAuthorizationHeader.ParseForClaimsPrinciple();
                    // and assign it to the current thread!

                    _principal = modified;
                    Thread.CurrentPrincipal = _principal;

                    Console.WriteLine($"In the authentication controller" +
                        $" identity name: {Thread.CurrentPrincipal.Identity.Name} ");

                    foreach (var clm in _principal.Claims)
                    {
                        Console.WriteLine($" claim type: { clm.Type } claim value: {clm.Value} ");
                    }

                }
            }

            return
                Ok($"The header extracted: {accessTokenHeader}" +
              $"\n\nCurrent Thread Priciple: {JsonSerializer.Serialize(Thread.CurrentPrincipal)}");
        }

        [HttpPost("{UserCred}")]
        public ActionResult<AuthUserDTO> AuthenticateUser(UserCredentials userCredentials)
        {
            this._userCredentials = userCredentials;
            var JWTToken = _loginManager.AuthenticateUser(_userCredentials);
            // along the call the defaukt principle should be updated

            return Ok(JWTToken);
        }
    }
}
