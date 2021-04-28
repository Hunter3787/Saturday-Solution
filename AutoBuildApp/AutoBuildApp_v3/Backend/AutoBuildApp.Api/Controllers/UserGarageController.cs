using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using AutoBuildApp.Managers;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Api.HelperFunctions;
using System;
using AutoBuildApp.Services;

/**
* User Garage controller that accepts incoming requests 
* and directs them to the appropriate manager.
* @Author Nick Marshall-Eminger
*/
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

        [HttpPost]
        public IActionResult AddBuild()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult CopyBuildToGarage()
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult ModifyBuild()
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteBuild()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult PublishBuild()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetShelf()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult AddToShelf()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult CopyToShelf()
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult RemoveFromShelf()
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteShelf()
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult ModifyCount()
        {
            return Ok();
        }
    }
}
