using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBuildApp.Loggg
{
    public static class LoggerExtensions // Needs to be static so that it can be called anywhere.
    {
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, Action<LoggingOptions> configure) // pass in an instance of a ILoggingBuilder and Options to access appsettings.
        {
            builder.Services.AddSingleton<ILoggerProvider, LoggingProvider>(); // Creates a new singleton for the IloggerProvider.
            builder.Services.Configure(configure); // Uses the configure method to configure the singleton logger with the specified values, also used to identify and navigate appsettings.

            return builder; // this will return the builder that we have configured. 
        }
    }
}
