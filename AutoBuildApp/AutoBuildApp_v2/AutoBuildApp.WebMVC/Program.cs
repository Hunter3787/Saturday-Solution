using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoBuildApp.BusinessLayer;
using AutoBuildApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Logging.LoggingFiles;
using Microsoft.Extensions.DependencyInjection;

namespace AutoBuildApp.WebMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            UserAccount admin = new UserAccount("TacOCat", "Nick", "Marshall", "tacoCat20@gmail.com", "ADMIN", "12345", "10-21-2020");
            UserAccount basic = new UserAccount("TwistadSista", "Kristi", "Marshall", "sistamista1999@gmail.com", "BASIC", "12345", "12-12-2020");
            UserAccount developer = new UserAccount("Cabic", "Carl", "Flurston", "cabic533@gmail.com", "DEVELOPER", "12345", "11-21-2020");
            UserAccount vendor = new UserAccount("Sigma", "Fitz", "Simmons", "gemma4142@gmail.com", "VENDOR", "12345", "10-31-2020");
            Boolean running = true;
            UserAccount[] accounts = { admin, basic, developer, vendor };

            UserManagementManager userManagementManager = new UserManagementManager("Server = localhost; Database = DB; Trusted_Connection = True;");
            userManagementManager.CreateUserRecord(admin,admin);

            while (running)
            {
                int option = 0, authority = 0, user = 0;
                Console.WriteLine("Make a selection to proceed\n1. Create user" +
                    "\n2. Delete user\n3. Disable User\n4. Enable User\n5. Quit");
                option = Convert.ToInt32(Console.ReadLine());
                logger.LogInformation("User has selected: {option}", option);
                switch (option)
                {
                    case 1:
                      

                        Console.WriteLine("Who is performing the add?\n1. Admin\n2. Basic User\n3. Developer\n4. Vendor");
                        while(authority<1 || authority>4)
                            authority = Convert.ToInt32(Console.ReadLine());
                        logger.LogInformation("Authority has selected option: {authority}",authority);

                        Console.WriteLine("Who to add?\n1. Admin\n2. Basic User\n3. Developer\n4. Vendor");
                        while (user < 1 || user > 4)
                            user = Convert.ToInt32(Console.ReadLine());
                        logger.LogInformation("User option: {user} has been selected", user);

                        String outcome = userManagementManager.CreateUserRecord(accounts[authority - 1], accounts[user - 1]);
                        Console.WriteLine(outcome);

                        logger.LogInformation("ADD outcome: {outcome} for authority: {authority} and user: {user}", outcome, accounts[authority - 1].role, accounts[user - 1].role);

                        break;
                    case 2:
                       

                        Console.WriteLine("Who is performing the delete?\n1. Admin\n2. Basic User\n3. Developer\n4. Vendor");
                        while (authority < 1 || authority > 4)
                            authority = Convert.ToInt32(Console.ReadLine());
                        logger.LogInformation("Authority has selected option: {authority}", authority);

                        Console.WriteLine("Who to delete?\n1. Admin\n2. Basic User\n3. Developer\n4. Vendor");
                        while (user < 1 || user > 4)
                            user = Convert.ToInt32(Console.ReadLine());
                        logger.LogInformation("User option: {user} has been selected", user);

                        String outcome2 = userManagementManager.DeleteUserRecord(accounts[authority - 1], accounts[user - 1]);
                        Console.WriteLine(outcome2);

                        logger.LogInformation("DELETE outcome: {outcome2} for authority: {authority} and user: {user}",outcome2, accounts[authority - 1].role, accounts[user - 1].role);

                        break;
                    case 3:
                       

                        Console.WriteLine("Who is performing the disable?\n1. Admin\n2. Basic User\n3. Developer\n4. Vendor");
                        while (authority < 1 || authority > 4)
                            authority = Convert.ToInt32(Console.ReadLine());
                        logger.LogInformation("Authority has selected option: {authority}", authority);

                        Console.WriteLine("Who to modify?\n1. Admin\n2. Basic User\n3. Developer\n4. Vendor");
                        while (user < 1 || user > 4)
                            user = Convert.ToInt32(Console.ReadLine());
                        logger.LogInformation("User option: {user} has been selected", user);


                        String outcome3 = userManagementManager.DisableUser(accounts[authority - 1], accounts[user - 1]);
                        Console.WriteLine(outcome3);

                        logger.LogInformation("DISABLE outcome: {outcome3} for authority: {authority} and user: {user}",outcome3, accounts[authority - 1].role, accounts[user - 1].role);
                        break;
                    case 4:
                     

                        Console.WriteLine("Who is performing the enable?\n1. Admin\n2. Basic User\n3. Developer\n4. Vendor");
                        while (authority < 1 || authority > 4)
                            authority = Convert.ToInt32(Console.ReadLine());
                        logger.LogInformation("Authority has selected option: {authority}", authority);

                        Console.WriteLine("Who to add?\n1. Admin\n2. Basic User\n3. Developer\n4. Vendor");
                        while (user < 1 || user > 4)
                            user = Convert.ToInt32(Console.ReadLine());
                        logger.LogInformation("User option: {user} has been selected", user);

                        int roleNum = 0;

                        String[] role = { "ADMIN", "BASIC", "DEVELOPER", "VENDOR" };
                        Console.WriteLine("What role are they to be?\n1. Admin\n2. Basic\n3. Developer\n4. Vendor");
                        while (roleNum < 1 || roleNum > 4)
                            roleNum = Convert.ToInt32(Console.ReadLine());
                        logger.LogInformation("role option: {roleNum} has been selected", roleNum);


                        String outcome4 = userManagementManager.EnableUser(accounts[authority - 1], accounts[user - 1], role[roleNum - 1]);
                        Console.WriteLine(outcome4);

                        logger.LogInformation("ENABLE outcome: {outcome4} for authority: {authority} and user: {user} with a role: {role}",outcome4, accounts[authority - 1].role, accounts[user - 1].role, role[roleNum - 1]);

                        break;
                    case 5:
                        Console.WriteLine("What would you like to update from this user?");
                        Console.WriteLine("1) " + basic.UserName + "\n2) " + basic.FirstName + "\n3) " + basic.LastName + "\n4) " + basic.UserEmail);
                        int choice = Convert.ToInt32(Console.ReadLine());
                        string newValue;
                        UpdateUserDTO updatedAccount;
                        Console.Write("Output the new input: ");
                        logger.LogInformation("User has selected choice: {choice}", choice);
                        switch (choice)
                        {
                            case 1:
                                newValue = Console.ReadLine();
                                updatedAccount = new UpdateUserDTO(newValue, "", "", "", "");
                                Console.WriteLine(userManagementManager.UpdateUserRecord(admin, basic, updatedAccount));

                                break;
                            case 2:
                                newValue = Console.ReadLine();
                                updatedAccount = new UpdateUserDTO("", newValue, "", "", "");
                                Console.WriteLine(userManagementManager.UpdateUserRecord(admin, basic, updatedAccount));
                                break;
                            case 3:
                                newValue = Console.ReadLine();
                                updatedAccount = new UpdateUserDTO("", "", newValue, "", "");
                                Console.WriteLine(userManagementManager.UpdateUserRecord(admin, basic, updatedAccount));
                                break;
                            case 4:
                                newValue = Console.ReadLine();
                                updatedAccount = new UpdateUserDTO("", "", "", newValue, "");
                                Console.WriteLine(userManagementManager.UpdateUserRecord(admin, basic, updatedAccount));
                                break;
                        }
                        break;
                    case 6:
                        running = false;
                        break;
                    default:
                        break;
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging((context, logging) => // passes in the host builder and also passes in the parameter for logging.
                {
                    logging.AddFileLogger(options => // Calls the FileLogger.cs file methods.
                    {
                        // sets configurations that we specified, reads from appsettings, gets the required sections for appsettings.json. Binding the options into it so that it can be reference in other places.
                        context.Configuration.GetSection("Logging").GetSection("LoggingFile").GetSection("Options").Bind(options);
                    });
                });
    }
}
