using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoBuildApp.ServiceLayer
{
    public class LoggingService : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public bool Log(string message, LogLevel level)
        {
            return true;
        }

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LogAsync(string message, LogLevel level)
        {
            throw new NotImplementedException();
        }
    }

    public static class LoggerEx
    {
        public static bool LogInformation(this LoggingService logger, string message)
        {
            return logger.Log(message, LogLevel.Information);
        }

        public static bool LogWarning(this LoggingService logger, string message)
        {
            return logger.Log(message, LogLevel.Warning);
        }
        public static bool LogError(this LoggingService logger, string message)
        {
            return logger.Log(message, LogLevel.Error);
        }
    }

    public enum LogLevel
    {
        Information,
        Warning,
        Error
    }
}
