using AutoBuildApp.DataAccess.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// References used from file: Solution Items/References.txt 
/// [1,15]
/// </summary>

namespace AutoBuildApp.DataAccess
{
    /// <summary>
    /// This class is the data access objects that will interact with the database and send data to be stored.
    /// </summary>
    public class ReviewRatingDAO
    {
        private readonly string _connectionString; // Stores connection string.

        /// <summary>
        /// Establishes the connection with the connection string that is passed through. 
        /// </summary>
        /// <param name="connectionString">sql database string to be able to connect to database.</param>
        public ReviewRatingDAO(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// Method that is used to create a log record in the logs table in the datastore.
        /// </summary>
        /// <param name="reviewRatingEntity">Object to be used to parse data to DB</param>
        /// <returns>bool true if success, and false if unsuccessful.</returns>
        public bool CreateReviewRatingRecord(ReviewRatingEntity reviewRatingEntity)
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
                        "INSERT INTO reviews(buildId, username, message, star, imagebuffer, filepath, datetime)" +
                        "SELECT @buildId, @username, @message, @starRating, @imageBuffer, @filePath, @dateTime";

                    var parameters = new SqlParameter[7]; // initialize 5 parameters to be read through incrementally.
                    parameters[0] = new SqlParameter("@buildId", reviewRatingEntity.BuildId); // string build post Id.
                    parameters[1] = new SqlParameter("@username", reviewRatingEntity.Username); // string username
                    parameters[2] = new SqlParameter("@message", reviewRatingEntity.Message); // string message.
                    parameters[3] = new SqlParameter("@starRating", reviewRatingEntity.StarRatingValue); // integer star rating value.
                    // checks if the image byte array is null, if not null, it will store image as byte.

                    // if it is null, it will store a DBNull value.
                    if(reviewRatingEntity.ImageBuffer != null) 
                    {
                        parameters[4] = new SqlParameter("@imageBuffer", reviewRatingEntity.ImageBuffer);
                    }
                    else
                    {
                        parameters[4] = new SqlParameter("@imageBuffer", SqlDbType.VarBinary, -1);
                        parameters[4].Value = DBNull.Value;
                    }
                    parameters[5] = new SqlParameter("@filePath", reviewRatingEntity.FilePath);
                    parameters[6] = new SqlParameter("@dateTime", reviewRatingEntity.DateTime); // string dateTime.

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
        /// This method will take in a 'unique' string ID value and return an object with
        /// the values taken from the DB.
        /// </summary>
        /// <param name="reviewId">unique identifier of a review object.</param>
        /// <returns>returns the review entity object that will be sent back down through the layers.</returns>
        public ReviewRatingEntity GetReviewsRatingsBy(string reviewId)
        {
            // uses var connection and will automatically close once the using block has reached the end.
            using (var conn = new SqlConnection(_connectionString))
            {
                // Open the connection to the database.
                conn.Open();

                // Uses the var command and will only use the command within this block.
                using (var command = new SqlCommand())
                {
                    var reviewRatingEntity = new ReviewRatingEntity(); // initialized an entity object that will be retrieved.
                    command.Transaction = conn.BeginTransaction(); // begins the transaction to the database.
                    command.Connection = conn; // sets the connection of the command equal to the connection that has already been starte in the outer using block.
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds; // automatically times out the connection after 60 seconds.
                    command.CommandType = CommandType.Text; // sets the command type to command text, allowing use of string 'parametrized' queries.

                    // Stored the query that will be used for retrieval where entityId = reviewId as a string.
                    command.CommandText =
                        "SELECT * from reviews where entityId = @v0;";

                    var parameters = new SqlParameter[1]; // parameter that will be sent through the query to identify a review.
                    parameters[0] = new SqlParameter("@v0", reviewId); // string ID

                    // This will add the range of parameters to the parameters that will be used in the query.
                    command.Parameters.AddRange(parameters);

                    // this will start the sql data reader that will be utilized to read through the database. for only the duration of the using block.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // while the reader is reading, it will sweep the database until it finds the id item and return it as values.
                        while (reader.Read())
                        {
                            reviewRatingEntity.EntityId = reader["entityId"].ToString(); // store the DB Id as a string in entityId.
                            reviewRatingEntity.Username = (string)reader["username"]; // store the DB username as a string in Username.
                            reviewRatingEntity.Message = (string)reader["message"]; // store the DB message as a string in Message.
                            reviewRatingEntity.StarRatingValue = (int)reader["star"]; // store the star value int as an int in StarRatingValue.

                            // checks if the image path is not null, if not it will return the imagepath, if not null then it wont.
                            if(reader["imagebuffer"] != DBNull.Value)
                            {
                                reviewRatingEntity.ImageBuffer = (byte[])reader["imagebuffer"];
                            }
                            reviewRatingEntity.FilePath = (string)reader["filepath"];
                            // store the datetime as a string in DateTime.
                            reviewRatingEntity.DateTime = (string)reader["datetime"];
                        }
                    }

                    // sExecutes the query.
                    command.ExecuteNonQuery();

                    // sends the transaction to be commited at the database.
                    command.Transaction.Commit();

                    // returns the entity object.
                    return reviewRatingEntity;
                }
            }

            #region Old SQL Code for getter, longer method.
            //// This will use the connection string initilized above.
            //using (SqlConnection connection = new SqlConnection(this._connectionString))
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
            //            string sql = "SELECT * FROM reviews WHERE reviewID =  " + reviewId + ";";


            //            // SQL Command to retrieve by key: select * from reviews where reviewID = 30002;


            //            var reviewEntities = new ReviewRatingEntity();

            //            SqlCommand command = new SqlCommand(sql, connection, transaction); // Takes in the three parameters to be allowed to make SQL commands.
            //            adapter.SelectCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = reviewEntities.Username; // Stores the log message.
            //            adapter.SelectCommand.Parameters.Add("@MESSAGE", SqlDbType.VarChar).Value = reviewEntities.Message; // Stores the log message.
            //            adapter.SelectCommand.Parameters.Add("@STAR", SqlDbType.VarChar).Value = reviewEntities.StarRatingValue; // Stores the enum LogLevel.
            //            adapter.SelectCommand.Parameters.Add("@IMAGEPATH", SqlDbType.VarBinary).Value = reviewEntities.ImageBuffer; // Stores the enum LogLevel.
            //            adapter.SelectCommand.Parameters.Add("@DATETIME", SqlDbType.VarChar).Value = reviewEntities.DateTime; // Stores the log message.


            //            adapter.SelectCommand = command; // Executes a Transaction-centered SQL statement.

            //            transaction.Commit(); // Commits the changes to the database,
            //            return reviewEntities; // Returns a message that the log has been successfully created.
            //        }
            //        // If the SQL statement fails, it will throw an SQL Exception.
            //        catch (SqlException ex)
            //        {
            //            if (ex.Number == -2)
            //            {
            //                transaction.Rollback(); // The transaction will be rolled back to before the try block.
            //                return (null); // Returns the error message.
            //            }
            //        }
            //        // Closes the connection at the end of the try and catch block.
            //        finally
            //        {
            //            connection.Close();
            //        }
            //        return null; // Returns a success message.
            //    }
            //}
            #endregion
        }

