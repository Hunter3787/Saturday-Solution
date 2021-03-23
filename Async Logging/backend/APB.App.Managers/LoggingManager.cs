using System;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;
using APB.App.Services;
using APB.App.DataAccess;

namespace APB.App.Managers
{
    public class LoggingManager : IDisposable // This will implement the IDisposable interface which is used to clean up and close connections.
    {
        private readonly IConnectionFactory connectionFactory; // This acts as an entry point to client APIs in this case ActiveMQ.
        private readonly IConnection connection; // This allows us to establish a persistent connection between client and server.
        private readonly ISession session; // Stores a session which is essentially the shared context between participants in a communication exchange.
        private readonly IMessageConsumer consumer; // This is the interface that a client uses to consume/recieve messages from the ActiveMQ.
        private bool isDisposed = false; // Bool to check if items have been disposed of, initialized to false because no items shall be pre-disposed.

        private const string URI = "tcp://localhost:61616"; // This sets a constant connection string to the Queue.
        private const string DESTINATION = "LoggingQueue"; // Destination or the name of the Queue that the JSON strings are stored into.

        // Desfault constructor for the LoggingManager, will establish connections to the Queue.
        public LoggingManager()
        {
            this.connectionFactory = new ConnectionFactory(URI); // Stores the connection string.
            this.connection = this.connectionFactory.CreateConnection(); // Creates a connection to the connection string destination path.
            this.connection.Start(); // Begins the connection to the specified location.


            // Sets the shared context of the session into session. AutoAcknowledge is an enum that assumes that the message was recieved successfully.
            this.session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

            IDestination destination = session.GetQueue(DESTINATION); // Gets the name of the Queue used and sets it to the destination.
            this.consumer = this.session.CreateConsumer(destination); // This sets up for messages to be consumed from the Queue.
            this.consumer.Listener += new MessageListener(OnMessage); // Will listen for the messages from the queue.
        }
        // This is a method used to consume messages, deserialize the JSON strings into LogObjects and send those logs to the data access layer for further processing.
        public void OnMessage(IMessage message)
        {
            ITextMessage textMessage = message as ITextMessage; // Created a message to be used to get the JSON string.
            LogObject logObject = JsonConvert.DeserializeObject<LogObject>(textMessage.Text); // This will deserialize JSON strings and re-store them as a LogObject.

            // Will initialize the LoggerDataAccess with a connection string.
            LoggerDAO loggerDataAccess = new LoggerDAO("Server = localhost; Database = DB; Trusted_Connection = True;"); 
            loggerDataAccess.CreateLogRecord(logObject); // send the log object through to be sent to the database.
        }
        // This method will simply close all connections and sessions and set the isDisposed bool to true to state that the connections have been closed.
        public void Dispose()
        {
            // Will dispose if not already disposed.
            if (!this.isDisposed)
            {
                this.consumer.Dispose();
                this.session.Dispose();
                this.connection.Dispose();
                this.isDisposed = true;
            }
        }
    }
}
