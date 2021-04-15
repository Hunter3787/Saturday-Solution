using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers;
using AutoBuildApp.Security.Models;
using AutoBuildSecure.WebApi.HelperFunctions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;

namespace Connor_s_Console
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");

            UserIdentity userId = new UserIdentity();
            userId.Name = "this is my name";
            IEnumerable<Claim> connorClaims = new List<Claim>() {
                new Claim(ClaimTypes.Email,"crkobel@verizon.net"),
            };
            ClaimsIdentity _claimsId =
                new ClaimsIdentity(
                    userId,
                    connorClaims,
                    userId.AuthenticationType,
                    userId.Name,
                    " ");
            ClaimsPrincipal _prince = new ClaimsPrincipal(_claimsId);
            Thread.CurrentPrincipal = _prince;

            // created a connection manager to access the connection strings in 
            // 1) the app settings .json file
            ConnectionManager conString = ConnectionManager.connectionManager;

            // 2) passing in the name I assigned my connection string 
            string connection = conString.GetConnectionStringByName("MyConnection");
            Console.WriteLine($"connection string passed in controller: {connection} ");

            UserManagementDAO userManagementDAO = new UserManagementDAO(connection);

            UserManagementManager userManagementManager = new UserManagementManager(userManagementDAO);
            Console.WriteLine(userManagementManager.UpdatePassword("P@ssword!12356"));

            Console.WriteLine(userManagementManager.UpdateEmail("crkobel@gmail.com"));

            Console.WriteLine(userManagementManager.UpdateUsername("Bobb"));
            
        }
    }
}
