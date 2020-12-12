using AutoBuildApp.Models;

using AutoBuildApp.DataAccess;

using System;
using AutoBuildApp.BusinessLayer;

namespace AutoBuildApp.ConsoleApp
{
    class startHere
    {
        static void Main(string[] args)
        {

            UserAccount admin = new UserAccount("TacOCat", "Nick", "Marshall", "tacoCat20@gmail.com", "ADMIN", "12345", "10-21-2020");
            UserAccount basic = new UserAccount("TwistadSista", "Kristi", "Marshall", "sistamista1999@gmail.com", "BASIC", "12345", "12-12-2020");
            UserAccount developer = new UserAccount("Cabic", "Carl", "Flurston", "cabic533@gmail.com", "DEVELOPER", "12345", "11-21-2020");
            UserAccount vendor = new UserAccount("Sigma", "Fitz", "Simmons", "gemma4142@gmail.com", "VENDOR", "12345", "10-31-2020");
            Boolean running = true;
            UserAccount[] accounts = { admin, basic, developer, vendor };

            UserManagementManager userManagementManager = new UserManagementManager(Settings1.Default.connectionString);

            while (running)
            {
                int option = 0, authority = 0, user = 0;
                Console.WriteLine("Make a selection to proceed\n1. Create user" +
                    "\n2. Delete user\n3. Disable User\n4. Enable User\n5. Update User\n6. Quit");
                option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Console.WriteLine("Who is performing the add?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (authority < 1 || authority > 4)
                            authority = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Who to add?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (user < 1 || user > 4)
                            user = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(authority + " " + user);
                        Console.WriteLine(userManagementManager.CreateUserRecord(accounts[authority - 1], accounts[user - 1]));
                        break;
                    case 2:
                        Console.WriteLine("Who is performing the delete?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (authority < 1 || authority > 4)
                            authority = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Who to delete?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (user < 1 || user > 4)
                            user = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(userManagementManager.DeleteUserRecord(accounts[authority - 1], accounts[user - 1]));
                        break;
                    case 3:
                        Console.WriteLine("Who is performing the disable?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (authority < 1 || authority > 4)
                            authority = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Who to modify?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (user < 1 || user > 4)
                            user = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(userManagementManager.DisableUser(accounts[authority - 1], accounts[user - 1]));
                        break;
                    case 4:
                        Console.WriteLine("Who is performing the enable?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (authority < 1 || authority > 4)
                            authority = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Who to add?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        while (user < 1 || user > 4)
                            user = Convert.ToInt32(Console.ReadLine());
                        int roleNum = 0;
                        String[] role = { "ADMIN", "BASIC", "DEVELOPER", "VENDOR" };
                        Console.WriteLine("What role are they to be?\n1. Admin\n2. Basic\n3. Developer\n4. Vendor");
                        while (roleNum < 1 || roleNum > 4)
                            roleNum = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(userManagementManager.EnableUser(accounts[authority - 1], accounts[user - 1], role[roleNum - 1]));
                        break;
                    case 5:
                        //Console.WriteLine("Who is performing the update?\n1. Admin\n2.Basic User\n3. Developer\n4. Vendor");
                        //while (authority < 1 || authority > 4)
                        //    authority = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("What would you like to update from this user?");

                        Console.WriteLine("1) " + basic.UserName + "\n2) " + basic.FirstName + "\n3) " + basic.LastName + "\n4) " + basic.UserEmail);
                        int choice = Convert.ToInt32(Console.ReadLine());
                        string newValue;
                        UpdateUserDTO updatedAccount;
                        switch(choice)
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







        //Console.WriteLine("Hello World!");
        //UserAccount userA = new UserAccount("usernamfff", "user", "name", "user@userssg.com", "BASIC", "pejnejenjnejenen", "09-12-1990");
        //UserAccount admin = new UserAccount("adminAcc", "ad", "min", "admin@admin.com", "ADMIN", "pkjewk;kjrew;kjnkjrwekjwnr", "10-29-1982");
        //Console.WriteLine(userA.ToString());
        //string hash = userA.passHash;

        //Console.WriteLine("hash1" + hash);
        //Console.WriteLine("hash1 Length" + hash.Length);



        //Console.WriteLine(admin.ToString());
        //string hash2 = admin.passHash;

        //Console.WriteLine("hash2" + hash2);
        //Console.WriteLine("hash2 Length" + hash.Length);


        //UpdateUserDTO updatedInformation = new UpdateUserDTO("test", "test", "test", "test@test.test", "test");

        //// how to connect to sql in a console application so you can pass the string : Settings1.Default.connectionString to the
        //// user account gateway NOTE THIS STRING CAN BE PASSED USING THE NAME GIVEN IN THE APP.CONFIG OR ANY OTHER FILE
        //// https://www.c-sharpcorner.com/UploadFile/5089e0/how-to-create-single-connection-string-in-console-applicatio/

        //Console.WriteLine("Creating a user A to the DB");
        ////  UserManagementManager manager = new UserManagementManager(Settings1.Default.connectionString);
        //UserManagementManager manager = new UserManagementManager(Settings1.Default.connectionString);


        //Console.WriteLine(manager.CreateUserRecord(admin, userA));


        //Console.ReadLine();
        //Console.WriteLine(manager.DeleteUserRecord(admin, userA));

        //UserManagementGateway userGateway = new UserManagementGateway(Settings1.Default.connectionString);

        //  //Console.WriteLine(userGateway.createUserAccountinDB(userA));
        //  //userGateway.checkConnection();
        //  //bool flag = userGateway.verifyAccountExists(userA);
        //  //if (flag == true) { Console.WriteLine("user exists"); }
        //  //else { Console.WriteLine("user does not exist so creating..."); userGateway.createUserAccountinDB(userA); }

        //  //Console.ReadLine();
        //  ////======================= testing the create User
        //  //Console.WriteLine("Creating a user B to the DB using DTO");
        //  //UserAccount userB = new UserAccount("HEYYYYY", "user", "hey", "hey@gmail.com", "BASIC");
        //  //userGateway.createUserAccountinDB(userB);

        //  //Console.ReadLine();
        //  ////-------------------testing the delete user

        //  //Console.WriteLine("deleting a user B in the DB using DTO");
        //  //Console.WriteLine("we are goign to delete user B");
        //  //userGateway.deleteUserAccountinDB(userB);

        //  //Console.ReadLine();
        //  //// retrieving user information

        //  //Console.WriteLine("creating user C in the DB and retrieving info");
        //  //UserAccount userC = new UserAccount("use", "heyer", "kaye", "kaye@gmail.com", "BASIC");
        //  //userGateway.createUserAccountinDB(userC);
        //  //userGateway.retrieveAccountInformation(userC);
        //  //Console.ReadLine();

        //  //Console.WriteLine("creating user C in the DB and retrieving info using DTO");

        //  //Console.ReadLine();


    }
    }

