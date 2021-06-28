using System;
using System.Collections.Generic;
using System.Text;

namespace Logging.Globals
{
    public static class LoggingGlobals
    {
        public const string ACTIVEMQ_URI = "tcp://localhost:61616?wireFormat.maxInactivityDuration=0";

        public const string DESTINATION = "LoggingQueue";

        public const string DB_CONNECTION = "Data Source=satudaysolution-rds.cc5jk01zeyle.us-west-1.rds.amazonaws.com, 1433; Initial Catalog=DB; Integrated Security=True; Trusted_Connection=False; Uid=admin; Pwd=SaturdaySolution;";

        public const string USE_ASYNC_URI = "tcp://localhost:61616?jms.UseAsyncSend=true";
    }
}
