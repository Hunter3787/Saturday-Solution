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

        [DataTestMethod]
        [DataRow("ADMIN", "username", "firstName", "lastName")]
        public void CreateUserRecord_Success(string role, string email, string username, string firstName, string lastName)
        {
            //Arrange
            Mock<UserAccount> admin = new Mock<UserAccount>();
            admin.Object.role = role;

            Mock<UserAccount> user = new Mock<UserAccount>();
            user.Object.UserEmail = email;
            user.Object.UserName = username;
            user.Object.FirstName = firstName;
            user.Object.LastName = lastName;

            //Mock<UserManagementService> service = new Mock<UserManagementService>();
            //service.Setup(service => service.CreateUser(user.Object)).Returns("Success");

            //Act
            string result = manager.CreateUserRecord(admin.Object, user.Object);

            //Assert
            Assert.IsTrue(result == "Success");
        }

        public void UpdateUserRecord_Success()
        {
            //Arrange

            //Act

            //Assert
        }

        public void DeleteUserRecord_Success()
        {
            //Arrange

            //Act

            //Assert
        }

        public void EnableUser_Success()
        {
            //Arrange

            //Act

            //Assert
        }

        public void DisableUser_Success()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
