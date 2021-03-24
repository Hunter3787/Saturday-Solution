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

            reviewRatingManager.ReviewRating("Terrible build, very slow computer.", StarType.Five_Stars);

            reviewRatingManager.ReviewRating("Great build! I would but it again!", StarType.Five_Stars);

            reviewRatingManager.ReviewRating("Very bad!", StarType.Two_Stars);

            reviewRatingManager.ReviewRating("I cried", StarType.Four_Stars);

            reviewRatingManager.ReviewRating("It broke", StarType.Three_Stars);

            reviewRatingManager.ReviewRating("Very slow", StarType.One_Star);

            reviewRatingManager.ReviewRating("very fast!!!!!!", StarType.Five_Stars);

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
