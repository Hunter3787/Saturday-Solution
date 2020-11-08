using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LoggingPractice2._0
{
    public class LoggingId
    {
        public const int webRunningCode = 1001;
        public const int ipCode = 1002;
        public const int privacyPageNavCode = 1003;
        public const int clickedButton = 1004;

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
    }
}
