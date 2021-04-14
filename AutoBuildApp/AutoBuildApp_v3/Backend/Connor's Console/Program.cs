using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers;
using AutoBuildSecure.WebApi.HelperFunctions;
using System;

namespace Connor_s_Console
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");

            // created a connection manager to access the connection strings in 
            // 1) the app settings .json file
            ConnectionManager conString = ConnectionManager.connectionManager;
            // 2) passing in the name I assigned my connection string 
            string connection = conString.GetConnectionStringByName("MyConnection");
            Console.WriteLine($"connection string passed in controller: {connection} ");

            UserManagementDAO userManagementDAO = new UserManagementDAO(connection);
            //userManagementDAO.UpdatePasswordDB("ZeinabFarhat@gmail.com", "password");

            UserManagementManager userManagementManager = new UserManagementManager(userManagementDAO);
            Console.WriteLine(userManagementManager.UpdatePassword("P@ssword!123"));
            
        }
    }
}
