using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using System;

namespace AutoBuildApp.BusinessLayer
{
    public class UserManagementManager
    {
        private UserManagementGateway gateway;

        public UserManagementManager(String connectionString)
        {
            gateway = new UserManagementGateway(connectionString);
        }

        public String CreateUserRecord(UserAccount caller, UserAccount user)
        {
            //log that someone is trying to create an acc?

            // role check
            if (caller.role != "ADMIN")
            {
                return "Unauthorized";
            }

            if (IsInformationValid(user))
            {
                return "Invalid input";
            }

            return gateway.CreateUserRecord(user);
        }

        public String DeleteUserRecord(UserAccount caller, UserAccount user)
        {
            //log that someone is trying to create an acc?

            // role check
            if (caller.role != "ADMIN")
            {
                return "Unauthorized";
            }

            return gateway.DeleteUserRecord(user);
        }


        public bool validEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        public bool validUserName(string username)
        {
            return !String.IsNullOrEmpty(username) && username.Length >= 4 && username.Length <= 12;
        }


        public bool IsInformationValid(UserAccount user)
        {
            return validEmail(user.UserEmail)
                && validUserName(user.UserName)
                && !String.IsNullOrEmpty(user.FirstName)
                && !String.IsNullOrEmpty(user.LastName);
        }
    }
}
