using System;
using System.Collections.Generic;
using System.Text;
using APB.App.DomainModels;
using APB.App.Entities;
//using static System.Net.Mime.MediaTypeNames;

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
