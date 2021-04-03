using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DataAccess.Entities
{
    public class LogEntity
    {
        public string Message { get; set; }
        public LogLevel LevelLog { get; set; }

        public LogEntity()
        {
            LevelLog = LogLevel.Empty;
            Message = "";

        }
        public LogEntity(string msg, LogLevel level)
        {
            LevelLog = level;
            Message = msg;
            ToString();

        }

        public override string ToString()
        {
            return $"the logObject message:  {Message}, log level {LevelLog}";
        }
    }
    public enum LogLevel
    {
        Empty,
        Information,
        Warning,
        Error,
        None
    }
}
