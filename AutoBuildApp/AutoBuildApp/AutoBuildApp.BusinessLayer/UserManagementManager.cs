using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.ServiceLayer;
using System;
using AutoBuildApp.Loggg;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;

namespace AutoBuildApp.BusinessLayer
{
    public class UserManagementManager
    {
        private readonly UserManagementService service;

        private readonly FileLogger logger;

        private readonly ILogger<UserManagementManager> _logger;


        public UserManagementManager(ILogger<UserManagementManager> logger)
        {
            _logger = logger;
        }

        public UserManagementManager(FileLogger logger)
        {
            this.logger = logger;
        }


        public UserManagementManager(String connectionString)
        {
            service = new UserManagementService(connectionString);
        }

        public String CreateUserRecord(UserAccount caller, UserAccount user)
        {

            if (caller.role != "ADMIN")
            {
                _logger.LogInformation("HEEEEEEEEEEEEEEEEEE");
                logger.LogInformation("HEEEEEEEEEEEEEEEEEE");
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
            if(!String.IsNullOrEmpty(user.UserEmail))
            {
                return ValidEmail(user.UserEmail);
            }
            else if(!String.IsNullOrEmpty(user.UserName)) {
                return ValidUserName(user.UserName);
            }
            return true;

        }
    }
}
