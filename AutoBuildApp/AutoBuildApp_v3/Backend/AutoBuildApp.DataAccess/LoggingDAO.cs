//using Microsoft.Data.SqlClient;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Text;

//namespace AutoBuildApp.DataAccess
//{
//    public class LoggingDAO
//    {


//        public string ConnectionString { get; set; }


//        public LoggingDAO(string connection)
//        {
//            try
//            {
//                // set the claims needed for this method call
//                ConnectionString = connection;}
//            catch (ArgumentNullException)
//            {
//                if (connection == null)
//                {
//                    var expectedParamName = "NULL OBJECT PROVIDED";
//                    throw new ArgumentNullException(expectedParamName);
//                }
//            }


//        }

//        public void method(object logObject)
//        {

//            using (SqlConnection conn = new SqlConnection(ConnectionString))
//            {
//                Console.WriteLine($"\tRetrieveUserPermissions METHOD \n");
//                conn.Open();
//                // naming convention SP_ name of procudrure
//                string SP_INSERTLOGS = "INSERTLOGS";
//                using (SqlCommand command = new SqlCommand(SP_INSERTLOGS, conn))
//                {
//                    try
//                    {
//                        command.Transaction = conn.BeginTransaction();
//                        #region SQL related

//                        // https://learning.oreilly.com/library/view/adonet-in-a/0596003617/ch04s05.html
//                        command.CommandTimeout = TimeSpan.FromSeconds(TimeoutLengths.TIMEOUT_LONG).Seconds;
//                        // 1) Create a Command, and set its CommandType property to StoredProcedure.
//                        command.CommandType = CommandType.StoredProcedure;
//                        // 2) Set the CommandText to the name of the stored procedure.
//                        command.CommandText = SP_INSERTLOGS;
//                        /// command.Parameters.AddWithValue   -> fix itttttttt!!!!!
//                        command.Parameters.AddWithValue("@USERNAME", "USERNAME");
//                        command.Parameters.AddWithValue("@EVENTTYPE", "EVENTTYPE");
//                        command.Parameters.AddWithValue("@EVENTVALUE", "EVENTVALUE");
//                        command.Parameters.AddWithValue("@MESSAGE", "MESSAGE");
//                        command.Parameters.AddWithValue("@LOGLEVEL", "LOGLEVEL");
//                        command.Parameters.AddWithValue("@DATETIME", "DATETIME");

//                        #endregion
//                        var reader = command.ExecuteNonQuery();
//                        command.Transaction.Commit();
//                        if (reader != 1) 
//                        {
//                            return; //return  
//                        }

//                    }
//                    catch (SqlException)
//                    {
//                        command.Transaction.Rollback();

//                    }
//                }

//            }


//        }


//    }
//}
