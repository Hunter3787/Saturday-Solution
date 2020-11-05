using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace LoggingPractice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Host created.");

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            // Logging gets configured here
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context,logging) => // allows for configutring logging.
                {
                    logging.ClearProviders(); // Clears out Microsoft prvided logging.
                    logging.AddConfiguration(context.Configuration.GetSection("Logging")); // Takes Logging section and loads it into new config from appsetting.json.
                    logging.AddDebug();
                    logging.AddConsole(); // EventSource, EventLog, TraceSource, AzureAppServicesFile, AzureAppServicesBlob, ApplicationInsights
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
