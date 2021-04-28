﻿
using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;

namespace AutoBuildApp.Api.Controllers
{

    [EnableCors("CorsPolicy")] // applying the cors policy so that the client can

    [Route("[controller]")] // the route
    // make a call the this controller 
    [ApiController]
    public class AuthDemoController : ControllerBase
    {

        private ClaimsFactory _claimsFactory = new ConcreteClaimsFactory();
        IClaims _admin;
        public AuthDemoController()
        {

            _admin = _claimsFactory.GetClaims(RoleEnumType.BASIC_ROLE);


        }
        [HttpGet]
        public IActionResult GetData()
        {

            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            Console.WriteLine("we are here A");
            if (!_threadPrinciple.Identity.IsAuthenticated)
            {
                Console.WriteLine("we are here B");
                // Add action logic here
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            if (!AuthorizationService.checkPermissions(_admin.Claims()))
            {
                Console.WriteLine("we are here C");
                // Add action logic here
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            /// RETRIEVING THE THREAD PRINCIPAL 
            /// SET IN THE JWT MIDDLEWARE
            AuthDemoManager authDemo = new AuthDemoManager();
            //. getting the data
            var data = authDemo.getData();
           //return Ok($"The list of claims given: { returnValue}," +
            return Ok($"\n The data retrieved from the authManager  : {data}" +
                $"\n Current Thread Priciple: { JsonSerializer.Serialize(Thread.CurrentPrincipal)} ");

        }




    }
}
