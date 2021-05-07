using System;
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
        private Models.Products.Component _testComponent;
        private Models.Products.Component _dupeTestComponent;
        private Shelf _emptyShelf;
        private Shelf _shelfWithTwoCPU;
        private List<Shelf> _shelfList;
        private Models.Products.Component _intelCPU700K;
        private Models.Products.Component _intelCPU700KA;
        private Models.Products.Component _amdCPU3W0F;
        private Models.Products.Component _amdCPU1BOX;


        public ShelfDAOTests()
        {
            _conString = ConnectionManager.
            connectionManager.
            GetConnectionStringByName(ControllerGlobals.DOCKER_CONNECTION);

            _amdCPU3W0F = new Models.Products.Component()
            {
                Quantity = 1,
                ProductType = ProductType.CPU,
                ModelNumber = "100-100000063WOF",
                ManufacturerName = "AMD"
            };

            _amdCPU1BOX = new Models.Products.Component()
            {
                Quantity = 1,
                ProductType = ProductType.CPU,
                ModelNumber = "100-100000071BOX",
                ManufacturerName = "AMD"
            };

            _intelCPU700K = new Models.Products.Component()
            {
                Quantity = 1,
                ProductType = ProductType.CPU,
                ModelNumber = "BX8070110700K",
                ManufacturerName = "Intel"
            };

            _intelCPU700KA = new Models.Products.Component()
            {
                Quantity = 1,
                ProductType = ProductType.CPU,
                ModelNumber = "BX8070110700KA",
                ManufacturerName = "Intel"
            };

            _emptyShelf = new Shelf()
            {
                ShelfName = "McDonalds"
            };

            _shelfWithTwoCPU = new Shelf()
            {
                ShelfName = "TacoBell",
                ComponentList = new List<Models.Products.Component>()
                    {
                        new Models.Products.Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Models.Products.Component()
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

        [TestInitialize]
        public void TestInit()
        {
            
        }

        [TestMethod]
        public void TestComponentEqualsOverride()
        {
            // Arrange
            var comp1 = new Models.Products.Component()
            {
                Quantity = 2,
                ProductType = ProductType.CPU,
                ModelNumber = "100-100000063WOF",
                ManufacturerName = "AMD"
            };

            var comp2 = new Models.Products.Component()
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
                    ComponentList = new List<Models.Products.Component>()
                    {
                        new Models.Products.Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Models.Products.Component()
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
                    ComponentList = new List<Models.Products.Component>()
                    {
                        new Models.Products.Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Models.Products.Component()
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
                ComponentList = new List<Models.Products.Component>()
                    {
                        new Models.Products.Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Models.Products.Component()
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
                ComponentList = new List<Models.Products.Component>()
                    {
                        new Models.Products.Component()
                        {
                            Quantity = 2,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Models.Products.Component()
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
            var expected = new SystemCodeWithObject<List<Shelf>>();
            ShelfDAO shelfDAO = new ShelfDAO(_conString);
            var username = "Zeina";

            List<Shelf> shelves = new List<Shelf>()
            {
                new Shelf()
                {
                    ShelfName = "McDonalds",
                },
                new Shelf()
                {
                    ShelfName = "TacoBell",
                    ComponentList = new List<Models.Products.Component>()
                    {
                        new Models.Products.Component()
                        {
                            Quantity = 1,
                            ProductType = ProductType.CPU,
                            ModelNumber = "BX8070110700K",
                            ManufacturerName = "Intel"
                        },
                        new Models.Products.Component()
                        {
                            Quantity = 1,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Models.Products.Component()
                        {
                            Quantity = 1,
                            ProductType = ProductType.CPU,
                            ModelNumber = "BX8070110700KA",
                            ManufacturerName = "Intel"
                        },
                        new Models.Products.Component()
                        {
                            Quantity = 1,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000071BOX",
                            ManufacturerName = "AMD"
                        }
                    }
                }
            };
            expected.Code = AutoBuildSystemCodes.Success;
            expected.GenericObject = shelves;
            var expectedList = expected.GenericObject;


            // Act
            var actual = shelfDAO.GetAllShelvesByUser(username);
            var actualList = actual.GenericObject;

            // Assert
            Assert.AreEqual(expected.Code, actual.Code);
            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void ShelfDAO_GetShelfByName_ReturnFourObjects()
        {

            // Arrange
            var expected = new SystemCodeWithObject<Shelf>();
            ShelfDAO shelfDAO = new ShelfDAO(_conString);
            var username = "Zeina";
            var shelfName = "TacoBell";
            expected.GenericObject = new Shelf()
            {
                ShelfName = shelfName,
                ComponentList = new List<Models.Products.Component>()
                {
                    _intelCPU700K,
                    _amdCPU3W0F,
                    _intelCPU700KA,
                    _amdCPU1BOX
                }
            };
            expected.Code = AutoBuildSystemCodes.Success;
            var expectedShelf = expected.GenericObject;

            // Act
            var actual = shelfDAO.GetShelfByName(shelfName, username);
            var actualShelf = actual.GenericObject;

            // Assert
            Assert.AreEqual(expected.Code, actual.Code);
            Assert.AreEqual(expectedShelf, actualShelf);
        }

        [TestMethod]
        public void ShelfDAO_GetShelfByName_ReturnEmptyShelf()
        {
            // Arrange
            var expected = new SystemCodeWithObject<Shelf>();
            ShelfDAO dao = new ShelfDAO(_conString);
            expected.GenericObject = new Shelf()
            {
                ShelfName = "McDonalds"
            };
            expected.Code = AutoBuildSystemCodes.Success;
            var username = "Zeina";
            string shelfName = "McDonalds";

            //Act
            var actual = dao.GetShelfByName(shelfName, username);

            // Assert
            Assert.AreEqual(expected.GenericObject, actual.GenericObject);
            Assert.AreEqual(expected.Code, actual.Code);
        }

        // Out of scope
        //[TestMethod]
        //public void ShelfDAO_GetComponentByID_ReturnSingleComponent()
        //{

        //    // Arrange
        //    var expected = new SystemCodeWithObject<Shelf>();
        //    ShelfDAO shelfDAO = new ShelfDAO(_conString);
        //    var username = "Zeinab";
        //    var shelfName = "TacoBell";
        //    expected.GenericObject = _shelfWithTwoCPU;
        //    expected.Code = AutoBuildSystemCodes.Success;
        //    var expectedShelf = expected.GenericObject;

        //    // Act
        //    var actual = shelfDAO.GetShelfByName(shelfName, username);
        //    var actualShelf = actual.GenericObject;

        //    // Assert
        //    Assert.AreEqual(expected.Code, actual.Code);
        //    Assert.AreEqual(expectedShelf, actualShelf);
        //}

        [TestMethod]
        public void ShelfDAO_InsertShelf_ReturnTrueAndSuccessMessage()
        {
            // Arrange

            // Act

            // Assert

        }

        [TestMethod]
        public void ShelfDAO_()
        {

        }

        [TestMethod]
        public void ShelfDAO_UpdateShelf_ReturnTrueAndSuccessMessage()
        {
            // Arrange

            // Act

            // Assert

        }


        [TestMethod]
        public void ShelfDAO_AddComponent_ReturnTrueAndSuccessMessage()
        {
            // Arrange

            // Act

            // Assert

        }

        [TestMethod]
        public void ShelfDAO_RemoveComponent_ReturnTrueAndSuccessMessage()
        {
            // Arrange

            // Act

            // Assert

        }

    }
}
