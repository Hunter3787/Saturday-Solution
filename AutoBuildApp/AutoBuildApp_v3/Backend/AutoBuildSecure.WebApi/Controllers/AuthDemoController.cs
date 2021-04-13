
using AutoBuildApp.Managers.FeatureManagers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;

namespace AutoBuildSecure.WebApi.Controllers
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

            string returnValue = "";
            Console.WriteLine("checking principle;");
            foreach (var clm in _threadPrinciple.Claims)
            {
                returnValue += $" claim type: { clm.Type } claim value: {clm.Value} \n";
            }
            AuthDemoManager authDemo = new AuthDemoManager();
            //. getting the data
            var data = authDemo.getData();
            return Ok($"the list of claims given: { returnValue}," +
                $"\n The data retrieved from the authManager  : {data}" +
                $"\n Current Thread Priciple: { JsonSerializer.Serialize(Thread.CurrentPrincipal)} ");

        }

    }
}
