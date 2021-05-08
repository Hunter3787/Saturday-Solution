using System;
using System.Collections.Generic;
using AutoBuildApp.Api.Controllers;
using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Models.Builds;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Products;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoBuildApp.DataAccess.Test
{
    [TestClass]
    public class BuildDAOTests
    {
        private string _conString;
        private Build _emptySecondBuild;
        private Build _myBuild;
        private Component _amdCPU1BOX;
        private Component _asusMoboZ390;
        private Component _gigaGPUGV;
        private Component _cmCaseMCM;
        private Component _monPsuCentury;
        private Component _gskillRAMF4;
        private HardDrive _samSSDMZ;
        List<Build> _twoBuilds;

        public BuildDAOTests()
        {
            _conString = ConnectionManager.
            connectionManager.
            GetConnectionStringByName(ControllerGlobals.DOCKER_CONNECTION);

            _emptySecondBuild = new Build()
            {
                BuildName = "SecondBuild"
            };
            _myBuild = new Build()
            {
                BuildName = "MyBuild"
            };

            _myBuild = new Build()
            {
                BuildName = "MyBuild",
                Cpu = (CentralProcUnit)_amdCPU1BOX,
                Mobo = (Motherboard)_asusMoboZ390,
                Case = (ComputerCase)_cmCaseMCM,
                Psu = (PowerSupplyUnit)_monPsuCentury,
                Ram = (RAM)_gskillRAMF4,
                Gpu = (GraphicsProcUnit)_gigaGPUGV
            };

            _amdCPU1BOX = new CentralProcUnit
            {
                Quantity = 1,
                ManufacturerName = "AMD",
                ProductType = ProductType.CPU,
                ModelNumber = "100-100000071BOX"
            };

            _asusMoboZ390 = new Motherboard()
            {
                Quantity = 1,
                ManufacturerName = "ASUS",
                ProductType = ProductType.Motherboard,
                ModelNumber = "Z390-Plus Gaming (Wi-Fi)"
            };

            _gigaGPUGV = new GraphicsProcUnit()
            {
                Quantity = 1,
                ManufacturerName = "GIGABYTE",
                ProductType = ProductType.GPU,
                ModelNumber = "GV-N2060OC-6GD ver 2.0"
            };

            _cmCaseMCM = new ComputerCase()
            {
                Quantity = 1,
                ManufacturerName = "Cooler Master",
                ProductType = ProductType.Case,
                ModelNumber = "MCM-H500P-MGNN-S11"
            };

            _monPsuCentury = new PowerSupplyUnit()
            {
                Quantity = 1,
                ManufacturerName = "Montech",
                ProductType = ProductType.PSU,
                ModelNumber = "Century Series"
            };

            _gskillRAMF4 = new RAM()
            {
                Quantity = 1,
                ManufacturerName = "G.SKILL",
                ProductType = ProductType.RAM,
                ModelNumber = "F4 - 3600C19D - 16GVRB"
            };

            _samSSDMZ = new HardDrive()
            {
                Quantity = 1,
                ManufacturerName = "SAMSUNG",
                ProductType = ProductType.SSD,
                ModelNumber = "MZ-V7S500B/AM"
            };

            _myBuild.AddHardDrive(_samSSDMZ);

            _twoBuilds = new List<Build>()
            {
                _myBuild,
                _emptySecondBuild
            };
        }

        [TestInitialize]
        public void TestInit()
        {

        }

        [TestMethod]
        public void TestBuildEquals()
        {
            // Arrange
            var build1 = new Build()
            {
                Psu = (PowerSupplyUnit)_monPsuCentury,
                Ram = (RAM)_gskillRAMF4
            };

            var build2 = new Build()
            {
                Psu = (PowerSupplyUnit)_monPsuCentury,
                Ram = (RAM)_gskillRAMF4
            };

            // Act
            // Assert
            Assert.AreEqual(build1, build2);

        }

    }
}
