using Project.DataAccess;
using Project.DataAccess.Entities;
using System;
using System.Configuration;


namespace Project.Manager
{
    public class SimpleLogServiceForDemo
    {
        private LogEntity logObject;
        private LoggerDAO loggerDAO;
        public SimpleLogServiceForDemo()
        {
            //utilization of the app config
            loggerDAO = new LoggerDAO(GetConnectionStringByName("Connection"));

        }
        static string GetConnectionStringByName(string name)
        {
            string retVal = null;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            // If found, return the connection string.
            if (settings != null)
                retVal = settings.ConnectionString;
            Console.WriteLine("This is the connection string: " + retVal);

            return retVal;
        }

        public void Log(string message, LogLevel level)
        {
            logObject = new LogEntity(message, level);
            // this.logObject.Message = message;
            // this.logObject.LevelLog = level;
            if (loggerDAO.tryConnection() != false)
            {
                sendLog();
            }
            else
            {
                Console.WriteLine("Connection is closed!");
            }

        }

        public void sendLog()
        {
            Console.WriteLine(logObject.ToString());
            Console.WriteLine(loggerDAO.CreateLogRecord(this.logObject));
        }
    }
}
