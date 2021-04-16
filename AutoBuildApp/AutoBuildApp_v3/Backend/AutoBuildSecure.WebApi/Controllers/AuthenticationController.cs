using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Managers.Registration_PackManger;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using AutoBuildApp.Api.HelperFunctions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using System.Threading;


/// <summary>
/// Reference: see /AuthReference.
/// 
/// </summary>
namespace AutoBuildApp.Api.Controllers
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
        #endregion

        public AuthenticationController()
        {
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
        /// this is for testing 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPrinciple()
        {
            //https://stackoverflow.com/questions/47513674/user-identity-name-is-null-when-integrating-asp-net-identity-with-owin-auth-to-a 
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            // ClaimsPrincipal.Current.FindFirst(ClaimsPrincipal.Current.Identities.First().NameClaimType)
            Console.WriteLine("XChecking in the authentication service\n");

            return
                Ok($"\n\nCurrent Thread Priciple: {JsonSerializer.Serialize(Thread.CurrentPrincipal)}/n" +
                $"Checking name per nick: { _threadPrinciple.Identity.Name}!!!!!!");
        }

        [HttpPost("{Login}")]
        public ActionResult<AuthUserDTO> AuthenticateUser(UserCredentials userCredentials, string returnURL)
        {
            this._userCredentials = userCredentials;
            var JWTToken = _loginManager.AuthenticateUser(_userCredentials);
            return Ok(JWTToken);
        }
        // GET: for main view  
        ////public EmptyResult EmptyData()
        ////{
        ////    return new EmptyResult();
        ////}
    }
}
