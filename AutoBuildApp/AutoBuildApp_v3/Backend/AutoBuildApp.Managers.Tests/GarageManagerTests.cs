using System;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Managers;
using AutoBuildApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;

namespace AutoBuildApp.Manger.Tests
{
    [TestClass]
    public class UserGarageManagerTests
    {
        private UserGarageManager _testManager;
        private readonly string _testString = "Data Source=localhost;Initial Catalog=DB;Integrated Security=True; user id=sa; password=Quake3arena!;";

        [TestInitialize]
        public void Setup()
        {
            _testManager = new UserGarageManager(_testString);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        /// <summary>
        /// Tests the initialization and
        /// collection of all the builds from the garage.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_InitializeGarage_ReturnBuilds()
        {
            // Arrange
            
            // Act

            // Assert
        }

        /// <summary>
        /// Return a shelf List.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_GetShelf_ReturnListShelfOk200()
        {
            // Arrange

            // Act

            // Assert

        }

        /// <summary>
        /// Return a List of Builds.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_GetBuilds_ReturnListOfBuildsOk200()
        {
            // Arrange

            // Act

            // Assert

        }

        /// <summary>
        /// Return a Build object with its details.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_GetBuildDetails_ReturnBuildOk200()
        {
            // Arrange

            // Act

            // Assert

        }

        /// <summary>
        /// Sorts the builds in the DB and returns them in order by date.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_SortGarage_BuildsByRecentDateCreated()
        {
            // Arrange

            // Act

            // Assert
        }

        /// <summary>
        /// Sorts the builds in the DB and returns
        /// them in order by image inclusion.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_SortGarage_BuildsWithImages()
        {
            // Arrange

            // Act

            // Assert
        }

        /// <summary>
        /// Sorts the builds in the DB and returns
        /// them in order by publication status.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_SortGarage_BuildsThatHaveBeenPublished()
        {
            // Arrange

            // Act

            // Assert
        }

        /// <summary>
        /// Sorts the builds in the DB and returns
        /// them in order by who created it.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_SortGarage_BuildsByCreator()
        {
            // Arrange

            // Act

            // Assert
        }

        /// <summary>
        /// ttempt to create a shelf and return true if successful.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_CreateShelf_True()
        {
            // Arrange
            CommonResponse expected = new CommonResponse();
            CommonResponse actual = new CommonResponse();
            expected.ResponseBool = true;
            expected.ResponseString = ResponseStringGlobals.SUCCESSFUL_CREATION;
            string user = "Nick";
            string shelfName = "Tacobell";

            // Act
            actual = _testManager.CreateShelf(shelfName, user);

            // Assert
            Assert.AreEqual(expected.ResponseBool,actual.ResponseBool);
            Assert.AreEqual(expected.ResponseString, actual.ResponseString);
        }

        /// <summary>
        /// Attempts to delete a shelf returns true on successs.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_DeleteShelf_True()
        {
            // Arrange

            // Act

            // Assert
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void UserGarageManager_AddItemToShelf_True()
        {
            // Arrange

            // Act

            // Assert
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void UserGarageManager_RemoveItemFromShelf_True()
        {
            // Arrange

            // Act

            // Assert
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void UserGarageManager_ChangeQuanity_True()
        {
            // Arrange

            // Act

            // Assert
        }

        /// <summary>
        /// Attempts to delete a build, returns true on success.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_DeleteBuild_True()
        {
            // Arrange

            // Act

            // Assert
        }

        /// <summary>
        /// Attempts to duplicate a build, return true on success.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_DuplicateBuild_True()
        {
            // Arrange

            // Act

            // Assert
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void UserGarageManager_EditBuild_ReturnModifiedBuild()
        {
            // Arrange

            // Act

            // Assert
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void UserGarageManager_AddBuild_True()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
