using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.Users;
using AutoBuildApp.Models.DTO;
using AutoBuildApp.Services;
using System;
using AutoBuildApp.Managers.UserManagers;
using BC = BCrypt.Net.BCrypt;
using System.Security.Claims;
using System.Threading;
using AutoBuildApp.DomainModels;
using System.Collections.Generic;
using AutoBuildApp.Services.UserServices;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Enumerations;

namespace AutoBuildApp.Managers
{
    public class UserManagementManager
    {
        private readonly UserManagementDAO _userManagementDAO;
        private InputValidityManager _inputValidityManager;
        private readonly UserManagementService _userManagementService;
        private readonly RegistrationDAO _registrationDAO;

        public UserManagementManager(UserManagementService userManagementService, string connectionString)
        {
            _inputValidityManager = new InputValidityManager();
            _userManagementDAO = new UserManagementDAO(connectionString);
            _userManagementService = userManagementService;
        }


        public string UpdatePassword(string password, string userEmail)
        {
            // get the current principle that is on the thread:
            ClaimsPrincipal _threadPrinciple
                = (ClaimsPrincipal)Thread.CurrentPrincipal;
            //userEmail = _threadPrinciple.FindFirst(ClaimTypes.Email).Value;
            if (_inputValidityManager.IsPasswordValid(password))
            {
                password = BC.HashPassword(password, BC.GenerateSalt());
                return _userManagementService._userManagementDAO.UpdatePasswordDB(userEmail, password);
            }
            else
            {
                return "Invalid Password";
            }
        }

        public string UpdateEmail(string userInputEmail, string activeEmail)
        {
            ClaimsPrincipal _threadPrinciple
                = (ClaimsPrincipal)Thread.CurrentPrincipal;
            //activeEmail = _threadPrinciple.FindFirst(ClaimTypes.Email).Value;
            if (_inputValidityManager.ValidEmail(userInputEmail))
            {
                if (!_userManagementDAO.DoesEmailExist(userInputEmail)) {
                    return _userManagementService._userManagementDAO.UpdateEmailDB(activeEmail, userInputEmail);
                }
                else
                {
                    return "Email already exists";
                }
            }
            else
            {
                return "Invalid Email";
            }
        }

        public string UpdateUsername(string userInputUsername, string activeEmail)
        {
            ClaimsPrincipal _threadPrinciple
                = (ClaimsPrincipal)Thread.CurrentPrincipal;
             //activeEmail = _threadPrinciple.FindFirst(ClaimTypes.Email).Value;
            if (_inputValidityManager.ValidUserName(userInputUsername))
            {
                if (!_userManagementDAO.DoesUsernameExist(userInputUsername))
                {
                    return _userManagementService._userManagementDAO.UpdateUserNameDB(activeEmail, userInputUsername);
                } 
                else
                {
                    return "Username already exists";
                }
            }
            else
            {
                return "Invalid Username";
            }
        }

        public List<UserResults> GetUsersList()
        {
            return _userManagementService.GetUsersList();
        }



        public string ChangePermissions(string username, string role)
        {
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();

            IClaims basic = claimsFactory.GetClaims(RoleEnumType.BASIC_ROLE);
            IClaims seniorAdmin = claimsFactory.GetClaims(RoleEnumType.SENIOR_ADMIN);
            IClaims vendor = claimsFactory.GetClaims(RoleEnumType.VENDOR_ROLE);
            IClaims unregistered = claimsFactory.GetClaims(RoleEnumType.UNREGISTERED_ROLE);

            //username = "KoolTrini";

            if (role == RoleEnumType.BASIC_ROLE)
            {
                return _userManagementDAO.ChangePermissionsDB(username, role, basic.Claims());
            }
            else if (role == RoleEnumType.SENIOR_ADMIN)
            {
                return _userManagementDAO.ChangePermissionsDB(username, role, seniorAdmin.Claims());
            }
            else if (role == RoleEnumType.VENDOR_ROLE)
            {
                return _userManagementDAO.ChangePermissionsDB(username, role, vendor.Claims());
            }
            //else if (role == "Developer")
            //{
            //    return _userManagementDAO.ChangePermissionsDB(username, developer.Claims());
            //}
            else if (role == RoleEnumType.UNREGISTERED_ROLE)
            {
                return _userManagementDAO.ChangePermissionsDB(username, role, unregistered.Claims());
            }
            else return "Needs  proper role";
        }

        public string ChangeLockState(string username, string lockState)
        {
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaims locked = claimsFactory.GetClaims(RoleEnumType.LOCKED);
            IClaims basic = claimsFactory.GetClaims(RoleEnumType.BASIC_ROLE);


            //username = "SERGE";
            if (lockState == RoleEnumType.LOCKED)
            {
                _userManagementDAO.ChangePermissionsDB(username, null, locked.Claims());
                return _userManagementDAO.Lock(username);
            }
            else if (lockState == RoleEnumType.BASIC_ROLE)
            {
                _userManagementDAO.ChangePermissionsDB(username, null, basic.Claims());
                return _userManagementDAO.Unlock(username);
            }
            else
            {
                return "Error: not set to Locked or Unlocked";
            }
        }

        public string DeleteUser(string email)
        {
            return _userManagementService._userManagementDAO.DeleteUserDB(email);
        }
    }
}
