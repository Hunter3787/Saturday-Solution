using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoBuildApp.Services.RecommendationServices;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Models.Enumerations;
using System;

/**
 * Component scoring service tests.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Services.Tests
{
    [TestClass]
    public class ComponentScoringServiceTests
    {
        [TestMethod]
        public void ComponentScoringSerice_ScoreCompnent_ReturnIntValueOfComoponent()
        {
            // Arrange
            var gpu = new GraphicsProcUnit()
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
            double[] weights = { -1.75, 1.1, 1.3, 1.1, 1.15, -1.4 };
            var service = new ComponentScoringService();
            var expected = weights[0] * 499.98 + weights[1] * 800 +
                weights[2] * 1500 + weights[3] * 1730 + weights[4] * 16000 +
                weights[5] * 220 + 100;

            expected = Math.Round(expected, MidpointRounding.AwayFromZero); 


            // Act
            var actual = service.ScoreComponent(gpu, BuildType.Gaming);

            // Assert
            Assert.AreEqual(expected, actual);

        }
    }
}
