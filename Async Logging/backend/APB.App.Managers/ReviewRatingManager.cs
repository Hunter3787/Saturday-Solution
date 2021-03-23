using APB.App.DomainModels;
using APB.App.Services;

namespace APB.App.Managers
{
    class ReviewsRatingsManager
    {
        private ReviewRating reviewsRatingsObject;

        public bool ReviewsRatings(string message, StarType starType)
        {
            reviewsRatingsObject = new ReviewRating();

            reviewsRatingsObject.Message = message;
            reviewsRatingsObject.StarRating = starType;
            //reviewsRatingsObject.Img = image;

            return true;
        }
    }
}
