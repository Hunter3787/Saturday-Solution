using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LoggingPractice2._0.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation(LoggingId.privacyPageNavCode,"\nSession ID: {sID} \n" +
                                                                "Public IP: {pIP} \n" +
                                                                "Privacy Page has been navigated to {Time}", HttpContext.Session.Id.ToString(),LoggingId.GetLocalIPAddress() , DateTime.UtcNow);
        }
    }
}
