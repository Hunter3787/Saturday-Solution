using APB.App.DomainModels;
using APB.App.Services;

namespace APB.App.Managers
{
    public class ReviewRatingManager
    {
        ReviewRatingService reviewRatingService = new ReviewRatingService();

        LoggingProducerService logger = LoggingProducerService.GetInstance;

        private ReviewRating reviewRating;

        public ReviewRatingManager()
        {

        }

        public void ReviewRating(string message, StarType starType)
        {
            reviewRating = new ReviewRating();

            logger.LogInformation("a review and rating has been entered");

            reviewRating.Message = message;
            reviewRating.StarRating = starType;
            //reviewsRatingsObject.Img = image;
            reviewRatingService.CreateReviewRating(reviewRating);
        }
    }
}
