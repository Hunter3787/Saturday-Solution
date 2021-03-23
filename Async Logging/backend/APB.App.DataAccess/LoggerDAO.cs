using APB.App.Entities;
using System;
using System.Data;
using System.Data.SqlClient;

namespace APB.App.DataAccess
{
    public class LoggerDAO
    {
        private string connection; // Stores connection string.
        private SqlDataAdapter adapter = new SqlDataAdapter(); // Allows the use to connect and use SQL statements and logic.

        // Establishes the connection with the connection string that is passed through. 
        public LoggerDAO(string connectionString)
        {
            this.connection = connectionString;
        }
        // Method that is used to create a log record in the logs table in the datastore.
        public string CreateLogRecord(LoggerEntity logger)
        {
            // This will use the connection string initilized above.
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                // Open the connection to the connection string destination.
                connection.Open();

                // Begins the SQL transaction to know when data will start to be passed through.
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    // Will try to insert into the logs table.
                    try
                    {
                        // Specifies the SQL command and parameters that will be used to send data to the database.
                        string sql = "INSERT INTO logs(message, loglevel, dateTime) VALUES(@MESSAGE,@LOGLEVEL,@DATETIME);"; 

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction); // Takes in the three parameters to be allowed to make SQL commands.
                        adapter.InsertCommand.Parameters.Add("@MESSAGE", SqlDbType.VarChar).Value = logger.Message; // Stores the log message.
                        adapter.InsertCommand.Parameters.Add("@LOGLEVEL", SqlDbType.VarChar).Value = logger.LogTypeValue; // Stores the enum LogLevel.
                        adapter.InsertCommand.Parameters.Add("@DATETIME", SqlDbType.VarChar).Value = logger.DateTime; // Stores the DateTime of the log.
                        adapter.InsertCommand.ExecuteNonQuery(); // Executes a Transaction-centered SQL statement.

                        transaction.Commit(); // Commits the changes to the database,
                        return "Successful Log creation"; // Returns a message that the log has been successfully created.
                    }
                    // If the SQL statement fails, it will throw an SQL Exception.
                    catch (SqlException ex)
                    {
                        if (ex.Number == -2)
                        {
                            transaction.Rollback(); // The transaction will be rolled back to before the try block.
                            return ("Data store has timed out."); // Returns the error message.
                        }
                    }
                    // Closes the connection at the end of the try and catch block.
                    finally
                    {
                        connection.Close();
                    }
                    return "Successful Log creation"; // Returns a success message.
                }
            }
        }
    }
}
