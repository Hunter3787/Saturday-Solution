using APB.App.Services;
using APB.App.Managers;
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

            ReviewRatingManager reviewRatingManager = new ReviewRatingManager();

            ReviewRating reviewRating = new ReviewRating();

            reviewRating.Username = "Zee";
            reviewRating.Message = "Hello";
            reviewRating.StarRating = StarType.Five_Stars;
            reviewRating.Picture = Image.FromFile("C:/Users/Serge/Desktop/images/5.jpg");

            reviewRatingManager.ReviewRating(reviewRating);

            Console.Read();
        }
    }
}
