
using System.Data;
using System;
using Microsoft.Data.SqlClient;
using Project.DataAccess.Entities;

namespace Project.DataAccess
{
    public class LoggerDAO
    {

        private CommonResonseLog CRLog;
        private string connection;
        int numberOfRowsAffected = -999;

        private SqlDataAdapter adapter = new SqlDataAdapter();


        public LoggerDAO(String connectionString)
        {
            this.connection = connectionString;
        }

        public bool tryConnection()
        {
            SqlConnection con = new SqlConnection(this.connection);
            con.Open();
            if (this.connection != null && con.State != ConnectionState.Closed)
            {
                return true;
            }
            else
                con.Close();
            return false;
        }
        public Object CreateLogRecord(LogEntity logObject)
        {

            CRLog = new CommonResonseLog();
            String result = "";
            using (SqlConnection conn = new SqlConnection(this.connection))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("In DAO " + logObject.ToString());
                        //string sql = "INSERT INTO logs(message, loglevel) VALUES('THIS IS A TEST 4', '4');";
                        String sql = "INSERT INTO logs (message, loglevel) VALUES (@MESSAGE,@LOGLEVEL);";

                        adapter.InsertCommand = new SqlCommand(sql, conn, transaction);
                        adapter.InsertCommand.Parameters.Add("@MESSAGE", SqlDbType.VarChar).Value = logObject.Message;
                        adapter.InsertCommand.Parameters.Add("@LOGLEVEL", SqlDbType.VarChar).Value = logObject.LevelLog;
                        numberOfRowsAffected = adapter.InsertCommand.ExecuteNonQuery();
                        if (numberOfRowsAffected != 1)
                        {
                            CRLog.SuccessBool= false;
                            CRLog.FailureString = $"UnSuccessful Log creation, rows affected {numberOfRowsAffected}";
                            result += $"UnSuccessful Log creation, rows affected {numberOfRowsAffected}";

                        }

                        CRLog.SuccessBool = true;
                        CRLog.SuccessString = $"1. Successful Log creation, rows affected {numberOfRowsAffected} \n";
                        result += $"1. Successful Log creation, rows affected {numberOfRowsAffected} \n";

                        transaction.Commit();

                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == -2)
                        {
                            transaction.Rollback();
                            CRLog.SuccessBool = false;
                            CRLog.FailureString = $"Data store has timed out.";

                            return CRLog;
                        }
                    }
                }
            }
            return CRLog;
        }

        public String ExecuteStoredProcedure(String logmonth)
        {

            String result = "";
            using (SqlConnection conn = new SqlConnection(this.connection))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("In DAO exec ");

                        String sql = " ";

                        adapter.InsertCommand = new SqlCommand(sql, conn, transaction);




                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == -2)
                        {
                            transaction.Rollback();
                            return ("Data store has timed out.");
                        }
                    }




                }
            }


            return result;
        }

    }

}
