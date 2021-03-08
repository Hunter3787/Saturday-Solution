using Consumer;
using DataAccess;
using Producer;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //LoggerDataAccess loggerDataAccess = new LoggerDataAccess("Server = localhost; Database = DB; Trusted_Connection = True;");

            Logger logger = new Logger();

            LoggingManager loggingManager = new LoggingManager();

            logger.LogInformation("This is an Information Log");

            logger.LogWarning("This is a Warning Log");

            logger.LogError("This is an Error Log");

            loggingManager.OnMessageReceived += new MessageReceivedDelegate(subscriber_OnMessageReceived);

            Console.ReadKey();
        }

        static void subscriber_OnMessageReceived(string message)
        {

            Console.WriteLine("message fron Queue! " + message);

            Console.WriteLine("message fron Queue!" + message);

        }
    }

}
