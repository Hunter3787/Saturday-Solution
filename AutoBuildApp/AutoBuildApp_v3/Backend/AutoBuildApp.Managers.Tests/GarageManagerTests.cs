using System;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Managers;
using AutoBuildApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Api.Controllers;
using System.Threading;
using AutoBuildApp.Security.Models;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.Interfaces;
using System.Security.Claims;
using System.Collections.Generic;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Models.Builds;
using System.Linq;

namespace AutoBuildApp.Manger.Tests
{
    [TestClass]
    public class UserGarageManagerTests
    {
        private UserGarageManager _testManager;
        private readonly string _connString;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public UserGarageManagerTests()
        {
            _connString = ConnectionManager
                .connectionManager
                .GetConnectionStringByName(ControllerGlobals.DOCKER_CONNECTION);

            UserIdentity userIdentity = new UserIdentity
            {
                Name = "Zeina",
                IsAuthenticated = true,
                AuthenticationType = "JWT",
                
            };

            ClaimsFactory claimFactory = new ConcreteClaimsFactory();
            IClaims basicClaims = claimFactory.GetClaims(RoleEnumType.BasicRole);
            ClaimsIdentity basicIdentity = new ClaimsIdentity(
                userIdentity,
                basicClaims.Claims(),
                userIdentity.AuthenticationType,
                userIdentity.Name,
                RoleEnumType.BasicRole);
            _claimsPrincipal = new ClaimsPrincipal(basicIdentity);
            Console.WriteLine(_claimsPrincipal.Identity.Name);
            Thread.CurrentPrincipal = _claimsPrincipal;

            _testManager = new UserGarageManager(_connString);
        }

