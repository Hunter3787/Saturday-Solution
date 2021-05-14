using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Logging;
using AutoBuildApp.Managers;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Services;
using AutoBuildApp.Services.FeatureServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AutoBuildApp.Api.Controllers
{
    /// <summary>
    /// This class will be the controller of the BuildPosts and call methods from the front end.
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class MostPopularBuildsController : ControllerBase
    {
        // Gets the connection string of the Database
        private static readonly string _connectionString = ConnectionManager.connectionManager.GetConnectionStringByName(ControllerGlobals.ADMIN_CREDENTIALS_CONNECTION);

        // The DAO that will be used for builds.
        private readonly MostPopularBuildsDAO _mostPopularBuildsDAO;

        // This will start a service when a post fetch is called.
        private readonly MostPopularBuildsService _mostPopularBuildsService;

        // This will start a manager and pass in the service.
        private readonly MostPopularBuildsManager _mostPopularBuildsManager;

        // Creates the local instance for the logger
        private readonly LoggingProducerService _logger;

        // The registered user authorization check.
        private readonly List<string> _allowedRolesForViewing;
        
        // The unregistered user authorization check.
        private readonly List<string> _allowedRolesForPosting;

        public MostPopularBuildsController()
        {
            _allowedRolesForViewing = new List<string>()
            {
                RoleEnumType.UnregisteredRole,
                RoleEnumType.BasicRole
            };

            _allowedRolesForPosting = new List<string>()
            {
                RoleEnumType.BasicRole
            };

            // Pass in the connection strings to initialize the DAO.
            _mostPopularBuildsDAO = new MostPopularBuildsDAO(_connectionString);

            // Pass in the DAO to the service.
            _mostPopularBuildsService = new MostPopularBuildsService(_mostPopularBuildsDAO);

            // Pass the service into the manager.
            _mostPopularBuildsManager = new MostPopularBuildsManager(_mostPopularBuildsService);

            _logger = LoggingProducerService.GetInstance;
        }

        /// <summary>
        /// This class will show no content if fetch Options is made.
        /// </summary>
        /// <returns>will return a page of no content to the view.</returns>
        [HttpOptions]
        public IActionResult PreflightRoute()
        {
            _logger.LogInformation("HttpOptions was called");
            return NoContent();
        }

        /// <summary>
        /// This method will be used to fetch post new Build Posts to the DB.
        /// </summary>
        /// <param name="data">takes in a IFormCollection object which reads from FormData.</param>
        /// <param name="image">takes in an image file from the FormData.</param>
        /// <returns>returns a status code result.</returns>
        [HttpPost]
        public async Task<IActionResult> PublishBuild(IFormCollection data, List<IFormFile> image)
        {
            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRolesForPosting))
            {
                _logger.LogInformation("MostPopularBuilds " + AuthorizationResultType.NotAuthorized.ToString());
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            // Takes the form data from javascript and converts it to an AddProductDTO.
            CommonResponseWithObject<BuildPost> commonResponseBuildPost = _mostPopularBuildsManager.ConvertFormToBuildPost(data, image);


            // This will store the bool result of the MPB creation to see if it is a success or fail.
            var returnsTrue = await _mostPopularBuildsManager.PublishBuild(commonResponseBuildPost.GenericObject);

            // if true, it will return OK, else it will return status code error of 500
            if (returnsTrue)
            {
                _logger.LogInformation("PublishBuild was a success.");
                return Ok();
            }

            _logger.LogInformation("PublishBuild was not successfully fetched.");
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }

        /// <summary>
        /// This method will get all Builds from the DB.
        /// </summary>
        /// <param name="orderLikes">Takes in query string to order likes.</param>
        /// <param name="buildType">Takes in query string to sort build type.</param>
        /// <returns>returns the status of OK as well as the list of Build Posts.</returns>
        [HttpGet]
        public IActionResult GetBuildPosts(string orderLikes, string buildType)
        {
            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRolesForViewing))
            {
                _logger.LogInformation("MostPopularBuilds " + AuthorizationResultType.NotAuthorized.ToString());
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            // Log the event of a get fetch.
            _logger.LogInformation("GetBuildPosts was fetched.");

            // This will try to get all Builds, and if not, it will catch it and return the error code.
            try
            {
                var buildPosts = _mostPopularBuildsManager.GetBuildPosts(orderLikes, buildType); // calls the manager to get all Builds sorted by build type, Graphic Artist.

                //_logger.LogInformation("GetBuildPosts was sucessfully fetched.");
                return Ok(buildPosts); // sends the BuildPost list through the OK to be read from the front end fetch request.
            }
            catch
            {
                _logger.LogWarning("GetBuildPosts was not sucessfully fetched.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
        }

        /// <summary>
        /// This method gets a single record
        /// </summary>
        /// <param name="buildId">takes in a build id string.</param>
        /// <returns>returns a single build object</returns>
        [HttpGet("build")]
        public IActionResult GetBuildPost(string buildId)
        {
            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRolesForViewing))
            {
                _logger.LogInformation("MostPopularBuilds " + AuthorizationResultType.NotAuthorized.ToString());
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            // Log the event of a get fetch.
            _logger.LogInformation("GetBuildPost was fetched.");

            // This will try to get a Build, and if not, it will catch it and return the error code.
            try
            {
                var buildPost = _mostPopularBuildsManager.GetBuildPost(buildId); // calls the manager to get all Builds sorted by build type, Graphic Artist.

                //_logger.LogInformation("GetBuildPost was sucessfully fetched.");
                return Ok(buildPost); // sends the BuildPost through the OK to be read from the front end fetch request.
            }
            catch
            {
                _logger.LogWarning("GetBuildPost was not sucessfully fetched.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
        }

        /// <summary>
        /// This controller method posts a like to a build post.
        /// </summary>
        /// <param name="like">takes in a like object (userid and postid)</param>
        /// <returns>returns an iaction result of success or fail.</returns>
        [HttpPost("like")]
        public IActionResult AddLike(Like like)
        {
            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRolesForPosting))
            {
                _logger.LogInformation("MostPopularBuilds " + AuthorizationResultType.NotAuthorized.ToString());
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            // Log event when a fetch happens for liking a build post.
            _logger.LogInformation("Add Like was fetched.");

            // This will store the bool result of the MPB creation to see if it is a success or fail.
            var returnsTrue = _mostPopularBuildsManager.AddLike(like);

            // if true, it will return OK, else it will return status code error of 500
            if (returnsTrue)
            {
                _logger.LogInformation("Add like was a success.");
                return Ok();
            }

            _logger.LogInformation("Add like was not successfully fetched.");
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }
    }
}
