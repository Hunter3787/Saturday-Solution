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
        private Component _samSSDMZ;
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

            //_myBuild = new Build()
            //{
            //    BuildName = "MyBuild",
            //    Cpu = _amdCPU1BOX,
            //    Mobo = _asusMoboZ390,
            //    Case = _cmCaseMCM,
            //    Psu = _monPsuCentury,
            //    Ram = _gskillRAMF4,
            //    Gpu = _gigaGPUGV
            //};
            _myBuild.AddHardDrive(_samSSDMZ);

            _amdCPU1BOX = new Component
            {
                Quantity = 1,
                ManufacturerName = "AMD",
                ProductType = ProductType.CPU,
                ModelNumber = "100-100000071BOX"
            };

            _asusMoboZ390 = new Component()
            {
                Quantity = 1,
                ManufacturerName = "ASUS",
                ProductType = ProductType.Motherboard,
                ModelNumber = "Z390-Plus Gaming (Wi-Fi)"
            };

            _gigaGPUGV = new Component()
            {
                Quantity = 1,
                ManufacturerName = "GIGABYTE",
                ProductType = ProductType.GPU,
                ModelNumber = "GV-N2060OC-6GD ver 2.0"
            };

            _cmCaseMCM = new Component()
            {
                Quantity = 1,
                ManufacturerName = "Cooler Master",
                ProductType = ProductType.Case,
                ModelNumber = "MCM-H500P-MGNN-S11"
            };

            _monPsuCentury = new Component()
            {
                Quantity = 1,
                ManufacturerName = "Montech",
                ProductType = ProductType.PSU,
                ModelNumber = "Century Series"
            };

            _gskillRAMF4 = new Component()
            {
                Quantity = 1,
                ManufacturerName = "G.SKILL",
                ProductType = ProductType.RAM,
                ModelNumber = "F4 - 3600C19D - 16GVRB"
            };

            _samSSDMZ = new Component()
            {
                Quantity = 1,
                ManufacturerName = "SAMSUNG",
                ProductType = ProductType.SSD,
                ModelNumber = "MZ-V7S500B/AM"
            };

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
