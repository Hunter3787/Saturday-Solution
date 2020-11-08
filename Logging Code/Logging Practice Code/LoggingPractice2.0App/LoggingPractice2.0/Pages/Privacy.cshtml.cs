using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using LoggingPractice2._0.LoggingFiles;

namespace LoggingPractice2._0.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        //private readonly LogSet _logSet;
        //public PrivacyModel(LogSet logger)
        //{
        //    _logSet = logger;
        //}
        public void OnGet()
        {
            // _logSet.infoLogger("Privacy page");
            _logger.LogInformation("Privacy page");
            _logger.LogWarning("warning");
        }
    }
}
