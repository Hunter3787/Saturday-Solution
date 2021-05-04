using AutoBuildApp.Models;
using AutoBuildApp.Models.Users;
using AutoBuildApp.Models.DataTransferObjects;
using System;
using AutoBuildApp.DomainModels;
using System.Collections.Generic;
using AutoBuildApp.Models.Entities;
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
                    UserRole = userResultEntity.UserRole
                };
                userResults.Add(userResult);
            }
            return userResults;
        }
    }
}
