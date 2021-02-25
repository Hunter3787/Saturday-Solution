using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Moq;
using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoBuildApp.Loggg;

namespace UnitTests {
    public class Tests {
        private FileLogger fileLogger;
        private Mock<IOptions<LoggingOptions>> optionsMock;
        private LoggingProvider loggingProvider;


        [SetUp]
        public void Setup() {
            // We have a dependency when we create our constructor: _options. 
            // We need to mock it (create a test object where we can simulate the behavior).
            optionsMock = new Mock<IOptions<LoggingOptions>>();

            // Setup is where we simulate the behavior. Here, I'm saying that for this mocked object, I want its FolderPath to be "Test".
            // I chose test because FolderPath is really a string, it's not necessary to actually put a whole folder path.
            optionsMock.Setup(x => x.Value.FolderPath).Returns("Test");

            // Now, we need to create an instance of LoggingProvider. ".Object" just returns the actual IOptions<LoggingOptions> object.
            loggingProvider = new LoggingProvider(optionsMock.Object);

            fileLogger = new FileLogger(loggingProvider);
        
        }

        [Test] 
        public void GetLocalIPAddress_LoggingId_ValidIP() {
            string result = LoggingId.GetLocalIPAddress();
            Assert.IsTrue(result != null);
        }

        [Test]
        public void GetLocalIPAddress_LogSet_ValidIP() {
            string result = LogSet.GetLocalIPAddress();
            Assert.IsTrue(result != null);
        }

        // Not sure how to correctly unit test this one. This doesn't compile, but I'll leave it here as a starting place.
        [Test]
        public void AddFileLogger_Success() {
            //var mockBuilder = new Mock<ILoggingBuilder>();
            //mockBuilder.Setup(x => x.Services.AddSingleton<ILoggerProvider, LoggingProvider>());
            //var mock = new Mock<Action<LoggingOptions>>();
            //LoggingPractice2._0.LoggingFiles.LoggerExtensions.AddFileLogger(mockBuilder.Object, mock.Object);

            //mockBuilder.Verify(x => x.Services.AddSingleton<ILoggerProvider, LoggingProvider>(), Times.Once());
            //mockBuilder.Verify(x => x.Services.Configure(mock.Object), Times.Once());
        }

        [Test]
        public void IsEnabled_True() {
            LogLevel logLevel = LogLevel.Information;
            bool IsEnabledResponse = fileLogger.IsEnabled(logLevel);

            Assert.IsTrue(IsEnabledResponse);
        }

        [Test]
        public void IsEnabled_False() {
            LogLevel logLevel = LogLevel.None;
            bool IsEnabledResponse = fileLogger.IsEnabled(logLevel);

            Assert.IsFalse(IsEnabledResponse);
        }

        [Test]
        public void FileLoggerConstructor_Success() {
            var mock = new Mock<LoggingProvider>(optionsMock.Object);
            var mockObject = mock.Object;
            FileLogger result = new FileLogger(mockObject);

            Assert.AreEqual(result._provider, mockObject);
        }

        [Test]
        public void LoggingProviderConstructor_Success() {
            // Verifies that the correct folder path gets read. 
            Assert.AreEqual("Test", loggingProvider.Options.FolderPath);
        }

        [Test]
        public void CreateLogger_Success() {
            FileLogger result = (FileLogger)loggingProvider.CreateLogger("");

            Assert.IsNotNull(result);
            Assert.AreEqual(loggingProvider, result._provider);
        }
    }
}