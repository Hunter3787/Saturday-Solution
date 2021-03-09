﻿using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
//using System.Threading.Tasks;

namespace Producer
{
    public class QueuePublisher
    {
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
            if (!isDisposed)
            {
                string json = JsonConvert.SerializeObject(log, Formatting.Indented);

                //IObjectMessage objectMessage = this.session.CreateObjectMessage(log);
                //this.producer.Send(objectMessage);
                ITextMessage textMessage = session.CreateTextMessage(json);
                producer.Send(textMessage);
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
    public class Logger : ILogger //service
    {
        QueuePublisher queuePublisher = new QueuePublisher();
        private LogObject logObject = new LogObject();
        public bool Log(string message, LogLevel level)
        {
            logObject.Message = message;
            logObject.LevelLog = level;
            sendLog();
            return true;
        }
        // notice here Task<bool> -> so this task return an boolean 
        public Task<bool> LogAsync(string message, LogLevel level)
        {
            throw new NotImplementedException();
            //logObject.Message = message;
           // logObject.LevelLog = level;
           

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
            Console.WriteLine("LogInfo");
            return logger.Log(message, LogLevel.Information);
        }

        public static bool LogWarning(this Logger logger, string message)
        {
            Console.WriteLine("LogWarn");
            return logger.Log(message, LogLevel.Warning);
        }
        public static bool LogError(this Logger logger, string message)
        {
            Console.WriteLine("LogError");
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
