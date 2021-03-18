using System;
using Producer;
using DataAccess;
using System.Collections.Generic;
using System.Text;

namespace Consumer
{
    public class ReviewsRatingsManager
    {
        private ReviewsRatingsManager()
        {

        }

        public void ProcessReviewRatingObject(ReviewsRatingsObject reviewsRatingsObject)
        {
            ReviewsRatingsDataAccess reviewsRatingsDataAccess = new ReviewsRatingsDataAccess("Server = localhost; Database = DB; Trusted_Connection = True;");
            reviewsRatingsDataAccess.CreateReviewRatingRecord(reviewsRatingsObject);
        }
    }
}
