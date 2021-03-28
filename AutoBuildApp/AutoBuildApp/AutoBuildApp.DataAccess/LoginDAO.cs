using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class LoginDAO
    {
        private String _connection;
        private SqlDataAdapter adapter = new SqlDataAdapter();
        public LoginDAO(String connectionString)
        {
            // instantiation of the connections string via a constructor to avoid any hardcoding
            this._connection = connectionString;
        }

        // check the database and see if the username and password is equal to what is input
        public String MatchData(String userName, String password)
        {
            using (SqlConnection connection = new SqlConnection(this._connection))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    String sequal = "SELECT COUNT (*) FROM userAccounts WHERE UserName = @USERNAME AND passwordHash = @PASSWORDHASH;";
                    adapter.InsertCommand = new SqlCommand(sequal, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = userName;
                    adapter.InsertCommand.Parameters.Add("@PASSWORDHASH", SqlDbType.VarChar).Value = password;

                    try
                    {
                        int rows = 0;
                        rows = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                        transaction.Commit();
                        if (rows == 1)
                        {
                            return "Logged in";
                        }
                        else return "Failed to login";
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        return ex.Message;
                    }
                }
            }
        }
    }
}
