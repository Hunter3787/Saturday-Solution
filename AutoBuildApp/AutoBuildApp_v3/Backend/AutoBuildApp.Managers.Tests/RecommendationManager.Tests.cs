using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoBuildApp.Managers;

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

        #region "Build Recommendation Tests"
        // Test recommendation of build with no peripherals.
        [TestMethod]
        public void
            RecommendationManager_RecomendBuilds_ReturnDictionaryOfIComponentLists()
        {
            // Arrange

            // Act

            // Assert

        }

        // No builds were able to be created. 
        [TestMethod]
        public void
            RecommendationManager_RecommendBuilds_ReturnNullWithNoSuggestions()
        {
            // Arrange

            // Act

            // Assert

        }

        // Successful return of recommended build with two duplicate monitor recommendations.
        [TestMethod]
        public void
            RecommendManager_RecommendBuilds_ReturnRecommendationWithTwoDuplicateMonitors()
        {
            // Arrange

            // Act

            // Assert

        }

        // No builds could be recommendedations could be found within budget.
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
        #endregion

        #region "Save Recommendaiton Tests"
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
        #endregion

        #region "Request Component Tests"
        [TestMethod]
        public void RecommendationManager_RequestComponentByType()
        {
            // Arrange

            // Act

            // Assert

        }
        #endregion

        #region "Request Saved Builds Tests"
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

        #region "GenerateBuildKey Tests"
        [TestMethod]
        public void RecommendationManager_GenerateBuildKey_ReturnsBuildKeyStructure()
        {
            // Arrange

            // Act

            // Assert

        }
        #endregion
    }
}
