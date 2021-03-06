using APB.App.DomainModels;
using APB.App.Managers;
using APB.App.Services;
using APB.App.Entities;
using APB.App.DataAccess;
using NUnit.Framework;
using System;

/// <summary>
/// References used from file: Solution Items/References.txt 
/// [14]
/// </summary>

namespace Logging.UnitTests
{
    [TestFixture]
    public class LoggingTests
    {
        /// <summary>
        /// This test will test if a log has been successfully sent to the message queue.
        /// If it has, then it will return true. If false, then it will return false.
        /// </summary>
        [Test]
        public void Logging_LogAsync_ReturnsTrue()
        {
            // Arrange
            var loggingConsumerManager = new LoggingConsumerManager();
            var logger = LoggingProducerService.GetInstance;

            // Act
            var result = logger.LogAsync("Test", LogType.None, DateTime.UtcNow.ToString());

            // Assert
            Assert.IsTrue(result.Result);
        }

        /// <summary>
        /// This test will verify that a log has been send through to the queue.
        /// </summary>
        [Test]
        public void Logging_SendLog_ReturnsTrue()
        {
            // Arrange
            var loggingConsumerManager = new LoggingConsumerManager();
            var logger = LoggingProducerService.GetInstance;

            var log = new Logger
            {
                Message = "Test",
                LogLevel = LogType.None,
                DateTime = DateTime.UtcNow.ToString()
            };

            // Act
            var result = logger.SendLog(log);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// This test will verify that a log has been sent to the DB
        /// </summary>
        [Test]
        public void Logging_CreateLogRecord_ReturnsTrue()
        {
            // Arrange
            var loggingConsumerManager = new LoggingConsumerManager();
            LoggerDAO loggerDataAccess = new LoggerDAO("Server = localhost; Database = DB; Trusted_Connection = True;");

            var logEntity = new LoggerEntity
            {
                Message = "Test",
                LogLevel = LogTypeEntity.None,
                DateTime = DateTime.UtcNow.ToString()
            };

            // Act
            var result = loggerDataAccess.CreateLogRecord(logEntity);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
