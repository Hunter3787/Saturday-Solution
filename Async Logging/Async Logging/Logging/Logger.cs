using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
//using System.Threading.Tasks;

namespace Producer
{
    public class QueuePublisher
    {
        private const string URI = "tcp://localhost:61616";
        private const string DESTINATION = "LogggingQueue";


        private readonly IConnectionFactory connectionFactory;
        private readonly IConnection connection;
        private readonly ISession session;
        private readonly IMessageProducer producer;

        private bool isDisposed = false;


        public QueuePublisher()
        {

            this.connectionFactory = new ConnectionFactory("tcp://localhost:61616");
            this.connection = this.connectionFactory.CreateConnection();
            this.connection.Start();
            this.session = connection.CreateSession();

            IDestination destination = session.GetQueue("LoggingQueue");

            this.producer = this.session.CreateProducer(destination);
        }

        public void Testing(LogObject log)
        {
            if (!this.isDisposed)
            {
                IObjectMessage objectMessage = session.CreateObjectMessage(log);
                producer.Send(objectMessage);
                //ITextMessage textMessage = this.session.CreateTextMessage(message);
                //this.producer.Send(textMessage);
            }
            else
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }


        }

        #region IDisposable Members
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
        #endregion

    }
    public class Logger : ILogger
    {
        QueuePublisher queuePublisher = new QueuePublisher();
        private LogObject logObject = new LogObject();
        public bool Log(string message, LogLevel level)
        {
            logObject.Message = message;
            logObject.LevelLog = level;
            sendLog();
            Console.WriteLine("hello");
            return true;
        }
        public Task<bool> LogAsync(string message, LogLevel level)
        {
            throw new NotImplementedException();
        }

        public void sendLog()
        {
            queuePublisher.Testing(logObject);
        }
    }

   

    public class LogObject
    {
        private String message;
        public String Message { get; set; }
        private LogLevel levelLog;
        public LogLevel LevelLog {get; set;}

    }

    public static class LoggerEx
    {
        public static bool LogInformation(this Logger logger, string message)
        {
            return logger.Log(message, LogLevel.Information);
        }

        public static bool LogWarning(this Logger logger, string message)
        {
            return logger.Log(message, LogLevel.Warning);
        }
        public static bool LogError(this Logger logger, string message)
        {
            return logger.Log(message, LogLevel.Error);
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
        bool Log(string message, LogLevel level);

        Task<bool> LogAsync(string message, LogLevel level);
    }
}
