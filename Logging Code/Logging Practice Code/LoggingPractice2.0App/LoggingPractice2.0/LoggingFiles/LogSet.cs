using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LoggingPractice2._0.LoggingFiles
{
    public class LogSet
    {
        public const int webRunningCode = 1001;
        public const int ipCode = 1002;
        public const int privacyPageNavCode = 1003;
        public const int clickedButton = 1004;

        private readonly ILogger<LogSet> _logger;

        public LogSet(ILogger<LogSet> logger)
        {
            _logger = logger;
            //return _logger;
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

        public static string getSessionID()
        {
            return "Session id";
        }

        public void infoLogger(string msg)
        {
            _logger.LogInformation("Session ID: {sID} \n IP Address: {IP} \n {Message} \n {Time}", getSessionID(), GetLocalIPAddress(), msg, DateTime.UtcNow);
        }
    }
}
