using AutoBuildApp.DataAccess.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class LoggingDAO
    {


        public string ConnectionString { get; set; }


        public LoggingDAO(string connection)
        {
            try
            {
                // set the claims needed for this method call
                ConnectionString = connection;
            }
            catch (ArgumentNullException)
            {
                if (connection == null)
                {
                    var expectedParamName = "NULL OBJECT PROVIDED";
                    throw new ArgumentNullException(expectedParamName);
                }
            }


        }

        public bool InsertLog(LoggerEntity logger)
        {

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                Console.WriteLine($"\t METHOD \n");
                conn.Open();
                // naming convention SP_ name of procudrure
                string SP_INSERTLOGS = "INSERTLOGS";
                using (SqlCommand command = new SqlCommand(SP_INSERTLOGS, conn))
                {
                    try
                    {
                        command.Transaction = conn.BeginTransaction();
                        #region SQL related

                        // https://learning.oreilly.com/library/view/adonet-in-a/0596003617/ch04s05.html
                        command.CommandTimeout = TimeSpan.FromSeconds(TimeoutLengths.TIMEOUT_LONG).Seconds;
                        // 1) Create a Command, and set its CommandType property to StoredProcedure.
                        command.CommandType = CommandType.StoredProcedure;
                        // 2) Set the CommandText to the name of the stored procedure.
                        command.CommandText = SP_INSERTLOGS;
                        /// command.Parameters.AddWithValue   -> fix itttttttt!!!!!
                        command.Parameters.AddWithValue("@USERNAME"  , logger.Username);
                        command.Parameters.AddWithValue("@EVENTTYPE" , logger.Event);
                        command.Parameters.AddWithValue("@EVENTVALUE", logger.EventValue);
                        command.Parameters.AddWithValue("@MESSAGE"   , logger.Message);
                        command.Parameters.AddWithValue("@LOGLEVEL"  , logger.LogLevel);
                        command.Parameters.AddWithValue("@DATETIME", logger.DateTime);

                        #endregion
                        var reader = command.ExecuteNonQuery();
                        Console.WriteLine(reader.ToString());

                        command.Transaction.Commit();
                        if (reader >= 1)
                        {
                            return true; //return  
                        }


                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                        command.Transaction.Rollback();

                        return false;
                    }
                }

            }

            return false;


        }


    }
}
