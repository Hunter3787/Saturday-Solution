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



        //public UserManagementManager(UserManagementDAO userManagementDAO)
        //{
        //    _inputValidityManager = new InputValidityManager();
        //    _userManagementDAO = userManagementDAO;
        //}

        public UserManagementManager(UserManagementService userManagementService, string connectionString)
        {
            _inputValidityManager = new InputValidityManager();
            _userManagementDAO = new UserManagementDAO(connectionString);
            _userManagementService = userManagementService;
        }


        public string UpdatePassword(string password)
        {
            //password = "P@ssw0rd!123";
            // get the current principle that is on the thread:
            ClaimsPrincipal _threadPrinciple
                = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string userEmail = _threadPrinciple.FindFirst(ClaimTypes.Email).Value;
            if (_inputValidityManager.IsPasswordValid(password))
            {
                //return _userManagementDAO.newDBPassword(password);
                password = BC.HashPassword(password, BC.GenerateSalt());
                return _userManagementService._userManagementDAO.UpdatePasswordDB(userEmail, password);
            }
            else
            {
                return "Invalid Password";
            }
        }

        public string UpdateEmail(string userInput)
        {
            //userInput = "crkobel@verizon.net";
            ClaimsPrincipal _threadPrinciple
                = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string userEmail = _threadPrinciple.FindFirst(ClaimTypes.Email).Value;
            if (_inputValidityManager.ValidEmail(userInput))
            {
                return _userManagementService._userManagementDAO.UpdateEmailDB(userEmail, userInput);
            }
            else
            {
                return "Invalid Email";
            }
        }

        public string UpdateUsername(string userInput)
        {
            //userInput = "crkobel";
            ClaimsPrincipal _threadPrinciple
                = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string userEmail = _threadPrinciple.FindFirst(ClaimTypes.Email).Value;
            if (_inputValidityManager.ValidUserName(userInput))
            {
                return _userManagementService._userManagementDAO.UpdateUserNameDB(userEmail, userInput);
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



        public string ChangePermissions(string role)
        {
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();

            IClaims basic = claimsFactory.GetClaims(RoleEnumType.BASIC_ROLE);
            IClaims seniorAdmin = claimsFactory.GetClaims(RoleEnumType.SENIOR_ADMIN);
            IClaims vendor = claimsFactory.GetClaims(RoleEnumType.VENDOR_ROLE);
            IClaims unregistered = claimsFactory.GetClaims(RoleEnumType.UNREGISTERED_ROLE);

            string username = "KoolTrini";

            if (role == "Basic")
            {
                return _userManagementDAO.ChangePermissionsDB(username, basic.Claims());

            }
            else if (role == "Senior Admin")
            {
                return _userManagementDAO.ChangePermissionsDB(username, seniorAdmin.Claims());
            }
            else if (role == "Vendor")
            {
                return _userManagementDAO.ChangePermissionsDB(username, vendor.Claims());
            }
            //else if (role == "Developer")
            //{
            //    return _userManagementDAO.ChangePermissionsDB(username, developer.Claims());
            //}
            else if (role == "Unregistered")
            {
                return _userManagementDAO.ChangePermissionsDB(username, unregistered.Claims());
            }
            else return "Needs  proper role";
        }

        public string ChangeLockState(string lockState)
        {
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaims locked = claimsFactory.GetClaims(RoleEnumType.LOCKED);
            IClaims basic = claimsFactory.GetClaims(RoleEnumType.BASIC_ROLE);


            string username = "SERGE";
            if (lockState == RoleEnumType.LOCKED)
            {
                _userManagementDAO.ChangePermissionsDB(username, locked.Claims());
                return _userManagementDAO.Lock(username);
            }
            else if (lockState == RoleEnumType.BASIC_ROLE)
            {
                _userManagementDAO.ChangePermissionsDB(username, basic.Claims());
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


        //public String CreateUserRecord(UserAccount caller, UserAccount user)
        //{
        //    logger.Log("hello", LogLevel.Information);
        //    if (caller.role != "ADMIN")
        //    {
        //        return "Unauthorized";
        //    }

        //    if (!IsInformationValid(user))
        //    {
        //        return "Invalid input";
        //    }
        //    return service.CreateUser(user);
        //}

        //public String UpdateUserRecord(UserAccount caller, UserAccount user, UpdateUserDTO updatedUser)
        //{

        //    if (caller.role != "ADMIN")
        //    {
        //        return "Unauthorized";
        //    }

        //    if (!IsInformationValid(updatedUser))
        //    {
        //        return "Invalid input";
        //    }

        //    return service.UpdateUser(user, updatedUser);

        //}

        //public String DeleteUserRecord(UserAccount caller, UserAccount user)
        //{

        //    if (caller.role != "ADMIN")
        //    {
        //        return "Unauthorized";
        //    }

        //    return service.DeleteUser(user);
        //}

        //public string EnableUser(UserAccount caller, UserAccount user, string role)
        //{
        //    if (caller.role != "ADMIN")
        //    {
        //        return "Unauthorized";
        //    }

        //    return service.EnableUser(user, role); ;
        //}

        //public String DisableUser(UserAccount caller, UserAccount user)
        //{
        //    if (caller.role != "ADMIN")
        //    {
        //        return "Unauthorized";
        //    }

        //    return service.DisableUser(user);
        //}

        //public bool ValidEmail(string email)
        //{
        //    return email.Contains("@") && email.Contains(".");
        //}

        //public bool ValidUserName(string username)
        //{
        //    return !String.IsNullOrEmpty(username) && username.Length >= 4 && username.Length <= 12;
        //}

        //public bool IsInformationValid(UserAccount user)
        //{
        //    return ValidEmail(user.UserEmail)
        //        && ValidUserName(user.UserName)
        //        && !String.IsNullOrEmpty(user.FirstName)
        //        && !String.IsNullOrEmpty(user.LastName);
        //}

        //public bool IsInformationValid(UpdateUserDTO user)
        //{
        //    if (!String.IsNullOrEmpty(user.UserEmail))
        //    {
        //        return ValidEmail(user.UserEmail);
        //    }
        //    else if (!String.IsNullOrEmpty(user.UserName))
        //    {
        //        return ValidUserName(user.UserName);
        //    }
        //    return true;

        //}
    }
}
