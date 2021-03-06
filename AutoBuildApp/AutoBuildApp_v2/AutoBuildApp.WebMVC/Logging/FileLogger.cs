using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Logging.LoggingFiles
{
    public class FileLogger : ILogger // Use ILogger interface.
    {
        
        public readonly LoggingProvider _provider;

        public FileLogger([NotNull] LoggingProvider provider) // Default constructor for this class. It should not be null. Also passing in an object from the LoggingProvider class as a parameter.
        {
            _provider = provider;
        }

        public IDisposable BeginScope<TState>(TState state) // Uneeded.
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None; // Loglevel.none (basically a useless log level) will not be enabled in this custom logging implement.
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) // This allows us to customize the output of our logs, any output changes will be made here!!
        {
           
            if (!IsEnabled(logLevel))
            {
                return; // makes sure the log level is actually enabled. (not a loglevel.none log)
            }
            // Creates the file path of the folder, grab folder and file path from appsettings.json usin the LoggingOptions file, replace the date with the current date (so that daily logs are made).
            var fullFilePath = string.Format("{0}/{1}", _provider.Options.FolderPath, _provider.Options.FilePath.Replace("{date}", DateTime.UtcNow.ToString("yyyyMMdd")));

         
            // Creates the actual log file inside the log folder created above with all the attributes that we want and need, captures and prints the exceptions in the case of a warning level log, Captures errors and the trace of an error.
            var logRecord = string.Format("{0},[{1}],({2}),{3},({4}),({5})", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), logLevel.ToString(), formatter(state, exception), (exception != null ? exception.StackTrace : ""), LogSet.GetLocalIPAddress(), LogSet.getSessionID());

            using (var streamWriter = new StreamWriter(fullFilePath, true)) // creates a new streamWriter
            {
                streamWriter.WriteLine(logRecord); // this will pass in the log file that we created above.
            }
        }

    }
}
