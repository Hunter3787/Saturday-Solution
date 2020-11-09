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
        //public PrivacyModel(Log logger)
        //{
        //    _logSet = logger;
        //}
        public void OnGet()
        {
            //_logSet.InfoLogger("Privacy page");
            _logger.LogInformation("information message");

            try
            {
                throw new Exception("catch");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,"The file is invalid");
            }

            _logger.LogError("The server is down");
        }
    }
}
