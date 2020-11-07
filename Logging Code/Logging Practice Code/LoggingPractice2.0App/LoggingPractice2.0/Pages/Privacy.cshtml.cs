﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LoggingPractice2._0.Pages
{
    public class PrivacyModel : PageModel
    {
        private string msg = "has been logged";
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation(LoggingId.privacyPageNavCode,"{IP} Has navigated to Privacy Page to at {Time}",LoggingId.GetLocalIPAddress(), DateTime.UtcNow);
        }
    }
}
