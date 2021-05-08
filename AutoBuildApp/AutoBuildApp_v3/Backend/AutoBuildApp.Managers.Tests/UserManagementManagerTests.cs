using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers;
using AutoBuildApp.Services.UserServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Manger.Tests
{
    [TestClass]
    public class UserManagementManagerTests
    {
        private readonly UserManagementDAO _userManagementDAO = new UserManagementDAO(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
        private readonly RegistrationManager _registrationManager = new RegistrationManager(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));


        /* ----------------------------------------------------------------------------------------------------------------------------
                                    START OF UPDATE TESTS
        ----------------------------------------------------------------------------------------------------------------------------*/

        [DataTestMethod]
        // successful data input
        [DataRow("Password123", "Password123", "goodBoy@gmail.com")]
        public void UpdateUserPasswordSuccess(string password, string passwordCheck, string activeEmail)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.UpdatePassword(password, passwordCheck, activeEmail);

            // Assert
            Assert.AreEqual("Password successfully updated", result);
        }

        [DataTestMethod]
        // no numbers
        [DataRow("Password", "Password123", "goodBoy@gmail.com")]
        // no uppercase
        [DataRow("password123", "Password123", "goodBoy@gmail.com")]
        // no lowercase
        [DataRow("PASSWORD123", "Password123", "goodBoy@gmail.com")]
        // too short (less than 8 characters)
        [DataRow("Pass12", "Password123", "goodBoy@gmail.com")]
        // empty password
        [DataRow("", "Password123", "goodBoy@gmail.com")]
        public void UpdateUserPasswordInvalidPasswords(string password, string passwordCheck, string activeEmail)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.UpdatePassword(password, passwordCheck, activeEmail);

            // Assert
            Assert.AreEqual("Invalid password", result);
        }

        [DataTestMethod]
        // first doesn't match second
        [DataRow("Password1234", "Password123", "goodBoy@gmail.com")]
        // second doesn't match first
        [DataRow("Password123", "Password1234", "goodBoy@gmail.com")]
        public void UpdateUserPasswordNotMatching(string password, string passwordCheck, string activeEmail)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.UpdatePassword(password, passwordCheck, activeEmail);

            // Assert
            Assert.AreEqual("Passwords do not match", result);
        }

        [DataTestMethod]
        // successful data input
        [DataRow("goofygoober@gmail.com", "goodBoy@gmail.com")]
        public void UpdateUserEmailSuccess(string InputEmail, string activeEmail)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.UpdateEmail(InputEmail, activeEmail);
            userManagementManager.UpdateEmail(activeEmail, InputEmail);
            // Assert
            Assert.AreEqual("Email successfully updated", result);
        }

        [DataTestMethod]
        // no @ symbol
        [DataRow("goofygoobergmail.com", "goodBoy@gmail.com")]
        // no . period
        [DataRow("goofygoober@gmailcom", "goodBoy@gmail.com")]
        // email left blank
        [DataRow("", "goodBoy@gmail.com")]
        public void UpdateUserEmailInvalidEmail(string InputEmail, string activeEmail)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.UpdateEmail(InputEmail, activeEmail);
            userManagementManager.UpdateEmail(activeEmail, InputEmail);
            // Assert
            Assert.AreEqual("Invalid email", result);
        }

        [DataTestMethod]
        // ZeinabFarhat@gmail.com is in database
        [DataRow("ZeinabFarhat@gmail.com", "goodBoy@gmail.com")]
        public void UpdateUserEmailAlreadyExists(string InputEmail, string activeEmail)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.UpdateEmail(InputEmail, activeEmail);
            // Assert
            Assert.AreEqual("Email already exists", result);
        }

        [DataTestMethod]
        // successful data input
        [DataRow("PyreFyre", "goodBoy@gmail.com")]
        public void UpdateUserUsernameSuccess(string username, string activeEmail)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.UpdateUsername(username, activeEmail);
            userManagementManager.UpdateUsername("goodBoy399", activeEmail);
            // Assert
            Assert.AreEqual("Username successfully updated", result);
        }

        [DataTestMethod]
        // shorter than 4 characters
        [DataRow("Pyr", "goodBoy@gmail.com")]
        // too long (20 max)
        [DataRow("PyreFyre1012345678901", "goodBoy@gmail.com")]
        public void UpdateUserUsernameInvalidUsername(string username, string activeEmail)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.UpdateUsername(username, activeEmail);
            // Assert
            Assert.AreEqual("Invalid username", result);
        }

        [DataTestMethod]
        // Zeina already is in the database
        [DataRow("Zeina", "goodBoy@gmail.com")]
        public void UpdateUserUsernameAlreadyExists(string username, string activeEmail)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.UpdateUsername(username, activeEmail);
            // Assert
            Assert.AreEqual("Username already exists", result);
        }
        /*----------------------------------------------------------------------------------------------------------------------------
         *                        END OF UPDATE TESTS
         *---------------------------------------------------------------------------------------------------------------------------*/

        /*----------------------------------------------------------------------------------------------------------------------------
        *                        START OF ADMIN USER MANAGEMENT TESTS
        *---------------------------------------------------------------------------------------------------------------------------*/

        [DataTestMethod]
        // Successfull change Basic
        [DataRow("pepper", "BasicRole")]
        // Successfull change Delegate Admin
        [DataRow("pepper", "DelegateAdmin")]
        // Successfull change Vendor
        [DataRow("pepper", "VendorRole")]
        // Successfull change Unregistered
        [DataRow("pepper", "UnregisteredRole")]
        // NOT TESTING SYSTEM ADMIN BECAUSE YOU CAN'T CHANGE IT BACK
        public void ChangeUserPermissionsSuccess(string username, string role)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.ChangePermissions(username, role);
            // Assert
            Assert.AreEqual("Permissions successfully updated", result);
        }

        [DataTestMethod]
        // This user is a system admin
        [DataRow("goodBoy399", "BasicRole")]
        public void ChangeUserPermissionsSystemAdmin(string username, string role)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.ChangePermissions(username, role);
            // Assert
            Assert.AreEqual("Error: you can't modify the permissions of a system admin", result);
        }

        [DataTestMethod]
        // This user is not a system admin (successful change)
        [DataRow("pepper", true)]
        public void ChangeUserLock(string username, bool lockstate)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.ChangeLockState(username, lockstate);
            // Assert
            Assert.AreEqual("Account has been locked", result);
        }

        [DataTestMethod]
        // This user is not a system admin (successful unlock)
        [DataRow("pepper", false)]
        public void ChangeUserUnlock(string username, bool lockstate)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.ChangeLockState(username, lockstate);
            // Assert
            Assert.AreEqual("Account has been unlocked", result);
        }

        [DataTestMethod]
        // This user is a System Admin
        [DataRow("goodBoy399", true)]
        public void ChangeUserLockedSystemAdmin(string username, bool lockstate)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.ChangeLockState(username, lockstate);
            // Assert
            Assert.AreEqual("Error: you can't lock a system admin", result);
        }

        [DataTestMethod]
        // This user is not a system admin (success)
        [DataRow("pepper")]
        public void DeleteUserSuccess(string username)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.DeleteUser(username);
            _registrationManager.RegisterUser("pepper", "Pep", "Pered", "pepper@gmail.com", "Passhash1", "Passhash1");
            // Assert
            Assert.AreEqual("Account has been successfully deleted", result);
        }

        [DataTestMethod]
        // This user is a System Admin
        [DataRow("goodBoy399")]
        public void DeleteUserSystemAdmin(string username)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.DeleteUser(username);
            // Assert
            Assert.AreEqual("Error: you can't delete a system admin", result);
        }

        [DataTestMethod]
        // Delete a user that doesn't exist
        [DataRow("ljlkjdlsfj")]
        public void DeleteUserNotExist(string username)
        {
            // Arrange
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // Act
            String result = userManagementManager.DeleteUser(username);
            // Assert
            Assert.AreEqual("Failed to delete", result);
        }
    }
}
