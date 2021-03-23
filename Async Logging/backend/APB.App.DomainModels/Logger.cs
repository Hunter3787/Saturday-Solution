using System;

namespace APB.App.DomainModels
{
    public class Logger
    {
        public string Message { get; set; }

        public LogType LogLevel { get; set; }

        public string DateTime { get; set; }
    }
}
