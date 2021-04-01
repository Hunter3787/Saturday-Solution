using APB.App.DomainModels;
using Microsoft.AspNetCore.Mvc;
using APB.App.Managers;
using APB.App.Services;
using Microsoft.AspNetCore.Http;
using APB.App.DataAccess;
using Microsoft.AspNetCore.Cors;

namespace APB.App.Apis.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("[controller]")]
    [ApiController]
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

        //[HttpGet("{reviewId}")]
        //public IActionResult GetReviewRating(string reviewId)
        //{
        //    ReviewRatingService reviewRatingService = new ReviewRatingService(reviewRatingDAO);
        //    ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

        //    try
        //    {
        //        var accounts = reviewRatingManager.GetReviewsRatings(reviewId);

        //        return Ok(accounts);
        //    }
        //    catch
        //    {
        //        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        //    }
        //}

        [HttpGet]
        public IActionResult GetAllReviewRatings()
        {
            ReviewRatingService reviewRatingService = new ReviewRatingService(reviewRatingDAO);
            ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            try
            {
                var accounts = reviewRatingManager.GetAllReviewsRatings();

                return Ok(accounts);
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReviewRating(string reviewId)
        {
            ReviewRatingService reviewRatingService = new ReviewRatingService(reviewRatingDAO);
            ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            var createResult = reviewRatingManager.DeleteReviewRating(reviewId);

            if (createResult)
            {
                return Ok();
            }

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [HttpPut]
        public IActionResult EditReviewRating(ReviewRating reviewRating)
        {
            ReviewRatingService reviewRatingService = new ReviewRatingService(reviewRatingDAO);
            ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            var createResult = reviewRatingManager.EditReviewRating(reviewRating);

            if (createResult)
            {
                return Ok();
            }

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
