using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Builds;
using AutoBuildApp.Models.Products;
using System.Collections.Generic;
using AutoBuildApp.Services.FactoryServices;

/// <summary>
/// Recommednation Manager Unit and integration Tests
/// </summary>
namespace AutoBuildApp.Managers.Tests
{
    [TestClass]
    public class RecommendationManagerTests
    {
        private RecommendationManager _manager;
        private IBuild _build;
        private List<IBuild> _gamingBuilds;
        private NVMeDrive _hd1;
        private NVMeDrive _hd2;
        private ComputerCase _compCase;
        private Motherboard _mobo;
        private PowerSupplyUnit _psu;
        private GPU _graphics;
        private CPU _processor;
        private RAM _ram;
        private ICooler _cooler;
        private List<IComponent> _periphs;


        // Initialize test class
        [TestInitialize]
        public void Initialize()
        {
            _manager = new RecommendationManager();
            _gamingBuilds = new List<IBuild>();
            // Arrange
            IBuild gamingBuild = BuildFactory.CreateBuild(BuildType.Gaming);
            _hd1 = new NVMeDrive
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
            _hd2 = new NVMeDrive
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
            _compCase = new ComputerCase
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
            _mobo = new Motherboard
            {
                ProductType = ProductType.MotherBoard,
                ModelNumber = "133",
                ManufacturerName = "Giga",
                Price = 148.03,
                Quantity = 1,
                Socket = "1155",
                MoboForm = MoboFormFactor.ATX,
                Chipset = "IntelThing",
                MaxMemory = "128 GB",
                MaxMemoryType = MemoryType.DDR4,
                SupportedMemory = new List<string>
                {
                    "DDR4 - 3600",
                    "DDR5 - 50000"
                },
                Color = new List<string> { "black" },
                PCIEXSixTeenSlots = 3,
                PCIEXEightSlots = 1,
                PCIEXFourSlots = 1,
                PCIEXOneSlots = 0,
                PCISlots = 0,
                M2Slots = new List<string> { "M.2 bestest" },
                MSataSlots = 6,
                OnboardEthernet = new List<string> { "some etherenet connect" },
                SataSixPorts = 6,
                OnboardVideo = "There is some",
                USBTwoHeadersCount = 2,
                GenOneUSBThreeCount = 2,
                GenTwoUSBThreeCount = 4,
                GenTwoXTwoUSBThreeCount = 1,
                ErrCorrctCodeSupport = false,
                WirelessNetworking = true,
                RaidSupport = true
            };
            _psu = new PowerSupplyUnit
            {
                ProductType = ProductType.PSU,
                ModelNumber = "PSU 12",
                ProductName = "PowerSupply",
                Price = 122.97,
                Quantity = 1,
                FormFactor = "ATX 12v",
                Wattage = 1000,
                Length = 154,
                EfficiencyRating = "Gold 80+",
                Fanless = false,
                PsuType = PSUModularity.FullyModular,
                EPSConnectors = 4,
                SataConnectors = 6,
                MolexConnectors = 4,
                SixPlusTwoConnectors = 3

            };
            _graphics = new GPU
            {
                ProductType = ProductType.GPU,
                ModelNumber = "34124n",
                ManufacturerName = "This",
                Price = 499.98,
                Quantity = 2,
                Chipset = "Really good",
                Memory = "200GB",
                CoreClock = "3.4 ZGHZ",
                BoostClock = "5 ZGHZ",
                EffctvMemoryClcok = "220GB",
                Interface = "PCI-E",
                Color = "Black",
                FrameSync = "G-Sync",
                PowerDraw = 123,
                Length = 165,
                DVIPorts = 1,
                HDMIPorts = 3,
                MiniHDMIPorts = 1,
                DisplayPortPorts = 2,
                MiniDisplayPortPorts = 1,
                ExpansionSlotWidth = 10,
                Cooling = 5,
                ExternalPower = "The Sun",
            };
            _processor = new CPU
            {
                ProductType = ProductType.CPU,
                ModelNumber = "AMD3",
                ProductName = "Intel Goods",
                ManufacturerName = "Who knows",
                Price = 250,
                Quantity = 1,
                CoreCount = 16,
                CoreClock = "5.6 ghz",
                PowerDraw = 125,
                Series = "I-Beach",
                MicrorArchitecture = "SandyButt",
                CoreFamily = "Core Family",
                Socket = "1300",
                IntegratedGraphics = "It has graphics",
                MaxRam = "A lot of RAM",
                ErrCorrectionCodeSupport = "It doesn't correc errors",
                Packaging = "Very pretty",
                L1Cache = new List<string> { "L1", "Cache", "series" },
                L2Cache = new List<string> { "L2", "Cache", "series" },
                L3Cache = new List<string> { "L3", "Cache", "series" },
                Lithograph = ".5 nm",
                HyperThreading = "Hyperthreads good",
            };
            _ram = new RAM
            {
                ProductType = ProductType.RAM,
                ModelNumber = "RAM12",
                ProductName = "Good RAM",
                ManufacturerName = "RAM Maker",
                FormFactor = "128 pin",
                Color = new List<string> { "black", "red" },
                Price = 60,
                Quantity = 2,
                FirstWordLat = "10 ns",
                CASLat = "5 ns",
                Voltage = 135,
                Timing = new List<int> { 8, 8, 10, 32 },
                ErrCorrctionCode = "None",
                Registered = "Non-registered",
                HeatSpreader = true
            };
            _cooler = new Fan
            {
                ProductType = ProductType.Fan,
                ModelNumber = "Fan33",
                ProductName = "Great Fan",
                ManufacturerName = "Fan Maker",
                Price = 22,
                Quantity = 1,
                Color = new List<string> { "Black", "Silver" },
                FanRPM = "1200 RPM",
                NoiseVolume = "34 dB",
                CompatableSocket = new List<string> { "Many", "different", "Sockets" },
                Fanless = false,
                WaterCooling = "No"
            };
            _periphs = new List<IComponent>()
            {
                _hd2, _hd2
            };
            gamingBuild.AddHardDrive(_hd1);
            gamingBuild.AddHardDrive(_hd2);
            gamingBuild.Case =_compCase;
            gamingBuild.Mobo =_mobo;
            gamingBuild.Psu = _psu;
            gamingBuild.Gpu = _graphics;
            gamingBuild.Cpu = _processor;
            gamingBuild.Ram = _ram;
            gamingBuild.CPUCooler = _cooler;
            gamingBuild.Peripheral = _periphs;

        }

