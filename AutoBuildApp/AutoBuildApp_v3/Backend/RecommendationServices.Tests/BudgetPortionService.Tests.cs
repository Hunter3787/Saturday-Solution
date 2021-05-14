using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoBuildApp.Services.FactoryServices;
using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Builds;

/**
 * Tests for the BudgetPortionService.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Services.RecommendationServices.Tests
{
    [TestClass]
    public class BudgetPortionServiceTests
    {
        private NVMeDrive _hd1;
        private NVMeDrive _hd2;
        private ComputerCase _compCase;
        private Motherboard _mobo;
        private PowerSupplyUnit _psu;
        private GraphicsProcUnit _graphics;
        private CentralProcUnit _processor;
        private RAM _ram;
        private ICooler _cooler;
        private List<IComponent> _periphs;
        private IBuild _gamingBuild;
        private BuildFactory _build;

        // Initialize test class
        [TestInitialize]
        public void Initialize()
        {
            _build = new BuildFactory();
            _gamingBuild = (IBuild)_build.CreateBuild(BuildType.Gaming);
            _hd1 = new NVMeDrive
            {
                DriveType = HardDriveType.NVMe,
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
                DriveType = HardDriveType.NVMe,
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
                new List<string>() {
                    MoboFormFactor.ATX.ToString(), MoboFormFactor.MiniATX.ToString() },
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
                ProductType = ProductType.Motherboard,
                ModelNumber = "133",
                ManufacturerName = "Giga",
                Price = 148.03,
                Quantity = 1,
                Socket = "1155",
                MoboForm = MoboFormFactor.ATX.ToString(),
                Chipset = "IntelThing",
                MaxMemory = "128 GB",
                MaxMemoryType = MemoryType.DDR4,
                SupportedMemory = new List<string>
            {
                "DDR4 - 3600",
                "DDR5 - 50000"
            },
                Colors = new List<string> { "black" },
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
                PsuModulartiy = PSUModularity.FullyModular,
                EPSConnectors = 4,
                SataConnectors = 6,
                MolexConnectors = 4,
                SixPlusTwoConnectors = 3

            };
            _graphics = new GraphicsProcUnit
            {
                ProductType = ProductType.GPU,
                ModelNumber = "34124n",
                ManufacturerName = "This",
                Price = 499.98,
                Quantity = 2,
                Chipset = "GeForce RTX 3070",
                Memory = "8 GB",
                MemoryType = "GDDR 6",
                CoreClock = "1500 MHz",
                BoostClock = "1730 MHz",
                EffectiveMemClock = "16000 MH",
                Interface = "PCIe x16",
                Color = "Black",
                FrameSync = "G-Sync",
                PowerDraw = 220,
                Length = 242,
                DVIPorts = 0,
                HDMIPorts = 1,
                MiniHDMIPorts = 0,
                DisplayPortPorts = 3,
                MiniDisplayPortPorts = 0,
                ExpansionSlotWidth = 2,
                Cooling = "2",
                ExternalPower = "1 PCIe 12-pin"
            };
            _processor = new CentralProcUnit
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
                ErrCorrectionCodeSupport = false,
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
                Colors = new List<string> { "Black", "Silver" },
                FanRPM = "1200 RPM",
                NoiseVolume = "34 dB",
                CompatableSockets = new List<string> { "Many", "different", "Sockets" },
                Fanless = false,
                WaterCooling = false
            };
            _periphs = new List<IComponent>()
        {
            _hd2, _hd2
        };
            _gamingBuild.AddHardDrive(_hd1);
            _gamingBuild.AddHardDrive(_hd2);
            _gamingBuild.Case = _compCase;
            _gamingBuild.Mobo = _mobo;
            _gamingBuild.Psu = _psu;
            _gamingBuild.Gpu = _graphics;
            _gamingBuild.Cpu = _processor;
            _gamingBuild.Ram = _ram;
            _gamingBuild.CPUCooler = _cooler;
            _gamingBuild.Peripherals = _periphs;

        }


        [TestMethod]
        public void BudgetPortionService_BudgetComponents_ReturnAListOfBudgetedIComponents()
        {
            // Arrange
            var list = new List<IComponent>
            {
                _graphics, _processor
            };
            double budget = 1700;
            BuildType type = BuildType.Gaming;
            double expected = 1700 * .35/(.35+.25);
            expected = Math.Round(expected, 2, MidpointRounding.AwayFromZero);
            PortionBudgetService portion = new PortionBudgetService();

            // Act
            var result = portion.PortionOutBudget(list, type, budget);

            double actual = 0;
            foreach (var add in result)
            {
                if(add == _graphics)
                    actual += add.Budget;
            }
            actual = Math.Round(actual, 2, MidpointRounding.AwayFromZero);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
