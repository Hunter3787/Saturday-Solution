using System;
using System.Collections.Generic;
using AutoBuildApp.Api.Controllers;
using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Models;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoBuildApp.DataAccess.Test
{
    [TestClass]
    public class ShelfDAOTests
    {
        string conString =
            ConnectionManager.
            connectionManager.
            GetConnectionStringByName(ControllerGlobals.DOCKER_CONNECTION);

        public ShelfDAOTests()
        {
        }

        [TestMethod]
        public void ShelfDAO_GetAllShelvesByUser_ReturnTwoShelves()
        {
            // Arrange
            ShelfDAO shelfDAO = new ShelfDAO(conString);
            var username = "kool";

            List<Shelf> expected = new List<Shelf>()
            {
                new Shelf()
                {
                    ShelfName = "McDonalds",
                },
                new Shelf()
                {
                    ShelfName = "TacoBell",
                    ComponentList = new List<IComponent>()
                    {
                        new Component()
                        {
                            Quantity = 2,
                           // ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Component()
                        {
                            Quantity = 2,
                           // ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        }
                    }
                }
            };
            
            // Act
            var actual = shelfDAO.GetAllShelvesByUser(username);


            // Assert
            Assert.AreEqual(expected[1].ShelfName, actual[1].ShelfName);
            Assert.AreEqual(expected[1].ComponentList[0].Quantity, actual[1].ComponentList[0].Quantity);
            Assert.AreEqual(expected[1].ComponentList[0].Quantity, actual[1].ComponentList[0].Quantity);
            Assert.AreEqual(expected[1].ComponentList[0].Quantity, actual[1].ComponentList[0].Quantity);
            Assert.AreEqual(expected[1].ComponentList[1].Quantity, actual[1].ComponentList[1].Quantity);
            Assert.AreEqual(expected[1].ComponentList[1].Quantity, actual[1].ComponentList[1].Quantity);
            Assert.AreEqual(expected[1].ComponentList[1].Quantity, actual[1].ComponentList[1].Quantity);


        }
    }
}
