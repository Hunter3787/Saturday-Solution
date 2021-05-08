using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.Users;
using AutoBuildApp.Models.DataTransferObjects;
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

        // sets up the UserResults object
        public List<UserResults> GetUsersList()
        {
            var userResultEntities = _userManagementDAO.getUsers();
            var userResults = new List<UserResults>();

            // for each userResult entity sets values for that user
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
                    UserRole = userResultEntity.UserRole,
                    LockState = userResultEntity.LockState
                };
                // adds the results to the growing list
                userResults.Add(userResult);
            }
            return userResults;
        }
    }
}
