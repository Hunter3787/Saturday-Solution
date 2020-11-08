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
                }).ConfigureLogging((context, logging) =>
                {
                    logging.AddFileLogger(options =>
                    {
                        context.Configuration.GetSection("Logging").GetSection("LoggingFile").GetSection("Options").Bind(options);
                    });
                });
    }
}
