using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

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


        private UADManager _uadManager;

        public AnalyticsController()
        {
            /// the user analysis dashboard need admin Priveldges so check:
            /// Step one specify the claim set required
            /// 
            _admin = _claimsFactory.GetClaims(RoleEnumType.BASIC_ADMIN);


            #region getting the connection string and passing to the loginmanager
            // created a connection manager to access the connection strings in 
            // 1) the app settings .json file
            ConnectionManager conString = ConnectionManager.connectionManager;
            // 2) passing in the name I assigned my connection string 
            string connection = conString.GetConnectionStringByName("MyConnection");
            // Console.WriteLine($"connection string passed in controller: {connection} ");
            //3) connection string passed to the analytics manager
            #endregion


            _uadManager = new UADManager(connection);

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

            Console.WriteLine("we are here22");
            if (!_threadPrinciple.Identity.IsAuthenticated)
            {

                // Add action logic here
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            if (!AuthorizationService.checkPermissions(_admin.Claims()))
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
            Console.WriteLine("we are here22");
            if (!_threadPrinciple.Identity.IsAuthenticated)
            {
                Console.WriteLine("we are here");
                // Add action logic here
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }



            if (!AuthorizationService.checkPermissions(_admin.Claims()))
            {
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
            /// 

            var result = _uadManager.getAllChartData();

            if( result == AuthorizationResultType.NOT_AUTHORIZED.ToString())
            {

                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
            else
            {
                return Ok(result);

            }

        }












        }
}
