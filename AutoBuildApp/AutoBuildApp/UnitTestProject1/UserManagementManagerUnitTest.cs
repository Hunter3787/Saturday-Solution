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

        //[DataTestMethod]
        //[DataRow("ABCD", "firstName", "lastName", "email@email.com", "ADMIN")]
        //[DataRow("abcdefghijkl", "firstName", "lastName", "email@email.com", "ADMIN")]


        //public void CreateUserRecord_Success(string username, string firstName, string lastName, string email, string role)
        //{
        //    //Arrange
        //    UserAccount admin = new UserAccount("a", "b", "c", "d", "ADMIN", "f", "09-12-1990");
        //    UserAccount user = new UserAccount(username, firstName, lastName, email, role, "f", "09-12-1990");

        //    //Act
        //    string result = manager.CreateUserRecord(admin, user);

        //    //Assert
        //    Assert.IsTrue(result == "Successful user creation");
        //}

        //[DataTestMethod]
        //[DataRow("ABCD", "firstName", "lastName", "email@email.com", "ADMIN")]
        //[DataRow("abcdefghijkl", "firstName", "lastName", "email@email.com", "ADMIN")]
        //public void CreateUserRecord_Failure(string username, string firstName, string lastName, string email, string role)
        //{
        //    //Arrange
        //    UserAccount admin = new UserAccount("a", "b", "c", "d", "ADMIN", "f", "09-12-1990");
        //    UserAccount user = new UserAccount(username, firstName, lastName, email, role, "f", "09-12-1990");
        //    Mock<UserManagementService> service = new Mock<UserManagementService>("");
        //    service.Setup(x => x.CreateUser(user)).Returns("hello");
        //    //Act
        //    string result = manager.CreateUserRecord(admin, user);

        //    //Assert
        //    Assert.IsTrue(result != "Successful user creation");
        //}

        //[TestMethod]
        //public void UserManagementManager_UnauthorizedUser_Authorized()
        //{
        //    //Arrange
        //    UserAccount admin = new UserAccount("asdf", "b", "c", "d@.", "ADMIN", "f", "09-12-1990");
        //    UserAccount user = new UserAccount("asdf", "b", "c", "d@.", "USER", "f", "09-12-1990");
        //    UserManagementManager manager = new UserManagementManager("");

        //    //Act
        //    var response = manager.CreateUserRecord(admin, user);

        //    //Assert
        //    Assert.IsTrue(response != "Unauthorized");
        //}

        [TestMethod]
        public void UserManagementManager_UnauthorizedUser_Unauthorized()
        {
            //Arrange
            UserAccount admin = new UserAccount("a", "b", "c", "d", "ADMIN", "f", "09-12-1990");
            UserAccount user = new UserAccount("a", "b", "c", "d", "USER", "f", "09-12-1990");
            UserManagementManager manager = new UserManagementManager("");

            //Act
            var response = manager.CreateUserRecord(user, admin);

            //Assert
            Assert.IsTrue(response == "Unauthorized");
        }

        public void service_test()
        {
            UserAccount uc = new UserAccount("a", "b", "c", "d", "e", "f", "g");
            UserAccount uc2 = new UserAccount("a", "b", "c", "d", "e", "f", "g");

            UserManagementService service = new UserManagementService("Data Source=DESKTOP-CI24C55\\MSSQLSERVER02;Initial Catalog=dannytemp2;Integrated Security=True");
            Mock <UserManagementGateway> gateway = new Mock<UserManagementGateway>();
            gateway.Setup(x => x.CreateUserRecord(It.IsAny<UserAccount>())).Returns("Hello");

            string response = service.CreateUser(uc);

            Assert.IsTrue(response == "Hello");
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
