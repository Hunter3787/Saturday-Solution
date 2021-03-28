using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecommendationManager;

/**
 * Recommendation unit tests.
 */
namespace RecommendationManager.Tests
{
    [TestClass]
    public class RecommendationManagerTests
    {

        // Initialize test class
        [TestInitialize]
        public void Initialize()
        {

        }

        // Test the ability to save a build.
        [TestMethod]
        public void SaveBuild_ShouldThrowArguement_IfBuildIsNull()
        {
            // Arrange

            // Act

            // Assert


        }

        // Test the ability to save a single item.
        [TestMethod]
        public void TestSaveItem()
        {
            // Arrange

            // Act

            // Assert

        }

        // Test the recommend upgrade feature on a single item.
        [TestMethod]
        public void TestRecommendUpgrade()
        {
            // Arrange

            // Act

            // Assert

        }

        // Test the recommend upgrade feature on several items.
        [TestMethod]
        public void TestRecommendMultipleUpgrades()
        {
            // Arrange

            // Act

            // Assert

        }

        // Test recommendation of build with no peripherals.
        [TestMethod]
        public void TestRecommendBuild()
        {
            // Arrange

            // Act

            // Assert

        }

        // Test recommendation of build with multiple peripherals selected.
        [TestMethod]
        public void TestRecommendBuildWithPeripherals()
        {
            // Arrange

            // Act

            // Assert

        }
    }
}
