using AutoBuildApp.DataAccess.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

/// <summary>
/// References used from file: Solution Items/References.txt 
/// N/A: Zeinab's code reformatted for specificity.
/// </summary>

namespace AutoBuildApp.Logging
{
    public class LoggerDAO
    {
        private readonly string _connection; // Stores connection string.
        private readonly SqlDataAdapter _adapter = new SqlDataAdapter(); // Allows the use to connect and use SQL statements and logic.

        // Establishes the connection with the connection string that is passed through. 
        public LoggerDAO(string connectionString)
        {
            _connection = connectionString;
        }
        // Method that is used to create a log record in the logs table in the datastore.
        public bool CreateLogRecord(LoggerEntity logger)
        {
            // This will use the connection string initilized above.
            using (SqlConnection connection = new SqlConnection(_connection))
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

                        _adapter.InsertCommand = new SqlCommand(sql, connection, transaction); // Takes in the three parameters to be allowed to make SQL commands.
                        _adapter.InsertCommand.Parameters.Add("@MESSAGE", SqlDbType.VarChar).Value = logger.Message; // Stores the log message.
                        _adapter.InsertCommand.Parameters.Add("@LOGLEVEL", SqlDbType.VarChar).Value = logger.LogLevel; // Stores the enum LogLevel.
                        _adapter.InsertCommand.Parameters.Add("@DATETIME", SqlDbType.VarChar).Value = logger.DateTime; // Stores the DateTime of the log.
                        _adapter.InsertCommand.ExecuteNonQuery(); // Executes a Transaction-centered SQL statement.

                        transaction.Commit(); // Commits the changes to the database,
                        return true; // Returns a message that the log has been successfully created.
                    }
                    // If the SQL statement fails, it will throw an SQL Exception.
                    catch (SqlException ex)
                    {
                        if (ex.Number == -2)
                        {
                            transaction.Rollback(); // The transaction will be rolled back to before the try block.
                            return false; // Returns the error message.
                        }
                    }
                    // Closes the connection at the end of the try and catch block.
                    finally
                    {
                        connection.Close();
                    }
                    return true; // Returns a success message.
                }
            }
        }
    }
}
