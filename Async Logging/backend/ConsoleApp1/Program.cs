using APB.App.Managers;
using APB.App.Services;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggingConsumerService loggingManager = new LoggingConsumerService(); 

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
