using AutoBuildApp.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AutoBuildApp.Models.DTO
{
    public class SearchGateway
    {
        string _connection;
        private SqlDataAdapter adapter = new SqlDataAdapter();
        public SearchGateway(string connectionString) 
        {
            _connection = connectionString;
        }

        public ISet<IResult> Search(string searchString, string resultType) {
            bool Flag = false;
            using (SqlConnection connection = new SqlConnection(this._connection))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string sql = "SELECT productName WHERE productName = @SEARCHSTRING OR productType = @SEARCHSTRING";
                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = searchString;

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
    }
}
