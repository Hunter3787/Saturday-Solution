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
        public bool DoesUserExist(UserAccount user)
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
                        adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = user.UserName;
                        adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.UserEmail;

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

        public String CreateUserRecord(UserAccount user)
        {
            using (SqlConnection connection = new SqlConnection(this._connection))
            {
                if(connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (DoesUserExist(user))
                        {
                            connection.Close();
                            return "User already exists.";
                        }
                        String sql = "INSERT INTO" +
                            " userAccounts(username, email, firstName, lastName, roley, passwordHash, registrationDate )" +
                            " VALUES(@USERNAME,@EMAIL, @FIRSTNAME, @LASTNAME, @ROLEY, @PASSWORD, @REG);";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = user.UserName;
                        adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.UserEmail;
                        adapter.InsertCommand.Parameters.Add("@FIRSTNAME", SqlDbType.VarChar).Value = user.FirstName;
                        adapter.InsertCommand.Parameters.Add("@LASTNAME", SqlDbType.VarChar).Value = user.LastName;
                        adapter.InsertCommand.Parameters.Add("@ROLEY", SqlDbType.VarChar).Value = user.role;
                        adapter.InsertCommand.Parameters.Add("@PASSWORD", SqlDbType.VarChar).Value = user.passHash;
                        adapter.InsertCommand.Parameters.Add("@REG", SqlDbType.Date).Value = user.registrationDate;
                        adapter.InsertCommand.ExecuteNonQuery();

                        transaction.Commit();
                        return "Successful user creation";
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == -2)
                        {
                            transaction.Rollback();
                            return ("Data store has timed out.");
                        }
                    }

                    return "Failed user creation";
                }
            }
        }



        public int updatePermissions(IEnumerable<Claim> claims) // youd want to pass in those permissions 
        {

            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaims basic = claimsFactory.GetClaims(RoleEnumType.BASIC_ROLE);


            using (SqlConnection connection = new SqlConnection(this._connection))
            {
                var SP_ChangePermissions = "SP_ChangePermissions";
                using (var command = new SqlCommand())
                {
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = SP_ChangePermissions;

                    DataTable pair = new DataTable();
                    DataColumn column = new DataColumn();

                    column.ColumnName = "UserID";
                    column.DataType = typeof(Int64);
                    pair.Columns.Add(column);
                    column.ColumnName = "permission";
                    column.DataType = typeof(string);
                    pair.Columns.Add(column);
                    column.ColumnName = "scopeOfPermission";
                    column.DataType = typeof(double);
                    pair.Columns.Add(column);

                    DataRow row;
                    foreach (var permissions in basic.Claims())
                    {
                        row = pair.NewRow();
                        row["permission"] = permissions.Type;
                        row["scopeOfPermission"] = permissions.Value;
                        pair.Rows.Add(row);
                    }
                    var param = new SqlParameter[1];
                    param[0] = command
                        .Parameters
                        .AddWithValue("@permissions", pair);
                    param[1] = command
                        .Parameters
                        .AddWithValue("@username", "new egg");


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) { return 0; }
                        else if (reader.HasRows) { return 1; }
                    }
                    command.Transaction.Commit();
                }
            }
            return 0;
        }

    }
}
