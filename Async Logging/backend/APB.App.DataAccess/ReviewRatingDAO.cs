using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using APB.App.DataAccess.Interfaces;
using APB.App.Entities;


namespace APB.App.DataAccess
{
    public class ReviewRatingDAO
    {
        private string _connectionString; // Stores connection string.
        private SqlDataAdapter adapter = new SqlDataAdapter(); // Allows the use to connect and use SQL statements and logic.

        private static string ReviewTable => nameof(ReviewRatingEntity);

        // Establishes the connection with the connection string that is passed through. 
        public ReviewRatingDAO(string connectionString)
        {
            this._connectionString = connectionString;
        }
        // Method that is used to create a log record in the logs table in the datastore.
        public bool CreateReviewRatingRecord(ReviewRatingEntity reviewRatingEntity)
        {
            //reviewRatingEntity.EntityId = $"{ReviewTable}_{DateTime.UtcNow.ToString("yyyyMMdd_hh_mm_ss_ms")}";
            
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var command = new SqlCommand())
                {
                    command.Transaction = conn.BeginTransaction();
                    command.Connection = conn;
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    command.CommandType = CommandType.Text;

                    command.CommandText =
                        "INSERT INTO reviews(username, message, star, imagepath, datetime)" +
                        "SELECT @v0, @v1, @v2, @v3, @v4";

                    var parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@v0", reviewRatingEntity.Username);
                    parameters[1] = new SqlParameter("@v1", reviewRatingEntity.Message);
                    parameters[2] = new SqlParameter("@v2", reviewRatingEntity.StarRatingValue);
                    if(reviewRatingEntity.ImageBuffer != null)
                    {
                        parameters[3] = new SqlParameter("@v3", reviewRatingEntity.ImageBuffer);
                    }
                    else
                    {
                        parameters[3] = new SqlParameter("@v3", SqlDbType.VarBinary, -1);
                        parameters[3].Value = DBNull.Value;
                    }
                    parameters[4] = new SqlParameter("@v4", reviewRatingEntity.DateTime);

                    command.Parameters.AddRange(parameters);

                    var rowsAdded = command.ExecuteNonQuery();


                    if (rowsAdded == 1)
                    {
                        command.Transaction.Commit();
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

        public ReviewRatingEntity GetReviewsRatingsBy(string reviewId)
        {
            //reviewRatingEntity.EntityId = $"{ReviewTable}_{DateTime.UtcNow.ToString("yyyyMMdd_hh_mm_ss_ms")}";

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var command = new SqlCommand())
                {
                    var reviewRatingEntity = new ReviewRatingEntity(); 
                    command.Transaction = conn.BeginTransaction();
                    command.Connection = conn;
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    command.CommandType = CommandType.Text;

                    command.CommandText =
                        "SELECT * from reviews where entityId = @v0;";


                    //SQL Command to retrieve by key: select* from reviews where reviewID = 30002;

                    var parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@v0", reviewId);

                    command.Parameters.AddRange(parameters);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reviewRatingEntity.EntityId = reader["entityId"].ToString();
                            reviewRatingEntity.Username = (string)reader["username"];
                            reviewRatingEntity.Message = (string)reader["message"];
                            reviewRatingEntity.StarRatingValue = (int)reader["star"];
                            if(reader["imagepath"] != DBNull.Value)
                            {
                                reviewRatingEntity.ImageBuffer = (byte[])reader["imagepath"];
                            }
                            reviewRatingEntity.DateTime = (string)reader["datetime"];
                        }
                    }


                    command.ExecuteNonQuery();

                    command.Transaction.Commit();
                    return reviewRatingEntity;

                }
            }

            return null;

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

        public List<ReviewRatingEntity> GetAllReviewsRatings()
        {
            //reviewRatingEntity.EntityId = $"{ReviewTable}_{DateTime.UtcNow.ToString("yyyyMMdd_hh_mm_ss_ms")}";

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var command = new SqlCommand())
                {
                    var reviewRatingEntityList = new List<ReviewRatingEntity>();
                    command.Transaction = conn.BeginTransaction();
                    command.Connection = conn;
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    command.CommandType = CommandType.Text;

                    command.CommandText =
                        "SELECT * from reviews ORDER BY dateTime DESC;";


                    //SQL Command to retrieve by key: select* from reviews where reviewID = 30002;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reviewRatingEntity = new ReviewRatingEntity();
                            reviewRatingEntity.EntityId = reader["entityId"].ToString();
                            reviewRatingEntity.Username = (string)reader["username"];
                            reviewRatingEntity.Message = (string)reader["message"];
                            reviewRatingEntity.StarRatingValue = (int)reader["star"];
                            if (reader["imagepath"] != DBNull.Value)
                            {
                                reviewRatingEntity.ImageBuffer = (byte[])reader["imagepath"];
                            }
                            reviewRatingEntity.DateTime = (string)reader["datetime"];

                            reviewRatingEntityList.Add(reviewRatingEntity);
                        }
                    }

                    command.ExecuteNonQuery();

                    command.Transaction.Commit();
                    return reviewRatingEntityList;

                }
            }

            return null;

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

        public bool DeleteReviewRatingById(string entityId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var command = new SqlCommand())
                {
                    command.Transaction = conn.BeginTransaction();
                    command.Connection = conn;
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    command.CommandType = CommandType.Text;

                    command.CommandText =
                        "DELETE FROM reviews WHERE entityId = @v0";

                    var parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@v0", entityId);

                    command.Parameters.AddRange(parameters);

                    var rowsDeleted = command.ExecuteNonQuery();

                    if (rowsDeleted == 1)
                    {
                        command.Transaction.Commit();
                        return true;
                    }
                    return false;
                }
            }
        }

        public bool EditReviewRatingRecord(ReviewRatingEntity reviewRatingEntity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var command = new SqlCommand())
                {
                    command.Transaction = conn.BeginTransaction();
                    command.Connection = conn;
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    command.CommandType = CommandType.Text;

                    command.CommandText =
                        "UPDATE reviews " +
                        "SET star=@v0, message=@v1, imagepath=@v2 " +
                        "WHERE entityId = @v3";


                    //var s =
                    //    "update reviews " +
                    //    "SET star = 4, message = 'HAHAHAHA', imagepath = null " +
                    //    "where entityId = 30000";

                    var parameters = new SqlParameter[4];
                    parameters[0] = new SqlParameter("@v0", reviewRatingEntity.StarRatingValue);
                    parameters[1] = new SqlParameter("@v1", reviewRatingEntity.Message);
                    if (reviewRatingEntity.ImageBuffer != null)
                    {
                        parameters[2] = new SqlParameter("@v2", reviewRatingEntity.ImageBuffer);
                    }
                    else
                    {
                        parameters[2] = new SqlParameter("@v2", SqlDbType.VarBinary, -1);
                        parameters[2].Value = DBNull.Value;
                    }
                    parameters[3] = new SqlParameter("@v3", reviewRatingEntity.EntityId);

                    command.Parameters.AddRange(parameters);

                    command.ExecuteNonQuery();
                    command.Transaction.Commit();

                    //if (rowsUpdated == 1)
                    //{
                    //    command.Transaction.Commit();
                    //    return true;
                    //}
                    return true;
                }
            }
        }
    }
}
