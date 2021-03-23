using APB.App.DataAccess;
using APB.App.Entities;
using APB.App.DomainModels;

namespace APB.App.Services
{
    public class ReviewRatingService
    {
        private ReviewRatingService()
        {

        }

        public void CreateReviewRating(ReviewRating reviewRating)
        {
            ReviewRatingDAO reviewsRatingsDataAccess = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");

            var reviewRatingEntity = new ReviewRatingEntity()
            {
                ReviewRatingTypeName = nameof(reviewRating.StarRating),
                Message = reviewRating.Message
            };

            reviewsRatingsDataAccess.CreateReviewRatingRecord(reviewRatingEntity);
        }
    }
}
