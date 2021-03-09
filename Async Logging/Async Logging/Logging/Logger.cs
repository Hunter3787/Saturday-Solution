﻿using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Threading.Tasks;

namespace Producer
{
    public class Logger : ILogger //service
    {
        private LogObject logObject = new LogObject();
        private readonly IConnectionFactory connectionFactory;
        private readonly IConnection connection;
        private readonly ISession session;
        private readonly IMessageProducer producer;
        private bool isDisposed = false;
        public Logger()
        {
            this.connectionFactory = new ConnectionFactory("tcp://localhost:61616");
            this.connection = this.connectionFactory.CreateConnection();
            this.connection.Start();
            this.session = connection.CreateSession();

            IDestination destination = session.GetQueue("LoggingQueue");

            this.producer = this.session.CreateProducer(destination);
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
            await logger.LogAsync(message, LogLevel.Information, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        public static async void LogWarning(this Logger logger, string message)
        {
            await logger.LogAsync(message, LogLevel.Warning, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        public static async void LogError(this Logger logger, string message)
        {
            await logger.LogAsync(message, LogLevel.Error, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
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