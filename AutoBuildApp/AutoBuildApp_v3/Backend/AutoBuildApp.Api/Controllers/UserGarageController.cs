using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using AutoBuildApp.Managers;
using AutoBuildApp.Api.HelperFunctions;
using System;
using AutoBuildApp.Logging;
using System.Collections.Generic;
using AutoBuildApp.Security.Enumerations;
using System.Threading;
using System.Security.Claims;
using AutoBuildApp.Security;

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
        //private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private UserGarageManager _manager;
        private readonly string _connString =
            ConnectionManager
            .connectionManager
            .GetConnectionStringByName(ControllerGlobals.DOCKER_CONNECTION);
        private readonly List<string> _approvedRoles = new List<string>()
        {
            RoleEnumType.BasicRole,
            RoleEnumType.DelegateAdmin,
            RoleEnumType.VendorRole,
            RoleEnumType.SystemAdmin
        };
        private readonly string _singleShelfFetch = "Requested a single shelf.";
        private readonly string _userShelvesFetch = "Requested all a users shelves.";
        private readonly string _internalError = "Internal error has occured.";
        private readonly string _badRequest = "A bad request was made.";

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
                IsAuthorized();
                var principle = (ClaimsPrincipal)Thread.CurrentPrincipal;
                Console.WriteLine(principle.Claims);
                //_logger.LogInformation(_singleShelfFetch);
                var output =_manager.GetShelfByName(shelfName, username);

                return Ok(output);
            }
            catch (ArgumentNullException)
            {
                //_logger.LogWarning(_badRequest);
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet("getShelves")]
        public IActionResult GetShelvesByUser(string username)
        {
            _manager = new UserGarageManager(_connString);
            try
            {
                IsAuthorized();
                //_logger.LogInformation();
                var output = _manager.GetShelvesByUser(username);

                return Ok(output);
            }
            catch (ArgumentNullException)
            {
                //_logger.LogWarning();
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

        [HttpPut("orderShelf")]
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

        public void IsAuthorized()
        {
            if (!AuthorizationCheck.IsAuthorized(_approvedRoles))
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
