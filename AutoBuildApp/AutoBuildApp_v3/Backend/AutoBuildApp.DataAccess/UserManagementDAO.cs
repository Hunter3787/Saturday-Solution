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

        public string newDBPassword(string password)
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

                    command.CommandText = "UPDATE UserAccounts SET PassHash = 'password' WHERE UserEmail = ";

                    command.Transaction.Commit();
                }
            }
            return "Password has been updated";
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
                    /// command.Parameters.AddWithValue   -> fix itttttttt!!!!!
                    //Add any required parameters to the Command.Parameters collection.
                    // command.Parameters.AddWithValue("@username", userCredentials.Username);
                    var param = new SqlParameter[2];
                    param[0] = new SqlParameter("@USEREMAIL", email);
                    param[0].Value = email;
                    param[1] = new SqlParameter("@PASSHASH", password);
                    param[1].Value = password;
                    // add the commands the parameters for the stored procedure
                    command.Parameters.AddRange(param);
                    #endregion

                    var rowsAdded = command.ExecuteNonQuery();

                    // If the row that was added is one, it will commit the transaction and return true, else false.
                    if (rowsAdded == 1)
                    {
                        command.Transaction.Commit(); // sends the transaction to be commited at the database.
                        return "Password successfully updated";
                    }
                }
            }
            return "YOU FAIL";
        }



           public string newDBEmail(string userInput)
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

                    command.CommandText = "UPDATE UserAccounts SET UserEmail = 'userInput' WHERE UserAccountID = ";

                    command.Transaction.Commit();
                }
            }
            return "Email has been updated";
        }

        public string newDBUserName(string userInput)
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

                    command.CommandText = "UPDATE UserAccounts SET UserName = 'userInput' WHERE UserAccountID = ";

                    command.Transaction.Commit();
                }
            }
            return "Username has been updated";
        }

        public string showUsers()
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

                    command.CommandText = "SELECT @UserAccountID, @UserName, @UserEmail, @Roley FROM UserAccounts";


                    command.Transaction.Commit();
                }
            }
            return "";
        }
    }
}
