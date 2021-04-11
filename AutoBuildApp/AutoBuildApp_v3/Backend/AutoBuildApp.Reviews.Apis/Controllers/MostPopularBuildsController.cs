using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Managers;
using AutoBuildApp.Services;
using AutoBuildApp.Services.FeatureServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBuildApp.WebApp.Controllers
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
        /// <param name="buildPost">takes in a JSON BuildPost object</param>
        /// <returns>returns a fail status code or an OK status code response.</returns>
        [HttpPost]
        public IActionResult PublishBuild(BuildPost buildPost)
        {
            _logger.LogInformation("PublishBuild was fetched.");

            // This will start a service when a post fetch is called. and pass in the DAO that will be used.
            MostPopularBuildsService mostPopularBuildsService = new MostPopularBuildsService(_mostPopularBuildsDAO);
            // This will start a manager and pass in the service.
            MostPopularBuildsManager mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // This will store the bool result of the MPB creation to see if it is a success or fail.
            var createResult = mostPopularBuildsManager.PublishBuild(buildPost);

            // if true, it will return OK, else it will return status code error of 500
            if (createResult)
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
        /// <returns>returns the status of OK as well as the list of Build Posts.</returns>
        [HttpGet]
        public IActionResult GetBuildPosts(string queryBy)
        {
            _logger.LogInformation("GetBuildPosts was fetched.");

            // This will start a service when a post fetch is called. and pass in the DAO that will be used.
            MostPopularBuildsService mostPopularBuildsService = new MostPopularBuildsService(_mostPopularBuildsDAO);
            // This will start a manager and pass in the service.
            MostPopularBuildsManager mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            List<BuildPost> buildPosts;

            // This will try to get all Builds, and if not, it will catch it and return the error code.
            try
            {
                switch (queryBy)
                {
                    case "BuildType_GraphicArtist":
                        buildPosts = mostPopularBuildsManager.GetBuildPosts(queryBy); // calls the manager to get all Builds sorted by build type, Graphic Artist.
                        break;
                    case "BuildType_Gaming":
                        buildPosts = mostPopularBuildsManager.GetBuildPosts(queryBy); // calls the manager to get all Builds sorted by build type, Gaming.
                        break;
                    case "BuildType_WordProcessing":
                        buildPosts = mostPopularBuildsManager.GetBuildPosts(queryBy); // calls the manager to get all Builds sorted by build type, Word Processing.
                        break;
                    case "AscendingLikes":
                        buildPosts = mostPopularBuildsManager.GetBuildPosts(queryBy); // calls the manager to get all Builds in ascending order likes (least) first.
                        break;
                    default:
                        buildPosts = mostPopularBuildsManager.GetBuildPosts(queryBy); // calls the manager to get all Builds in default descending order (most) first.
                        break;
                }
                //var buildPosts = mostPopularBuildsManager.GetBuildPosts(); // calls the manager to get all Builds.

                //_logger.LogInformation("GetBuildPosts was sucessfully fetched.");
                return Ok(buildPosts); // sends the BuildPost list through the OK to be read from the front end fetch request.
            }
            catch
            {
                _logger.LogWarning("GetBuildPosts was not sucessfully fetched.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
        }
    }
}
