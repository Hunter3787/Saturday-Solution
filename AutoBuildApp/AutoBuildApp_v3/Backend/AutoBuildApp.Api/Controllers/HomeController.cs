using AutoBuildApp.Api.HelperFunctions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBuildApp.Api.Controllers
{

    [EnableCors("CorsPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private static readonly string _defaultRoute = ConnectionManager.connectionManager.GetConnectionStringByName(ControllerGlobals.DEFAULT_ROUTE);

        [HttpGet]
        public IActionResult Index()
        {
            return Ok($"You have hit the Home Controller!!");

        }

        [HttpGet("Error")]
        public IActionResult Error()
        {
            return Redirect(_defaultRoute);
        }
    }
}
