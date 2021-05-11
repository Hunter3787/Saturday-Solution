//using AutoBuildApp.Api.HelperFunctions;
//using AutoBuildApp.DataAccess;
//using AutoBuildApp.DataAccess.Entities;
//using AutoBuildApp.Logging;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;

//namespace AutoBuildApp.CrossCutting.Tests
//{
//    [TestClass]
//    public class LoggingServiceTest
//    {





//        ConnectionManager conString = ConnectionManager.connectionManager;
//        [TestMethod]
//        public void TestMethod1()
//        {



//        }


//        [TestMethod]
//        public void SendLogToDB_ReturnTrueRowsAffected()
//        {
//         // Creates the local instance for the logger
//            LoggingProducerService logger;
//            logger = LoggingProducerService.GetInstance;


//            // creating an instance of the loggerDAO:

//            string connection = conString.GetConnectionStringByName("MyConnection");
//            LoggerDAO loggerDAO = new LoggerDAO(connection);


//            //logger.LogInformation("testing the send log to db!!");

//            LoggerEntity loggerEntity = new LoggerEntity()
//            {
//                Message = "this is a test from Unit test",
//                LogLevel = LogTypeEntity.Information,
//                DateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:FFFFFFF")
//            };
//            bool actualBool = loggerDAO.CreateLogRecord(loggerEntity);
//            Assert.IsTrue(actualBool);
//        }


//        [TestMethod]
//        public void SendLogToConsumerToStoreIntoDB_ReturnTrueRowsAffected()
//        {
//            // Creates the local instance for the logger
//            LoggingProducerService logger;
//            logger = LoggingProducerService.GetInstance;


//            // creating an instance of the loggerDAO:

//            string connection = conString.GetConnectionStringByName("MyConnection");
//            LoggerDAO loggerDAO = new LoggerDAO(connection);


//            logger.LogInformation("testing the rgwrgwrgsend log hghfdhfdfd");

           
//        }

//        [TestMethod]
//        public void SendLogToDBInCaseOfGuestUser_ReturnTrueRowsAffected()
//        {
//            // Creates the local instance for the logger


//            // creating an instance of the loggerDAO:

//            string connection = conString.GetConnectionStringByName("MyConnection");
//            LoggingDAO SP_loggingDAO  = new LoggingDAO(connection);


//            //logger.LogInformation("testing the send log to db!!");

//            LoggerEntity loggerEntity = new LoggerEntity()
//            {
//                Message = "U test 3!!!!",
//                LogLevel = LogTypeEntity.Information,
//                DateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
//                //Username = "Zeina",
//                //Event = EventType.None.ToString(),
//                //EventValue = "U test 2",
                
//            };

//            // utilizing the logger procedure:

//            bool actualBool = SP_loggingDAO.InsertLog(loggerEntity);
//            Assert.IsTrue(actualBool);





//        }






//    }
//}
