using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;


using AutoBuildApp.Models;
using System.Data;

namespace AutoBuildApp.DataAccess
{
    class LoggingDataGateway
    {
        // now will be making a SQL connection  check
        public static void checkConnection(string connectionString)
        {
            //https://prod.liveshare.vsengsaas.visualstudio.com/join?0C811C8DF3B3EA0C85449FA8739BD16D004D
            using (SqlConnection connection = new SqlConnection(ConnectionStringHelperClass.ConnectNow(connectionString)))
            {
                connection.Open();
                Console.WriteLine("ServerVersion: {0}", connection.ServerVersion);
                Console.WriteLine("State: {0}", connection.State);
            }
        }


        // create log 
        public static String storeLogDatainDB(logging logger)
        {

            using (SqlConnection connection = new SqlConnection(ConnectionStringHelperClass.ConnectNow("AutoBuildDB")))
            {

               
                String sql = "INSERT INTO logs(creationDate, event, message, tag) VALUES(@CREATEDATE,@EVENTLOG, @MESSAGE, @TAG);";

                Microsoft.Data.SqlClient.SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.InsertCommand = new SqlCommand(sql, connection);
                adapter.InsertCommand.Parameters.Add("@CREATEDATE", SqlDbType.VarChar).Value = logger.CreationDate;
                adapter.InsertCommand.Parameters.Add("@EVENTLOG", SqlDbType.VarChar).Value = logger.Event;
                adapter.InsertCommand.Parameters.Add("@MESSAGE", SqlDbType.VarChar).Value = logger.Message;
                adapter.InsertCommand.Parameters.Add("@TAG", SqlDbType.VarChar).Value = logger.tag;

                try
                {
                    connection.Open();
                    adapter.InsertCommand.ExecuteNonQuery();
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    //https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlerror?view=dotnet-plat-ext-5.0 
                    // 
                    throw new NotImplementedException();

                }
                Console.ReadLine();

                return " ";

            }

        }


        //retrieve log at day x?


        // delete log? do we eveen need this?



    }
}
