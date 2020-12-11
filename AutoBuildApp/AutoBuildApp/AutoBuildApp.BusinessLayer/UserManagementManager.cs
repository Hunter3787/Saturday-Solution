using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using System;

namespace AutoBuildApp.BusinessLayer
{
    public class UserManagementManager
    {
        private userAccountGateway gateway;

        public UserManagementManager()
        {
            gateway = new userAccountGateway();
        }
        public String createUserAccountinDB(UserAccount userA)
        {
            if (IsInformationValid(userA))
            {
                return "Invalid input";
            }
            userAccountGateway gateway = new userAccountGateway();
            return "User successfully created";
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
