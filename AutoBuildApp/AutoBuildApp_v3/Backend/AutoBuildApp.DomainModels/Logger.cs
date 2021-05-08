using AutoBuildApp.DomainModels.Enumerations;
using AutoBuildApp.Models;
/// <summary>
/// References used from file: Solution Items/References.txt 
/// [1]
/// </summary>
namespace AutoBuildApp.DomainModels
{
    /// <summary>
    /// This is the DTO for a logger object, transfers data through the layers 
    /// so that is is able to be sent to the database without circular dependencies.
    /// </summary>
    public class Logger
    {
        // string message, LogType level, EventType eventType, string eventValue, string username, string dateTime

        public string Message { get; set; }

        public LogType LogLevel { get; set; }

        public EventType Event { get; set; }

        public string EventValue { get; set; }

        public string Username { get; set; }

        public string DateTime { get; set; }
    }
}
