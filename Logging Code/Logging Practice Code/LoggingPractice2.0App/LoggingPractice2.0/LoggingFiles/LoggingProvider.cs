using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingPractice2._0.LoggingFiles
{
    public class LoggingProvider : ILoggerProvider
    {
        public readonly LoggingOptions Options;

        public LoggingProvider(IOptions<LoggingOptions> _options)
        {
            Options = _options.Value;

            if(!Directory.Exists(Options.FolderPath))
            {
                Directory.CreateDirectory(Options.FolderPath);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new LoggingPractice(this);
        }

        public void Dispose()
        {
        }
    }
}
