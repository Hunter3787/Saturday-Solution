using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Models.Users;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

        //public string newDBPassword(string password)
        //{
        //    using (var conn = new SqlConnection(_connectionString))
        //    {
        //        conn.Open();
        //        using (var command = new SqlCommand())
        //        {
        //            command.Transaction = conn.BeginTransaction();
        //            command.Connection = conn;
        //            command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
        //            command.CommandType = CommandType.Text;

        //            command.CommandText = "UPDATE UserAccounts SET PassHash = 'password' WHERE UserEmail = ";

        //            command.Transaction.Commit();
        //        }
        //    }
        //    return "Password has been updated";
        //}

        //   public string newDBEmail(string userInput)
        //{
        //    using (var conn = new SqlConnection(_connectionString))
        //    {
        //        conn.Open();
        //        using (var command = new SqlCommand())
        //        {
        //            command.Transaction = conn.BeginTransaction();
        //            command.Connection = conn;
        //            command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
        //            command.CommandType = CommandType.Text;

        //            command.CommandText = "UPDATE UserAccounts SET UserEmail = 'userInput' WHERE UserAccountID = ";

        //            command.Transaction.Commit();
        //        }
        //    }
        //    return "Email has been updated";
        //}

        //public string newDBUserName(string userInput)
        //{
        //    using (var conn = new SqlConnection(_connectionString))
        //    {
        //        conn.Open();
        //        using (var command = new SqlCommand())
        //        {
        //            command.Transaction = conn.BeginTransaction();
        //            command.Connection = conn;
        //            command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
        //            command.CommandType = CommandType.Text;

        //            command.CommandText = "UPDATE UserAccounts SET UserName = 'userInput' WHERE UserAccountID = ";

        //            command.Transaction.Commit();
        //        }
        //    }
        //    return "Username has been updated";
        //}

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
                                            "INNER JOIN UserCredentials " +
                                            "ON UserCredentials.userCredID = UserAccounts.userID";

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
                            userEntity.ModifiedBy = (reader["modifiedby"] == DBNull.Value) ? string.Empty : (string)reader["modifiedby"].ToString();
                            //userEntity.ModifiedBy = (string)reader["modifiedby"];
                            userEntityList.Add(userEntity);
                        }
                    }
                    command.ExecuteNonQuery();

                    command.Transaction.Commit();

                    return userEntityList;
                }
            }
        }


        public string DeleteUserDB(string email)
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

                    command.CommandText = "DELETE UserAccounts FROM UserAccounts " +
                        //"INNER JOIN UserCredentials" +
                        //"ON UserCredentials.userCredID = UserAccounts.userID" +
                        "WHERE email = @email";
                    var parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@email", email);

                    command.Parameters.AddRange(parameters);
                    var rowsDeleted = command.ExecuteNonQuery();

                    if (rowsDeleted == 1)
                    {
                        command.Transaction.Commit();
                        return "User deleted";
                    }
                    return "User NOT deleted";
                }
            }
        }


    }
}
