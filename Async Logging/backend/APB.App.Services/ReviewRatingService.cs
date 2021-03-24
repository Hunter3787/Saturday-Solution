using APB.App.DataAccess;
using APB.App.Entities;
using APB.App.DomainModels;

namespace APB.App.Services
{
    public class ReviewRatingService
    {
        LoggingProducerService logger = LoggingProducerService.GetInstance;

        public ReviewRatingService()
        {

        }

        public bool CreateReviewRating(ReviewRating reviewRating)
        {
            ReviewRatingDAO reviewsRatingsDataAccess = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");

            var reviewRatingEntity = new ReviewRatingEntity()
            {
                StarRatingValue = (int)reviewRating.StarRating,
                Message = reviewRating.Message
            };

            logger.LogInformation("A new Review and Rating record has been created in the database");

            return reviewsRatingsDataAccess.CreateReviewRatingRecord(reviewRatingEntity);
        }
    }
}
