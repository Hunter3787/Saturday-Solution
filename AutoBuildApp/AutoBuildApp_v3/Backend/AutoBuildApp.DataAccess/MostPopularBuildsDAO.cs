using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.DataAccess.Reflections;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    /// <summary>
    /// This class is responsible for sending data to the DB.
    /// </summary>
    public class MostPopularBuildsDAO
    {
        private readonly string _connectionString; // Stores connection string.

        private static string BuildPostTable => nameof(BuildPostEntity);
        /// <summary>
        /// Establishes the connection with the connection string that is passed through. 
        /// </summary>
        /// <param name="connectionString">sql database string to be able to connect to database.</param>
        public MostPopularBuildsDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region DAO Publish method not using reflections
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
                    command.Connection = conn; // sets the connection of the command equal to the connection that has already been started in the outer using block.
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds; // automatically times out the connection after 60 seconds.
                    command.CommandType = CommandType.Text; // sets the command type to command text, allowing use of string 'parametrized' queries.

                    // Stored the query that will be used for insertion. This is an insertion statement.
                    command.CommandText =
                        "INSERT INTO mostpopularbuilds(Username,Title,Description,LikeIncrementor,BuildTypeValue,BuildImagePath,DateTime)" +
                        "SELECT @Username, @Title, @Description, @LikeIncrementor, @BuildTypeValue, @BuildImagePath, @DateTime";

                    var parameters = new SqlParameter[7]; // initialize 5 parameters to be read through incrementally.
                    parameters[0] = new SqlParameter("@Username", buildPostEntity.Username);
                    parameters[1] = new SqlParameter("@Title", buildPostEntity.Title);
                    parameters[2] = new SqlParameter("@Description", buildPostEntity.Description);
                    parameters[3] = new SqlParameter("@LikeIncrementor", buildPostEntity.LikeIncrementor);
                    parameters[4] = new SqlParameter("@BuildTypeValue", buildPostEntity.BuildTypeValue);
                    parameters[5] = new SqlParameter("@BuildImagePath", buildPostEntity.BuildImagePath);
                    parameters[6] = new SqlParameter("@DateTime", buildPostEntity.DateTime);


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
        }
        #endregion

        #region DAO Publish method using reflections
        /// <summary>
        /// Method that is used to create a MPB record in the DB.
        /// </summary>
        /// <param name="buildPostEntity">Object to be used to parse data to DB</param>
        /// <returns>bool true if success, and false if unsuccessful.</returns>
        public bool PublishBuildRecord(BuildPostEntity buildPostEntity, string tableName)
        {
            buildPostEntity.EntityId = $"{BuildPostTable}_{DateTime.UtcNow.ToString("yyyyMMdd_hh_mm_ss_ms")}";
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

                    string sqlBefore = "insert into " + tableName +"(";
                    string sqlAfter = " values(";

                    Type type = buildPostEntity.GetType();
                    PropertyInfo[] pis = type.GetProperties();
                    bool addRemove = true;

                    Type t = buildPostEntity.GetType();
                    //DAOAttribute dao;
                    StringCollection Fields = new StringCollection();

                    foreach (PropertyInfo pi in pis)
                    {
                        foreach(Attribute att in Attribute.GetCustomAttributes(pi))
                        {
                            if (pi.Name.ToLower().Equals(att.ToString().ToLower()))
                            {
                                addRemove = false;
                            }
                        }
                        if (addRemove)
                        {
                            string strType = type.GetProperty(pi.Name).PropertyType.ToString().ToLower();
                            bool blNum = strType.Contains("int") || strType.Contains("float") || strType.Contains("double");
                            sqlBefore += pi.Name + ",";
                            if (blNum)
                            {
                                sqlAfter += type.GetProperty(pi.Name).GetValue(buildPostEntity, null) + " ,";
                            }
                            else
                            {
                                sqlAfter += "'" + type.GetProperty(pi.Name).GetValue(buildPostEntity, null) + "' ,";
                            }
                        }
                    }

                    var query = sqlBefore.Substring(0, sqlBefore.Length - 1) + ") " + sqlAfter.Substring(0, sqlAfter.Length - 1) + ")";

                    command.CommandText = query;

                    // stores the number of rows added in the statement.
                    var rowsAdded = command.ExecuteNonQuery();

                    // If the row that was added is one, it will commit the transaction and return true, else false.
                    if (rowsAdded == 1)
                    {
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return true;
                    }
                    return false;
                }
            }
        }
        #endregion

        /// <summary>
        /// This method retrieves data from the DB by passing in the ID.
        /// </summary>
        /// <param name="buildId">takes in an id string.</param>
        /// <returns>retruns an entity object.</returns>
        public BuildPostEntity GetBuildPostRecord(string buildId)
        {
            // uses var connection and will automatically close once the using block has reached the end.
            using (var conn = new SqlConnection(_connectionString))
            {
                // Open the connection to the database.
                conn.Open();

                // Uses the var command and will only use the command within this block.
                using (var command = new SqlCommand())
                {
                    var buildPostEntity = new BuildPostEntity(); // initialized a list of entity objects that will be retrieved.
                    command.Transaction = conn.BeginTransaction(); // begins the transaction to the database.
                    command.Connection = conn; // sets the connection of the command equal to the connection that has already been starte in the outer using block.
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds; // automatically times out the connection after 60 seconds.
                    command.CommandType = CommandType.Text; // sets the command type to command text, allowing use of string 'parametrized' queries.

                    // Stored the query that will be used for retrieval of all build posts.
                    command.CommandText =
                        $"SELECT * from mostpopularbuilds WHERE EntityId = @buildId;";

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
                            buildPostEntity.EntityId = reader["EntityId"].ToString();
                            buildPostEntity.Username = (string)reader["Username"];
                            buildPostEntity.Title = (string)reader["Title"];
                            buildPostEntity.Description = (string)reader["Description"];
                            buildPostEntity.LikeIncrementor = (int)reader["LikeIncrementor"];
                            buildPostEntity.BuildTypeValue = (int)reader["BuildTypeValue"];
                            buildPostEntity.BuildImagePath = (string)reader["BuildImagePath"];
                            buildPostEntity.DateTime = (string)reader["DateTime"];
                        }
                    }

                    // Executes the query.
                    command.ExecuteNonQuery();

                    // sends the transaction to be commited at the database.
                    command.Transaction.Commit();

                    // returns the list of entity object.
                    return buildPostEntity;
                }
            }
        }

        /// <summary>
        /// This method will retrieve all records from the DB to be displayed by order of likes.
        /// </summary>
        /// <param name="orderLikes">takes in the condition for likes ordering.</param>
        /// <param name="buildType">takes in the condition to order by build type.</param>
        /// <returns>returns a list of entities.</returns>
        public List<BuildPostEntity> GetAllBuildPostRecordsByQuery(string orderLikes, string buildType)
        {
            // Initialize the query string.
            string orderBy = "ORDER BY LikeIncrementor DESC";
            string whereClause = "";

            // Checks the condition of the query by string and sets the where clause accordingly, else it will run a default query of descneding likes.
            if (buildType == "BuildType_GraphicArtist")
            {
                whereClause = "WHERE BuildTypeValue = 1";
            }
            else if (buildType == "BuildType_Gaming")
            {
                whereClause = "WHERE BuildTypeValue = 2";
            }
            else if (buildType == "BuildType_WordProcessing")
            {
                whereClause = "WHERE BuildTypeValue = 3";
            }

            // Will check the condition of which to sort likes, the default else clause will sort in descending order of likes.
            if (orderLikes == "AscendingLikes")
            {
                orderBy = "ORDER BY LikeIncrementor ASC";
            }

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
                        $"SELECT * from mostpopularbuilds {whereClause} {orderBy};";

                    // this will start the sql data reader that will be utilized to read through the database. for only the duration of the using block.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // while the reader is reading, it will sweep the database until it finds the id item and return it as values.
                        while (reader.Read())
                        {
                            var buildPostEntity = new BuildPostEntity(); // create a new object for each while loop.
                            buildPostEntity.EntityId = reader["EntityId"].ToString();
                            buildPostEntity.Username = (string)reader["Username"];
                            buildPostEntity.Title = (string)reader["Title"];
                            buildPostEntity.Description = (string)reader["Description"];
                            buildPostEntity.LikeIncrementor = (int)reader["LikeIncrementor"];
                            buildPostEntity.BuildTypeValue = (int)reader["BuildTypeValue"];
                            buildPostEntity.BuildImagePath = (string)reader["BuildImagePath"];
                            buildPostEntity.DateTime = (string)reader["DateTime"];

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

        /// <summary>
        /// This DAO method adds a like to the database for the respective build.
        /// </summary>
        /// <param name="likeEntity">takes in an entity object.</param>
        /// <returns>returns a success state bool.</returns>
        public bool AddLike(LikeEntity likeEntity)
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
                        "INSERT INTO likes(postId, userId) VALUES (@PostId, @UserId); " +
                        "update mostpopularbuilds set LikeIncrementor = (select count(*) from likes where postId = @PostId) " +
                        "where EntityId = @PostId";

                    var parameters = new SqlParameter[2]; // initialize 5 parameters to be read through incrementally.
                    parameters[0] = new SqlParameter("@PostId", likeEntity.PostId);
                    parameters[1] = new SqlParameter("@UserId", likeEntity.UserId);

                    // This will add the range of parameters to the parameters that will be used in the query.
                    command.Parameters.AddRange(parameters);

                    try
                    {
                        var rowsAdded = command.ExecuteNonQuery();

                        // If the row that was added is one, it will commit the transaction and return true, else false.
                        if (rowsAdded == 2)
                        {
                            command.Transaction.Commit(); // sends the transaction to be commited at the database.
                            return true;
                        }
                    }
                    catch (SqlException e) when (e.Number == 2627)
                    {
                        return false;
                    }
                    return false;
                }
            }
        }
    }
}
