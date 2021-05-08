using System;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;
using AutoBuildApp.DomainModels;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Logging;
using Logging.Globals;
using AutoBuildApp.DataAccess;

/// <summary>
/// References used from file: Solution Items/References.txt 
/// [1,10,12,13]
/// </summary>

namespace AutoBuildApp.Logging
{
    /// <summary>
    /// This class acts as the consumer for the logging Objects, will be consumed from the
    /// queue and uploaded to the database from this class.
    /// </summary>
    public class LoggingConsumerManager : IDisposable // This will implement the IDisposable interface which is used to clean up and close connections.
    {
        private readonly IConnectionFactory _connectionFactory; // This acts as an entry point to client APIs in this case ActiveMQ.
        private readonly IConnection _connection; // This allows us to establish a persistent connection between client and server.
        private readonly ISession _session; // Stores a session which is essentially the shared context between participants in a communication exchange.
        private readonly IMessageConsumer _consumer; // This is the interface that a client uses to consume/recieve messages from the ActiveMQ.
        private bool _isDisposed = false; // Bool to check if items have been disposed of, initialized to false because no items shall be pre-disposed.

        private const string _URI = LoggingGlobals.ACTIVEMQ_URI; // This sets a constant connection string to the Queue.
        private const string _DESTINATION = LoggingGlobals.DESTINATION; // Destination or the name of the Queue that the JSON strings are stored into.

        // Desfault constructor for the LoggingManager, will establish connections to the Queue.
        public LoggingConsumerManager()
        {
            _connectionFactory = new ConnectionFactory(_URI); // Stores the connection string.
            _connection = _connectionFactory.CreateConnection(); // Creates a connection to the connection string destination path.
            _connection.Start(); // Begins the connection to the specified location.


            // Sets the shared context of the session into session. AutoAcknowledge is an enum that assumes that the message was recieved successfully.
            _session = _connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

            IDestination destination = _session.GetQueue(_DESTINATION); // Gets the name of the Queue used and sets it to the destination.
            _consumer = _session.CreateConsumer(destination); // This sets up for messages to be consumed from the Queue.
            _consumer.Listener += new MessageListener(OnMessage); // Will listen for the messages from the queue.
        }
        // This is a method used to consume messages, deserialize the JSON strings into LogObjects and send those logs to the data access layer for further processing.
        public void OnMessage(IMessage message)
        {

            ITextMessage textMessage = message as ITextMessage; // Created a message to be used to get the JSON string.
            Logger logger = JsonConvert.DeserializeObject<Logger>(textMessage.Text); // This will deserialize JSON strings and re-store them as a LogObject.

            var loggerEntity = new LoggerEntity()
            {
                Message = logger.Message,
                LogLevel = (LogTypeEntity)logger.LogLevel,
                Event = logger.Event.ToString(),
                EventValue = logger.EventValue,
                Username = logger.Username,
                DateTime = logger.DateTime
            };

            // Will initialize the LoggerDataAccess with a connection string.

            //commented out

           // LoggerDAO loggerDataAccess = new LoggerDAO(LoggingGlobals.DB_CONNECTION); 
           // loggerDataAccess.CreateLogRecord(loggerEntity); // send the log object through to be sent to the database.

            // added by zeinab
            LoggingDAO loggingDAO = new LoggingDAO(LoggingGlobals.DB_CONNECTION);
            loggingDAO.InsertLog(loggerEntity);
        
        }
        // This method will simply close all connections and sessions and set the isDisposed bool to true to state that the connections have been closed.
        public void Dispose()
        {
            // Will dispose if not already disposed.
            if (!_isDisposed)
            {
                _consumer.Dispose();
                _session.Dispose();
                _connection.Dispose();
                _isDisposed = true;
            }
        }
    }
}
