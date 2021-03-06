using AutoBuildApp.BusinessLayer;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class UserManagementManagerUnitTest
    {
        private UserManagementManager manager;

        [TestInitialize]
        public void Setup()
        {
            manager = new UserManagementManager("");
        }

        [DataTestMethod]
        [DataRow("test@test.test", "username", "firstName", "lastName")]
        [DataRow("test@test.test", "username--12", "firstName", "lastName")]
        [DataRow("test@test.test", "user", "firstName", "lastName")]
        public void IsInformationValid_Pass(string email, string username, string firstName, string lastName)
        {
            //Arrange
            Mock<UserAccount> user = new Mock<UserAccount>();
            user.Object.UserEmail = email;
            user.Object.UserName = username;
            user.Object.FirstName = firstName;
            user.Object.LastName = lastName;

            //Act
            bool result = manager.IsInformationValid(user.Object);

            //Assert
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow("test@testtest", "username", "firstName", "lastName")]
        [DataRow("testtest.test", "username", "firstName", "lastName")]
        [DataRow("test@test.test", "", "firstName", "lastName")]
        [DataRow("test@test.test", "use", "firstName", "lastName")]
        [DataRow("test@test.test", "username---13", "firstName", "lastName")]
        [DataRow("test@test.test", "username", "", "lastName")]
        [DataRow("test@test.test", "username", "firstName", "")]
        public void IsInformationValid_Fail(string email, string username, string firstName, string lastName)
        {
            //Arrange
            Mock<UserAccount> user = new Mock<UserAccount>();
            user.Object.UserEmail = email;
            user.Object.UserName = username;
            user.Object.FirstName = firstName;
            user.Object.LastName = lastName;

            //Act
            bool result = manager.IsInformationValid(user.Object);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UserManagementManager_UnauthorizedUser_Unauthorized()
        {
            //Arrange
            UserAccount admin = new UserAccount("abcd", "b", "c", "d", "ADMIN", "f", "09-12-1990");
            UserAccount user = new UserAccount("abcd", "b", "c", "d", "USER", "f", "09-12-1990");
            UserManagementManager manager = new UserManagementManager("");

            //Act
            var response = manager.CreateUserRecord(user, admin);

            //Assert
            Assert.IsTrue(response == "Unauthorized");
        }
    }
}
