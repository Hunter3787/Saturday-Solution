
using System;
using System.Collections.Generic;
using System.Text;

//this is a test
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
        //Represents a Transact-SQL transaction to be made in a SQL Server database
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

                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        String sequal = "SELECT USERID FROM userAccounts WHERE username = @USERNAME AND email = @EMAIL;";
                        adapter.InsertCommand = new SqlCommand(sequal, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = userA.UserName;
                        adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = userA.UserEmail;

                        object test = adapter.InsertCommand.ExecuteScalar();
                        transaction.Commit();

                        if (test != null)
                        {  // then user does exist 
                            Flag = true;
                        }
                        else { Flag = false; }
                    }
                    catch (SqlException ex)
                    {
                        //https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlerror?view=dotnet-plat-ext-5.0  
                        transaction.Rollback();
                   
                        switch (ex.Number)
                        {
                            case -2:
                                Console.WriteLine(ex.Message);
                                break;
                        }

                       }
                    finally{ connection.Close();}
                    Console.ReadLine();
                }
                return Flag;
            }
        }
        // now will be making a SQL connection  check
        public void checkConnection()
        { using (SqlConnection connection = new SqlConnection(this.connection))
            {
                connection.Open();
                Console.WriteLine("ServerVersion: {0}", connection.ServerVersion);
                Console.WriteLine("State: {0}", connection.State);
            }
        }

        public String CreateUserRecord(UserAccount user)
        {
            
            using (SqlConnection connection = new SqlConnection(this.connection))
            { connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (DoesUserExist(user))
                        {
                            connection.Close();
                            // log here?
                            return "User already exists.";
                        }
                        String sql = "INSERT INTO userAccounts(username, email, firstName, lastName, roley, registrationDate, passwordHash ) VALUES(@USERNAME,@EMAIL, @FIRSTNAME, @LASTNAME, @ROLEY, @REG, @PASSWORD);";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = user.UserName;
                        adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.UserEmail;
                        adapter.InsertCommand.Parameters.Add("@FIRSTNAME", SqlDbType.VarChar).Value = user.FirstName;
                        adapter.InsertCommand.Parameters.Add("@LASTNAME", SqlDbType.VarChar).Value = user.LastName;
                        adapter.InsertCommand.Parameters.Add("@ROLEY", SqlDbType.VarChar).Value = user.role;
                        adapter.InsertCommand.Parameters.Add("@PASSWORD", SqlDbType.VarChar).Value = user.passHash;
                        adapter.InsertCommand.Parameters.Add("@REG", SqlDbType.Date).Value = user.registrationDate;
                        adapter.InsertCommand.ExecuteNonQuery();

                        // log here? saying it was successful idk
                        transaction.Commit();
                        return "Successful user creation";
                    }
                    catch (SqlException ex)
                    { // the number that represents timeout
                        if (ex.Number == -2)
                        {
                            transaction.Rollback();
                            return ("Data store has timed out.");
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }

                    return "Successful user creation";
                }
            }
        }

        public String UpdateUserRecord(UserAccount user)
        {
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (!DoesUserExist(user))
                        {
                            transaction.Rollback();
                            connection.Close();
                            return "User doesn't exist.";
                        }

                        String sql = "INSERT INTO userAccounts(username, email, firstName, lastName, roley, passwordHash, registrationDate) VALUES(@USERNAME,@EMAIL, @FIRSTNAME, @LASTNAME, @ROLEY, @PASSWORD, @REGISTRATIONDATE);";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = user.UserName;
                        adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.UserEmail;
                        adapter.InsertCommand.Parameters.Add("@FIRSTNAME", SqlDbType.VarChar).Value = user.FirstName;
                        adapter.InsertCommand.Parameters.Add("@LASTNAME", SqlDbType.VarChar).Value = user.LastName;
                        adapter.InsertCommand.Parameters.Add("@ROLEY", SqlDbType.VarChar).Value = user.role;
                        adapter.InsertCommand.Parameters.Add("@PASSWORD", SqlDbType.VarChar).Value = user.passHash;
                        adapter.InsertCommand.Parameters.Add("@REGISTRATIONDATE", SqlDbType.Date).Value = user.registrationDate;
                        adapter.InsertCommand.ExecuteNonQuery();

                        transaction.Commit();

                    }
                    catch (SqlException ex)
                    {// the number that represents timeout
                        if (ex.Number == -2)
                        {
                            transaction.Rollback();
                            return ("Data store has timed out.");
                        } 
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return "Successfully edited";
            }
        }

        public String DeleteUserRecord(UserAccount user)
        {
            String ret = " ";
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (!DoesUserExist(user))
                        {
                            connection.Close();
                            return "User doesn't exist.";
                        }
                        String sql = "DELETE FROM userAccounts WHERE email = @EMAIL";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.UserEmail;
                        adapter.InsertCommand.ExecuteNonQuery();

                        transaction.Commit();
                        ret = "Complete";
                    }
                    catch (SqlException ex)
                    {
                        // the number that represents timeout
                        if (ex.Number == -2)
                        {
                            transaction.Rollback();
                            ret = "Data store has timed out.";
                        }

                        //https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlerror?view=dotnet-plat-ext-5.0 
                        // 
                        transaction.Rollback();
                    }
                    finally
                    {
                        connection.Close();
                    }

                    return ret;
                }
            }
        }

        public bool DoesUserExist( UserAccount user)
        {
            bool Flag = true;
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        String sql = "SELECT USERID FROM userAccounts WHERE username = @USERNAME OR email = @EMAIL;";
                        adapter.InsertCommand = new SqlCommand(sql, connection);
                        adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = user.UserName;
                        adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.UserEmail;

                        adapter.InsertCommand.Transaction = transaction;

                        var result = adapter.InsertCommand.ExecuteScalar();

                        transaction.Commit();
                        connection.Close();
                        return result != null;
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