        public List<ReviewRatingEntity> GetAllReviewsRatingsByBuildId(string buildId)
        {
            // uses var connection and will automatically close once the using block has reached the end.
            using (var conn = new SqlConnection(_connectionString))
            {
                // Open the connection to the database.
                conn.Open();

                // Uses the var command and will only use the command within this block.
                using (var command = new SqlCommand())
                {
                    var reviewRatingEntityList = new List<ReviewRatingEntity>(); // initialized a list of entity objects that will be retrieved.
                    command.Transaction = conn.BeginTransaction(); // begins the transaction to the database.
                    command.Connection = conn; // sets the connection of the command equal to the connection that has already been starte in the outer using block.
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds; // automatically times out the connection after 60 seconds.
                    command.CommandType = CommandType.Text; // sets the command type to command text, allowing use of string 'parametrized' queries.

                    // Stored the query that will be used for retrieval of all reviews.
                    command.CommandText =
                        "SELECT * from reviews WHERE buildId = @buildId ORDER BY dateTime DESC;";

                    var parameters = new SqlParameter[1]; // parameter that will be sent through the query to identify a review.
                    parameters[0] = new SqlParameter("@buildId", buildId); // string ID

                    // This will add the range of parameters to the parameters that will be used in the query.
                    command.Parameters.AddRange(parameters);

                    // this will start the sql data reader that will be utilized to read through the database. for only the duration of the using block.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // while the reader is reading, it will sweep the database until it finds the id item and return it as values.
                        while (reader.Read())
                        {
                            var reviewRatingEntity = new ReviewRatingEntity(); // create a new object for each while loop.
                            reviewRatingEntity.EntityId = reader["entityId"].ToString(); // store the DB Id as a string in entityId.
                            reviewRatingEntity.BuildId = reader["buildId"].ToString(); // gets the build id from the DB.
                            reviewRatingEntity.Username = (string)reader["username"]; // store the DB username as a string in Username.
                            reviewRatingEntity.Message = (string)reader["message"]; // store the DB message as a string in Message.
                            reviewRatingEntity.StarRatingValue = (int)reader["star"]; // store the star value int as an int in StarRatingValue.

                            // checks if the image path is not null, if not it will return the imagepath, if not null then it wont.
                            if (reader["imagebuffer"] != DBNull.Value)
                            {
                                reviewRatingEntity.ImageBuffer = (byte[])reader["imagebuffer"];
                            }
                            reviewRatingEntity.FilePath = (string)reader["filepath"];
                            reviewRatingEntity.DateTime = (string)reader["datetime"];

                            reviewRatingEntityList.Add(reviewRatingEntity); // adds the entity object, with retrieved data to the list.
                        }
                    }

                    // Executes the query.
                    command.ExecuteNonQuery();

                    // sends the transaction to be commited at the database.
                    command.Transaction.Commit();

                    // returns the list of entity object.
                    return reviewRatingEntityList;
                }
            }
        }

