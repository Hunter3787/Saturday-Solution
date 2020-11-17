using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LoggingPractice2._0.LoggingFiles;

namespace LoggingPractice2._0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation(LoggingId.webRunningCode, "Host Created.");
            logger.LogInformation(LoggingId.ipCode, LoggingId.GetLocalIPAddress());
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging((context, logging) => // passes in the host builder and also passes in the parameter for logging.
                {
                    logging.AddFileLogger(options => // Calls the FileLogger.cs file methods.
                    {
                        // sets configurations that we specified, reads from appsettings, gets the required sections for appsettings.json. Binding the options into it so that it can be reference in other places.
                        context.Configuration.GetSection("Logging").GetSection("LoggingFile").GetSection("Options").Bind(options);
                    });
                });
    }
}
