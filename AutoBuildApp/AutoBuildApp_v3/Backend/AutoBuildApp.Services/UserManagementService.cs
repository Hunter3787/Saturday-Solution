using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.Users;
using AutoBuildApp.Models.DTO
using System;

namespace AutoBuildApp.ServiceLayer
{
    public class UserManagementService
    {
        private UserManagementGateway gateway;

        public UserManagementService(string connectionString)
        {
            gateway = new UserManagementGateway(connectionString);
        }
        public virtual String CreateUser(UserAccount user)
        {
            return gateway.CreateUserRecord(user);
        }

        public String UpdateUser(UserAccount user, UpdateUserDTO updatedUser)
        {
            return gateway.UpdateUserRecord(user, updatedUser);
        }

        public String DeleteUser(UserAccount user)
        {
            return gateway.DeleteUserRecord(user);
        }

        public String EnableUser(UserAccount user, string role)
        {
            user.role = role;
            return "User has been enabled.";
        }

        public String DisableUser(UserAccount user)
        {
            user.role = "DEFAULT";
            return "User has been disabled.";
        }
    }
}
