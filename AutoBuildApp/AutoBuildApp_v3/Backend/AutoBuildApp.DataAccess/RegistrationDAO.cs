using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Text;
using AutoBuildApp.Models.Users;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using Microsoft.Data.SqlClient;

namespace AutoBuildApp.DataAccess
{
    public class RegistrationDAO
    {

        private String _connection;
        /*
         * "Represents a set of data commands and a database connection 
         * that are used to fill the DataSet and update a SQL Server database"
         * https://docs.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqldataadapter?view=sqlclient-dotnet-core-2.1
         * 
         * the adapter is essentially the "gateway" to the actual database side
         */
        private SqlDataAdapter adapter = new SqlDataAdapter();
        //Represents a Transact-SQL transaction to be made in a SQL Server database
        public RegistrationDAO(string connectionString)
        {
            // instantiation of the connections string via a constructor to avoid any hardcoding
            this._connection = connectionString;
        }
        public bool DoesUserExist(string username, string email)
        {
            bool Flag = false;
            using (SqlConnection connection = new SqlConnection(this._connection))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        String sql = "SELECT COUNT (*) FROM userAccounts WHERE username = @USERNAME OR email = @EMAIL;";
                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;
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


        public string RegisterAccount(UserAccount user)
        {
            ClaimsFactory _claimsFactory = new ConcreteClaimsFactory();
            IClaims basicUser = _claimsFactory.GetClaims(RoleEnumType.BasicRole);
         
            using (SqlConnection connection = new SqlConnection(_connection))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {

                        SqlDataAdapter adapter = new SqlDataAdapter();

                        string SP_RegisterAccount = "RegisterAccount";
                        adapter.InsertCommand = new SqlCommand(SP_RegisterAccount, connection, transaction);
                        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                        adapter.InsertCommand.CommandText = SP_RegisterAccount;

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
                        foreach (var claim in basicUser.Claims())
                        {
                            row = pair.NewRow();
                            row["permission"] = claim.Type.ToString();
                            row["scopeOfPermission"] = claim.Value.ToString();
                            pair.Rows.Add(row);
                        }

                        //foreach (DataRow r in pair.Rows)
                        //{
                        //    foreach (DataColumn c in pair.Columns)
                        //        Console.Write("\t{0}", r[c]);

                        //    Console.WriteLine("\t\t\t" + r.RowState);
                        //}

                        var param = new SqlParameter[7];
                        param[0] = adapter.InsertCommand.Parameters.AddWithValue("@PERMISSIONS", pair);
                        param[1] = adapter.InsertCommand.Parameters.AddWithValue("@USERNAME", user.UserName);
                        param[2] = adapter.InsertCommand.Parameters.AddWithValue("@FNAME", user.FirstName);
                        param[3] = adapter.InsertCommand.Parameters.AddWithValue("@LNAME", user.LastName);
                        param[4] = adapter.InsertCommand.Parameters.AddWithValue("@EMAIL", user.UserEmail);
                        param[5] = adapter.InsertCommand.Parameters.AddWithValue("@PASSWORD", user.passHash);
                        param[6] = adapter.InsertCommand.Parameters.AddWithValue("@CREATEDATE", DateTimeOffset.Now);

                        var _reader = adapter.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();
                        Console.WriteLine($" reader rows: {_reader}");
                        if (_reader != 0)
                        {
                            return "account has been successfully created";
                        }
                        else
                        {
                            return "failed to create user1";
                        }

                    }

                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                        {
                            transaction.Rollback();
                            return "User already exists";
                        }
                        transaction.Rollback();
                        return "failed to create user2";
                    }
                }
            }
        }
    }
}
