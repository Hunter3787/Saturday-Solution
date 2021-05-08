using AutoBuildApp.DomainModels.Enumerations;
using AutoBuildApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    // This allows the ILogger to be an interface and able to be implemented by other classes.
    public interface ILogger
    {
        bool Log(string message, LogType level, EventType eventType, string eventValue, string username, string dateTime);
        Task<bool> LogAsync(string message, LogType level, EventType eventType, string eventValue, string username, string dateTime);

        bool Log(string message, LogType level, string dateTime);
        Task<bool> LogAsync(string message, LogType level, string dateTime);
    }
}
