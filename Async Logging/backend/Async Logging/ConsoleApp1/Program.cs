using APB.App.Managers;
using APB.App.Services;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggingManager loggingManager = new LoggingManager(); 

            LoggingService logger1 = LoggingService.GetInstance;

            logger1.LogInformation("This is an Information Log");

            LoggingService logger2 = LoggingService.GetInstance;

            logger2.LogWarning("This is a Warning Log");

            LoggingService logger3 = LoggingService.GetInstance;

            logger3.LogError("This is an Error Log");

            Console.WriteLine("Hello");

            Console.Read();
        }
    }
}
