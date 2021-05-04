﻿using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;

namespace AutoBuildApp.Api.Controllers
{

    [EnableCors("CorsPolicy")] // applying the cors policy so that the client can

    [Route("[controller]")] // the route
    // make a call the this controller 
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;

        private ClaimsFactory _claimsFactory = new ConcreteClaimsFactory();
        IClaims _admin;

        // Creates the local instance for the logger
       // private LoggingProducerService _logger = LoggingProducerService.GetInstance;


        private AnalyticsManager _uadManager;

        public AnalyticsController()
        {
            /// the user analysis dashboard need admin Priveldges so check:
            /// Step one specify the claim set required
            /// 
            _admin = _claimsFactory.GetClaims(RoleEnumType.SYSTEM_ADMIN);


            #region getting the connection string and passing to the loginmanager
            // created a connection manager to access the connection strings in 
            // 1) the app settings .json file
            ConnectionManager conString = ConnectionManager.connectionManager;
            // 2) passing in the name I assigned my connection string 
            string connection = conString.GetConnectionStringByName("MyConnection");
            // Console.WriteLine($"connection string passed in controller: {connection} ");
            //3) connection string passed to the analytics manager
            #endregion


            _uadManager = new AnalyticsManager(connection);

        }

        /*
         * There are two cases to consider:
         * A request coming through from:
         * 1) user is logged in but do not have permissions -> 
         * status code:  403 (don’t have permissions to do what they requested)
         * 
         * 2) user is NOT logged in and are not authorized -> 
         * status code: 401. here comes in the REDIRECT URL.
         * They are directed to the login page and later 
         * to the page intended (if they are aurthorized)
         * 
         */

        public IActionResult Index()
        {

            if (!_threadPrinciple.Identity.IsAuthenticated)
            {
                // Add action logic here
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            if (!AuthorizationService.CheckPermissions(_admin.Claims()))
             {


                    // Add action logic here
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
              }
            return Ok("Good to Continue");
            
        }

        [HttpGet]
        public IActionResult RetrieveGraphs()
        {

            /// this will be put into the middleware.
            //Console.WriteLine("we are here22");
            //if (!_threadPrinciple.Identity.IsAuthenticated)
            //{
            //    Console.WriteLine("we are here");
            //    // Add action logic here
            //    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            //}

            if (!AuthorizationService.CheckPermissions(_admin.Claims()))
            {

               // _logger.LogWarning("Unauthorized Access Attempted");
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
            _uadManager.GetChartData(0);
            
            if(true)
            {
                if (0 == (int)AuthorizationResultType.NotAuthorized){

                   // _logger.LogWarning("Unauthorized Access Attempted");
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                else
                {
                   // _logger.LogWarning(result.result);
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }
            }
            // lesson: postman doesnt work with list. move on 

            return Ok();

            
        }

    }
}
