using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
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
        public String CreateUser(UserAccount user)
        {
            return gateway.CreateUserRecord(user);
        }

        public String UpdateUser(UserAccount user)
        {
            return gateway.UpdateUserRecord(user);
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
            user.role = "DISABLED";
            return "User has been disabled.";
        }
    }
}
