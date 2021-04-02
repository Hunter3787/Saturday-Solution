namespace APB.App.Entities
{
    /// <summary>
    /// This is the DTO for a logger object, transfers data through the layers 
    /// so that is is able to be sent to the database without circular dependencies.
    /// </summary>
    public class LoggerEntity
    {
        public string Message { get; set; }

        public LogTypeEntity LogLevel { get; set; }

        public string DateTime { get; set; }
    }
}