        #region "Build Recommendation Tests"
        // Test recommendation for a gaming pc with $1700 budget
        // requesting: no peripherals, 3 hard drives(prefering NVMe),
        // and a modular power supply.
        // Should return a List of Builds matching the criteria.
        [TestMethod]
        public void
            RecommendationManager_RecomendBuilds_ReturnListOfGamingBuilds()
        {
            // Arrange
            BuildType buildType = BuildType.Gaming;
            double budget = 1700.00;
            Dictionary<ProductType, int> productTypes = null;
            PSUModularity powerSupply = PSUModularity.FullyModular;
            HardDriveType hddType = HardDriveType.NVMe;
            int hardDriveCount = 3;

            // Act
            //List<IBuild> output = _manager.RecommendBuilds(buildType, budget,
            //    productTypes, powerSupply, hddType, hardDriveCount);

            // Assert
            //Assert.IsNotNull(output);
        }

        // Successful return of recommended build with two duplicate monitor recommendations.
        [TestMethod]
        public void
            RecommendManager_RecommendBuilds_ReturnRecommendationWithTwoDuplicateMonitors()
        {
            // Arrange
            BuildType buildType = BuildType.Gaming;
            double budget = 2500.00;
            Dictionary<ProductType, int> productTypes = new Dictionary<ProductType, int>();
            productTypes.Add(ProductType.Monitor, 2);
            PSUModularity powerSupply = PSUModularity.FullyModular;
            HardDriveType hddType = HardDriveType.NVMe;
            int hardDriveCount = 3;

            // Act
            //List<IBuild> output = _manager.RecommendBuilds(buildType, budget,
            //    productTypes, powerSupply, hddType, hardDriveCount);

            // Assert 
            //Assert.IsNotNull(output);
        }

