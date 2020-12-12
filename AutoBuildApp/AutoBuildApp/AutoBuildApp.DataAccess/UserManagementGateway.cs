
using System;
using System.Collections.Generic;
using System.Text;


/*
 * this "Microsoft.Data.SqlClient" is a swap 
 * out for the existing "System.Data.SqlClient" namespace. since i started using the latter
 * https://www.connectionstrings.com/the-new-microsoft-data-sqlclient-explained/
 * https://devblogs.microsoft.com/dotnet/introducing-the-new-microsoftdatasqlclient/
 */
using Microsoft.Data.SqlClient;
using System.Data;
using AutoBuildApp.Models;


namespace AutoBuildApp.DataAccess
{
    public class UserManagementGateway
    {
        private String connection;

        /*
         * "Represents a set of data commands and a database connection 
         * that are used to fill the DataSet and update a SQL Server database"
         * https://docs.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqldataadapter?view=sqlclient-dotnet-core-2.1
         * 
         * the adapter is essentially the "gateway" to the actual database side
         */
        private SqlDataAdapter adapter = new SqlDataAdapter();

        public UserManagementGateway(String connectionString)
        {
            // instantiation of the connections string via a constructor to avoid any hardcoding
            this.connection = connectionString;
        }


        /*
         * this method carries out a query that checks an instance of the userAccount
         * module to see if it exists in the database or not
         * by returning a boolean
         * 
         */
        public Boolean verifyAccountExists(UserAccount userA)
        {
            // now how to verify account exists? -> from their pk: userID
            bool Flag = true;
            using (SqlConnection connection = new SqlConnection(this.connection))
            {  // using statement is used because it automatically closes when you reach the end curly brace
                // from what i have read this saves us from th chance of our system slowing down because a 
                // connection was not closed


                String sequal = "SELECT USERID FROM userAccounts WHERE username = @USERNAME AND email = @EMAIL;";
                adapter.InsertCommand = new SqlCommand(sequal, connection);
                adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = userA.UserName;
                adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = userA.UserEmail;
                try
                {
                    connection.Open();
                    object test = adapter.InsertCommand.ExecuteScalar();
                    if (test != null)
                    {
                        // then user does exist 
                        Flag = true;
                    }
                    else { Flag = false; }

                    connection.Close();
                }
                catch (SqlException ex)
                {
                    //https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlerror?view=dotnet-plat-ext-5.0 
                    // 
                    throw new NotImplementedException();

                }
                Console.ReadLine();
                // }
            }

            return Flag;


        }


        // now will be making a SQL connection  check
        public void checkConnection()
        {
            //https://prod.liveshare.vsengsaas.visualstudio.com/join?0C811C8DF3B3EA0C85449FA8739BD16D004D
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                connection.Open();
                Console.WriteLine("ServerVersion: {0}", connection.ServerVersion);
                Console.WriteLine("State: {0}", connection.State);
            }
        }

