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
using System.Linq;
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
        ClaimsPrincipal _threadPrinciple;
        #endregion

        public AuthenticationController()
        {

            // setting a default principle object t=for the thread.
            #region Instantiating the Claims principle
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaimsFactory unregistered = claimsFactory.GetClaims(RoleEnumType.UNREGISTERED_ROLE);


            // set it to th current thread
            //Thread.CurrentPrincipal = _principal;
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
            _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;

            Console.WriteLine($"The Claims prinicple set in the JWT validator:");
            foreach (Claim c in _threadPrinciple.Claims)
            {
                Console.WriteLine($"Permission:  {c.Type}, Scope: {c.Value} ");
            }
            
            
            return
                Ok( $"\n\nCurrent Thread Priciple: {JsonSerializer.Serialize(Thread.CurrentPrincipal)}/n" +
                $"OUTPUTTING THE USEREMAIL IN THE CURRENT THREAD FOR NICK: {_threadPrinciple.Identity.Name} " +
                $"");
        }

        [HttpPost("{Login}")]
        public ActionResult<AuthUserDTO> AuthenticateUser(UserCredentials userCredentials, string returnURL)
        {

             _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;

            foreach(Claim c in _threadPrinciple.Claims)
            {
                Console.WriteLine($"Permission:  {c.Type}, Scope: {c.Value} ");
            }

            this._userCredentials = userCredentials;
            var JWTToken = _loginManager.AuthenticateUser(_userCredentials);
            Console.WriteLine($"AFTER THE JWT HAS BEEN ISSUED: \n");

            _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            foreach (Claim c in _threadPrinciple.Claims)
            {
                Console.WriteLine($"Permission:  {c.Type}, Scope: {c.Value} ");
            }

            return Ok( $" { JWTToken} { _threadPrinciple.Identity.Name}");
        }
    }
}
