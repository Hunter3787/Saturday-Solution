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
            Logger logger = new Logger();

            LoggingManager loggingManager = new LoggingManager();

            logger.LogInformation("This is an Information Log");

            logger.LogWarning("This is a Warning Log");

            logger.LogError("This is an Error Log");

            Console.ReadKey();
        }

        static void subscriber_OnMessageReceived(string message)
        {
            Console.WriteLine("message fron Queue! " + message);
        }
    }

}
