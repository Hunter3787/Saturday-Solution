using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoBuildApp.BusinessLayer;
using AutoBuildApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AutoBuildApp.WebMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            UserAccount admin = new UserAccount("TacOCat", "Nick", "Marshall", "tacoCat20@gmail.com", "ADMIN", "12345", "10/21/2020");
            UserAccount basic = new UserAccount("TwistadSista", "Kristi", "Marshall", "sistamista1999@gmail.com", "BASIC", "12345", "12/12/2020");
            UserAccount developer = new UserAccount("Cabic", "Carl", "Flurston", "cabic533@gmail.com", "DEVELOPER", "12345", "11/21/2020");
            UserAccount vendor = new UserAccount("Sigma", "Fitz", "Simmons", "gemma4142@gmail.com", "VENDOR", "12345", "10/31/2020");
            Boolean running = true;
            UserAccount[] accounts = { admin, basic, developer, vendor };

            UserManagementManager userManagementManager = new UserManagementManager("Data Source=DESKTOP-CI24C55\\MSSQLSERVER02;Initial Catalog=dannytemp2;Integrated Security=True");
            userManagementManager.CreateUserRecord(admin,admin);

            while (running)
            {
                int option = 0, authority = 0, user = 0;
                Console.WriteLine("Make a selection to proceed\n1. Create user" +
                    "\n2. Delete user\n3. Disable User\n4. Enable User\n5. Quit");
                option = Convert.ToInt32(Console.ReadLine());
                
                switch (option)
                {
                    case 1:
                        Console.WriteLine("Who is performing the add?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while(authority<1 && authority>4)
                            authority = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Who to add?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (user < 1 && user > 4)
                            user = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(userManagementManager.CreateUserRecord(accounts[authority - 1], accounts[user - 1]));
                        break;
                    case 2:
                        Console.WriteLine("Who is performing the delete?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (authority < 1 && authority > 4)
                            authority = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Who to delete?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (user < 1 && user > 4)
                            user = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(userManagementManager.DeleteUserRecord(accounts[authority - 1], accounts[user - 1]));
                        break;
                    case 3:
                        Console.WriteLine("Who is performing the disable?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (authority < 1 && authority > 4)
                            authority = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Who to modify?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (user < 1 && user > 4)
                            user = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(userManagementManager.DisableUser(accounts[authority - 1], accounts[user - 1]));
                        break;
                    case 4:
                        Console.WriteLine("Who is performing the enable?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (authority < 1 && authority > 4)
                            authority = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Who to add?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (user < 1 && user > 4)
                            user = Convert.ToInt32(Console.ReadLine());
                        int roleNum = 0;
                        String[] role = { "ADMIN", "BASIC", "DEVELOPER", "VENDOR" };
                        Console.WriteLine("What role are they to be?\n1. Admin\n2. Basic\n3. Developer\n4. Vendor");
                        while (roleNum < 1 && roleNum > 4)
                            roleNum = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(userManagementManager.EnableUser(accounts[authority - 1], accounts[user - 1], role[roleNum-1]));
                        break;
                    case 5:
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
                });
    }
}
