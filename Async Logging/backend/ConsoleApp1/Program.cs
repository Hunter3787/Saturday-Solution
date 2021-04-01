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
            var reviewRatingManager = new ReviewRatingManager(reviewService);

            reviewRatingManager.CreateReviewRating(new ReviewRating
            {
                Username = "Nick",
                Message = "Hello",
                StarRating = StarType.Four_Stars,
                FilePath = "C:/Users/Serge/Desktop/images/3.jpg"
            });

            //var returnedReview = reviewRatingManager.GetReviewsRatings("30002");

            //Console.WriteLine(returnedReview.Username);


            var list = reviewRatingManager.GetAllReviewsRatings();

            foreach (var review in list)
            {
                Console.WriteLine(review.Username);
            }

            //Console.WriteLine(reviewRatingManager.GetAllReviewsRatings().Count);

            Console.Read();
        }
    }
}
