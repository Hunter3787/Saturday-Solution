namespace APB.App.DomainModels
{
    /// <summary>
    /// This is the DTO for a logger object, transfers data through the layers 
    /// so that is is able to be sent to the database without circular dependencies.
    /// </summary>
    public class Logger
    {
        public string Message { get; set; }

        public LogType LogLevel { get; set; }

        public string DateTime { get; set; }
    }
}
