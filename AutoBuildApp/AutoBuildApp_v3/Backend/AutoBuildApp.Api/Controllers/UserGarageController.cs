using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using AutoBuildApp.Managers;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Api.HelperFunctions;
using System;
using AutoBuildApp.Services;

namespace AutoBuildApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("CorsPolicy")]
    public class UserGarageController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetBuilds()
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteBuild()
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult ModifyBuild()
        {
            return Ok();
        }


    }
}
