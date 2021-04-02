using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoBuildApp.Models.Builds;
using AutoBuildApp.Services;
using AutoBuildApp.Models.Interfaces;
using System.Collections.Generic;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Models.Enumerations;

namespace AutoBuildApp.Models.Tests
{
    [TestClass]
    public class BuildsTests
    {

        [TestInitialize]
        public void Initialize()
        {

        }


        [TestMethod]
        public void Gaming_GetTotalCost_TotalCostOfBuild()
        {
            // Arrange
            IBuild gamingBuild = BuildFactory.CreateBuild(BuildType.Gaming);
            NVMeDrive hd1 = new NVMeDrive
            {
                HardDrive = HardDriveType.NVMe,
                ProductType = ProductType.HDD,
                ModelNumber = "1234",
                ProductName = "HardDrive1",
                ManufacturerName = "Sony",
                Quantity = 2,
                Price = 65.23,
                Capacity = "256 GB",
                Cache = "10-10-11-34",
                FormFactor = "M2 244",
                Interface = "An interface",
                NVMe = true
            };
            NVMeDrive hd2 = new NVMeDrive
            {
                HardDrive = HardDriveType.NVMe,
                ProductType = ProductType.HDD,
                ModelNumber = "1234-2",
                ProductName = "HardDrive2",
                ManufacturerName = "Sony",
                Quantity = 1,
                Price = 65,
                Capacity = "128 GB",
                Cache = "10-10-11-34",
                FormFactor = "M2 244",
                Interface = "An interface",
                NVMe = true
            };
            ComputerCase compCase = new ComputerCase
            {
                ProductType = ProductType.Case,
                ModelNumber = "1544",
                ProductName = "Case1",
                ManufacturerName = "Man2",
                Quantity = 1,
                Price = 85.99,
                MoboFormSupport =
                new List<MoboFormFactor>() {
                    MoboFormFactor.ATX, MoboFormFactor.MiniATX },
                TowerType = TowerType.FullTower,
                ExpansionSlots = 3,
                PsuShroud = false,
                SidePanel = "Glass",
                Color = new List<string>() { "Black", "White" },
                ThreeInchDriveBays = 2,
                TwoInchDriveBays = 0,
                FrontPanel = new List<string>() { "USB 3.2", "SATA-E" },
                MaxGPULength = 184,
                Dimensions = new List<double>() { 144, 126, 345 }
            };
            Motherboard mobo = new Motherboard
            {
                Price = 148.03,
                Quantity = 1
            };
            PowerSupplyUnit psu = new PowerSupplyUnit
            {
                Price = 122.97,
                Quantity = 1
            };
            GPU graphics = new GPU
            {
                Price = 499.98,
                Quantity = 2
            };
            CPU processor = new CPU
            {
                Price = 250,
                Quantity = 1
            };
            RAM ram = new RAM
            {
                Price = 60,
                Quantity = 2
            };
            Fan cooler = new Fan
            {
                Price = 22,
                Quantity = 1
            };
            List<IComponent> periphs = new List<IComponent>()
            {

            };
            List<IHardDrive> hdd = new List<IHardDrive>() { hd1, hd2 };
            var expected = 1384.63;


            // Act
            double actual = gamingBuild.GetTotalCost();

            // Assert
            Assert.AreEqual(expected, actual);

        }
    }
}
