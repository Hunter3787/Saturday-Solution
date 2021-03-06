/*using AutoBuildApp.BusinessLayer;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.ServiceLayer;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data;

namespace UnitTests
{
    [TestMethod]
    public void whatever()
    {
        using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-LG4QVLU;Initial Catalog=Registration_Pack;Integrated Security=True"))
        {
            int rows = 0;
            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {


                SqlDataAdapter adapter = new SqlDataAdapter();

                String sql = "INSERT INTO userAccounts(username, roley) VALUES(@USERNAME, @ROLEY);";

                adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = "OOGABOOGA";
                adapter.InsertCommand.Parameters.Add("@ROLEY", SqlDbType.VarChar).Value = "BASIC";
                rows = adapter.InsertCommand.ExecuteNonQuery();

                transaction.Commit();

            }

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                SqlDataAdapter adapter = new SqlDataAdapter();

                String sql = "DELETE from userAccounts where username = @USERNAME;";

                adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = "usernamjj";
                adapter.InsertCommand.ExecuteNonQuery();

                transaction.Commit();
            }

            Assert.AreEqual(1, rows);
        }

    }
}
*/