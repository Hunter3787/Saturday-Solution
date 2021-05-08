using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoBuildApp.Models;
using AutoBuildApp.DomainModels;
using AutoBuildApp.DomainModels.Enumerations;
using Logging;
using Logging.Globals;

/// <summary>
/// References used from file: Solution Items/References.txt 
/// [1,7-13]
/// </summary>

namespace AutoBuildApp.Logging
{
    /// <summary>
    /// This class will be used to send logs to a Queue, this is the producer end that will
    /// produce the queue with logs.
    /// </summary>
    public sealed class LoggingProducerService : ILogger //service
    {
        private Logger _logger; // Initializes a log object for storing and transfering data about a log.
        private readonly IConnectionFactory _connectionFactory; // This acts as an entry point to client APIs in this case ActiveMQ.
        private readonly IConnection _connection; // This allows us to establish a persistent connection between client and server.
        private readonly ISession _session; // Stores a session which is essentially the shared context between participants in a communication exchange.
        private readonly IMessageProducer _producer; // This is the interface that a client uses to send messages to the ActiveMQ.
        private bool _isDisposed = false; // Bool to check if items have been disposed of, initialized to false because no items shall be pre-disposed.
        private int _counter = 0; // Counter int that will be incremented to keep track of the singleton.
        private static LoggingProducerService _instance = null; // Initializes the logger object to zero, it has not been called yet.
        private static readonly object _padlock = new object();

        SemaphoreSlim semaphore = new SemaphoreSlim(1);

        /// <summary>
        /// simple version of singleton "thread saftey"
        /// This function will be used to get the logger instance or create a new one if one has not been created.
        /// </summary>
        public static LoggingProducerService GetInstance 
        {
            get
            {
                // there is a lock taken on the shared object
                /**
                 * in our case we are implementing a "thread 
                 * saftey singleton" by locking the thread in the shared 
                 * logger object. this takes care of the case if 
                 * two different thread both checked " instance == null" 
                 * and the result for both showed as true causing them both
                 * to create instances, THIS VIOLATES THE SINGLETON PATTERn!!
                 * 
                 * now at to address that^ is with the use of a lock, 
                 * problem with this is that it takes a hit on performance
                 * because of the lock required each time...
                 * 
                 */

                lock (_padlock)
                {
                    // then checks whether an instance has been created before 
                    // creating the instance.
                    // If there is no instance of logger, then a new one will be creates, and only one.
                    if (_instance == null)
                    {
                        _instance = new LoggingProducerService();
                    }

                    return _instance;
                }
            }
        }
        // Logger standard constructor, will establish connection when new logger is created.
        private LoggingProducerService() 
        {
            // This will start the logging consumer manager in the background so that logs may be sent to the DB.
            LoggingConsumerManager _loggingConsumerManager = new LoggingConsumerManager();
            
            _connectionFactory = new ConnectionFactory(LoggingGlobals.USE_ASYNC_URI); // Stores the connection string.
            _connection = _connectionFactory.CreateConnection(); // Creates a connection to the connection string destination path.
            _connection.Start(); // Begins the connection to the specified location.
            _session = _connection.CreateSession(); // Sets the shared context of the session into session.

            IDestination destination = _session.GetQueue(LoggingGlobals.DESTINATION); // Gets the name of the Queue used and sets it to the destination.

            _producer = _session.CreateProducer(destination); // This sets up for messages to be sent to the location of LoggingQueue.

            _counter++; // Increments the count of the Logger to keep track of instances.
        }

        // Constructor for log and sets operations for log to be sent to the Queue.
        public bool Log(string message, LogType level, string dateTime)
        {
            // stores log variables into the LogObject to be sent to the Queue.
            _logger.Message = message;
            _logger.LogLevel = level;
            _logger.DateTime = dateTime;
            SendLog(_logger); // method used to send LogObject to the Queue.
            return true;
        }

        // Constructor for log and sets operations for log to be asynchronously sent to the Queue.
        // These will be asynchronously sent to the Queue by starting a new thread.
        public async Task<bool> LogAsync(string message, LogType level, string dateTime)
        {
            _logger = new Logger(); // store a new log object every time a new log is called.

            #region Logging write lock implementation
            // This will ensure that only one write operation is happening at a single moment.

            //await semaphore.WaitAsync();
            //try
            //{
            //    logObject.Message = message;
            //    logObject.LevelLog = level;
            //    logObject.Datetime = dateTime;
            //    sendLog(logObject); // Call function to send log object to the queue.
            //    Thread.Sleep(2000);
            //}
            //finally
            //{
            //    semaphore.Release();
            //}

            // stores log variables into the LogObject to be sent to the Queue.
            #endregion

            _logger.Message = message;
            _logger.LogLevel = level;
            _logger.DateTime = dateTime;
            var result = await Task.Run(() => SendLog(_logger)); // method used to send LogObject to the Queue.

            return result;
        }


