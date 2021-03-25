using APB.App.Services;
using APB.App.Managers;
using System;
using APB.App.DomainModels;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggingConsumerManager loggingService = new LoggingConsumerManager();

            ReviewRatingManager reviewRatingManager = new ReviewRatingManager();

            ReviewRating reviewRating = new ReviewRating();

            reviewRating.Message = "Hello";
            reviewRating.StarRating = StarType.Five_Stars;
            reviewRating.ImagePath = "C:/Users/Serge/Desktop/images/5.jpg";

            reviewRatingManager.ReviewRating(reviewRating);

            LoggingProducerService logger1 = LoggingProducerService.GetInstance;

            logger1.LogInformation("This is an Information Log");

            LoggingProducerService logger2 = LoggingProducerService.GetInstance;

            logger2.LogWarning("This is a Warning Log");

            LoggingProducerService logger3 = LoggingProducerService.GetInstance;

            logger3.LogError("This is an Error Log");

            Console.WriteLine("Hello");

            Console.Read();
        }
    }
}
