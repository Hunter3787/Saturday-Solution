using System;
using System.Collections.Generic;
using System.Text;
using AutoBuildApp.Models;


namespace AutoBuildApp.DataAccess
{
    class storeLogDTO
    {
        private LoggingDataGateway logGateway;

        public storeLogDTO(String connection)
        {
            logGateway = new LoggingDataGateway(connection);
        }

        public void storeLogtoDb(logging logger)
        {
            logGateway.storeLogDatainDB(logger);
        }



    }
}
