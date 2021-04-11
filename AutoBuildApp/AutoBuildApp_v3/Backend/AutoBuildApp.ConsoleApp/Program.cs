using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers;
using AutoBuildApp.Services;
using System;

namespace AutoBuildApp.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ReviewRatingDAO reviewRatingDAO = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            ReviewRatingService reviewRatingService = new ReviewRatingService(reviewRatingDAO);
            ReviewRatingManager reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            reviewRatingManager.GetAllReviewsRatings();

            var result = reviewRatingManager.GetReviewsRatings("30000");

            Console.WriteLine("Hello World");

            Console.Read();
        }
    }
}
