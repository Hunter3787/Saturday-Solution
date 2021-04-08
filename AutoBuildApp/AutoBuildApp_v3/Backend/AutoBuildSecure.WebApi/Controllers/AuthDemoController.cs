
using AutoBuildApp.Managers.FeatureManagers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading;

namespace AutoBuildSecure.WebApi.Controllers
{

    [EnableCors("CorsPolicy")] // applying the cors policy so that the client can

    [Route("[controller]")] // the route
    // make a call the this controller 
    [ApiController]
    public class AuthDemoController : ControllerBase
    {

        #region 
        private ClaimsPrincipal _principal;
        #endregion

        public AuthDemoController()
        {

        }

        [HttpGet]
        public IActionResult GetData()
        {
            _principal = HttpContext.User;
            Thread.CurrentPrincipal = HttpContext.User;
            string returnValue = "";
            Console.WriteLine("checking principle;");
            foreach (var clm in HttpContext.User.Claims)
            {
                returnValue += $" claim type: { clm.Type } claim value: {clm.Value} \n";
                Console.WriteLine(returnValue);
            }
            Console.WriteLine("checking principle;");
            foreach (var clm in _principal.Claims)
            {
                returnValue += $" claim type: { clm.Type } claim value: {clm.Value} \n";
                Console.WriteLine(returnValue);
            }
            AuthDemoManager authDemo = new AuthDemoManager();
            //. getting the data
            var data = authDemo.getData();
            return Ok($"the list of claims given: { returnValue}," +
                $"/n the data in the demo manager : {data}");

        }

    }
}
