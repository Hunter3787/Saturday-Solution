using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using AutoBuildApp.Managers;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Api.HelperFunctions;
using System;

/**
* AutoBuild Recommendation Tool Controller.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Controllers
{
    /// <summary>
    /// Controller for the Recommendation tool.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [EnableCors("CorsPolicy")]
    public class RecommendationController : ControllerBase
    {

        private readonly string _connectionString = ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection");
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
        [HttpPost]
        public IActionResult GetFullBuildRecommend(UserRequestParameters requests)
        {
             manager = new RecommendationManager(_connectionString);

            try { 
                var builds = manager.RecommendBuilds(requests.Build, requests.Budget,
                    requests.List,requests.Psu, requests.HddType, requests.HddCount);

                    if (builds != null)
                        return Ok(builds);

            }
            catch(UnauthorizedAccessException ex)
            {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
