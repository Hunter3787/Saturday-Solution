using Consumer;
using Producer;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggingService logger = LoggingService.GetInstance; 

            LoggingManager loggingManager = new LoggingManager();

            logger.LogInformation("This is an Information Log");

            LoggingService logger2 = LoggingService.GetInstance;

            logger2.LogWarning("This is a Warning Log");

            LoggingService logger3 = LoggingService.GetInstance;

            logger3.LogError("This is an Error Log");

            Console.Read();
        }
    }
}
