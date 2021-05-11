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
using System.Text;
using System.Security.Cryptography;
using System.IO;
using AutoBuildApp.Logging;

namespace AutoBuildApp.Managers
{

    public class UserManagementManager
    {
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private readonly UserManagementDAO _userManagementDAO;
        private readonly UserManagementService _userManagementService;
        private readonly InputValidityManager _inputValidityManager;
        public UserManagementManager(UserManagementService userManagementService)
        {
            _userManagementDAO = userManagementService._userManagementDAO;
            _userManagementService = userManagementService;
            _inputValidityManager = new InputValidityManager();
        }

        // Updates the password of the active user
        public string UpdatePassword(string password, string passwordCheck, string activeUsername)
        {
            // checks the password for validity
            if (_inputValidityManager.IsPasswordValid(password))
            {
                // checks that the password and password verification are the same
                if (_inputValidityManager.passwordCheck(password, passwordCheck))
                {
                    // password is valid and checked
                    // both values hashed for security before DB store
                    passwordCheck = BCrypt.Net.BCrypt.HashPassword(passwordCheck); ;
                    password = BCrypt.Net.BCrypt.HashPassword(password);
                    // go to DAO for storage
                    return _userManagementService._userManagementDAO.UpdatePasswordDB(password, activeUsername);
                }
                else
                {
                    _logger.LogWarning("Password verify failed.");
                    return "Passwords do not match";
                }
            }
            else
            {
                _logger.LogWarning("Not valid password used.");
                return "Invalid password";
            }
        }

        // Updates the email of the active user
        public string UpdateEmail(string InputEmail, string activeUsername)
        {
            // checks that email is valid
            if (_inputValidityManager.ValidEmail(InputEmail))
            {
                // calls the DAO to check if email is already in the database
                if (!_userManagementDAO.DoesEmailExist(InputEmail)) {
                    // email is valid and is not already used
                    // pass to DAO for storage
                    return _userManagementService._userManagementDAO.UpdateEmailDB(InputEmail, activeUsername);
                }
                else
                {
                    _logger.LogWarning("Email already exists.");
                    return "Email already exists";
                }
            }
            else
            {
                _logger.LogWarning("Not valid email used.");
                return "Invalid email";
            }
        }

        // Updates the username of the active user
        public string UpdateUsername(string username, string activeEmail)
        {
            // checks that username is valid
            if (_inputValidityManager.ValidUserName(username))
            {
                // calls the DAO to check if username is already in the database
                if (!_userManagementDAO.DoesUsernameExist(username))
                {
                    // username is valid
                    // pass to DAO for storage
                    return _userManagementService._userManagementDAO.UpdateUserNameDB(username, activeEmail);
                } 
                else
                {
                    _logger.LogWarning("Username already exists.");
                    return "Username already exists";
                }
            }
            else
            {
                _logger.LogWarning("Not valid username used.");
                return "Invalid username";
            }
        }

        // provides a full list of all users currently in the database
        public List<UserResults> GetUsersList()
        {
            // calls the list of users from the service layer
            return _userManagementService.GetUsersList();
        }


        // Changes the permissions of the selected user
        public string ChangePermissions(string username, string role)
        {
            // the claims factory get's the set of permissions associated with each role
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();

            // set up the claims for each role
            IClaims basic = claimsFactory.GetClaims(RoleEnumType.BasicRole);
            IClaims systemAdmin = claimsFactory.GetClaims(RoleEnumType.SystemAdmin);
            IClaims delegateAdmin = claimsFactory.GetClaims(RoleEnumType.DelegateAdmin);
            IClaims vendor = claimsFactory.GetClaims(RoleEnumType.VendorRole);
            IClaims unregistered = claimsFactory.GetClaims(RoleEnumType.UnregisteredRole);

            // calls the DAO to see if the user you are changing is a system admin
            if (_userManagementDAO.RoleCheckDB(username) == RoleEnumType.SystemAdmin)
            {
                _logger.LogWarning("Can't modify a system admin.");
                return "Error: you can't modify the permissions of a system admin";
            }
            else
            {
                // sets the user to basic
                if (role == RoleEnumType.BasicRole)
                {
                    _logger.LogInformation("User set to basic role.");
                    return _userManagementDAO.ChangePermissionsDB(username, role, basic.Claims());
                }
                // sets the user to system admin (can't be modified)
                else if (role == RoleEnumType.SystemAdmin)
                {
                    _logger.LogInformation("User set to system admin.");
                    return _userManagementDAO.ChangePermissionsDB(username, role, systemAdmin.Claims());
                }
                // sets the user to delegate admin
                else if (role == RoleEnumType.DelegateAdmin)
                {
                    _logger.LogInformation("User set to delegate admin.");
                    return _userManagementDAO.ChangePermissionsDB(username, role, delegateAdmin.Claims());
                }
                // sets the user to vendor
                else if (role == RoleEnumType.VendorRole)
                {
                    _logger.LogInformation("User set to vendor role.");
                    return _userManagementDAO.ChangePermissionsDB(username, role, vendor.Claims());
                }
                // sets the user to unregistered
                else if (role == RoleEnumType.UnregisteredRole)
                {
                    _logger.LogInformation("User set to unregistered.");
                    return _userManagementDAO.ChangePermissionsDB(username, role, unregistered.Claims());
                }
                // will never reach this code
                else return "Needs  proper role";
            }
        }

