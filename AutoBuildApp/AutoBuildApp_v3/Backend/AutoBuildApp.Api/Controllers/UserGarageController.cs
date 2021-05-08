using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using AutoBuildApp.Managers;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Api.HelperFunctions;
using System;
using AutoBuildApp.Services;
using AutoBuildApp.Logging;

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
        private LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private UserGarageManager _manager;
        private readonly string _connString = ConnectionManager.connectionManager.GetConnectionStringByName(ControllerGlobals.DOCKER_CONNECTION);

        [HttpGet("getBuilds")]
        public IActionResult GetBuilds()
        {
            // TODO
            return Ok();
        }

        [HttpPost("saveBuild")]
        public IActionResult AddBuild()
        {
            // TODO
            return Ok();
        }

        [HttpPost("copyBuild")]
        public IActionResult CopyBuildToGarage()
        {
            // TODO
            return Ok();
        }

        [HttpPut("modBuild")]
        public IActionResult ModifyBuild()
        {
            // TODO
            return Ok();
        }

        [HttpDelete("deleteBuild")]
        public IActionResult DeleteBuild()
        {
            // TODO
            return Ok();
        }

        [HttpPost("publishBuild")]
        public IActionResult PublishBuild()
        {
            // TODO
            return Ok();
        }

        [HttpGet("getShelf")]
        public IActionResult GetShelf(string shelfName, string username)
        {
            _manager = new UserGarageManager(_connString);
            try
            {
                var output =_manager.GetShelfByName(shelfName, username);

                if (!output.IsSuccessful)
                {
                    return Ok(output);
                }

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (ArgumentNullException)
            {
                _logger.LogWarning("GetShelf: Bad Request");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost("addItem")]
        public IActionResult AddToShelf()
        {
            // TODO
            return Ok();
        }

        [HttpPost("copyItem")]
        public IActionResult CopyToShelf()
        {
            // TODO
            return Ok();
        }

        [HttpDelete("deleteItem")]
        public IActionResult RemoveFromShelf()
        {
            // TODO
            return Ok();
        }

        [HttpDelete("deleteShelf")]
        public IActionResult DeleteShelf()
        {
            // TODO
            return Ok();
        }

        [HttpPut]
        public IActionResult OrderShelf()
        {

            // TODO
            return Ok();
        }

        [HttpPut("changeQuantity")]
        public IActionResult ModifyCount()
        {
            // TODO
            return Ok();
        }
    }
}