        /// <summary>
        /// This method will be used to fetch data from the database and load to a page.
        /// </summary>
        /// <returns>returns a list of entity objects</returns>
        public List<ReviewRatingEntity> GetAllReviewsRatings()
        {
            // uses var connection and will automatically close once the using block has reached the end.
            using (var conn = new SqlConnection(_connectionString))
            {
                // Open the connection to the database.
                conn.Open();

                // Uses the var command and will only use the command within this block.
                using (var command = new SqlCommand())
                {
                    var reviewRatingEntityList = new List<ReviewRatingEntity>(); // initialized a list of entity objects that will be retrieved.
                    command.Transaction = conn.BeginTransaction(); // begins the transaction to the database.
                    command.Connection = conn; // sets the connection of the command equal to the connection that has already been starte in the outer using block.
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds; // automatically times out the connection after 60 seconds.
                    command.CommandType = CommandType.Text; // sets the command type to command text, allowing use of string 'parametrized' queries.

                    // Stored the query that will be used for retrieval of all reviews.
                    command.CommandText =
                        "SELECT * from reviews ORDER BY dateTime DESC;";

                    // this will start the sql data reader that will be utilized to read through the database. for only the duration of the using block.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // while the reader is reading, it will sweep the database until it finds the id item and return it as values.
                        while (reader.Read())
                        {
                            var reviewRatingEntity = new ReviewRatingEntity(); // create a new object for each while loop.
                            reviewRatingEntity.EntityId = reader["entityId"].ToString(); // store the DB Id as a string in entityId.
                            reviewRatingEntity.Username = (string)reader["username"]; // store the DB username as a string in Username.
                            reviewRatingEntity.Message = (string)reader["message"]; // store the DB message as a string in Message.
                            reviewRatingEntity.StarRatingValue = (int)reader["star"]; // store the star value int as an int in StarRatingValue.

                            // checks if the image path is not null, if not it will return the imagepath, if not null then it wont.
                            if (reader["imagebuffer"] != DBNull.Value)
                            {
                                reviewRatingEntity.ImageBuffer = (byte[])reader["imagebuffer"];
                            }
                            reviewRatingEntity.FilePath = (string)reader["filepath"];
                            reviewRatingEntity.DateTime = (string)reader["datetime"];

                            reviewRatingEntityList.Add(reviewRatingEntity); // adds the entity object, with retrieved data to the list.
                        }
                    }

                    // Executes the query.
                    command.ExecuteNonQuery();

                    // sends the transaction to be commited at the database.
                    command.Transaction.Commit();

                    // returns the list of entity object.
                    return reviewRatingEntityList;
                }
            }

            #region Old SQL Code for getter, longer method.
            //// This will use the connection string initilized above.
            //using (SqlConnection connection = new SqlConnection(this._connectionString))
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
            //            string sql = "SELECT * FROM reviews WHERE reviewID =  " + reviewId + ";";


            //            // SQL Command to retrieve by key: select * from reviews where reviewID = 30002;


            //            var reviewEntities = new ReviewRatingEntity();

