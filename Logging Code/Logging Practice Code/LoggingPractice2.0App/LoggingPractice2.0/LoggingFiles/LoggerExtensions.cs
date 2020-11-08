using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingPractice2._0.LoggingFiles
{
    public static class LoggerExtensions
    {
        public static ILoggingBuilder AddLoggerType(this ILoggingBuilder builder, Action<LoggingOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, LoggingProvider>();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
