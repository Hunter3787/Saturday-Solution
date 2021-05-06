using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Logging;
using AutoBuildApp.Managers;
using AutoBuildApp.Models.Enumerations;
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
        // Initializes the DAO that will be used for builds.
        private readonly MostPopularBuildsDAO _mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");

        // This will start a service when a post fetch is called. and pass in the DAO that will be used.
        private MostPopularBuildsService mostPopularBuildsService;

        // This will start a manager and pass in the service.
        private MostPopularBuildsManager mostPopularBuildsManager;

        // This will start the logging consumer manager in the background so that logs may be sent to the DB.
        private LoggingConsumerManager _loggingConsumerManager = new LoggingConsumerManager();

        // Creates the local instance for the logger
        private LoggingProducerService _logger = LoggingProducerService.GetInstance;

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
            // Pass in the DAO to the service.
            mostPopularBuildsService = new MostPopularBuildsService(_mostPopularBuildsDAO);

            // Pass the service into the manager.
            mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // Initialize a local BuildPost Object to store data into and then pass to the manager.
            var post = new BuildPost();

            // The following 6 lines parse data from the FormData to a BuildPost object.
            post.Username = data["username"]; 
            post.Title = data["title"];
            post.Description = data["description"];
            post.BuildType = (BuildType)int.Parse(data["buildType"]);
            post.BuildImagePath = data["buildImagePath"];
            post.Image = image;

            // This will store the bool result of the MPB creation to see if it is a success or fail.
            var returnsTrue = await mostPopularBuildsManager.PublishBuild(post);

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
            // Log the event of a get fetch.
            _logger.LogInformation("GetBuildPosts was fetched.");

            // Pass in the DAO to the service.
            mostPopularBuildsService = new MostPopularBuildsService(_mostPopularBuildsDAO);

            // Pass the service into the manager.
            mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // This will try to get all Builds, and if not, it will catch it and return the error code.
            try
            {
                var buildPosts = mostPopularBuildsManager.GetBuildPosts(orderLikes, buildType); // calls the manager to get all Builds sorted by build type, Graphic Artist.

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
            // Log the event of a get fetch.
            _logger.LogInformation("GetBuildPost was fetched.");

            // Pass in the DAO to the service.
            mostPopularBuildsService = new MostPopularBuildsService(_mostPopularBuildsDAO);

            // Pass the service into the manager.
            mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // This will try to get a Build, and if not, it will catch it and return the error code.
            try
            {
                var buildPost = mostPopularBuildsManager.GetBuildPost(buildId); // calls the manager to get all Builds sorted by build type, Graphic Artist.

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
            // Log event when a fetch happens for liking a build post.
            _logger.LogInformation("Add Like was fetched.");

            // Pass in the DAO to the service.
            mostPopularBuildsService = new MostPopularBuildsService(_mostPopularBuildsDAO);

            // Pass the service into the manager.
            mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // This will store the bool result of the MPB creation to see if it is a success or fail.
            var returnsTrue = mostPopularBuildsManager.AddLike(like);

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
