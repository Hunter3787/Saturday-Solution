using Producer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class LoggerDataAccess
    {
        private String connection;
        //private LogObject log;
        private SqlDataAdapter adapter = new SqlDataAdapter();
        public LoggerDataAccess(String connectionString)
        {
            this.connection = connectionString;
        }
        public String CreateLogRecord(LogObject logObject)
        {
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        String sql = "INSERT INTO logs(message, loglevel, dateTime) VALUES(@MESSAGE,@LOGLEVEL,@DATETIME);";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@MESSAGE", SqlDbType.VarChar).Value = logObject.Message;
                        adapter.InsertCommand.Parameters.Add("@LOGLEVEL", SqlDbType.VarChar).Value = logObject.LevelLog;
                        adapter.InsertCommand.Parameters.Add("@DATETIME", SqlDbType.VarChar).Value = logObject.Datetime;
                        adapter.InsertCommand.ExecuteNonQuery();

                        transaction.Commit();
                        return "Successful Log creation";
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == -2)
                        {
                            transaction.Rollback();
                            return ("Data store has timed out.");
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return "Successful Log creation";
                }
            }
        }
    }
}
