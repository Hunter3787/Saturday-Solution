using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using AutoBuildApp.Managers;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;
using System.Collections.Generic;

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
        public IActionResult GetFullBuildRecommend(BuildType build,
            double initial, List<IComponent> peripherals, PSUModularity psuType,
                HardDriveType hardDriveType, int hardDriveCount)
        {
            RecommendationManager manager = new RecommendationManager();

            var builds = manager.RecommendBuilds(build, initial, peripherals,
                psuType, hardDriveType, hardDriveCount);

            if (builds != null)
                return Ok(builds);

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }


        [HttpPost]
        public IActionResult GetUpgradeRecommendation()
        {

            return Ok();
        }
    }
}
