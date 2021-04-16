using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers;
using AutoBuildApp.Security.Models;
using AutoBuildApp.Services.UserServices;
using AutoBuildSecure.ConsoleApp;
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
            //var UserManagementService = new UserManagementService(MostPopularBuildsDAO);
            UserIdentity userId = new UserIdentity();
            userId.Name = "this is my name";
            IEnumerable<Claim> connorClaims = new List<Claim>() {
                new Claim(ClaimTypes.Email,"bob111@gmail.com"),
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

            foreach (var clm in _prince.Claims)
            {
                Console.WriteLine($" claim type: {clm.Type} claim value: {clm.Value}");
            }
            CP test = new CP();
            test.updateEmailThread();

            // created a connection manager to access the connection strings in 
            // 1) the app settings .json file
            ConnectionManager conString = ConnectionManager.connectionManager;

            // 2) passing in the name I assigned my connection string 
            string connection = conString.GetConnectionStringByName("MyConnection");
            Console.WriteLine($"connection string passed in controller: {connection} ");

            UserManagementDAO userManagementDAO = new UserManagementDAO(connection);

            UserManagementService userManagementService = new UserManagementService(userManagementDAO);

            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            _prince = (ClaimsPrincipal)Thread.CurrentPrincipal;
            Console.WriteLine($"this is updated email: " +
                $"{_prince.FindFirst(ClaimTypes.Email).Value}\n");


            Console.WriteLine(userManagementManager.UpdatePassword("P@ssword!12356"));

            Console.WriteLine(userManagementManager.UpdateEmail("crkobel@verizon.net"));

            Console.WriteLine(userManagementManager.UpdateUsername("Bobb"));

            Console.WriteLine("\nList should appear here:");
            foreach(var user in userManagementManager.GetUsersList())
            {
                Console.WriteLine(user.UserID);
                Console.WriteLine(user.UserName);
                Console.WriteLine(user.Email);
                Console.WriteLine(user.FirstName);
                Console.WriteLine(user.LastName);
                Console.WriteLine(user.CreatedAt);
                Console.WriteLine(user.ModifiedAt);
            }
            //Console.WriteLine(userManagementManager.GetUsersList());

        }
    }
}
