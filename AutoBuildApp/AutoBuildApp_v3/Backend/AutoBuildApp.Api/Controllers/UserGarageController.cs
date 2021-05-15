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
using AutoBuildApp.Models.Builds;
using AutoBuildApp.DomainModels;

/**
* User Garage controller that accepts incoming requests 
* and directs them to the appropriate manager.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Api.Controllers
{
    [EnableCors("CorsPolicy")] // applying the cors policy so that the client can
    // make a call the this controller 
    [ApiController]
    [Route("[controller]")] // the route
    public class UserGarageController : ControllerBase
    {
        //private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private UserGarageManager _manager;
        private readonly string _connString =
            ConnectionManager
            .connectionManager
            .GetConnectionStringByName(ControllerGlobals.ADMIN_CREDENTIALS_CONNECTION);

        public UserGarageController()
        {
            _manager = new UserGarageManager(connectionString: _connString);
        }
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

        [HttpGet("getListBuilds")]
        public IActionResult GetBuildList()
        {
            // TODO
           
            // TODO
            var response = _manager.GetAllUserBuilds(Thread.CurrentPrincipal.Identity.Name,null);
           if(response is null)
            {

                return StatusCode(StatusCodes.Status400BadRequest, "No Builds For User");

            }
            return StatusCode(StatusCodes.Status200OK, response);

        }


        [HttpPost("CreateBuild")]
        public IActionResult AddBuild(string buildName)
        {
            if(string.IsNullOrEmpty(buildName))
            {

                return StatusCode(StatusCodes.Status400BadRequest, "Invalid Request");
            }
            Build myBuild = new Build()
            {
                BuildName = buildName,
            };
            // TODO
            var response = _manager.AddBuild(myBuild, myBuild.BuildName);
            Console.WriteLine($"Response : { response.ResponseString}");
            if (!response.IsSuccessful)
            {
                return StatusCode(StatusCodes.Status400BadRequest, response.ResponseString);
            }
            return StatusCode(StatusCodes.Status200OK,response.ResponseString);
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

        [HttpPost("SaveRecommendedBuild")]
        public IActionResult SaveBuild
            (IList<string> modelNumbers, string buildName)
        {
            try
            {
                // TODO
                var response = _manager.AddRecomendedBuild(modelNumbers, buildName);
                Console.WriteLine($"Response : { response.ResponseString}");
                if (!response.IsSuccessful)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, response.ResponseString);
                }
                return StatusCode(StatusCodes.Status200OK, response.ResponseString);


            }
            catch(ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status400BadRequest,"Invalid Request");

            }
        }

        [HttpDelete("deleteBuild")]
        public IActionResult DeleteBuild(string buildName)
        {
            _manager = new UserGarageManager(_connString);
            try
            {
                IsAuthorized();
                var principle = (ClaimsPrincipal)Thread.CurrentPrincipal;
                Console.WriteLine(principle.Claims);
                //_logger.LogInformation(_singleShelfFetch);
                var output = _manager.DeleteBuild(buildName);
                return StatusCode(StatusCodes.Status200OK,output.ResponseString);

            }
            catch (ArgumentNullException)
            {
                //_logger.LogWarning(_badRequest);
                return StatusCode(StatusCodes.Status400BadRequest, "Bad Request");
            }
        }

        [HttpPost("publishBuild")]
        public IActionResult PublishBuild(BuildPost BuildPost)
        {
            _manager = new UserGarageManager(_connString);
            try
            {
                //_logger.LogInformation(_singleShelfFetch);
                var output = _manager.PublishBuild(BuildPost);
                return StatusCode(StatusCodes.Status200OK, output.ResponseString);

            }
            catch (ArgumentNullException)
            {
                //_logger.LogWarning(_badRequest);
                return StatusCode(StatusCodes.Status400BadRequest, "Bad Request");
            }
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
