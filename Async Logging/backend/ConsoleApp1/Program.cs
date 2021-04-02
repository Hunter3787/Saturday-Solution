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

            //reviewRatingManager.CreateReviewRating(new ReviewRating
            //{
            //    Username = "Nick",
            //    Message = "Nicholas nick",
            //    StarRating = StarType.One_Star,
            //    FilePath = "C:/Users/Serge/Desktop/images/1.jpg"
            //});

            //reviewRatingManager.CreateReviewRating(new ReviewRating
            //{
            //    Username = "Sirage",
            //    Message = "Seeradge",
            //    StarRating = StarType.Two_Stars,
            //    FilePath = "C:/Users/Serge/Desktop/images/2.jpg"
            //});

            //reviewRatingManager.CreateReviewRating(new ReviewRating
            //{
            //    Username = "Dean",
            //    Message = "Summa",
            //    StarRating = StarType.Three_Stars,
            //    FilePath = "C:/Users/Serge/Desktop/images/3.jpg"
            //});

            reviewRatingManager.CreateReviewRating(new ReviewRating
            {
                Username = "Sam",
                Message = "bro",
                StarRating = StarType.Four_Stars,
                FilePath = null
            });

            //reviewRatingManager.CreateReviewRating(new ReviewRating
            //{
            //    Username = "Zeinab",
            //    Message = "hey",
            //    StarRating = StarType.Five_Stars,
            //    FilePath = "C:/Users/Serge/Desktop/images/5.jpg"
            //});

            //var reviewRating = new ReviewRating
            //{
            //    EntityId = "30004",
            //    Message = "Bobby!!!!!!!!!",
            //    StarRating = StarType.One_Star,
            //    FilePath = "C:/Users/Serge/Desktop/images/1.jpg"
            //};

            //var result = reviewRatingManager.EditReviewRating(reviewRating);

            //Console.WriteLine(result);

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
