using System;
using System.Collections.Generic;
using System.Text;
//using static System.Net.Mime.MediaTypeNames;

namespace Producer
{
    class ReviewsRatingsService
    {
        private ReviewsRatingsObject reviewsRatingsObject = new ReviewsRatingsObject(); 

        public bool ReviewsRatings(String message, StarRating starRating)
        {
            reviewsRatingsObject.Msg = message;
            reviewsRatingsObject.Star = starRating;
            //reviewsRatingsObject.Img = image;

            return true;
        }
    }

    public class ReviewsRatingsObject
    {
        private String msg;
        private StarRating star;
        //private Image img;
        public String Msg { get; set; }
        public StarRating Star { get; set; }
        //public Image Img { get; set; }
    }

    public enum StarRating
    {
        Five_Stars,
        Four_Stars,
        Three_Stars,
        Two_Stars,
        One_Star
    }


}
