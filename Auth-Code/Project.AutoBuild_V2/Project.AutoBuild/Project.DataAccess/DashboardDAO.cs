
using System.Data;
using System;
using Microsoft.Data.SqlClient;

namespace Project.DataAccess
{
    public class DashboardDAO
    {

        private String _connection;
        //private LogObject log;

        private SqlDataAdapter adapter = new SqlDataAdapter();
        public DashboardDAO(String connectionString)
        {
            this._connection = connectionString;
        }

        public bool tryConnection()
        {
            SqlConnection con = new SqlConnection(this._connection);
            con.Open();
            if (this._connection != null && con.State != ConnectionState.Closed)
            {
                return true;
            }
            else
                con.Close();
            return false;
        }

        public Object GetNumberOfUsers()
        {
            using (SqlConnection connection = new SqlConnection(this._connection))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    String sequal = "SELECT COUNT (*) FROM userAccounts"; // WHERE UserName = @USERNAME AND passwordHash = @PASSWORDHASH;";
                    adapter.InsertCommand = new SqlCommand(sequal, connection, transaction);
                    //adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = userName;
                    //adapter.InsertCommand.Parameters.Add("@PASSWORDHASH", SqlDbType.VarChar).Value = password;

                    try
                    {
                        int rows = 0;
                        rows = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                        transaction.Commit();
                        if (rows >= 0)
                        {
                            return rows;
                        }
                        else return "no rows";
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