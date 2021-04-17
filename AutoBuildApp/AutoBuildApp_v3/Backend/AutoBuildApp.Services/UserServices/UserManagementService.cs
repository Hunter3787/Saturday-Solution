using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.Users;
using AutoBuildApp.Models.DTO;
using System;
using AutoBuildApp.DomainModels;
using System.Collections.Generic;
using AutoBuildApp.DataAccess.Entities;
using System.Text;


namespace AutoBuildApp.Services.UserServices
{
    public class UserManagementService
    {
        public readonly UserManagementDAO _userManagementDAO;
        public UserManagementService(UserManagementDAO userManagementDAO)
        {
            _userManagementDAO = userManagementDAO;
        }

        public List<UserResults> GetUsersList()
        {
            var userResultEntities = _userManagementDAO.getUsers();
            var userResults = new List<UserResults>();

            foreach(UserEntity userResultEntity in userResultEntities)
            {
                var userResult = new UserResults()
                {
                    UserID = userResultEntity.UserID,
                    UserName = userResultEntity.UserName,
                    Email = userResultEntity.Email,
                    FirstName = userResultEntity.FirstName,
                    LastName = userResultEntity.LastName,
                    CreatedAt = userResultEntity.CreatedAt,
                    ModifiedAt = userResultEntity.ModifiedAt,
                    ModifiedBy = userResultEntity.ModifiedBy,
                };
                userResults.Add(userResult);
            }
            return userResults;
        }



        //public virtual String CreateUser(UserAccount user)
        //{
        //    return gateway.CreateUserRecord(user);
        //}

        //public String UpdateUser(UserAccount user, UpdateUserDTO updatedUser)
        //{
        //    return gateway.UpdateUserRecord(user, updatedUser);
        //}

        //public String DeleteUser(UserAccount user)
        //{
        //    return gateway.DeleteUserRecord(user);
        //}

        //public String EnableUser(UserAccount user, string role)
        //{
        //    user.role = role;
        //    return "User has been enabled.";
        //}

        //public String DisableUser(UserAccount user)
        //{
        //    user.role = "DEFAULT";
        //    return "User has been disabled.";
        //}
    }
}
