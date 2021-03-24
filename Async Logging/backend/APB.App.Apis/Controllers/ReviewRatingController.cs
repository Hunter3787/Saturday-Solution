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

        [HttpOptions]
        public IActionResult PreflightRoute()
        {
            return NoContent();
        }

        [HttpPost]
        public IActionResult CreateReviewRating(ReviewRating reviewRating)
        {
            ReviewRatingService service = new ReviewRatingService();
            ReviewRatingManager manager = new ReviewRatingManager();

            var createResult = manager.ReviewRating(reviewRating);

            if (createResult)
            {
                return Ok();
            }

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        public IActionResult GetReviewRating()
        {
            ReviewRatingService service = new ReviewRatingService();
            ReviewRatingManager manager = new ReviewRatingManager();

            ReviewRating reviewRating = new ReviewRating();

            reviewRating.Message = "TERRIBLE REVIEW";
            reviewRating.StarRating = StarType.Four_Stars;

            try
            {
                var accounts = reviewRating;

                return Ok(accounts);
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
