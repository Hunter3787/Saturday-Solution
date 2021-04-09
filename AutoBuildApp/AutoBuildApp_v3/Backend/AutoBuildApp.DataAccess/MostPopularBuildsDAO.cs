using AutoBuildApp.DataAccess.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    /// <summary>
    /// This class is responsible for sending data to the DB.
    /// </summary>
    public class MostPopularBuildsDAO
    {
        private readonly string _connectionString; // Stores connection string.

        /// <summary>
        /// Establishes the connection with the connection string that is passed through. 
        /// </summary>
        /// <param name="connectionString">sql database string to be able to connect to database.</param>
        public MostPopularBuildsDAO(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// Method that is used to create a MPB record in the DB.
        /// </summary>
        /// <param name="buildPostEntity">Object to be used to parse data to DB</param>
        /// <returns>bool true if success, and false if unsuccessful.</returns>
        public bool PublishBuildRecord(BuildPostEntity buildPostEntity)
        {
            // uses var connection and will automatically close once the using block has reached the end.
            using (var conn = new SqlConnection(_connectionString))
            {
                // Open the connection to the database.
                conn.Open();

                // Uses the var command and will only use the command within this block.
                using (var command = new SqlCommand())
                {
                    command.Transaction = conn.BeginTransaction(); // begins the transaction to the database.
                    command.Connection = conn; // sets the connection of the command equal to the connection that has already been starte in the outer using block.
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds; // automatically times out the connection after 60 seconds.
                    command.CommandType = CommandType.Text; // sets the command type to command text, allowing use of string 'parametrized' queries.

                    // Stored the query that will be used for insertion. This is an insertion statement.
                    command.CommandText =
                        "INSERT INTO mostpopularbuilds(username, title, description, likes, buildtype, imagepath, datetime)" +
                        "SELECT @username, @title, @description, @likeincrementor, @buildtypevalue, @buildimagepath, @datetime";

                    var parameters = new SqlParameter[7]; // initialize 5 parameters to be read through incrementally.
                    parameters[0] = new SqlParameter("@username", buildPostEntity.Username); 
                    parameters[1] = new SqlParameter("@title", buildPostEntity.Title);
                    parameters[2] = new SqlParameter("@description", buildPostEntity.Description); 
                    parameters[3] = new SqlParameter("@likeincrementor", buildPostEntity.LikeIncrementor);
                    parameters[4] = new SqlParameter("@buildtypevalue", buildPostEntity.BuildTypeValue);
                    parameters[5] = new SqlParameter("@buildimagepath", buildPostEntity.BuildImagePath); 
                    parameters[6] = new SqlParameter("@datetime", buildPostEntity.DateTime);


                    // This will add the range of parameters to the parameters that will be used in the query.
                    command.Parameters.AddRange(parameters);

                    // stores the number of rows added in the statement.
                    var rowsAdded = command.ExecuteNonQuery();

                    // If the row that was added is one, it will commit the transaction and return true, else false.
                    if (rowsAdded == 1)
                    {
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return true;
                    }
                }
            }

            return false;

            #region Old SQL Code, longer method.
            //// This will use the connection string initilized above.
            //using (SqlConnection connection = new SqlConnection(this.connection))
            //{
            //    // Open the connection to the connection string destination.
            //    connection.Open();

            //    // Begins the SQL transaction to know when data will start to be passed through.
            //    using (SqlTransaction transaction = connection.BeginTransaction())
            //    {
            //        // Will try to insert into the logs table.
            //        try
            //        {
            //            // Specifies the SQL command and parameters that will be used to send data to the database.
            //            string sql = "INSERT INTO reviews(username, message, star, imagepath, datetime) VALUES(@USERNAME, @MESSAGE, @STAR, @IMAGEPATH, @DATETIME);";

            //            adapter.InsertCommand = new SqlCommand(sql, connection, transaction); // Takes in the three parameters to be allowed to make SQL commands.
            //            adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = reviewRatingEntity.Username; // Stores the log message.
            //            adapter.InsertCommand.Parameters.Add("@MESSAGE", SqlDbType.VarChar).Value = reviewRatingEntity.Message; // Stores the log message.
            //            adapter.InsertCommand.Parameters.Add("@STAR", SqlDbType.VarChar).Value = reviewRatingEntity.StarRatingValue; // Stores the enum LogLevel.
            //            adapter.InsertCommand.Parameters.Add("@IMAGEPATH", SqlDbType.VarBinary).Value = reviewRatingEntity.ImageBuffer; // Stores the enum LogLevel.
            //            adapter.InsertCommand.Parameters.Add("@DATETIME", SqlDbType.VarChar).Value = reviewRatingEntity.DateTime; // Stores the log message.

            //            adapter.InsertCommand.ExecuteNonQuery(); // Executes a Transaction-centered SQL statement.

            //            transaction.Commit(); // Commits the changes to the database,
            //            return true; // Returns a message that the log has been successfully created.
            //        }
            //        // If the SQL statement fails, it will throw an SQL Exception.
            //        catch (SqlException ex)
            //        {
            //            if (ex.Number == -2)
            //            {
            //                transaction.Rollback(); // The transaction will be rolled back to before the try block.
            //                return (false); // Returns the error message.
            //            }
            //        }
            //        // Closes the connection at the end of the try and catch block.
            //        finally
            //        {
            //            connection.Close();
            //        }
            //        return true; // Returns a success message.
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// This method will retrieve all records from the DB to be displayed.
        /// </summary>
        /// <returns>returns a list of Build Post Entities.</returns>
        public List<BuildPostEntity> GetAllBuildPostRecords()
        {
            // uses var connection and will automatically close once the using block has reached the end.
            using (var conn = new SqlConnection(_connectionString))
            {
                // Open the connection to the database.
                conn.Open();

                // Uses the var command and will only use the command within this block.
                using (var command = new SqlCommand())
                {
                    var buildPostEntities = new List<BuildPostEntity>(); // initialized a list of entity objects that will be retrieved.
                    command.Transaction = conn.BeginTransaction(); // begins the transaction to the database.
                    command.Connection = conn; // sets the connection of the command equal to the connection that has already been starte in the outer using block.
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds; // automatically times out the connection after 60 seconds.
                    command.CommandType = CommandType.Text; // sets the command type to command text, allowing use of string 'parametrized' queries.

                    // Stored the query that will be used for retrieval of all build posts.
                    command.CommandText =
                        "SELECT * from mostpopularbuilds;";

                    // this will start the sql data reader that will be utilized to read through the database. for only the duration of the using block.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // while the reader is reading, it will sweep the database until it finds the id item and return it as values.
                        while (reader.Read())
                        {
                            var buildPostEntity = new BuildPostEntity(); // create a new object for each while loop.
                            buildPostEntity.EntityId = reader["entityId"].ToString();
                            buildPostEntity.Username = (string)reader["username"];
                            buildPostEntity.Title = (string)reader["title"];
                            buildPostEntity.Description = (string)reader["description"];
                            buildPostEntity.LikeIncrementor = (int)reader["likes"];
                            buildPostEntity.BuildTypeValue = (int)reader["buildtype"];
                            buildPostEntity.BuildImagePath = (string)reader["imagepath"];
                            buildPostEntity.DateTime = (string)reader["datetime"];

                            buildPostEntities.Add(buildPostEntity); // adds the entity object, with retrieved data to the list.
                        }
                    }

                    // Executes the query.
                    command.ExecuteNonQuery();

                    // sends the transaction to be commited at the database.
                    command.Transaction.Commit();

                    // returns the list of entity object.
                    return buildPostEntities;
                }
            }
        }
    }
}
