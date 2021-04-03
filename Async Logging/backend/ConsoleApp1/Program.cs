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
