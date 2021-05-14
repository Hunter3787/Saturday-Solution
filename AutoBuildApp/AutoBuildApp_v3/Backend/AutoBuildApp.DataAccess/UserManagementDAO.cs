using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Models.Users;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class UserManagementDAO
    {
        // the connection string used by service layer and manager layer
        private readonly String _connectionString;
        
        public UserManagementDAO(String connectionString)
        {
            // initializes the connection string
            this._connectionString = connectionString;
        }

        private SqlDataAdapter adapter = new SqlDataAdapter();

        // checks to see if email is in DB
        public bool DoesEmailExist(string email)
        {
            bool Flag = false;
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // emails are unique so if it exists it will not be zero
                        String sql = "SELECT COUNT (*) FROM userAccounts WHERE email = @EMAIL;";
                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = email;

                        adapter.InsertCommand.Transaction = transaction;

                        int result = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());

                        transaction.Commit();
                        connection.Close();
                        return result != 0;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.Source);

                    }
                    return Flag;
                }
            }
        }

        // checks to see if username is in DB
        public bool DoesUsernameExist(string username)
        {
            bool Flag = false;
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // username is also unique so if it exists it will not be zero
                        String sql = "SELECT COUNT (*) FROM userCredentials WHERE username = @USERNAME;";
                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

                        adapter.InsertCommand.Transaction = transaction;

                        int result = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());

                        transaction.Commit();
                        connection.Close();
                        return result != 0;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.Source);

                    }
                    return Flag;
                }
            }
        }

        // updates the password in DB
        public string UpdatePasswordDB(string password, string activeUsername)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                Console.WriteLine($"\tRetrieveUserPermissions METHOD \n");
                conn.Open();
                // naming convention SP name of procudrure
                string SP_UpdatePassword = "UpdatePassword";
                using (SqlCommand command = new SqlCommand(SP_UpdatePassword, conn))
                {
                    command.Transaction = conn.BeginTransaction();
                    #region SQL related

                    // https://learning.oreilly.com/library/view/adonet-in-a/0596003617/ch04s05.html
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    // 1) Create a Command, and set its CommandType property to StoredProcedure.
                    command.CommandType = CommandType.StoredProcedure;
                    // 2) Set the CommandText to the name of the stored procedure.
                    command.CommandText = SP_UpdatePassword;
                    //Add any required parameters to the Command.Parameters collection.
                    var param = new SqlParameter[3];
                    // sets values for the sql statement
                    param[0] = new SqlParameter("@ACTIVEUSERNAME", activeUsername);
                    param[1] = new SqlParameter("@PASSHASH", password);
                    param[2] = new SqlParameter("@MODIFYDATE", DateTimeOffset.Now);
                    // add the commands the parameters for the stored procedure
                    command.Parameters.AddRange(param);
                    #endregion

                    var rowsAdded = command.ExecuteNonQuery();

                    // If the row that was added is two, it will commit the transaction.
                    // reason it's 2: updating modifiedAt happens in UserAccounts and UserCredentials
                    if (rowsAdded == 2)
                    {
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return "Password successfully updated";
                    }
                }
            }
            return "Password WAS NOT successfully updated";
        }

        // updates the email in DB
        public string UpdateEmailDB(string inputEmail, string activeUsername)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                Console.WriteLine($"\tRetrieveUserPermissions METHOD \n");
                conn.Open();
                // naming convention SP name of procudrure
                string SP_UpdateEmail = "UpdateEmail";
                using (SqlCommand command = new SqlCommand(SP_UpdateEmail, conn))
                {
                    command.Transaction = conn.BeginTransaction();
                    #region SQL related

                    // https://learning.oreilly.com/library/view/adonet-in-a/0596003617/ch04s05.html
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    // 1) Create a Command, and set its CommandType property to StoredProcedure.
                    command.CommandType = CommandType.StoredProcedure;
                    // 2) Set the CommandText to the name of the stored procedure.
                    command.CommandText = SP_UpdateEmail;
                    //Add any required parameters to the Command.Parameters collection.
                    var param = new SqlParameter[3];
                    // sets values for the sql statement
                    param[0] = new SqlParameter("@ACTIVEUSERNAME", activeUsername);
                    param[1] = new SqlParameter("@INPUTEMAIL", inputEmail);
                    param[2] = new SqlParameter("@MODIFYDATE", DateTimeOffset.Now);
                    // add the commands the parameters for the stored procedure
                    command.Parameters.AddRange(param);
                    #endregion

                    var rowsAdded = command.ExecuteNonQuery();

                    // If the row that was added is two, it will commit the transaction.
                    // reason it's 2: updating modifiedAt happens in UserAccounts and UserCredentials
                    if (rowsAdded == 2)
                    {
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return "Email successfully updated";
                    }
                }
            }
            return "Email WAS NOT successfully updated";
        }

        // updates the username in DB
        public string UpdateUserNameDB(string username, string activeUsername)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                Console.WriteLine($"\tRetrieveUserPermissions METHOD \n");
                conn.Open();
                // naming convention SP name of procudrure
                string SP_UpdateUsername = "UpdateUsername";
                using (SqlCommand command = new SqlCommand(SP_UpdateUsername, conn))
                {
                    command.Transaction = conn.BeginTransaction();
                    #region SQL related

                    // https://learning.oreilly.com/library/view/adonet-in-a/0596003617/ch04s05.html
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    // 1) Create a Command, and set its CommandType property to StoredProcedure.
                    command.CommandType = CommandType.StoredProcedure;
                    // 2) Set the CommandText to the name of the stored procedure.
                    command.CommandText = SP_UpdateUsername;
                    //Add any required parameters to the Command.Parameters collection.
                    var param = new SqlParameter[3];
                    // sets values for the sql statement
                    param[0] = new SqlParameter("@ACTIVEUSERNAME", activeUsername);
                    param[1] = new SqlParameter("@USERNAME", username);
                    param[2] = new SqlParameter("@MODIFYDATE", DateTimeOffset.Now);
                    // add the commands the parameters for the stored procedure
                    command.Parameters.AddRange(param);
                    #endregion

                    var rowsAdded = command.ExecuteNonQuery();

                    // If the row that was added is two, it will commit the transaction.
                    // reason it's 2: updating modifiedAt happens in UserAccounts and UserCredentials
                    if (rowsAdded == 2)
                    {
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return "Username successfully updated";
                    }
                }
            }
            return "Username WAS NOT successfully updated";
        }

        // retrieves information on all accounts in DB
        public List<UserEntity> getUsers()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var command = new SqlCommand())
                {
                    var userEntityList = new List<UserEntity>();
                    command.Transaction = conn.BeginTransaction();
                    command.Connection = conn;
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    command.CommandType = CommandType.Text;

                    command.CommandText = "SELECT * FROM UserAccounts " +
                        "INNER JOIN MappingHash " +
                        "ON MappingHash.userID = UserAccounts.userID " +
                        "INNER JOIN UserCredentials " +
                        "ON UserCredentials.userHashID = MappingHash.userHashID;";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // uses reader to set values from the DB to the userEntity
                        // entity is given back to service layer to be added to the list
                        while (reader.Read())
                        {
                            var userEntity = new UserEntity();
                            userEntity.UserID = reader["userID"].ToString();
                            userEntity.UserName = (string)reader["username"];
                            userEntity.Email = (string)reader["email"];
                            userEntity.FirstName = (string)reader["firstName"];
                            userEntity.LastName = (string)reader["lastName"];
                            userEntity.CreatedAt = reader["createdat"].ToString();
                            userEntity.ModifiedAt = reader["modifiedat"].ToString();
                            userEntity.UserRole = (string)reader["userRole"];
                            userEntity.LockState = (bool)reader["locked"];
                            userEntityList.Add(userEntity);
                        }
                    }
                    command.ExecuteNonQuery();

                    command.Transaction.Commit();

                    return userEntityList;
                }
            }
        }

        // changes the permissions of a user
        // passes in claims to get the set of permissions based on the role
        public string ChangePermissionsDB(string username, string role, IEnumerable<Claim> claims)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();

                        string SP_UpdateUserPermissions = "UpdateUserPermissions";
                        adapter.InsertCommand = new SqlCommand(SP_UpdateUserPermissions, connection, transaction);
                        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                        adapter.InsertCommand.CommandText = SP_UpdateUserPermissions;

                        DataTable pair = new DataTable();
                        // gets the permission column
                        DataColumn column = new DataColumn();
                        column.ColumnName = "permission";
                        column.DataType = typeof(string);
                        pair.Columns.Add(column);

                        column = new DataColumn();
                        // gets the scopeOfPermission
                        column.ColumnName = "scopeOfPermission";
                        column.DataType = typeof(string);
                        pair.Columns.Add(column);

                        DataRow row;
                        foreach (var claim in claims)
                        {
                            // goes down the columns adding to the new permission set
                            row = pair.NewRow();
                            row["permission"] = claim.Type.ToString();
                            row["scopeOfPermission"] = claim.Value.ToString();
                            pair.Rows.Add(row);
                        }

                        var param = new SqlParameter[4];
                        // sets values for sql statement
                        param[0] = adapter.InsertCommand.Parameters.AddWithValue("@PERMISSIONS", pair);
                        param[1] = adapter.InsertCommand.Parameters.AddWithValue("@USERNAME", username);
                        param[2] = adapter.InsertCommand.Parameters.AddWithValue("@USERROLE", role);
                        param[3] = adapter.InsertCommand.Parameters.AddWithValue("@MODIFYDATE", DateTimeOffset.Now);


                        var _reader = adapter.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();
                        Console.WriteLine($" reader rows: {_reader}");
                        if (_reader != 0)
                        {
                            return "Permissions successfully updated";
                        }
                        else
                        {
                            return "Failed to update permissions";
                        }

                    }

                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        return ex.Message;
                    }
                }
            }
        }

        // changes a user's lockstate to locked
        public string Lock(string username)
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

                    command.CommandText = "UPDATE UserCredentials " +
                        "SET locked = @v0 " +
                        "WHERE username = @v1";

                    
                    var parameters = new SqlParameter[2];
                    // lockstate is stored as a BIT for true = 1, false = 0
                    // gives values for the sql statement
                    parameters[0] = new SqlParameter("@v0", '1');
                    parameters[1] = new SqlParameter("@v1", username);

                    command.Parameters.AddRange(parameters);

                    try
                    {
                        var rowsAdded = command.ExecuteNonQuery();

                        if (rowsAdded == 1)
                        {
                            command.Transaction.Commit();
                            return "Account has been locked";
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return "Account was NOT locked";
                    }
                }
            }
            return "Account was NOT locked";
        }

        // changes a user's lockstate to unlocked
        public string Unlock(string username)
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

                    command.CommandText = "UPDATE UserCredentials " +
                        "SET locked = @v0 " +
                        "WHERE username = @v1";

                    var parameters = new SqlParameter[2];
                    // lockstate is stored as a BIT for true = 1, false = 0
                    // gives values for the sql statement
                    parameters[0] = new SqlParameter("@v0", '0');
                    parameters[1] = new SqlParameter("@v1", username);

                    command.Parameters.AddRange(parameters);
                    
                    try
                    {
                        var rowsAdded = command.ExecuteNonQuery();

                        if (rowsAdded == 1)
                        {
                            command.Transaction.Commit();
                            return "Account has been unlocked";
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return "Account has NOT been unlocked";
                    }
                }
            }
            return "Account has been unlocked";
        }

        // Deletes a user from the DB
        public string DeleteUserDB(string username)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                Console.WriteLine($"\tRetrieveUserPermissions METHOD \n");
                conn.Open();
                // naming convention SP_ name of procudrure
                string SP_DeleteAccount = "DeleteAccount";
                using (SqlCommand command = new SqlCommand(SP_DeleteAccount, conn))
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
                        command.CommandText = SP_DeleteAccount;
                        //Add any required parameters to the Command.Parameters collection.
                        var param = new SqlParameter[2];
                        // gives values for the sql statement
                        param[0] = new SqlParameter("@USERNAME", username);
                        param[1] = new SqlParameter("@DATEDELETE", DateTimeOffset.Now);
                        // add the commands the parameters for the stored procedure
                        command.Parameters.AddRange(param);
                        #endregion

                        var executeCommand = command.ExecuteNonQuery();
                        command.Transaction.Commit();
                        Console.WriteLine($" reader rows: {executeCommand}");
                        if (executeCommand != 0)
                        {
                            return "Account has been successfully deleted";
                        }
                        else
                        {
                            return "Failed to delete";
                        }
                        
                    }
                    catch (SqlException ex)
                    {
                        command.Transaction.Rollback();
                        if (!conn.State.Equals(ConnectionState.Open))
                        {
                            Console.WriteLine(ex.Message);
                            return "Cannot process request";
                        }
                    }
                    return "Account was deleted";
                }

            }
        }

        // retrieves the role of the selected user
        public string RoleCheckDB(string username)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var command = new SqlCommand())
                {
                    string role = "";
                    command.Transaction = conn.BeginTransaction();
                    command.Connection = conn;
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    command.CommandType = CommandType.Text;

                    // sql statement to retrieve role
                    command.CommandText = "SELECT userRole FROM UserCredentials " +
                        "WHERE username = @username";
                    var parameters = new SqlParameter[1];
                    // add value for sql statement
                    parameters[0] = new SqlParameter("@username", username);

                    command.Parameters.AddRange(parameters);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // gives value to role
                            role = (string)reader["userRole"];
                        }
                    }

                    // Executes the query.
                    command.ExecuteNonQuery();

                    // sends the transaction to be commited at the database.
                    command.Transaction.Commit();

                    // returns the role from DB
                    return role;
                }
            }
        }
    }
}
