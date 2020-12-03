using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace LoggingApp
{
    public class LoggingId
    {
        public const int webRunningCode = 1001;
        public const int ipCode = 1002;
        public const int privacyPageNavCode = 1003;
        public const int clickedButton = 1004;

        private readonly ILogger<LoggingId> _logger;


        public LoggingId(ILogger<LoggingId> logger)
        {
            _logger = logger;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Invalid");
        }

        public string getSessionID()
        {
            return "Session id";
        }

        public void infoLogger(string msg)
        {
            _logger.LogInformation("Session ID: {sID} \n IP Address: {IP} \n {Message} \n {Time}", getSessionID(), GetLocalIPAddress(), msg, DateTime.UtcNow);
        }
    }
}


//namespace LoggingPractice2._0.Pages
//{
//    public class PrivacyModel : PageModel
//    {
//        private readonly ILogger<PrivacyModel> _logger;

//        public PrivacyModel(ILogger<PrivacyModel> logger)
//        {
//            _logger = logger;
//        }

//        public void OnGet()
//        {
//            _logger.LogInformation(LoggingId.privacyPageNavCode, "\nSession ID: {sID} \n" +
//                                                                "Public IP: {pIP} \n" +
//                                                                "Privacy Page has been navigated to {Time}", HttpContext.Session.Id.ToString(), LoggingId.GetLocalIPAddress(), DateTime.UtcNow);
//        }
//    }
//}