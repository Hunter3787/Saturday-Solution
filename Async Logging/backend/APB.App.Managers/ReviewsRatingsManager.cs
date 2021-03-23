using APB.App.DomainModels;

namespace APB.App.Services
{
    class ReviewsRatingsService
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