            //            SqlCommand command = new SqlCommand(sql, connection, transaction); // Takes in the three parameters to be allowed to make SQL commands.
            //            adapter.SelectCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = reviewEntities.Username; // Stores the log message.
            //            adapter.SelectCommand.Parameters.Add("@MESSAGE", SqlDbType.VarChar).Value = reviewEntities.Message; // Stores the log message.
            //            adapter.SelectCommand.Parameters.Add("@STAR", SqlDbType.VarChar).Value = reviewEntities.StarRatingValue; // Stores the enum LogLevel.
            //            adapter.SelectCommand.Parameters.Add("@IMAGEPATH", SqlDbType.VarBinary).Value = reviewEntities.ImageBuffer; // Stores the enum LogLevel.
            //            adapter.SelectCommand.Parameters.Add("@DATETIME", SqlDbType.VarChar).Value = reviewEntities.DateTime; // Stores the log message.


            //            adapter.SelectCommand = command; // Executes a Transaction-centered SQL statement.

            //            transaction.Commit(); // Commits the changes to the database,
            //            return reviewEntities; // Returns a message that the log has been successfully created.
            //        }
            //        // If the SQL statement fails, it will throw an SQL Exception.
            //        catch (SqlException ex)
            //        {
            //            if (ex.Number == -2)
            //            {
            //                transaction.Rollback(); // The transaction will be rolled back to before the try block.
            //                return (null); // Returns the error message.
            //            }
            //        }
            //        // Closes the connection at the end of the try and catch block.
            //        finally
            //        {
            //            connection.Close();
            //        }
            //        return null; // Returns a success message.
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// This method will be used to search for a review in the DB and delete it by the ID that is passed in.
        /// </summary>
        /// <param name="entityId">string unique identifier that will be used to search for an item in DB</param>
        /// <returns>returns a boolean result of false if unsuccessful or true if successful</returns>
        public bool DeleteReviewRatingById(string entityId)
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

                    // Stored the query that will be used to delete a review where the ids match.
                    command.CommandText =
                        "DELETE FROM reviews WHERE entityId = @v0";

                    var parameters = new SqlParameter[1]; // parameter that will be sent through the query to identify a review.
                    parameters[0] = new SqlParameter("@v0", entityId); // string ID

                    // This will add the range of parameters to the parameters that will be used in the query.
                    command.Parameters.AddRange(parameters);

                    // stores the number of rows added in the statement.
                    var rowsDeleted = command.ExecuteNonQuery();

                    // If the row that was added is one, it will commit the transaction and return true, else false.
                    if (rowsDeleted == 1)
                    {
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// This method will be used to edit an already existing review with values and ID.
        /// </summary>
        /// <param name="reviewRatingEntity">Takes in an entity object that will be used to find and edit values in the DB.</param>
        /// <returns>returns a boolean, true if success and false if failure.</returns>
        public bool EditReviewRatingRecord(ReviewRatingEntity reviewRatingEntity)
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

                    // Stored the query that will be used to edit a review WHERE the ids match and replacing values via the SET command.
                    command.CommandText =
                        "UPDATE reviews " +
                        "SET star=@v0, message=@v1, imagebuffer=@v2, filepath=@v3 " +
                        "WHERE entityId = @v4";

                    var parameters = new SqlParameter[5]; // initialize four parameters to be sent through.
                    parameters[0] = new SqlParameter("@v0", reviewRatingEntity.StarRatingValue); // send the star value to be replaced.
                    parameters[1] = new SqlParameter("@v1", reviewRatingEntity.Message); // send the message that will be used to replace.

                    // if it is null, it will store a DBNull value.
                    if (reviewRatingEntity.ImageBuffer != null)
                    {
                        parameters[2] = new SqlParameter("@v2", reviewRatingEntity.ImageBuffer);
                    }
                    else
                    {
                        parameters[2] = new SqlParameter("@v2", SqlDbType.VarBinary, -1);
                        parameters[2].Value = DBNull.Value;
                    }
                    parameters[3] = new SqlParameter("@v3", reviewRatingEntity.FilePath);
                    parameters[4] = new SqlParameter("@v4", reviewRatingEntity.EntityId);

                    // This will add the range of parameters to the parameters that will be used in the query.
                    command.Parameters.AddRange(parameters);

                    // This will execute the query.
                    command.ExecuteNonQuery();

                    // sends the transaction to be commited at the database.
                    command.Transaction.Commit();

                    return true;
                }
            }
        }
    }
}
