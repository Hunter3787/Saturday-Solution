using Consumer;
using Producer;
using System;
using System.Configuration;

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

            loggingManager.OnMessageReceived += new MessageReceivedDelegate(subscriber_OnMessageReceived);


           // Console.WriteLine(GetConnectionStringByName());

            Console.ReadKey();
        }

        static void subscriber_OnMessageReceived(string message)
        {
            Console.WriteLine("message fron Queue!" + message);
        }

        static string GetConnectionStringByName()
        {

            string retVal = null;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["ZeeC"];
            // If found, return the connection string.
            if (settings != null)
                retVal = settings.ConnectionString;
            Console.WriteLine($"the retval:   {retVal}");
            return retVal;
        }




    }

}
