namespace APB.App.Entities
{
    public class LoggerEntity
    {
        public string Message { get; set; }

        public LogTypeEntity LogLevel { get; set; }

        public string DateTime { get; set; }
    }
}
