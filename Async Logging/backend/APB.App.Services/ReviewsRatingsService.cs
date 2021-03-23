using System;
using System.Collections.Generic;
using System.Text;
using APB.App.Services;
using APB.App.DataAccess;
using APB.App.Entities;
using APB.App.DomainModels;

namespace APB.App.Managers
{
    public class ReviewsRatingsService
    {
        private ReviewsRatingsService()
        {

        }

        public void CreateReviewRating(ReviewRating reviewRating)
        {
            ReviewsRatingsDAO reviewsRatingsDataAccess = new ReviewsRatingsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");

            var reviewRatingEntity = new ReviewRatingEntity()
            {
                ReviewRatingTypeName = nameof(reviewRating.StarRating),
                Message = reviewRating.Message
            };

            reviewsRatingsDataAccess.CreateReviewRatingRecord(reviewRatingEntity);
        }
    }
}
