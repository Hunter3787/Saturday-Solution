﻿using AutoBuildApp.Models;

using AutoBuildApp.DataAccess;

using System;

namespace AutoBuildApp.ConsoleApp
{
    class startHere
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            UserAccount userA = new UserAccount("username" ,"user", "name", "user@gmail.com", "BASIC");
            // how to connect to sql in a console application so you can pass the string : Settings1.Default.connectionString to the
            // user account gateway NOTE THIS STRING CAN BE PASSED USING THE NAME GIVEN IN THE APP.CONFIG OR ANY OTHER FILE
            // https://www.c-sharpcorner.com/UploadFile/5089e0/how-to-create-single-connection-string-in-console-applicatio/


            Console.WriteLine("Creating a user A to the DB");
            userAccountGateway userGateway = new userAccountGateway(Settings1.Default.connectionString);
            userGateway.checkConnection();
            bool flag = userGateway.verifyAccountExists(userA);
            if(flag == true){ Console.WriteLine("user exists"); }
            else { Console.WriteLine("user does not exist so creating..."); userGateway.createUserAccountinDB(userA);  }
           
            Console.ReadLine();
           
            //======================= testing the create User
            Console.WriteLine("Creating a user B to the DB using DTO");
            UserAccount userB = new UserAccount("HEYYYYY", "user", "hey", "hey@gmail.com", "BASIC");
            userGateway.createUserAccountinDB(userB);
            Console.ReadLine();
            //-------------------testing the delete user

            Console.WriteLine("deleting a user B in the DB using DTO");
            Console.WriteLine("we are goign to delete user B");
            userGateway.deleteUserAccountinDB(userB);

            Console.ReadLine();
            // retrieving user information

            Console.WriteLine("creating user C in the DB and retrieving info");
            UserAccount userC = new UserAccount("use", "heyer", "kaye", "kaye@gmail.com", "BASIC");
            userGateway.createUserAccountinDB(userC);
            userGateway.retrieveAccountInformation(userC);
            Console.ReadLine();

            Console.WriteLine("creating user C in the DB and retrieving info using DTO");

            //using a DTO to retrive that information:
            UserAccountInfromationDTO DTOuserinfo = new UserAccountInfromationDTO(Settings1.Default.connectionString);
            Console.WriteLine(DTOuserinfo.DisplayInfoOnUser(userC));
            Console.ReadLine();

        }
    }
}