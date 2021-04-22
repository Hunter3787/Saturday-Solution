﻿using AutoBuildApp.Security;
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
        public AnalyticsController()
        {
            /// the user analysis dashboard need admin Priveldges so check:
            /// Step one specify the claim set required
            /// 
            _admin = _claimsFactory.GetClaims(RoleEnumType.BASIC_ADMIN);

          

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

            Console.WriteLine("we are here22");
            if (!_threadPrinciple.Identity.IsAuthenticated)
            {
                Console.WriteLine("we are here");
                // Add action logic here
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            if (!AuthorizationService.checkPermissions(_admin.Claims()))
            {

                return Ok("good2");
                // Add action logic here
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
            /// 



            return Ok("here");

        }












        }
}
