﻿using AutoBuildApp.Models;

using AutoBuildApp.DataAccess;

using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace AutoBuildApp.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            UserAccount userA = new UserAccount("username" ,"user", "name", "user@gmail.com", "BASIC");
            
            
            // how to connect to sql in a console application
            // https://www.c-sharpcorner.com/UploadFile/5089e0/how-to-create-single-connection-string-in-console-applicatio/

            using (SqlConnection connection = new SqlConnection(Settings1.Default.connectionString))
            {

                Microsoft.Data.SqlClient.SqlDataAdapter adapter = new SqlDataAdapter();
                // Console.WriteLine(isUserCreatedDTO.createUserinDB(userA));
                //Console.WriteLine(UserAccountInfromationDTO.DisplayInfoOnUser(userA));




                String sequal = "SELECT USERID FROM userAccounts WHERE username = @USERNAME AND email = @EMAIL;";

                adapter.InsertCommand = new SqlCommand(sequal, connection);
                adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = userA.UserName;
                adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = userA.UserEmail;

                try
                {
                    //====================
                    connection.Open();
                    Console.WriteLine("ServerVersion: {0}", connection.ServerVersion);
                    Console.WriteLine("State: {0}", connection.State);

                    //====================
                    object test = adapter.InsertCommand.ExecuteScalar();
                    if (test != null)
                    {
                        String result = test.ToString();
                        Console.WriteLine(test);

                    }
                    else
                    {
                        Console.WriteLine("sorry doesnt exist");
                    }
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();




                //======================= testing the create User

                UserAccount userB = new UserAccount("HEYYYYY", "user", "hey", "hey@gmail.com", "BASIC");


                String sql = "INSERT INTO userAccounts(username, email, firstName, lastName, roley)  VALUES(@USERNAME,@EMAIL, @FIRSTNAME, @LASTNAME, @ROLEY);";

                adapter.InsertCommand = new SqlCommand(sql, connection);
                adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = userB.UserName;
                adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = userB.UserEmail;
                adapter.InsertCommand.Parameters.Add("@FIRSTNAME", SqlDbType.VarChar).Value = userB.FirstName;
                adapter.InsertCommand.Parameters.Add("@LASTNAME", SqlDbType.VarChar).Value = userB.LastName;
                adapter.InsertCommand.Parameters.Add("@ROLEY", SqlDbType.VarChar).Value = userB.role;

                try
                {
                    connection.Open();
                    adapter.InsertCommand.ExecuteNonQuery();
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("stoop");
                Console.ReadLine();


                Console.WriteLine("we are goign to delete user B");


                sql = "DELETE FROM userAccounts WHERE userID = ( SELECT userID FROM userAccounts WHERE username = @USERNAME AND email = @EMAIL);";

                adapter.InsertCommand = new SqlCommand(sql, connection);
                adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = userB.UserName;
                adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = userB.UserEmail;

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
                    Console.WriteLine(ex.Message);


                }
                Console.ReadLine();


                // ================

                // using (var command = new Microsoft.Data.SqlClient.SqlCommand()){
                // using statement is used because it automatically closes when you reach the end curly brace

                UserAccount userC = new UserAccount("use", "heyer", "kaye", "kaye@gmail.com", "BASIC");


                sequal = "SELECT userID, username, firstName, lastName, roley FROM userAccounts WHERE username = @USERNAME AND email = @EMAIL;";



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
                            Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));


                        }
                        Console.WriteLine("out");

                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

           }    }
        }
    }
}
