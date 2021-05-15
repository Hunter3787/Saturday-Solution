using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using AutoBuildApp.Managers;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Api.HelperFunctions;
using System;
using AutoBuildApp.Services;
using AutoBuildApp.Models.Enumerations;

/**
* AutoBuild Recommendation Tool Controller.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Api.Controllers
{
    /// <summary>
    /// Controller for the Recommendation tool.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [EnableCors("CorsPolicy")]
    public class RecommendationController : ControllerBase
    {
        // Logging setup
        //private LoggingConsumerManager _logManager = new LoggingConsumerManager();
        //private LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private readonly string _connectionString =
            ConnectionManager
            .connectionManager
            .GetConnectionStringByName(ControllerGlobals.ADMIN_CREDENTIALS_CONNECTION);

        //private readonly string _connectionString = "Data Source=satudaysolution-rds.cc5jk01zeyle.us-west-1.rds.amazonaws.com, 1433; Initial Catalog=DB; Integrated Security=True; Trusted_Connection=False; Uid=admin; Pwd=SaturdaySolution;";

        private RecommendationManager manager;

        /// <summary>
        /// Get a full build recommendation from the system.
        /// </summary>
        /// <param name="build"></param>
        /// <param name="principal"></param>
        /// <param name="peripherals"></param>
        /// <param name="psuType"></param>
        /// <param name="hardDriveType"></param>
        /// <param name="hardDriveCount"></param>
        /// <returns></returns>
        [HttpPost("BuildRecommend")]
        public IActionResult GetBuildRecommend(RecommenderReqParams requests)
        {
            manager = new RecommendationManager(_connectionString);

            Console.Write(requests);

            try { 
                var builds = manager.RecommendBuilds(BuildType.Gaming, requests.Budget,
                    requests.PeripheralsList,requests.Psu, requests.HddType, requests.HddCount);

                    if (builds != null)
                        return Ok(builds);

            }
            catch(UnauthorizedAccessException )
            {
                //_logger.LogWarning(ex.Message);
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            //catch(ArgumentOutOfRangeException )
            //{
            //    //_logger.LogWarning(ex.Message);
            //    return new StatusCodeResult(StatusCodes.Status400BadRequest);
            //}

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [Route("BuildUpgrade")]
        public IActionResult GetBuildUpgrade()
        {

            return Ok();
        }
        
        [HttpPost]
        [Route("ItemUpgrade")]
        public IActionResult GetItemUpgrade()
        {

            return Ok();
        }
    }
}
