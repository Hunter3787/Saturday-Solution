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
        private readonly String _connectionString;
        
        public UserManagementDAO(String connectionString)
        {
            this._connectionString = connectionString;
        }

        private SqlDataAdapter adapter = new SqlDataAdapter();


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


        public string UpdatePasswordDB(string email, string password)
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
                    var param = new SqlParameter[2];
                    param[0] = new SqlParameter("@USEREMAIL", email);
                    param[0].Value = email;
                    param[1] = new SqlParameter("@PASSHASH", password);
                    param[1].Value = password;
                    // add the commands the parameters for the stored procedure
                    command.Parameters.AddRange(param);
                    #endregion

                    var rowsAdded = command.ExecuteNonQuery();

                    // If the row that was added is one, it will commit the transaction.
                    if (rowsAdded == 1)
                    {
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return "Password successfully updated";
                    }
                }
            }
            return "Password WAS NOT successfully updated";
        }

        public string UpdateEmailDB(string email, string updatedEmail)
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
                    var param = new SqlParameter[2];
                    param[0] = new SqlParameter("@USEREMAIL", email);
                    param[0].Value = email;
                    param[1] = new SqlParameter("@EMAIL", updatedEmail);
                    param[1].Value = updatedEmail;
                    // add the commands the parameters for the stored procedure
                    command.Parameters.AddRange(param);
                    #endregion

                    var rowsAdded = command.ExecuteNonQuery();

                    // If the row that was added is one, it will commit the transaction.
                    if (rowsAdded == 1)
                    {
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return "Email successfully updated";
                    }
                }
            }
            return "Email WAS NOT successfully updated";
        }


        public string UpdateUserNameDB(string email, string updatedUserName)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                Console.WriteLine($"\tRetrieveUserPermissions METHOD \n");
                conn.Open();
                // naming convention SP name of procudrure
                string SP_UpdateUserName = "UpdateUserName";
                using (SqlCommand command = new SqlCommand(SP_UpdateUserName, conn))
                {
                    command.Transaction = conn.BeginTransaction();
                    #region SQL related

                    // https://learning.oreilly.com/library/view/adonet-in-a/0596003617/ch04s05.html
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    // 1) Create a Command, and set its CommandType property to StoredProcedure.
                    command.CommandType = CommandType.StoredProcedure;
                    // 2) Set the CommandText to the name of the stored procedure.
                    command.CommandText = SP_UpdateUserName;
                    //Add any required parameters to the Command.Parameters collection.
                    var param = new SqlParameter[2];
                    param[0] = new SqlParameter("@USEREMAIL", email);
                    param[0].Value = email;
                    param[1] = new SqlParameter("@USERNAME", updatedUserName);
                    param[1].Value = updatedUserName;
                    // add the commands the parameters for the stored procedure
                    command.Parameters.AddRange(param);
                    #endregion

                    var rowsAdded = command.ExecuteNonQuery();

                    // If the row that was added is one, it will commit the transaction.
                    if (rowsAdded == 1)
                    {
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return "Username successfully updated";
                    }
                }
            }
            return "Username WAS NOT successfully updated";
        }

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

                        DataColumn column = new DataColumn();
                        column.ColumnName = "permission";
                        column.DataType = typeof(string);
                        pair.Columns.Add(column);

                        column = new DataColumn();

                        column.ColumnName = "scopeOfPermission";
                        column.DataType = typeof(string);
                        pair.Columns.Add(column);

                        DataRow row;
                        foreach (var claim in claims)
                        {
                            row = pair.NewRow();
                            row["permission"] = claim.Type.ToString();
                            row["scopeOfPermission"] = claim.Value.ToString();
                            pair.Rows.Add(row);
                        }

                        var param = new SqlParameter[3];
                        param[0] = adapter.InsertCommand.Parameters.AddWithValue("@PERMISSIONS", pair);
                        param[1] = adapter.InsertCommand.Parameters.AddWithValue("@USERNAME", username);
                        param[2] = adapter.InsertCommand.Parameters.AddWithValue("@USERROLE", role);


                        var _reader = adapter.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();
                        Console.WriteLine($" reader rows: {_reader}");
                        if (_reader != 0)
                        {
                            return "permissions have been successfully updated";
                        }
                        else
                        {
                            return "failed to update permissions";
                        }

                    }

                    catch (SqlException ex)
                    {

                        transaction.Rollback();
                        return ex.Message;
                        //return "failed to update permissions2";
                    }
                }
            }
        }

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
                        command.CommandTimeout = TimeSpan.FromSeconds(DAOGlobals.TIMEOUT_LONG).Seconds;
                        // 1) Create a Command, and set its CommandType property to StoredProcedure.
                        command.CommandType = CommandType.StoredProcedure;
                        // 2) Set the CommandText to the name of the stored procedure.
                        command.CommandText = SP_DeleteAccount;
                        /// command.Parameters.AddWithValue   -> fix itttttttt!!!!!
                        //Add any required parameters to the Command.Parameters collection.
                        // command.Parameters.AddWithValue("@username", userCredentials.Username);
                        var param = new SqlParameter[2];
                        param[0] = new SqlParameter("@USERNAME", username);
                        param[0].Value = username;
                        param[1] = new SqlParameter("@DATEDELETE", DateTimeOffset.Now);
                        param[1].Value = DateTimeOffset.Now;
                        // add the commands the parameters for the stored procedure
                        command.Parameters.AddRange(param);
                        #endregion

                        var executeCommand = command.ExecuteNonQuery();
                        command.Transaction.Commit();
                        Console.WriteLine($" reader rows: {executeCommand}");
                        if (executeCommand != 0)
                        {
                            return "account has been successfully deleted";
                        }
                        else
                        {
                            return "failed to delete";
                        }
                        
                    }
                    catch (SqlException e)
                    {
                        command.Transaction.Rollback();
                        if (!conn.State.Equals(ConnectionState.Open))
                        {
                            return "Cannot process request";
                        }
                        Console.WriteLine("SqlException.GetType: {0}", e.GetType());
                        Console.WriteLine("SqlException.Source: {0}", e.Source);
                        Console.WriteLine("SqlException.ErrorCode: {0}", e.ErrorCode);
                        Console.WriteLine("SqlException.Message: {0}", e.Message);
                    }
                    

                    return "Account was deleted";
                }

            }
        }

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

                    command.CommandText = "SELECT userRole FROM UserCredentials " +
                        "WHERE username = @username";
                    var parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@username", username);

                    command.Parameters.AddRange(parameters);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            role = (string)reader["userRole"];
                        }
                    }

                    // Executes the query.
                    command.ExecuteNonQuery();

                    // sends the transaction to be commited at the database.
                    command.Transaction.Commit();

                    // returns the list of entity object.
                    return role;
                }
            }
        }
    }
}
