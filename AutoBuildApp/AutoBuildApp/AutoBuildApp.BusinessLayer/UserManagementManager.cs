using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.ServiceLayer;
using System;

namespace AutoBuildApp.BusinessLayer
{
    public class UserManagementManager
    {
        //private UserManagementGateway gateway;
        private UserManagementService service;

        public UserManagementManager(String connectionString)
        {
            service = new UserManagementService(connectionString);
            //gateway = new UserManagementGateway(connectionString);
        }

        public String CreateUserRecord(UserAccount caller, UserAccount user)
        {
            //log that someone is trying to create an acc?

            // role check
            if (caller.role != "ADMIN")
            {
                return "Unauthorized";
            }

            if (!IsInformationValid(user))
            {
                return "Invalid input";
            }
            return service.CreateUser(user);
            //return gateway.CreateUserRecord(user);
        }

        public String UpdateUserRecord(UserAccount caller, UserAccount user, UpdateUserDTO updatedUser)
        {
            //log that someone is trying to update an acc?

            // role check
            if (caller.role != "ADMIN")
            {
                return "Unauthorized";
            }

            if(!IsInformationValid(user))
            {
                return "Invalid input";
            }

            return service.UpdateUser(user);
            //return gateway.UpdateUserRecord(user);

        }

        public String DeleteUserRecord(UserAccount caller, UserAccount user)
        {
            //log that someone is trying to delete an acc?

            // role check
            if (caller.role != "ADMIN")
            {
                return "Unauthorized";
            }

            return service.DeleteUser(user);
            //return gateway.DeleteUserRecord(user);
        }

        public string EnableUser(UserAccount caller, UserAccount user, string role)
        {
            // role check
            if (caller.role != "ADMIN")
            {
                return "Unauthorized";
            }
            
            return service.EnableUser(user, role); ;
        }

        public String DisableUser(UserAccount caller, UserAccount user)
        {
            // role check
            if (caller.role != "ADMIN")
            {
                return "Unauthorized";
            }

            return service.DisableUser(user);
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
