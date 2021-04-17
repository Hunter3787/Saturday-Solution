using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBuildSecure.WebApi.Controllers
{

    [EnableCors("CorsPolicy")] // applying the cors policy so that the client can
    // make a call the this controller 
    [ApiController]
    [Route("[controller]")] // the route
    public class HomeController : ControllerBase
    {


        [HttpGet]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok($"You have hit the Home Controller!!");

        }

    }
}
