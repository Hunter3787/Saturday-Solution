using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingPractice2._0.LoggingFiles
{
    public class FileLogger : ILogger
    {
        protected readonly LoggingProvider _provider;

        public FileLogger([NotNull] LoggingProvider provider)
        {
            _provider = provider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            var fullFilePath = string.Format("{0}/{1}", _provider.Options.FolderPath, _provider.Options.FilePath.Replace("{date}", DateTime.UtcNow.ToString("yyyyMMdd")));

            var logRecord = string.Format("{0}{1}{2}{3}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), logLevel.ToString(), formatter(state, exception), (exception != null ? exception.StackTrace : ""));

            using (var streamWriter = new StreamWriter(fullFilePath, true))
            {
                streamWriter.WriteLine(logRecord);
            }
        }

    }
}
