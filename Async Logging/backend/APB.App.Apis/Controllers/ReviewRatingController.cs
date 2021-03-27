using APB.App.DomainModels;
using Microsoft.AspNetCore.Mvc;
using APB.App.Managers;
using APB.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APB.App.DataAccess;

namespace APB.App.Apis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewRatingController : ControllerBase
    {
        ReviewRatingDAO reviewRatingDAO = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
        LoggingConsumerManager loggingConsumerManager = new LoggingConsumerManager();

        [HttpOptions]
        public IActionResult PreflightRoute()
        {
            return NoContent();
        }

        [HttpPost]
        public IActionResult CreateReviewRating(ReviewRating reviewRating)
        {
            ReviewRatingService reviewRatingService = new ReviewRatingService(reviewRatingDAO);
            ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            var createResult = reviewRatingManager.CreateReviewRating(reviewRating);

            if (createResult)
            {
                return Ok();
            }

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [HttpGet("{reviewId}")]
        public IActionResult GetReviewRating(string reviewId)
        {
            ReviewRatingService reviewRatingService = new ReviewRatingService(reviewRatingDAO);
            ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            try
            {
                var accounts = reviewRatingManager.GetReviewsRatings(reviewId);

                return Ok(accounts);
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
