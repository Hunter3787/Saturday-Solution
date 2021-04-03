/// <summary>
/// References used from file: Solution Items/References.txt 
/// [1]
/// </summary>
namespace AutoBuildApp.DomainModels.Enumerations
{
    /// <summary>
    /// This is the enumeration class to differetiate between log levels.
    /// Information: Basic logs, general information.
    /// Warning: Used for try, catch blocks to log when a warning has occured and where.
    /// Error: Used to log crirical 'App-Crashing' errors and where they occur.
    /// </summary>
    public enum LogType
    {
        Information,
        Warning,
        Error,
        None
    }
}
