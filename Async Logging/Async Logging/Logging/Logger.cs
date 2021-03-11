using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Producer
{
    public sealed class Logger : ILogger //service
    {
        private LogObject logObject = new LogObject();
        private readonly IConnectionFactory connectionFactory;
        private readonly IConnection connection;
        private readonly ISession session;
        private readonly IMessageProducer producer;
        private bool isDisposed = false;
        private int counter = 0;


        private static Logger instance = null;

        private static readonly object padlock = new object();

        public static Logger GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {

                            instance = new Logger();
                        }
                    }
                }
                return instance;
            }
        }
        private Logger()
        {
            this.connectionFactory = new ConnectionFactory("tcp://localhost:61616");
            this.connection = this.connectionFactory.CreateConnection();
            this.connection.Start();
            this.session = connection.CreateSession();

            IDestination destination = session.GetQueue("LoggingQueue");

            this.producer = this.session.CreateProducer(destination);

            counter++;
        }
        public bool Log(string message, LogLevel level, String dateTime)
        {
            
                logObject.Message = message;
                logObject.LevelLog = level;
                logObject.Datetime = dateTime;
                sendLog(logObject);
                return true;
        }
        public async Task LogAsync(string message, LogLevel level, String dateTime)
        {
            logObject.Message = message;
            logObject.LevelLog = level;
            logObject.Datetime = dateTime;
            sendLog(logObject);
            await Task.Delay(2000);
        }
        public void sendLog(LogObject log)
        {
            
            if (!isDisposed)
            {
                string json = JsonConvert.SerializeObject(log, Formatting.Indented);

                ITextMessage textMessage = session.CreateTextMessage(json);
                producer.Send(textMessage);
            }
            else
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.producer.Dispose();
                this.session.Dispose();
                this.connection.Dispose();
                this.isDisposed = true;
            }
        }
    }
    public class LogObject
    {
        private String message;
        private LogLevel levelLog;
        private String dateTime;
        public String Message { get; set; }
        public LogLevel LevelLog {get; set;}
        public String Datetime { get; set; }
    }
    public static class LoggerEx
    {
        public static async void LogInformation(this Logger logger, string message)
        {
            await logger.LogAsync(message, LogLevel.Information, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:FFFFFFF"));
        }
        public static async void LogWarning(this Logger logger, string message)
        {
            await logger.LogAsync(message, LogLevel.Warning, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:FFFFFFF"));
        }
        public static async void LogError(this Logger logger, string message)
        {
            await logger.LogAsync(message, LogLevel.Error, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:FFFFFFF"));
        }
    }
    public enum LogLevel
    {
        Information,
        Warning,
        Error,
        None
    }
    public interface ILogger
    {
        bool Log(string message, LogLevel level, String dateTime);
        Task LogAsync(string message, LogLevel level, String dateTime);
    }
}
