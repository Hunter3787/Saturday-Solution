﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        private string _conString;
        private Component _testComponent;
        private Component _dupeTestComponent;
        private Shelf _emptyShelf;
        private Shelf _shelfWithTwoCPU;
        private List<Shelf> _shelfList;


        public ShelfDAOTests()
        {
        }

        [TestInitialize]
        public void TestInit()
        {
            _conString = ConnectionManager.
            connectionManager.
            GetConnectionStringByName(ControllerGlobals.DOCKER_CONNECTION);

            _testComponent = new Component()
            {
                Quantity = 2,
                ProductType = ProductType.CPU,
                ModelNumber = "100-100000063WOF",
                ManufacturerName = "AMD"
            };

            _dupeTestComponent = new Component()
            {
                Quantity = 2,
                ProductType = ProductType.CPU,
                ModelNumber = "100-100000063WOF",
                ManufacturerName = "AMD"
            };

            _emptyShelf = new Shelf()
            {
                ShelfName = "McDonalds"
            };

            _shelfWithTwoCPU = new Shelf()
            {
                ShelfName = "TacoBell",
                ComponentList = new List<Component>()
                    {
                        new Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        }
                    }
            };

            _shelfList = new List<Shelf>()
            {
                _emptyShelf,
                _shelfWithTwoCPU
            };

        }

        [TestMethod]
        public void TestComponentEqualsOverride()
        {
            // Arrange
            var comp1 = new Component()
            {
                Quantity = 2,
                ProductType = ProductType.CPU,
                ModelNumber = "100-100000063WOF",
                ManufacturerName = "AMD"
            };

            var comp2 = new Component()
            {
                Quantity = 2,
                ProductType = ProductType.CPU,
                ModelNumber = "100-100000063WOF",
                ManufacturerName = "AMD"
            };

            // Act

            // Assert
            Assert.AreEqual(comp1, comp2);
        }

        [TestMethod]
        public void TestShelfEqualsOverride()
        {
            // Arrange
            List<Shelf> shelvesList1 = new List<Shelf>()
            {
                new Shelf()
                {
                    ShelfName = "McDonalds",
                },
                new Shelf()
                {
                    ShelfName = "TacoBell",
                    ComponentList = new List<Component>()
                    {
                        new Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        }
                    }
                }
            };

            List<Shelf> shelvesList2 = new List<Shelf>()
            {
                new Shelf()
                {
                    ShelfName = "McDonalds",
                },
                new Shelf()
                {
                    ShelfName = "TacoBell",
                    ComponentList = new List<Component>()
                    {
                        new Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        }
                    }
                }
            };

            // Act

            // Assert
            CollectionAssert.AreEqual(shelvesList1, shelvesList2);
        }

        [TestMethod]
        public void ShelfEqualsTest()
        {
            var mc1 = new Shelf()
            {
                ShelfName = "McDonalds",
                ComponentList = new List<Component>()
                    {
                        new Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        }
                    }

            };
            var mc2 = new Shelf()
            {
                ShelfName = "McDonalds",
                ComponentList = new List<Component>()
                    {
                        new Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        }
                    }
            };

            Assert.IsTrue(mc1.Equals(mc2));
        }

        [TestMethod]
        public void ShelfDAO_GetAllShelvesByUser_ReturnTwoShelves()
        {
            // Arrange
            var expected = new SystemCodeWithCollection<List<Shelf>>();
            ShelfDAO shelfDAO = new ShelfDAO(_conString);
            var username = "Zeinab";

            List<Shelf> shelves = new List<Shelf>()
            {
                new Shelf()
                {
                    ShelfName = "McDonalds",
                },
                new Shelf()
                {
                    ShelfName = "TacoBell",
                    ComponentList = new List<Component>()
                    {
                        new Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        }
                    }
                }
            };
            expected.Code = AutoBuildSystemCodes.Success;
            expected.GenericCollection = shelves;
            var expectedList = expected.GenericCollection;


            // Act
            var actual = shelfDAO.GetAllShelvesByUser(username);
            var actualList = actual.GenericCollection;

            // Assert
            Assert.AreEqual(expected.Code, actual.Code);
            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void ShelfDAO_GetShelfByName_ReturnTwoObjects()
        {

            // Arrange
            var expected = new SystemCodeWithCollection<Shelf>();
            ShelfDAO shelfDAO = new ShelfDAO(_conString);
            var username = "Zeinab";
            var shelfName = "TacoBell";
            expected.GenericCollection = _shelfWithTwoCPU;
            expected.Code = AutoBuildSystemCodes.Success;
            var expectedShelf = expected.GenericCollection;

            // Act
            var actual = shelfDAO.GetShelfByName(shelfName, username);
            var actualShelf = actual.GenericCollection;

            // Assert
            Assert.AreEqual(expected.Code, actual.Code);
            Assert.AreEqual(expectedShelf, actualShelf);
        }

        [TestMethod]
        public void ShelfDAO_GetComponentByID_ReturnSingleComponent()
        {

            // Arrange
            var expected = new SystemCodeWithCollection<Shelf>();
            ShelfDAO shelfDAO = new ShelfDAO(_conString);
            var username = "Zeinab";
            var shelfName = "TacoBell";
            expected.GenericCollection = _shelfWithTwoCPU;
            expected.Code = AutoBuildSystemCodes.Success;
            var expectedShelf = expected.GenericCollection;

            // Act
            var actual = shelfDAO.GetShelfByName(shelfName, username);
            var actualShelf = actual.GenericCollection;

            // Assert
            Assert.AreEqual(expected.Code, actual.Code);
            Assert.AreEqual(expectedShelf, actualShelf);
        }


    }
}
