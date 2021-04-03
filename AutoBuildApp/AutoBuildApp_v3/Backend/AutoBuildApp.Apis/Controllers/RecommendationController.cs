using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using AutoBuildApp.Managers;
using AutoBuildApp.DomainModels;

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
        private readonly string _connectionString = "Server = localhost; Database = DB; Trusted_Connection = True;";

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
            RecommendationManager manager = new RecommendationManager(_connectionString);

            var builds = manager.RecommendBuilds(requests.Build, requests.Budget,
                requests.List,requests.Psu, requests.HddType, requests.HddCount);

            if (builds == null)
                return Ok(builds);

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
