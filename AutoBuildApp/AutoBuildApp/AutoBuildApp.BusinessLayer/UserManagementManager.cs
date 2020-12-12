using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.ServiceLayer;
using System;

namespace AutoBuildApp.BusinessLayer
{
    public class UserManagementManager
    {
        private readonly UserManagementService service;

        public UserManagementManager(String connectionString)
        {
            service = new UserManagementService(connectionString);
        }

        public String CreateUserRecord(UserAccount caller, UserAccount user)
        {

            if (caller.role != "ADMIN")
            {
                return "Unauthorized";
            }

            if (!IsInformationValid(user))
            {
                return "Invalid input";
            }
            return service.CreateUser(user);
        }

        public String UpdateUserRecord(UserAccount caller, UserAccount user, UpdateUserDTO updatedUser)
        {

            if (caller.role != "ADMIN")
            {
                return "Unauthorized";
            }

            if (!IsInformationValid(updatedUser))
            {
                return "Invalid input";
            }

            return service.UpdateUser(user, updatedUser);

        }

        public String DeleteUserRecord(UserAccount caller, UserAccount user)
        {

            if (caller.role != "ADMIN")
            {
                return "Unauthorized";
            }

            return service.DeleteUser(user);
        }

        public string EnableUser(UserAccount caller, UserAccount user, string role)
        {
            if (caller.role != "ADMIN")
            {
                return "Unauthorized";
            }

            return service.EnableUser(user, role); ;
        }

        public String DisableUser(UserAccount caller, UserAccount user)
        {
            if (caller.role != "ADMIN")
            {
                return "Unauthorized";
            }

            return service.DisableUser(user);
        }

        public bool ValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        public bool ValidUserName(string username)
        {
            return !String.IsNullOrEmpty(username) && username.Length >= 4 && username.Length <= 12;
        }

        public bool IsInformationValid(UserAccount user)
        {
            return ValidEmail(user.UserEmail)
                && ValidUserName(user.UserName)
                && !String.IsNullOrEmpty(user.FirstName)
                && !String.IsNullOrEmpty(user.LastName);
        }

        public bool IsInformationValid(UpdateUserDTO user)
        {
            return ValidEmail(user.UserEmail)
                && ValidUserName(user.UserName)
                && !String.IsNullOrEmpty(user.FirstName)
                && !String.IsNullOrEmpty(user.LastName);
        }
    }
}
