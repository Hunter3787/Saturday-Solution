using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingPractice2._0.LoggingFiles
{
    public class LoggingProvider : ILoggerProvider // Will use the Ilogger provider.
    {
        public readonly LoggingOptions Options; // sets instance for this class of options.

        public LoggingProvider(IOptions<LoggingOptions> _options) // Default constructor. 
        {
            Options = _options.Value; // 

            if(!Directory.Exists(Options.FolderPath)) // Check to see if directory is made.
            {
                Directory.CreateDirectory(Options.FolderPath); // Will create directory if not made.
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(this); // Will return a new log instance, allows for custom log outputs.
        }

        public void Dispose() // Not needed or used.
        {
        }
    }
}
