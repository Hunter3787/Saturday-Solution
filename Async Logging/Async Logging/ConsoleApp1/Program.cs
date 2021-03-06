using Producer;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger();
            //logger.Log("hello", LogLevel.Information);

            logger.LogInformation("Hello");

            QueuePublisher demo = new QueuePublisher();

            //demo.Testing(logger);

            Console.WriteLine("Hello World!");
        }
    }
}
