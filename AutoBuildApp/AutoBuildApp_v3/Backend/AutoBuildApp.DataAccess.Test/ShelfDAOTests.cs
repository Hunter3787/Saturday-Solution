﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using AutoBuildApp.Api.Controllers;
using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Models;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
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
        private Component _intelCPU700K;
        private Component _intelCPU700KA;
        private Component _amdCPU3W0F;
        private Component _amdCPU1BOX;
        private readonly ClaimsPrincipal _claimsPrincipal;


        public ShelfDAOTests()
        {
            _conString = ConnectionManager.
            connectionManager.
            GetConnectionStringByName(ControllerGlobals.DOCKER_CONNECTION);

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

            _amdCPU3W0F = new Component()
            {
                Quantity = 1,
                ProductType = ProductType.CPU,
                ModelNumber = "100-100000063WOF",
                ManufacturerName = "AMD"
            };

            _amdCPU1BOX = new Component()
            {
                Quantity = 1,
                ProductType = ProductType.CPU,
                ModelNumber = "100-100000071BOX",
                ManufacturerName = "AMD"
            };

            _intelCPU700K = new Component()
            {
                Quantity = 1,
                ProductType = ProductType.CPU,
                ModelNumber = "BX8070110700K",
                ManufacturerName = "Intel"
            };

            _intelCPU700KA = new Component()
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

        [TestInitialize]
        public void TestInit()
        {
            
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
            string _conString = ConnectionManager.
          connectionManager.
          GetConnectionStringByName(ControllerGlobals.LOCALHOST_CONNECTION);

            // Arrange
            var expected = new SystemCodeWithObject<List<Shelf>>();
            ShelfDAO shelfDAO = new ShelfDAO(_conString);
            var username = "Zeina";

            List<Shelf> shelves = new List<Shelf>()
            {
                new Shelf()
                {
                   // ShelfName = "McDonalds",
                    ShelfName = "ZeeMyShelf",
                },
                new Shelf()
                {
                    ShelfName = "TacoBell",
                    ComponentList = new List<Component>()
                    {
                        new Component()
                        {
                            Quantity = 1,
                            ProductType = ProductType.CPU,
                            ModelNumber = "BX8070110700K",
                            ManufacturerName = "Intel"
                        },
                        new Component()
                        {
                            Quantity = 1,
                            ProductType = ProductType.CPU,
                            ModelNumber = "100-100000063WOF",
                            ManufacturerName = "AMD"
                        },
                        new Component()
                        {
                            Quantity = 1,
                            ProductType = ProductType.CPU,
                            ModelNumber = "BX8070110700KA",
                            ManufacturerName = "Intel"
                        },
                        new Component()
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
            var actual = shelfDAO.GetShelvesByUser(username);
            var actualList = actual.GenericObject;

            // Assert
            Assert.AreEqual(expected.Code, actual.Code);
            //CollectionAssert.AreEqual(expectedList, actualList);
        }

        
        [TestMethod]
        public void ShelfDAO_GetShelfByName_ReturnFourObjects()
        {
            string _conString = ConnectionManager.
           connectionManager.
           GetConnectionStringByName(ControllerGlobals.LOCALHOST_CONNECTION);

            // Arrange
            var expected = new SystemCodeWithObject<Shelf>();
            ShelfDAO shelfDAO = new ShelfDAO(_conString);
            var username = "Zeina";
            var shelfName = "ZeeMyShelf";
            //var shelfName = "TacoBell";
            expected.GenericObject = new Shelf()
            {
                ShelfName = shelfName,
                ComponentList = new List<Component>()
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

            Console.WriteLine($"in the tester : {actual.Code}");

            // Assert
            Assert.AreEqual(expected.Code, actual.Code);
            //Assert.AreEqual(expectedShelf, actualShelf);
        }

        [TestMethod]
        public void ShelfDAO_GetShelfByName_ReturnEmptyShelf()
        {

            string  _conString = ConnectionManager.
            connectionManager.
            GetConnectionStringByName(ControllerGlobals.LOCALHOST_CONNECTION);
            // Arrange
            var expected = new SystemCodeWithObject<Shelf>();
            ShelfDAO dao = new ShelfDAO(_conString);
            expected.GenericObject = new Shelf();
            expected.Code = AutoBuildSystemCodes.NoEntryFound;
            var username = "Zeina";
            string shelfName = "ZeeMyShelf";

            //Act
            var actual = dao.GetShelfByName(shelfName, username);

            Console.WriteLine(actual.Code.ToString());
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
