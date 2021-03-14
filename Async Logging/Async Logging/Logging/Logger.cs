using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Producer
{
    public sealed class Logger : ILogger //service
    {
        private LogObject logObject = new LogObject(); // Initializes a log object for storing and transfering data about a log.
        private readonly IConnectionFactory connectionFactory; // This acts as an entry point to client APIs in this case ActiveMQ.
        private readonly IConnection connection; // This allows us to establish a persistent connection between client and server.
        private readonly ISession session; // Stores a session which is essentially the shared context between participants in a communication exchange.
        private readonly IMessageProducer producer; // This is the interface that a client uses to send messages to the ActiveMQ.
        private bool isDisposed = false; // Bool to check if items have been disposed of, initialized to false because no items shall be pre-disposed.
        private int counter = 0; // Counter int that will be incremented to keep track of the singleton.


        private static Logger instance = null; // Initializes the logger object to zero, it has not been called yet.

        private static readonly object padlock = new object();

        // This function will be used to get the logger instance or create a new one if one has not been created.
        public static Logger GetInstance 
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        // If there is no instance of logger, then a new one will be creates, and only one.
                        if (instance == null)
                        {
                            instance = new Logger();
                        }
                    }
                }
                return instance;
            }
        }
        // Logger standard constructor, will establish connection when new logger is created.
        private Logger() 
        {
            this.connectionFactory = new ConnectionFactory("tcp://localhost:61616"); // Stores the connection string.
            this.connection = this.connectionFactory.CreateConnection(); // Creates a connection to the connection string destination path.
            this.connection.Start(); // Begins the connection to the specified location.
            this.session = connection.CreateSession(); // Sets the shared context of the session into session.

            IDestination destination = session.GetQueue("LoggingQueue"); // Gets the name of the Queue used and sets it to the destination.

            this.producer = this.session.CreateProducer(destination); // This sets up for messages to be sent to the location of LoggingQueue.

            counter++; // Increments the count of the Logger to keep track of instances.
        }
        // Constructor for log and sets operations for log to be sent to the Queue.
        public bool Log(string message, LogLevel level, String dateTime)
        {
                // stores log variables into the LogObject to be sent to the Queue.
                logObject.Message = message; 
                logObject.LevelLog = level;
                logObject.Datetime = dateTime;
                sendLog(logObject); // method used to send LogObject to the Queue.
                return true;
        }
        // Constructor for log and sets operations for log to be asynchnonously sent to the Queue.
        // These will be asynchronusly sent to the Queue by starting a new thread.
        public async Task LogAsync(string message, LogLevel level, String dateTime)
        {
            // stores log variables into the LogObject to be sent to the Queue.
            logObject.Message = message;
            logObject.LevelLog = level;
            logObject.Datetime = dateTime;
            sendLog(logObject); // method used to send LogObject to the Queue.
            await Task.Delay(2000);
        }
        public void sendLog(LogObject log)
        {
            // If the connection has not been disposed, then send the object to the Log.
            if (!isDisposed)
            {
                string json = JsonConvert.SerializeObject(log, Formatting.Indented); // Serialize the log object into a JSON to be able to insterted clearly into the Queue.

                ITextMessage textMessage = session.CreateTextMessage(json); // This will get the message of the JSON log to be sent to the Queue.
                producer.Send(textMessage); // This finally sends the serialized object to the Queue.
            }
            else
            {
                throw new ObjectDisposedException(this.GetType().FullName); // If the connection is disposed then it will return this exception message.
            }
        }
        // This method will simply close all connections and sessions and set the isDisposed bool to true to state that the connections have been closed.
        public void Dispose()
        {
            // Will dispose if not already disposed.
            if (!this.isDisposed)
            {
                this.producer.Dispose();
                this.session.Dispose();
                this.connection.Dispose();
                this.isDisposed = true;
            }
        }
    }
    // This is the LogObject class and the datatypes that it stores.
    public class LogObject
    {
        private String message;
        private LogLevel levelLog;
        private String dateTime;
        public String Message { get; set; }
        public LogLevel LevelLog {get; set;}
        public String Datetime { get; set; }
    }
    // This is a class for more descriptive Logs to be called at various log levels.
    public static class LoggerEx
    {
        public static async void LogInformation(this Logger logger, string message) // Information log level, this is a variant of Logger.
        {
            // Appends the date and time to the Log for easy info to query.
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
    // Enumerated values for LogLevels.
    public enum LogLevel
    {
        Information,
        Warning,
        Error,
        None
    }
    // This allows the ILogger to be an interface and able to be implemented by other classes.
    public interface ILogger
    {
        bool Log(string message, LogLevel level, String dateTime);
        Task LogAsync(string message, LogLevel level, String dateTime);
    }
}
