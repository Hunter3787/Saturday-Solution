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

            ReviewRatingDAO reviewRatingDAO = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            ReviewRatingService reviewRatingService = new ReviewRatingService(reviewRatingDAO);
            ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            ReviewRating reviewRating = new ReviewRating();

            reviewRating.Username = "Zee";
            reviewRating.Message = "Hello";
            reviewRating.StarRating = StarType.Five_Stars;
            reviewRating.FilePath = "C:/Users/Serge/Desktop/images/5.jpg";

            reviewRatingManager.CreateReviewRating(reviewRating);

            Console.Read();
        }
    }
}
