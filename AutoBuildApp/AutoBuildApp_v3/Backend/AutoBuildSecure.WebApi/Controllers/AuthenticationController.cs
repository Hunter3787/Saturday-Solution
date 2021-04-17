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
        #endregion

        #region variables for the redirect issue, method


        private JWTValidator _validateAuthorizationHeader;

        // i want to store this into the 
        // http.context.item["ClaimsPrincipal"] =.
        private ClaimsPrincipal _threadPrinciple;

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
        public ActionResult<AuthUserDTO> AuthenticateUser(UserCredentials userCredentials)
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
        ///

        /*
        
        ///
        /// 
        /// 
        /// <summary>
        /// https://www.youtube.com/watch?v=-asykt9Zo_w 
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <param name="returnURL"></param>
        /// <returns></returns>
        [HttpPost("{LoginTest}")]
        public ActionResult<AuthUserDTO> Login(UserCredentials userCredentials, string returnURL)
        {
            this._userCredentials = userCredentials;
            var JWTToken = _loginManager.AuthenticateUser(_userCredentials);


            if(!JWTToken.Equals("User not found"))
            {
                _validateAuthorizationHeader = new JWTValidator(JWTToken); // validate the token 

                var userPrinciple =
                   _validateAuthorizationHeader.ParseForClaimsPrinciple();
                _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
                Thread.CurrentPrincipal = _threadPrinciple; // SETTING THE PARSED TOKEN, TO THE THREAD.

                Console.WriteLine("\nchecking principle claims in this redirect: \n");
                foreach (var clm in _threadPrinciple.Claims)
                {
                    Console.WriteLine(
                        $" claim type: { clm.Type } " +
                        $"claim value: {clm.Value} \n");
                }


                // this causes open redirect attack
                //https://www.youtube.com/watch?v=0q0CZTliQ7A
                // major security holec-> so use local redirect
                if (!string.IsNullOrEmpty(returnURL))
                {
                    return LocalRedirect(returnURL);
                }
                else
                {

                    return RedirectToAction("GetData", "authdemo");
                    //return RedirectToAction("index", "home");
                }

            }

            return Ok(JWTToken);
        }

        */


    }
}
