using APB.App.Services;
using APB.App.Managers;
using APB.App.DataAccess;
using System;
using APB.App.DomainModels;
using System.Drawing;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggingConsumerManager loggingConsumerManager = new LoggingConsumerManager();

            var dataAccess = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var reviewService = new ReviewRatingService(dataAccess);
            var reviewRating = new ReviewRatingManager(reviewService);

            reviewRating.CreateReviewRating(new ReviewRating
            {
                Username = "Zee",
                Message = "Hello",
                StarRating = StarType.Four_Stars,
                FilePath = "C:/Users/Serge/Desktop/images/3.jpg"
            });

            var returnedReview = reviewRating.GetReviewsRatings("ReviewRatingEntity_20210326_08_33_36_3336");

            Console.WriteLine(returnedReview.Message);

            Console.Read();
        }
    }
}
