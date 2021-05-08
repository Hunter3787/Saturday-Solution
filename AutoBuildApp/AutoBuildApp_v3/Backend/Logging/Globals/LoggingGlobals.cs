using System;
using System.Collections.Generic;
using System.Text;

namespace Logging.Globals
{
    public static class LoggingGlobals
    {
        public const string ACTIVEMQ_URI = "tcp://localhost:61616?wireFormat.maxInactivityDuration=0";

        public const string DESTINATION = "LoggingQueue";

        public const string DB_CONNECTION = "Server = localhost; Database = DB; Trusted_Connection = True;";

        public const string USE_ASYNC_URI = "tcp://localhost:61616?jms.UseAsyncSend=true";
    }
}
