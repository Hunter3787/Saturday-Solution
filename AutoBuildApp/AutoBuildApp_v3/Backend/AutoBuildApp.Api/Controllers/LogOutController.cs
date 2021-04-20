
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
    public class LogOutController : ControllerBase
    {

        [HttpGet]
        public IActionResult LogOut()
        {
            /// RETRIEVING THE THREAD PRINCIPAL 
            /// SET IN THE JWT MIDDLEWARE
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;

            Console.WriteLine($"\nTHIS GUY REQUESTED TO BE LOGGED OUT:\n {_threadPrinciple.Identity.Name}");
            string returnValue = "";
            Console.WriteLine("\nTHE CURRENT PERMISSIONS ON THREAD: \n");
            foreach (var clm in _threadPrinciple.Claims)
            {
                returnValue += $" claim type: { clm.Type } claim value: {clm.Value} \n";
            }
            AuthDemoManager authDemo = new AuthDemoManager();
            //. getting the data
            var data = authDemo.LogMeOut();
            //return Ok($"The list of claims given: { returnValue}," +
            return Ok(" ");

        }
    }

}
