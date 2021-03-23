using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using APB.App.DomainModels;

namespace APB.App.Services
{
    public sealed class LoggingProducerService : ILogger //service
    {
        private Logger logger; // Initializes a log object for storing and transfering data about a log.
        private readonly IConnectionFactory connectionFactory; // This acts as an entry point to client APIs in this case ActiveMQ.
        private readonly IConnection connection; // This allows us to establish a persistent connection between client and server.
        private readonly ISession session; // Stores a session which is essentially the shared context between participants in a communication exchange.
        private readonly IMessageProducer producer; // This is the interface that a client uses to send messages to the ActiveMQ.
        private bool isDisposed = false; // Bool to check if items have been disposed of, initialized to false because no items shall be pre-disposed.
        private int counter = 0; // Counter int that will be incremented to keep track of the singleton.

        private static LoggingProducerService instance = null; // Initializes the logger object to zero, it has not been called yet.

        private static readonly object padlock = new object();

        SemaphoreSlim semaphore = new SemaphoreSlim(1);

        // This function will be used to get the logger instance or create a new one if one has not been created.
       
        /// <summary>
        /// simple version of singleton "thread saftey"
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

                lock (padlock)
                {
                    // then checks whether an instance has been created before 
                    // creating the instance.
                    // If there is no instance of logger, then a new one will be creates, and only one.
                    if (instance == null)
                    {
                        instance = new LoggingProducerService();
                    }

                    return instance;
                }
            }
        }
        // Logger standard constructor, will establish connection when new logger is created.
        private LoggingProducerService() 
        {
            this.connectionFactory = new ConnectionFactory("tcp://localhost:61616?jms.UseAsyncSend=true"); // Stores the connection string.
            this.connection = this.connectionFactory.CreateConnection(); // Creates a connection to the connection string destination path.
            this.connection.Start(); // Begins the connection to the specified location.
            this.session = connection.CreateSession(); // Sets the shared context of the session into session.

            IDestination destination = session.GetQueue("LoggingQueue"); // Gets the name of the Queue used and sets it to the destination.

            this.producer = this.session.CreateProducer(destination); // This sets up for messages to be sent to the location of LoggingQueue.

            counter++; // Increments the count of the Logger to keep track of instances.
        }
        // Constructor for log and sets operations for log to be sent to the Queue.
        public bool Log(string message, LogType level, string dateTime)
        {
                // stores log variables into the LogObject to be sent to the Queue.
                logger.Message = message; 
                logger.LogLevel = level;
                logger.DateTime = dateTime;
                sendLog(logger); // method used to send LogObject to the Queue.
                return true;
        }

        // Constructor for log and sets operations for log to be asynchronously sent to the Queue.
        // These will be asynchronously sent to the Queue by starting a new thread.
        public async Task LogAsync(string message, LogType level, string dateTime)
        {
            logger = new Logger(); // store a new log object every time a new log is called.

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


            logger.Message = message;
            logger.LogLevel = level;
            logger.DateTime = dateTime;
            //sendLog(logObject);
            await Task.Run(() => sendLog(logger)); // method used to send LogObject to the Queue.

            Console.WriteLine("sent");
        }
        public void sendLog(Logger log)
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

    // This is a class for more descriptive Logs to be called at various log levels.
    public static class LoggerEx
    {
        public static async void LogInformation(this LoggingProducerService logger, string message) // Information log level, this is a variant of Logger.
        {
            // Appends the date and time to the Log for easy info to query.
            await logger.LogAsync(message, LogType.Information, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:FFFFFFF")); 
        }
        public static async void LogWarning(this LoggingProducerService logger, string message)
        {
            await logger.LogAsync(message, LogType.Warning, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:FFFFFFF"));
        }
        public static async void LogError(this LoggingProducerService logger, string message)
        {
            await logger.LogAsync(message, LogType.Error, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:FFFFFFF"));
        }
    }

    // This allows the ILogger to be an interface and able to be implemented by other classes.
    public interface ILogger
    {
        bool Log(string message, LogType level, string dateTime);
        Task LogAsync(string message, LogType level, string dateTime);
    }
}
