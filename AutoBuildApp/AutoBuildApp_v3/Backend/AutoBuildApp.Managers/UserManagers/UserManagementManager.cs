using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.Users;
using AutoBuildApp.Models.DataTransferObjects;
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

        public UserManagementManager(UserManagementService userManagementService, string connectionString)
        {
            _inputValidityManager = new InputValidityManager();
            _userManagementDAO = new UserManagementDAO(connectionString);
            _userManagementService = userManagementService;
        }


        public string UpdatePassword(string password, string passwordCheck, string activeEmail)
        {
            if (_inputValidityManager.IsPasswordValid(password))
            {
                if (_inputValidityManager.passwordCheck(password, passwordCheck))
                {
                    passwordCheck = BC.HashPassword(passwordCheck, BC.GenerateSalt());
                    password = BC.HashPassword(password, BC.GenerateSalt());
                    return _userManagementService._userManagementDAO.UpdatePasswordDB(password, activeEmail);
                }
                else
                {
                    return "Passwords do not match";
                }
            }
            else
            {
                return "Invalid Password";
            }
        }

        public string UpdateEmail(string InputEmail, string activeEmail)
        {
            if (_inputValidityManager.ValidEmail(InputEmail))
            {
                if (!_userManagementDAO.DoesEmailExist(InputEmail)) {
                    return _userManagementService._userManagementDAO.UpdateEmailDB(InputEmail, activeEmail);
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

        public string UpdateUsername(string username, string activeEmail)
        {
            if (_inputValidityManager.ValidUserName(username))
            {
                if (!_userManagementDAO.DoesUsernameExist(username))
                {
                    return _userManagementService._userManagementDAO.UpdateUserNameDB(username, activeEmail);
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

            IClaims basic = claimsFactory.GetClaims(RoleEnumType.BasicRole);
            IClaims systemAdmin = claimsFactory.GetClaims(RoleEnumType.SystemAdmin);
            IClaims delegateAdmin = claimsFactory.GetClaims(RoleEnumType.DelegateAdmin);
            IClaims vendor = claimsFactory.GetClaims(RoleEnumType.VendorRole);
            IClaims unregistered = claimsFactory.GetClaims(RoleEnumType.UnregisteredRole);

            //username = "KoolTrini";

            if (_userManagementDAO.RoleCheckDB(username) == RoleEnumType.SystemAdmin)
            {
                return "Error: you can't modify the permissions of a system admin";
            }
            else
            {
                if (role == RoleEnumType.BasicRole)
                {
                    return _userManagementDAO.ChangePermissionsDB(username, role, basic.Claims());
                }
                else if (role == RoleEnumType.SystemAdmin)
                {
                    return _userManagementDAO.ChangePermissionsDB(username, role, systemAdmin.Claims());
                }
                else if (role == RoleEnumType.DelegateAdmin)
                {
                    return _userManagementDAO.ChangePermissionsDB(username, role, delegateAdmin.Claims());
                }
                else if (role == RoleEnumType.VendorRole)
                {
                    return _userManagementDAO.ChangePermissionsDB(username, role, vendor.Claims());
                }
                //else if (role == "Developer")
                //{
                //    return _userManagementDAO.ChangePermissionsDB(username, developer.Claims());
                //}
                else if (role == RoleEnumType.UnregisteredRole)
                {
                    return _userManagementDAO.ChangePermissionsDB(username, role, unregistered.Claims());
                }
                else return "Needs  proper role";
            }
        }

        public string ChangeLockState(string username, bool lockState)
        {
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaims locked = claimsFactory.GetClaims(RoleEnumType.Locked);
            IClaims basic = claimsFactory.GetClaims(RoleEnumType.BasicRole);


            //username = "SERGE";
            if (_userManagementDAO.RoleCheckDB(username) == RoleEnumType.SystemAdmin)
            {
                return "Error: you can't lock a system admin";
            }
            else
            {
                if (lockState == true)
                {
                    _userManagementDAO.ChangePermissionsDB(username, null, locked.Claims());
                    return _userManagementDAO.Lock(username);
                }
                else if (lockState == false)
                {
                    _userManagementDAO.ChangePermissionsDB(username, null, basic.Claims());
                    return _userManagementDAO.Unlock(username);
                }
                else
                {
                    return "Error: not set to Locked or Unlocked";
                }
            }
        }

        public string DeleteUser(string username)
        {
            if (_userManagementDAO.RoleCheckDB(username) == RoleEnumType.SystemAdmin)
            {
                return "Error: you can't delete a system admin";
            } else
            {
                return _userManagementService._userManagementDAO.DeleteUserDB(username);
            }
        }

        public string RoleCheck(string username)
        {
            return _userManagementService._userManagementDAO.RoleCheckDB(username);
        }
    }
}
