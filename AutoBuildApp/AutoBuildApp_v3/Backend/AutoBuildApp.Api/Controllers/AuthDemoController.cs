﻿
using AutoBuildApp.Managers.FeatureManagers;
using Microsoft.AspNetCore.Cors;
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

        public AuthDemoController()
        {

        }
        [HttpGet]
        public IActionResult GetData()
        {
            /// RETRIEVING THE THREAD PRINCIPAL 
            /// SET IN THE JWT MIDDLEWARE
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            AuthDemoManager authDemo = new AuthDemoManager();
            //. getting the data
            var data = authDemo.getData();
           //return Ok($"The list of claims given: { returnValue}," +
            return Ok($"\n The data retrieved from the authManager  : {data}" +
                $"\n Current Thread Priciple: { JsonSerializer.Serialize(Thread.CurrentPrincipal)} ");

        }




    }
}
