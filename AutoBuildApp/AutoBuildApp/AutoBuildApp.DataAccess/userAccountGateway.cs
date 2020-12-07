
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;
using System.Data;
using AutoBuildApp.Models;

using System.Configuration; // for ConfigurationManager
namespace AutoBuildApp.DataAccess
{
    public class userAccountGateway
    {
        private String connection =" ";
        public userAccountGateway()
        {
            this.connection = null;
        }
        
        public userAccountGateway(String connectionString)
        {
            this.connection = connectionString;
        }

        Microsoft.Data.SqlClient.SqlDataAdapter adapter = new SqlDataAdapter();

       public Boolean verifyAccountExists(UserAccount userA)
        {
            // now how to verify account exists? -> from their pk: userID
            bool Flag = true;
            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                // using (var command = new Microsoft.Data.SqlClient.SqlCommand()){
                // using statement is used because it automatically closes when you reach the end curly brace

                Microsoft.Data.SqlClient.SqlDataAdapter adapter = new SqlDataAdapter();


                String sequal = "SELECT USERID FROM userAccounts WHERE username = @USERNAME AND email = @EMAIL;";
                adapter.InsertCommand = new SqlCommand(sequal, connection);
                adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = userA.UserName;
                adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = userA.UserEmail;
                try
                    {
                        connection.Open();
                        object test = adapter.InsertCommand.ExecuteScalar();
                        if (test != null) { 
                        // then user does exist 
                        Flag = true; }
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
            

        public  String createUserAccountinDB(UserAccount userA)
        {

            using (SqlConnection connection = new SqlConnection(this.connection))
            {
                String sql = "INSERT INTO userAccounts(username, email, firstName, lastName, roley)  VALUES(@USERNAME,@EMAIL, @FIRSTNAME, @LASTNAME, @ROLEY);";

            Microsoft.Data.SqlClient.SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.InsertCommand = new SqlCommand(sql, connection);
                adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = userA.UserName;
                adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = userA.UserEmail;
                adapter.InsertCommand.Parameters.Add("@FIRSTNAME", SqlDbType.VarChar).Value = userA.FirstName;
                adapter.InsertCommand.Parameters.Add("@LASTNAME", SqlDbType.VarChar).Value = userA.LastName;
                adapter.InsertCommand.Parameters.Add("@ROLEY", SqlDbType.VarChar).Value = userA.role;

                try
                {
                    connection.Open();
                    adapter.InsertCommand.ExecuteNonQuery();
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    //https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlerror?view=dotnet-plat-ext-5.0 
                    // 
                    throw new NotImplementedException();


                }
                Console.ReadLine();

                return " ";

            }

        }

   
        public String deleteUserAccountinDB(UserAccount userA)
        {

            using (SqlConnection connection = new SqlConnection(this.connection))
            {


                String sql = "DELETE FROM userAccounts WHERE userID = ( SELECT userID FROM userAccounts WHERE username = @USERNAME AND email = @EMAIL);";

                Microsoft.Data.SqlClient.SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.InsertCommand = new SqlCommand(sql, connection);
                adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = userA.UserName;
                adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = userA.UserEmail;

                try
                {
                    connection.Open();
                    adapter.InsertCommand.ExecuteNonQuery();
                    connection.Close();
                    return "Complete";
                }
                catch (SqlException ex)
                {
                    //https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlerror?view=dotnet-plat-ext-5.0 
                    // 
                    return ex.Message;


                }

            }

        }


        public String retrieveAccountInformation(UserAccount userA)
        {
            String ret = "";

            using (SqlConnection connection = new SqlConnection(this.connection))
            {

                String sequal = "SELECT userID, username, firstName, lastName, roley FROM userAccounts WHERE username = @USERNAME AND email = @EMAIL;";

                using (SqlCommand cmd = new SqlCommand(sequal, connection))
                {
                    cmd.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = userA.UserName;
                    cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = userA.UserEmail;

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ret = $"user ID: {reader.GetInt32(0)} UserName: {reader.GetString(1)} First Name: {reader.GetString(2)}";
                        }

                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    // }
                }
            }

            return ret;

        }













    }
}
