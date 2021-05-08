using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers;
using AutoBuildApp.Services.UserServices;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BC = BCrypt.Net.BCrypt;
using System;
using System.Collections.Generic;
using System.Data;
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
        public void UpdateUserPasswordSuccessDAO(string password, string passwordCheck, string activeEmail)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.UpdatePassword(password, passwordCheck, activeEmail);
                    String sql = "SELECT passwordHash FROM UserCredentials INNER JOIN MappingHash ON MappingHash.userHashID = UserCredentials.userHashID " +
                                    "INNER JOIN UserAccounts ON UserAccounts.userID = MappingHash.userID " +
                                    "WHERE UserAccounts.email = @EMAIL;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = activeEmail;

                    using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string value = reader["passwordHash"].ToString();
                            bool verified = BCrypt.Net.BCrypt.Verify(password, value);
                            //Assert
                            Assert.AreEqual(true, verified);
                        }
                    }
                    transaction.Commit();
                }
            }
        }

        [DataTestMethod]
        // no numbers
        [DataRow("Password", "Password", "goodBoy@gmail.com")]
        // no uppercase
        [DataRow("password123", "password123", "goodBoy@gmail.com")]
        // no lowercase
        [DataRow("PASSWORD123", "PASSWORD123", "goodBoy@gmail.com")]
        // too short (less than 8 characters)
        [DataRow("Pass12", "Pass12", "goodBoy@gmail.com")]
        // empty password
        [DataRow("", "", "goodBoy@gmail.com")]
        public void UpdateUserPasswordInvalidPasswords(string password, string passwordCheck, string activeEmail)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.UpdatePassword(password, passwordCheck, activeEmail);
                    String sql = "SELECT passwordHash FROM UserCredentials INNER JOIN MappingHash ON MappingHash.userHashID = UserCredentials.userHashID " +
                                    "INNER JOIN UserAccounts ON UserAccounts.userID = MappingHash.userID " +
                                    "WHERE UserAccounts.email = @EMAIL;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = activeEmail;

                    using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string value = reader["passwordHash"].ToString();
                            bool verified = BCrypt.Net.BCrypt.Verify(password, value);
                            //Assert
                            Assert.AreEqual(false, verified);
                        }
                    }
                    transaction.Commit();
                }
            }
        }


        [DataTestMethod]
        // successful data input
        [DataRow("goofygoober@gmail.com", "goodBoy@gmail.com")]
        public void UpdateUserEmailSuccess(string inputEmail, string activeEmail)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.UpdateEmail(inputEmail, activeEmail);
                    String sql = "SELECT COUNT (email) FROM UserAccounts " + 
                                    "WHERE email = @INPUTEMAIL;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@INPUTEMAIL", SqlDbType.VarChar).Value = inputEmail;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    userManagementManager.UpdateEmail(activeEmail, inputEmail);
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(1, rowCount);
                }
            }
        }

        [DataTestMethod]
        // no @ symbol
        [DataRow("goofygoobergmail.com", "goodBoy@gmail.com")]
        // no . period
        [DataRow("goofygoober@gmailcom", "goodBoy@gmail.com")]
        // email left blank
        [DataRow("", "goodBoy@gmail.com")]
        public void UpdateUserEmailInvalidEmail(string inputEmail, string activeEmail)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.UpdateEmail(inputEmail, activeEmail);
                    String sql = "SELECT COUNT (email) FROM UserAccounts " +
                                    "WHERE email = @INPUTEMAIL;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@INPUTEMAIL", SqlDbType.VarChar).Value = inputEmail;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(0, rowCount);
                }
            }
        }

        [DataTestMethod]
        // ZeinabFarhat@gmail.com is in database
        [DataRow("ZeinabFarhat@gmail.com", "goodBoy@gmail.com")]
        public void UpdateUserEmailAlreadyExists(string inputEmail, string activeEmail)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.UpdateEmail(inputEmail, activeEmail);
                    String sql = "SELECT COUNT (email) FROM UserAccounts " +
                                    "WHERE email = @INPUTEMAIL;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@INPUTEMAIL", SqlDbType.VarChar).Value = inputEmail;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(1, rowCount);
                }
            }
        }

        [DataTestMethod]
        // successful data input
        [DataRow("PyreFyre", "goodBoy@gmail.com")]
        public void UpdateUserUsernameSuccess(string username, string activeEmail)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.UpdateUsername(username, activeEmail);
                    String sql = "SELECT COUNT (username) FROM UserCredentials " +
                                    "WHERE username = @USERNAME;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    userManagementManager.UpdateUsername("goodBoy399", activeEmail);
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(1, rowCount);
                }
            }
        }

        [DataTestMethod]
        // shorter than 4 characters
        [DataRow("Pyr", "goodBoy@gmail.com")]
        // too long (20 max)
        [DataRow("PyreFyre1012345678901", "goodBoy@gmail.com")]
        public void UpdateUserUsernameInvalidUsername(string username, string activeEmail)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.UpdateUsername(username, activeEmail);
                    String sql = "SELECT COUNT (username) FROM UserCredentials " +
                                    "WHERE username = @USERNAME;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(0, rowCount);
                }
            }
        }

        [DataTestMethod]
        // Zeina already is in the database
        [DataRow("Zeina", "goodBoy@gmail.com")]
        public void UpdateUserUsernameAlreadyExists(string username, string activeEmail)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.UpdateUsername(username, activeEmail);
                    String sql = "SELECT COUNT (username) FROM UserCredentials " +
                                    "WHERE username = @USERNAME;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(1, rowCount);
                }
            }
        }
        ///*----------------------------------------------------------------------------------------------------------------------------
        // *                        END OF UPDATE TESTS
        // *---------------------------------------------------------------------------------------------------------------------------*/

        ///*----------------------------------------------------------------------------------------------------------------------------
        //*                        START OF ADMIN USER MANAGEMENT TESTS
        //*---------------------------------------------------------------------------------------------------------------------------*/

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
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.ChangePermissions(username, role);
                    String sql = "SELECT COUNT (username) FROM UserCredentials " +
                                    "WHERE userRole = @USERROLE AND username = @USERNAME;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@USERROLE", SqlDbType.VarChar).Value = role;
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(1, rowCount);
                }
            }
        }

        [DataTestMethod]
        // This user is a system admin
        [DataRow("SERGE", "BasicRole")]
        public void ChangeUserPermissionsSystemAdmin(string username, string role)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.ChangePermissions(username, role);
                    String sql = "SELECT COUNT (username) FROM UserCredentials " +
                                    "WHERE userRole = @USERROLE AND username = @USERNAME";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@USERROLE", SqlDbType.VarChar).Value = role;
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(0, rowCount);
                }
            }
        }

        [DataTestMethod]
        // This user is not a system admin (successful change)
        [DataRow("pepper", true)]
        public void ChangeUserLock(string username, bool lockstate)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.ChangeLockState(username, lockstate);
                    String sql = "SELECT COUNT (username) FROM UserCredentials " +
                                    "WHERE locked = @LOCKSTATE AND username = @USERNAME;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@LOCKSTATE", SqlDbType.VarChar).Value = lockstate;
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(1, rowCount);
                }
            }
        }

        [DataTestMethod]
        // This user is not a system admin (successful unlock)
        [DataRow("SERGE", false)]
        public void ChangeUserUnlock(string username, bool lockstate)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.ChangeLockState(username, lockstate);
                    String sql = "SELECT COUNT (username) FROM UserCredentials " +
                                    "WHERE locked = @LOCKSTATE AND username = @USERNAME;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@LOCKSTATE", SqlDbType.VarChar).Value = lockstate;
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(1, rowCount);
                }
            }
        }

        [DataTestMethod]
        // This user is a System Admin
        [DataRow("goodBoy399", true)]
        public void ChangeUserLockedSystemAdmin(string username, bool lockstate)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.ChangeLockState(username, lockstate);
                    String sql = "SELECT COUNT (username) FROM UserCredentials " +
                                    "WHERE locked = @LOCKSTATE AND username = @USERNAME;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@LOCKSTATE", SqlDbType.VarChar).Value = lockstate;
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(0, rowCount);
                }
            }
        }
     
        [DataTestMethod]
        // This user is not a system admin (success)
        [DataRow("pepper")]
        public void DeleteUserSuccess(string username)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.DeleteUser(username);
                    String sql = "SELECT COUNT (username) FROM UserCredentials " +
                                    "WHERE username = @USERNAME;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    _registrationManager.RegisterUser("pepper", "Pep", "Pering", "pepper@gmail.com", "Passhash1", "Passhash1");
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(0, rowCount);
                }
            }
        }

        [DataTestMethod]
        // This user is a System Admin
        [DataRow("goodBoy399")]
        public void DeleteUserSystemAdmin(string username)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.DeleteUser(username);
                    String sql = "SELECT COUNT (username) FROM UserCredentials " +
                                    "WHERE username = @USERNAME;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(1, rowCount);
                }
            }
        }

        [DataTestMethod]
        // Delete a user that doesn't exist
        [DataRow("ljlkjdlsfj")]
        public void DeleteUserNotExist(string username)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection")))
            {
                //Arrange
                UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
                UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
                SqlDataAdapter adapter = new SqlDataAdapter();

                int rowCount = 0;
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    //Act
                    String result = userManagementManager.DeleteUser(username);
                    String sql = "SELECT COUNT (username) FROM UserCredentials " +
                                    "WHERE username = @USERNAME;";

                    adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                    adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

                    rowCount = Convert.ToInt32(adapter.InsertCommand.ExecuteScalar());
                    transaction.Commit();

                    //Assert
                    Assert.AreEqual(0, rowCount);
                }
            }
        }
    }
}
