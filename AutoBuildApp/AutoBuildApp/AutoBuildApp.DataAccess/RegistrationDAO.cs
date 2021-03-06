﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using AutoBuildApp.Models;
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
        public RegistrationDAO(String connectionString)
        {
            // instantiation of the connections string via a constructor to avoid any hardcoding
            this._connection = connectionString;
        }
        public bool DoesUserExist(UserAccount user)
        {
            bool Flag = true;
            using (SqlConnection connection = new SqlConnection(this._connection))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        String sql = "SELECT USERID FROM userAccounts WHERE username = @USERNAME OR email = @EMAIL;";
                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
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

        public String CreateUserRecord(UserAccount user)
        {
            using (SqlConnection connection = new SqlConnection(this._connection))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (DoesUserExist(user))
                        {
                            connection.Close();
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

                        transaction.Commit();
                        return "Successful user creationff";
                    }
                    catch (SqlException ex)
                    {
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

        public String UpdateUserRecord(UserAccount user, UpdateUserDTO updatedUser)
        {
            using (SqlConnection connection = new SqlConnection(this._connection))
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

                        String sql = "Update userAccounts set username = @USERNAME, email = @EMAIL, firstname = @FIRSTNAME, lastname = @LASTNAME where email = @OLDEMAIL";
                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = String.IsNullOrEmpty(updatedUser.UserName) ? user.UserName : updatedUser.UserName;
                        adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = String.IsNullOrEmpty(updatedUser.UserEmail) ? user.UserEmail : updatedUser.UserEmail;
                        adapter.InsertCommand.Parameters.Add("@FIRSTNAME", SqlDbType.VarChar).Value = String.IsNullOrEmpty(updatedUser.FirstName) ? user.FirstName : updatedUser.FirstName;
                        adapter.InsertCommand.Parameters.Add("@LASTNAME", SqlDbType.VarChar).Value = String.IsNullOrEmpty(updatedUser.LastName) ? user.LastName : updatedUser.LastName;
                        adapter.InsertCommand.Parameters.Add("@OLDEMAIL", SqlDbType.VarChar).Value = user.UserEmail;
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
            using (SqlConnection connection = new SqlConnection(this._connection))
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
                        ret = "User deleted";
                    }
                    catch (SqlException ex)
                    {
                        // the number that represents timeout
                        if (ex.Number == -2)
                        {
                            transaction.Rollback();
                            ret = "Data store has timed out.";
                        }

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

        public String retrieveAccountInformation(UserAccount userA)
        {
            String ret = "";
            using (SqlConnection connection = new SqlConnection(this._connection))
            {
                String sequal = "SELECT userID, username, email, firstName, lastName, roley, passwordHash, registrationDate FROM userAccounts;";

                using (SqlCommand cmd = new SqlCommand(sequal, connection))
                {

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ret = $"user ID: {reader.GetInt32(0)} UserName: {reader.GetString(1)} Password Hashed { reader.GetString(5) } First Name: {reader.GetString(2)} LastNme: {reader.GetString(3)} Role { reader.GetString(4) } Refistration Date {reader.GetString(6)}";
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return ret;
        }

        public string Match(String userName, String password)
        {
            String ret = "";
            using (SqlConnection connection = new SqlConnection(this._connection))
            {
                String sequal = "SELECT UserName, passHash FROM userAccounts;";

                using (SqlCommand cmd = new SqlCommand(sequal, connection))
                {

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader.GetString(1) == userName && reader.GetString(5) == password)
                            {
                                ret = "logged in";
                                return ret;
                            }
                            else
                            {
                                ret = "incorrect login";
                                return ret;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return ret;
        }
    }
}