        // Constructor for log and sets operations for log to be sent to the Queue.
        public bool Log(string message, LogType level, EventType eventType, string eventValue, string username, string dateTime)
        {
            // stores log variables into the LogObject to be sent to the Queue.
            _logger.Message = message; 
            _logger.LogLevel = level;
            _logger.Event = eventType;
            _logger.EventValue = eventValue;
            _logger.Username = username;
            _logger.DateTime = dateTime;
            SendLog(_logger); // method used to send LogObject to the Queue.
            return true;
        }

        // Constructor for log and sets operations for log to be asynchronously sent to the Queue.
        // These will be asynchronously sent to the Queue by starting a new thread.
        public async Task<bool> LogAsync(string message, LogType level, EventType eventType, string eventValue, string username, string dateTime)
        {
            _logger = new Logger(); // store a new log object every time a new log is called.

            #region Logging write lock implementation
            // This will ensure that only one write operation is happening at a single moment.

            //await semaphore.WaitAsync();
            //try
            //{
            //    logObject.Message = message;
            //    logObject.LevelLog = level;
            //    logObject.Datetime = dateTime;
            //    sendLog(logObject); // Call function to send log object to the queue.
            //    Thread.Sleep(2000);
            //}
            //finally
            //{
            //    semaphore.Release();
            //}

            // stores log variables into the LogObject to be sent to the Queue.
            #endregion

            _logger.Message = message;
            _logger.LogLevel = level;
            _logger.Event = eventType;
            _logger.EventValue = eventValue;
            _logger.Username = username;
            _logger.DateTime = dateTime;
            var result = await Task.Run(() => SendLog(_logger)); // method used to send LogObject to the Queue.

            return result;
        }
        public bool SendLog(Logger log)
        {
            // If the connection has not been disposed, then send the object to the Log.
            if (!_isDisposed)
            {

                string json = JsonConvert.SerializeObject(log, Formatting.Indented); // Serialize the log object into a JSON to be able to insterted clearly into the Queue.
                ITextMessage textMessage = _session.CreateTextMessage(json); // This will get the message of the JSON log to be sent to the Queue.
                _producer.Send(textMessage); // This finally sends the serialized object to the Queue.
                return true;
            }
            else
            {
                throw new ObjectDisposedException(GetType().FullName); // If the connection is disposed then it will return this exception message.
            }
        }
        // This method will simply close all connections and sessions and set the isDisposed bool to true to state that the connections have been closed.
        public void Dispose()
        {
            // Will dispose if not already disposed.
            if (!_isDisposed)
            {
                _producer.Dispose();
                _session.Dispose();
                _connection.Dispose();
                _isDisposed = true;
            }
        }
    }

    // This is a class for more descriptive Logs to be called at various log levels.
    public static class LoggerEx
    {
        public static async void LogInformation(this LoggingProducerService logger, string message, EventType eventType, string eventValue, string username) // Information log level, this is a variant of Logger.
        {
            // Appends the date and time to the Log for easy info to query.
            await logger.LogAsync(message, LogType.Information, eventType, eventValue, username, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        public static async void LogWarning(this LoggingProducerService logger, string message, EventType eventType, string eventValue, string username)
        {
            await logger.LogAsync(message, LogType.Warning, eventType, eventValue, username, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        public static async void LogError(this LoggingProducerService logger, string message, EventType eventType, string eventValue, string username)
        {
            await logger.LogAsync(message, LogType.Error, eventType, eventValue, username, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public static async void LogInformation(this LoggingProducerService logger, string message) // Information log level, this is a variant of Logger.
        {
            // Appends the date and time to the Log for easy info to query.
            await logger.LogAsync(message, LogType.Information, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        public static async void LogWarning(this LoggingProducerService logger, string message)
        {
            await logger.LogAsync(message, LogType.Warning, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        public static async void LogError(this LoggingProducerService logger, string message)
        {
            await logger.LogAsync(message, LogType.Error, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