        [TestInitialize]
        public void Setup()
        {
            
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        ///// <summary>
        ///// Return a List of Builds.
        ///// </summary>
        //[TestMethod]
        //public void UserGarageManager_GetBuilds_ReturnListOfBuilds()
        //{
        //    // Arrange

        //    // Act

        //    // Assert

        //}


        ///// <summary>
        ///// Sorts the builds in the DB and returns them in order by date.
        ///// </summary>
        //[TestMethod]
        //public void UserGarageManager_SortGarage_BuildsByRecentDateCreated()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        ///// <summary>
        ///// Sorts the builds in the DB and returns
        ///// them in order by image inclusion.
        ///// </summary>
        //[TestMethod]
        //public void UserGarageManager_SortGarage_BuildsWithImages()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        ///// <summary>
        ///// Sorts the builds in the DB and returns
        ///// them in order by publication status.
        ///// </summary>
        //[TestMethod]
        //public void UserGarageManager_SortGarage_BuildsThatHaveBeenPublished()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        ///// <summary>
        ///// Sorts the builds in the DB and returns
        ///// them in order by who created it.
        ///// </summary>
        //[TestMethod]
        //public void UserGarageManager_SortGarage_BuildsByCreator()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        /// <summary>
        /// ttempt to create a shelf and return true if successful.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_CreateShelf_True()
        {
            // Arrange
            CommonResponse expected = new CommonResponse();
            CommonResponse actual;
            expected.IsSuccessful = true;
            expected.ResponseString = ResponseStringGlobals.SUCCESSFUL_CREATION;
            string shelfName = "Tacobell";


            // Act
            actual = _testManager.CreateShelf(shelfName);
            Console.WriteLine(actual.ResponseString);
            // Cleanup before assertion.
            _testManager.DeleteShelf(shelfName);

            // Assert
            Assert.AreEqual(expected.IsSuccessful,actual.IsSuccessful);
            Assert.AreEqual(expected.ResponseString, actual.ResponseString);  
        }

        [TestMethod]
        public void UserGarageManager_GetShelvesByUser_ReturnTwoShelvesAndSuccess()
        {
            // Arrange
            _testManager.CreateShelf("TacoBell");
            _testManager.CreateShelf("Lincoln");
            var expectedList = new List<Shelf>()
            {
                new Shelf()
                {
                    ShelfName = "TacoBell"
                },
                new Shelf()
                {
                    ShelfName = "Lincoln"
                }
            };
            var expectedBool = true;


            // Act
            // This is in case we ever want to call other users shelves.
            var actual = _testManager.GetShelvesByUser(Thread.CurrentPrincipal.Identity.Name);
            _testManager.DeleteShelf("TacoBell");
            _testManager.DeleteShelf("Lincoln");
            var actualList = actual.GenericObject;


            // Assert
            Assert.IsTrue(expectedList.SequenceEqual(actualList));
            //CollectionAssert.AreEqual(expectedList, actualList);
            Assert.AreEqual(expectedBool, actual.IsSuccessful);
            
        }

        /// <summary>
        /// Attempts to delete a shelf returns true on successs.
        /// </summary>
        [TestMethod]
        public void UserGarageManager_DeleteShelf_True()
        {
            // Arrange
            _testManager.CreateShelf("TacoBell");
            var expectedBool = true;
            var expectedString = ResponseStringGlobals.SUCCESSFUL_DELETION;

            // Act
            var actual = _testManager.DeleteShelf("TacoBell");

            // Assert
            Assert.AreEqual(expectedString, actual.ResponseString);
            Assert.AreEqual(expectedBool, actual.IsSuccessful);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void UserGarageManager_AddItemToShelf_True()
        {
            // Arrange
            var shelfName = "TacoBell";
            _testManager.CreateShelf(shelfName);
            var component = new Component()
            {
                Quantity = 1,
                ModelNumber = "MODEL_1"
            };
            var expectedBool = true;
            var expectedString = ResponseStringGlobals.SUCCESSFUL_ADDITION;

            // Act
            var actual = _testManager.AddToShelf(component, shelfName);

            // Cleanup
            _testManager.DeleteShelf(shelfName);

            // Assert
            Assert.AreEqual(expectedString, actual.ResponseString);
            Assert.AreEqual(expectedBool, actual.IsSuccessful);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void UserGarageManager_RemoveItemFromShelf_True()
        {
            // Arrange
            var shelfName = "TacoBell";
            _testManager.CreateShelf(shelfName);
            var component = new Component()
            {
                Quantity = 1,
                ModelNumber = "MODEL_1"
            };
            var expectedBool = true;
            var expectedString = ResponseStringGlobals.SUCCESSFUL_REMOVAL;
            var index = 0;

            // Act
            var actual = _testManager.RemoveFromShelf(index,shelfName);

            // Assert
            Assert.AreEqual(expectedString, actual.ResponseString);
            Assert.AreEqual(expectedBool, actual.IsSuccessful);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void UserGarageManager_ChangeQuanity_True()
        {
            // Arrange
            var shelfName = "TacoBell";
            _testManager.CreateShelf(shelfName);
            var component = new Component()
            {
                Quantity = 1,
                ModelNumber = "MODEL_1"
            };
            _testManager.AddToShelf(component, shelfName);

            var expectedBool = true;
            var expectedString = ResponseStringGlobals.SUCCESSFUL_MODIFICATION;
            var itemIndex = 0;
            var newQuantity = 3;

            // Act
            var actual = _testManager.UpdateQuantity(itemIndex, newQuantity, shelfName);

            _testManager.DeleteShelf(shelfName);

            // Assert
            Assert.AreEqual(expectedBool, actual.IsSuccessful);
            Assert.AreEqual(expectedString, actual.ResponseString);
        }

        ///// <summary>
        ///// Attempts to delete a build, returns true on success.
        ///// </summary>
        //[TestMethod]
        //public void UserGarageManager_DeleteBuild_True()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        ///// <summary>
        ///// Attempts to duplicate a build, return true on success.
        ///// </summary>
        //[TestMethod]
        //public void UserGarageManager_DuplicateBuild_True()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //[TestMethod]
        //public void UserGarageManager_EditBuild_ReturnModifiedBuild()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //[TestMethod]
        //public void UserGarageManager_AddBuild_True()
        //{
        //    // Arrange
        //    Build test = new Build()
        //    {
                
        //    };

        //    // Act

        //    // Assert
        //}
    }
}
