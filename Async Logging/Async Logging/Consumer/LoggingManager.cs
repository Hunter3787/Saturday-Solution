using System;
using System.Collections.Generic;
using System.Text;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;
using Producer;
<<<<<<< HEAD
<<<<<<< HEAD
using DataAccess;
using System.Configuration;

=======
>>>>>>> parent of 0622254 (database working)
=======
>>>>>>> parent of 0622254 (database working)

namespace Consumer
{
    public delegate void MessageReceivedDelegate(string message);
    public class LoggingManager : IDisposable
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly IConnection connection;
        private readonly ISession session;
        private readonly IMessageConsumer consumer;
        private bool isDisposed = false;
        public event MessageReceivedDelegate OnMessageReceived;

        private const string URI = "tcp://localhost:61616";
        private const string DESTINATION = "LoggingQueue";
        public LoggerDataAccess loggerDataAccess;


        public LoggingManager()
        {
            this.connectionFactory = new ConnectionFactory(URI);
            this.connection = this.connectionFactory.CreateConnection();
            //this.connection.ClientId = clientId;
            this.connection.Start();
            this.session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
            IDestination destination = session.GetQueue(DESTINATION);
            this.consumer = this.session.CreateConsumer(destination);
            this.consumer.Listener += new MessageListener(OnMessage);
            this.loggerDataAccess = new LoggerDataAccess(GetConnectionStringByName("ZeeC"));
        }

        static string GetConnectionStringByName(string name)
        {
            string retVal = null;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            // If found, return the connection string.
            if (settings != null)
                retVal = settings.ConnectionString;
            Console.WriteLine($"the retval:   {retVal}");
            return retVal;
        }

        public void OnMessage(IMessage message)
        {
            ITextMessage textMessage = message as ITextMessage;
            LogObject logObject = JsonConvert.DeserializeObject<LogObject>(textMessage.Text);
<<<<<<< HEAD
<<<<<<< HEAD
         

            //OnMessageReceived += new MessageReceivedDelegate(subscriber_OnMessageReceived);

=======
>>>>>>> parent of 0622254 (database working)
=======
>>>>>>> parent of 0622254 (database working)
            if (this.OnMessageReceived != null)
            {
                this.OnMessageReceived(logObject.Message);
            }
        }



        #region IDisposable Members

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.consumer.Dispose();
                this.session.Dispose();
                this.connection.Dispose();
                this.isDisposed = true;
            }
        }

        #endregion
    }


}
