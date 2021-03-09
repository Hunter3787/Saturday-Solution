using System;
using System.Collections.Generic;
using System.Text;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Newtonsoft.Json;
using Producer;
using DataAccess;

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
        }

        public void OnMessage(IMessage message)
        {
            ITextMessage textMessage = message as ITextMessage;
            LogObject logObject = JsonConvert.DeserializeObject<LogObject>(textMessage.Text);
            LoggerDataAccess loggerDataAccess = new LoggerDataAccess("Server = localhost; Database = DB; Trusted_Connection = True;");

            loggerDataAccess.CreateLogRecord(logObject);
            //this.OnMessageReceived(textMessage.Text);
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
