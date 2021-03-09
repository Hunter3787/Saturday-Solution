using Consumer;
using DataAccess;
using Producer;
using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = Logger.GetInstance;

            LoggingManager loggingManager = new LoggingManager();

            logger.LogInformation("This is an Information Log");

            Logger logger2 = Logger.GetInstance;

            logger2.LogWarning("This is a Warning Log");

            Logger logger3 = Logger.GetInstance;

            logger3.LogError("This is an Error Log");

            Console.ReadKey();
        }

        static void subscriber_OnMessageReceived(string message)
        {
            Console.WriteLine("message fron Queue! " + message);
        }
    }

}
