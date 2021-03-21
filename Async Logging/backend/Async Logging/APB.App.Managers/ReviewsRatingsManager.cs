using System;
using System.Collections.Generic;
using System.Text;
using APB.App.Services;
using APB.App.DataAccess;

namespace APB.App.Managers
{
    public class ReviewsRatingsManager
    {
        private ReviewsRatingsManager()
        {

        }

        public void ProcessReviewRatingObject(ReviewsRatingsObject reviewsRatingsObject)
        {
            ReviewsRatingsDAO reviewsRatingsDataAccess = new ReviewsRatingsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            reviewsRatingsDataAccess.CreateReviewRatingRecord(reviewsRatingsObject);
        }
    }
}
