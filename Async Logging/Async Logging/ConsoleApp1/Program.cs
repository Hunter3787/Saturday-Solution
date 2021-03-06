using Producer;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger();

            logger.LogInformation("Hello");

            logger.LogWarning("Hello");

            logger.LogError("Hello");

            Console.WriteLine("Hello World!");
        }
    }
}
