using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

using AutoBuildApp.Managers;
using AutoBuildApp.Services;
using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels;
using System.Collections.Generic;
using AutoBuildApp.DomainModels.Enumerations;
using System.Threading.Tasks;
using AutoBuildApp.Logging;
using AutoBuildApp.Api.HelperFunctions;

/// <summary>
/// References used from file: Solution Items/References.txt 
/// [1,3-5]
/// </summary>

namespace AutoBuildApp.Api.Controllers
{
    /// <summary>
    /// This class will be the controller of the ReviewRatings and call methods from the front end.
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class ReviewRatingController : ControllerBase
    {
        private static readonly string _connectionString = ConnectionManager.connectionManager.GetConnectionStringByName(ControllerGlobals.LOCALHOST_CONNECTION);

        // Initializes the DAO that will be used for review ratings.
        private readonly ReviewRatingDAO _reviewRatingDAO = new ReviewRatingDAO(_connectionString);

        // This will start the logging consumer manager in the background so that logs may be sent to the DB.
        private LoggingConsumerManager _loggingConsumerManager = new LoggingConsumerManager();

        private LoggingProducerService _logger = LoggingProducerService.GetInstance;

        /// <summary>
        /// This class will show no contend if fetch Options is made.
        /// </summary>
        /// <returns>will return a page of no content to the view.</returns>
        [HttpOptions]
        public IActionResult PreflightRoute()
        {
            _logger.LogInformation("HttpOptions was called");
            return NoContent();
        }

        /// <summary>
        /// This method will be used to fetch post new reviews to the DB.
        /// </summary>
        /// <param name="reviewRating">takes in a JSON Review Rating object</param>
        /// <returns>returns a fail status code or an OK status code response.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateReviewRating(IFormCollection data, List<IFormFile> image)
        {
            _logger.LogInformation("CreateReviewRating was fetched.");

            // This will start a service when a post fetch is called. and pass in the DAO that will be used.
            ReviewRatingService reviewRatingService = new ReviewRatingService(_reviewRatingDAO);

            // This will start a manager and pass in the service.
            ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            // This will store the bool result of the review creation to see if it is a success or fail.
            var createResult = await reviewRatingManager.CreateReviewRating(data, image);

            // if true, it will return OK, else it will return status code error of 500
            if (createResult)
            {
                _logger.LogInformation("CreateReviewRating was a success.");
                return Ok();
            }

            _logger.LogInformation("CreateReviewRating was not successfully fetched.");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// This method will get all reviews from the DB.
        /// </summary>
        /// <returns>returns the status of OK as well as teh list of reviews.</returns>
        [HttpGet]
        public IActionResult GetAllReviewRatings()
        {
            _logger.LogInformation("GettAllReviewRatings was fetched.");

            // This will start a service when a post fetch is called. and pass in the DAO that will be used.
            ReviewRatingService reviewRatingService = new ReviewRatingService(_reviewRatingDAO);

            // This will start a manager and pass in the service.
            ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            // This will try to get all reviews, and if not, it will catch it and return the error code.
            try
            {
                var reviewRatings = reviewRatingManager.GetAllReviewsRatings(); // calls the manager to get all reviews.

                _logger.LogInformation("GettAllReviewRatings was sucessfully fetched.");
                return Ok(reviewRatings); // sends the review list through the OK to be read from the front end fetch request.
            }
            catch
            {
                _logger.LogWarning("GettAllReviewRatings was not sucessfully fetched.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("build")]
        public IActionResult GetAllReviewRatingsByBuildId(string buildId)
        {
            _logger.LogInformation("GettAllReviewRatingsById was fetched.");

            // This will start a service when a post fetch is called. and pass in the DAO that will be used.
            ReviewRatingService reviewRatingService = new ReviewRatingService(_reviewRatingDAO);

            // This will start a manager and pass in the service.
            ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            // This will try to get all reviews, and if not, it will catch it and return the error code.
            try
            {
                var reviewRatings = reviewRatingManager.GetAllReviewsRatingsByBuildId(buildId); // calls the manager to get all reviews.

                _logger.LogInformation("GettAllReviewRatings was sucessfully fetched.");
                return Ok(reviewRatings); // sends the review list through the OK to be read from the front end fetch request.
            }
            catch
            {
                _logger.LogWarning("GettAllReviewRatings was not sucessfully fetched.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// This will take in an ID and delete that corresponding review.
        /// </summary>
        /// <param name="reviewId">string ID value that will be used to identify a review item.</param>
        /// <returns>will return the OK status or 500 error</returns>
        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReviewRating(string reviewId)
        {
            _logger.LogInformation("DeleteReviewRating was fetched.");

            // This will start a service when a post fetch is called. and pass in the DAO that will be used.
            ReviewRatingService reviewRatingService = new ReviewRatingService(_reviewRatingDAO);

            // This will start a manager and pass in the service.
            ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            // This will store the bool result of the review deletion to see if it is a success or fail.
            var createResult = reviewRatingManager.DeleteReviewRating(reviewId);

            // if true, it will return OK, else it will return status code error of 500
            if (createResult)
            {
                _logger.LogInformation("DeleteReviewRating was successfully fetched.");
                return Ok();
            }

            _logger.LogInformation("DeleteReviewRating was not successfully fetched.");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// This method can be fetched PUT to call for teh editing of a review item in the DB.
        /// </summary>
        /// <param name="reviewRating">passes in a ReviewRating object that will be edited.</param>
        /// <returns>will return OK status code or error code of 500</returns>
        [HttpPut]
        public async Task<IActionResult> EditReviewRating(IFormCollection data, List<IFormFile> image)
        {
            _logger.LogInformation("EditReviewRating was fetched.");

            // This will start a service when a post fetch is called. and pass in the DAO that will be used.
            ReviewRatingService reviewRatingService = new ReviewRatingService(_reviewRatingDAO);

            // This will start a manager and pass in the service.
            ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            // This will store the bool result of the review edit to see if it is a success or fail.
            var createResult = await reviewRatingManager.EditReviewRating(data, image);

            // if true, it will return OK, else it will return status code error of 500
            if (createResult)
            {
                _logger.LogInformation("EditReviewRating was successfully fetched.");
                return Ok();
            }

            _logger.LogInformation("EditReviewRating was not successfully fetched.");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
