using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LoggingPractice.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            //// Levels of logging
            //_logger.LogTrace("Trace Log"); // heavy debugging detailed view on what is happening.
            //_logger.LogDebug("Debug Log"); // heavy debugging info for data of each value/attribute.
            //_logger.LogInformation(LoggingId.DemoCode,"Info Log"); // Flow of application, how users are using application.  **MOST USED**.
            //_logger.LogWarning("Error log"); // throwing an exception that has been caught (open file and file isnt there)   **MOST USED**.
            //_logger.LogError("error log"); // exception that causes a crash, unexpected error (database now avalible or log file is corrupted).
            //_logger.LogCritical("critical log"); // application is crashing, website is down, losing data, losing sale, server down.

            _logger.LogError("The server went down temporarily at {Time}", DateTime.UtcNow); // Can filter by time by sending as an argument.
        }
    }

    public class LoggingId
    {
        public const int DemoCode = 1001;
    }
}