        public String CreateUserRecord(UserAccount user)
        {
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                try
                {
                    connection.Open();
                    // best way to check?
                    if (DoesUserExist(connection, user))
                    {
                        connection.Close();
                        // log here?
                        return "User already exists.";
                    }

                    String sql = "INSERT INTO userAccounts(username, email, firstName, lastName, roley)  VALUES(@USERNAME,@EMAIL, @FIRSTNAME, @LASTNAME, @ROLEY);";

                    adapter.InsertCommand = new SqlCommand(sql, connection);
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = user.UserName;
                    adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.UserEmail;
                    adapter.InsertCommand.Parameters.Add("@FIRSTNAME", SqlDbType.VarChar).Value = user.FirstName;
                    adapter.InsertCommand.Parameters.Add("@LASTNAME", SqlDbType.VarChar).Value = user.LastName;
                    adapter.InsertCommand.Parameters.Add("@ROLEY", SqlDbType.VarChar).Value = user.role;

                    adapter.InsertCommand.ExecuteNonQuery();
                    connection.Close();

                    // log here? saying it was successful idk
                    return "Successful user creation";
                }
                catch (SqlException ex)
                {
                    // the number that represents timeout
                    if (ex.Number == -2)
                    {
                        return ("Data store has timed out.");
                    }

                    //https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlerror?view=dotnet-plat-ext-5.0 
                    // 
                    throw new NotImplementedException();
                }
            }
        }

        public String UpdateUserRecord(UserAccount user)
        {
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                try
                {
                    connection.Open();
                    // best way to check?
                    if (!DoesUserExist(connection, user))
                    {
                        connection.Close();
                        // log here?
                        return "User doesn't exist.";
                    }

                    String sql = "INSERT INTO userAccounts(username, email, firstName, lastName, roley)  VALUES(@USERNAME,@EMAIL, @FIRSTNAME, @LASTNAME, @ROLEY);";

                    adapter.InsertCommand = new SqlCommand(sql, connection);
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = user.UserName;
                    adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.UserEmail;
                    adapter.InsertCommand.Parameters.Add("@FIRSTNAME", SqlDbType.VarChar).Value = user.FirstName;
                    adapter.InsertCommand.Parameters.Add("@LASTNAME", SqlDbType.VarChar).Value = user.LastName;
                    adapter.InsertCommand.Parameters.Add("@ROLEY", SqlDbType.VarChar).Value = user.role;

                    adapter.InsertCommand.ExecuteNonQuery();
                    connection.Close();

                }
                catch (SqlException ex)
                {
                    // the number that represents timeout
                    if (ex.Number == -2)
                    {
                        return ("Data store has timed out.");
                    }

                    //https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlerror?view=dotnet-plat-ext-5.0 
                    // 
                    throw new NotImplementedException();
                }
            }

            return "Successfully edited";
        }

        public String DeleteUserRecord(UserAccount user)
        {

            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                try
                {
                    connection.Open();
                    if (!DoesUserExist(connection, user))
                    {
                        connection.Close();
                        return "User doesn't exist.";
                    }

                    String sql = "DELETE FROM userAccounts WHERE email = @EMAIL";

                    adapter.InsertCommand = new SqlCommand(sql, connection);
                    adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.UserEmail;
                    adapter.InsertCommand.ExecuteNonQuery();
                    connection.Close();
                    return "Complete";
                }
                catch (SqlException ex)
                {
                    // the number that represents timeout
                    if (ex.Number == -2)
                    {
                        return ("Data store has timed out.");
                    }

                    //https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlerror?view=dotnet-plat-ext-5.0 
                    // 
                    throw new NotImplementedException();
                }

            }

        }


        //public String retrieveAccountInformation(UserAccount userA)
        //{
        //    String ret = "";

        //    using (SqlConnection connection = new SqlConnection(this.connection))
        //    {

        //        String sequal = "SELECT userID, username, firstName, lastName, roley FROM userAccounts WHERE username = @USERNAME AND email = @EMAIL;";

        //        using (SqlCommand cmd = new SqlCommand(sequal, connection))
        //        {
        //            cmd.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = userA.UserName;
        //            cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = userA.UserEmail;

        //            try
        //            {
        //                connection.Open();

        //                SqlDataReader reader = cmd.ExecuteReader();
        //                while (reader.Read())
        //                {
        //                    ret = $"user ID: {reader.GetInt32(0)} UserName: {reader.GetString(1)} First Name: {reader.GetString(2)}";
        //                }

        //            }
        //            catch (SqlException ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }

        //            // }
        //        }
        //    }

        //    return ret;

        //}

        public bool DoesUserExist(SqlConnection connection, UserAccount user)
        {
            String sql = "SELECT USERID FROM userAccounts WHERE username = @USERNAME OR email = @EMAIL;";
            adapter.InsertCommand = new SqlCommand(sql, connection);
            adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = user.UserName;
            adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.UserEmail;
            var result = adapter.InsertCommand.ExecuteScalar();

            return result != null;
        }
        public bool validEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        public bool validUserName(string username)
        {
            return !String.IsNullOrEmpty(username) && username.Length >= 4 && username.Length <= 12;
        }


        public bool IsInformationValid(UserAccount user)
        {
            return validEmail(user.UserEmail)
                && validUserName(user.UserName)
                && !String.IsNullOrEmpty(user.FirstName)
                && !String.IsNullOrEmpty(user.LastName);
        }


    }
}