        // No builds were able to be created. 
        [TestMethod]
        public void
            RecommendationManager_RecommendBuilds_ReturnNullWithNoSuggestions()
        {
            // Arrange
            BuildType buildType = BuildType.Gaming;
            double budget = 100.00;
            Dictionary<ProductType, int> productTypes = null;
            PSUModularity powerSupply = PSUModularity.FullyModular;
            HardDriveType hddType = HardDriveType.NVMe;
            int hardDriveCount = 3;

            // Act
            //List<IBuild> output = _manager.RecommendBuilds(buildType, budget,
            //    productTypes, powerSupply, hddType, hardDriveCount);

            // Assert
            //Assert.IsNull(output);
        }

        // No builds were able to be created. 
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void
            RecommendationManager_RecommendBuilds_ThrowsErrorForNegativeBudget()
        {
            // Arrange
            BuildType buildType = BuildType.Gaming;
            double budget = -100.00;
            Dictionary<ProductType, int> productTypes = null;
            PSUModularity powerSupply = PSUModularity.None;
            HardDriveType hddType = HardDriveType.None;
            int hardDriveCount = 0;

            // Act
            //List<IBuild> output = _manager.RecommendBuilds(buildType, budget,
            //    productTypes, powerSupply, hddType, hardDriveCount);

            // Assert - Throws Argument out of Range, should be handled in Controller.
        }

        // No build recommendedations could be found within budget.
        [TestMethod]
        public void
            RecommendationManager_RecommendBuildsWithPeripherals_ReturnNullWithNoSuggestions()
        {
            // Arrange

            // Act

            // Assert

        }

        // RecommendBuild passed an invalid value. 
        [TestMethod]
        public void RecommendationManager_RecommendBuilds_ThrowError()
        {
            // Arrange

            // Act

            // Assert

        }

        // Build speed test.
        [TestMethod]
        public void RecommendationManager_RecommendBuilds_RecommendationCompletesUnder5Seconds()
        {
            // Arrange

            // Act

            // Assert

        }
        #endregion

        #region "Upgrade Recommendation Tests"
        // Return list of components that will be an upgrade for the 
        // passed component.
        [TestMethod]
        public void RecommendationMnaager_RecommendUpgrades_ReturnUpgradeList()
        {
            // Arrange

            // Act

            // Assert

        }

        // No recommendatoins available for the component.
        [TestMethod]
        public void RecommendationManager_RecommendUpgrades_ReturnNull()
        {
            // Arrange

            // Act

            // Assert

        }

        // 
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RecommendationManger_RecommendUpgrades_ThrowsException()
        {

        }
        #endregion

        #region "Private Methods Tests"
        [TestMethod]
        public void RecommendationManager_CreateComponentList_ReturnsComponentListParsedFromBuild()
        {
            // Arrange
            IBuild build = BuildFactory.CreateBuild(BuildType.Gaming);
            var buildList = new List<IComponent> {

            };
            var expected = null;
            // Act
            var actual = _manager.CreateComponentList(build);

            // Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void RecommendationManager_RemoveOverBudgetItems_ReturnsCroppedListOfComponents()
        {
            // Arrange

            // Act

            // Assert

        }

        [TestMethod]
        public void RecommendationManager_ScoreComponent_ScoresComponent()
        {
            // Arrange

            // Act

            // Assert

        }

        [TestMethod]
        public void RecommendationManager_CompareBuidls_ReturnsHigherScoredBuild()
        {
            // Arrange

            // Act

            // Assert

        }
        #endregion

        #region "Request Saved Builds Tests(Out of Scope)"
        // Returns builds associated with the current logged in account, if any. 
        [TestMethod]
        public void RecommendationManager_RecommendBuilds_ReturnSavedBuilds()
        {
            // Arrange

            // Act

            // Assert

        }


        // Attempts to return builds without permission or a logged in user. 
        [TestMethod]
        public void RecommendationManager_RecommendBuilds_ThrowsPermissionError()
        {
            // Arrange

            // Act

            // Assert

        }


        // Returns no builds. 
        [TestMethod]
        public void RecommendationManager_RecommendBuilds_()
        {
            // Arrange

            // Act

            // Assert

        }
        #endregion

    }
}
