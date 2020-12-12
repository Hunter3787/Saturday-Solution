using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [DataTestMethod]
        [DataRow("test@test.test", "username", "firstName", "lastName")]
        [DataRow("test@test.test", "username--12", "firstName", "lastName")]
        [DataRow("test@test.test", "user", "firstName", "lastName")]


        public void IsInformationValid_Pass(string email, string username, string firstName, string lastName)
        {
            //Arrange
            UserManagementGateway gateway = new UserManagementGateway();
            Mock<UserAccount> user = new Mock<UserAccount>();
            user.Object.UserEmail = email;
            user.Object.UserName = username;
            user.Object.FirstName = firstName;
            user.Object.LastName = lastName;

            //Act
            bool result = gateway.IsInformationValid(user.Object);

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
            UserManagementGateway gateway = new UserManagementGateway();
            Mock<UserAccount> user = new Mock<UserAccount>();
            user.Object.UserEmail = email;
            user.Object.UserName = username;
            user.Object.FirstName = firstName;
            user.Object.LastName = lastName;

            //Act
            bool result = gateway.IsInformationValid(user.Object);

            //Assert
            Assert.IsFalse(result);
        }
    }
}