        // either locks or unlocks a user
        public string ChangeLockState(string username, bool lockState)
        {
            // locking gives the locked permissions
            // unlocking gives basic permissions
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaims locked = claimsFactory.GetClaims(RoleEnumType.Locked);
            IClaims basic = claimsFactory.GetClaims(RoleEnumType.BasicRole);

            // checks to see if the user is a system admin
            if (_userManagementDAO.RoleCheckDB(username) == RoleEnumType.SystemAdmin)
            {
                _logger.LogWarning("Can't modify a system admin.");
                return "Error: you can't lock a system admin";
            }
            else
            {
                // locking an account
                if (lockState == true)
                {
                    // changes the permissions
                    _userManagementDAO.ChangePermissionsDB(username, null, locked.Claims());
                    // sets the account lockstate to locked
                    _logger.LogInformation("User set to locked.");
                    return _userManagementDAO.Lock(username);
                }
                // unlocking an account
                else if (lockState == false)
                {
                    // changes the permissions
                    _userManagementDAO.ChangePermissionsDB(username, null, basic.Claims());
                    // sets the account lockstate to unlocked
                    _logger.LogInformation("User set to unlocked.");
                    return _userManagementDAO.Unlock(username);
                }
                else
                {
                    // will not reach this code
                    return "Error: not set to Locked or Unlocked";
                }
            }
        }

        // deletes a user and removes them from the database
        public string DeleteUser(string username)
        {
            // checks to see if the user is a system admin
            if (_userManagementDAO.RoleCheckDB(username) == RoleEnumType.SystemAdmin)
            {
                _logger.LogWarning("Can't modify a system admin.");
                return "Error: you can't delete a system admin";
            } else
            {
                // calls the DAO to remove from database
                _logger.LogInformation("User deleted.");
                return _userManagementService._userManagementDAO.DeleteUserDB(username);
            }
        }

        // checks a user's role
        public string RoleCheck(string username)
        {
            // DAO checks DB for role
            _logger.LogInformation("Role retrieved for user.");
            return _userManagementService._userManagementDAO.RoleCheckDB(username);
        }


        /*-----------------------------------------------------------------------------------------------------------------------
         * https://www.programmersought.com/article/9242241361/
         * ---------------------------------------------------------------------------------------------------------------------*/
        // encryption is done on the front end with js

        // Advanced Encryption Standard Initialization Vector
        const string AES_IV = "1234567890000000";//16 bits    

        /// <summary>  
        /// AES decryption  
        /// </summary>  
        /// <param name="input"> ciphertext byte array</param>  
        /// <param name="key">key (32 bit)</param>  
        /// <returns> returns the decrypted string</returns>  
        public string DecryptByAES(string input, string key)
        {
            byte[] inputBytes = HexStringToByteArray(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV.Substring(0, 16));

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream(inputBytes))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srEncrypt = new StreamReader(csEncrypt))
                        {
                            return srEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Convert the specified hex string to a byte array
        /// </summary>
        /// <param name="s">hexadecimal string (eg "7F 2C 4A" or "7F2C4A")</param>
        /// <returns>byte array corresponding to hexadecimal string</returns>
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }
    }
}
