using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Builds;
using System.Collections.Generic;

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

        // Initialize test class
        [TestInitialize]
        public void Initialize()
        {
            _manager = new RecommendationManager();
            _gamingBuilds = new List<IBuild>();
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
            PowerSupplyType powerSupply = PowerSupplyType.Modular;
            HardDriveType hddType = HardDriveType.NVMe;
            int hardDriveCount = 3;
            
            // Act
            List<IBuild> output = _manager.RecommendBuilds(buildType, budget,
                productTypes, powerSupply, hddType, hardDriveCount);

            // Assert
            Assert.IsNotNull(output);
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
            PowerSupplyType powerSupply = PowerSupplyType.Modular;
            HardDriveType hddType = HardDriveType.NVMe;
            int hardDriveCount = 3;

            // Act
            List<IBuild> output = _manager.RecommendBuilds(buildType, budget,
                productTypes, powerSupply, hddType, hardDriveCount);

            // Assert 
            Assert.IsNotNull(output);
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
            PowerSupplyType powerSupply = PowerSupplyType.Modular;
            HardDriveType hddType = HardDriveType.NVMe;
            int hardDriveCount = 3;

            // Act
            List<IBuild> output = _manager.RecommendBuilds(buildType, budget,
                productTypes, powerSupply, hddType, hardDriveCount);

            // Assert
            Assert.IsNull(output);
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
            PowerSupplyType powerSupply = PowerSupplyType.None;
            HardDriveType hddType = HardDriveType.None;
            int hardDriveCount = 0;

            // Act
            List<IBuild> output = _manager.RecommendBuilds(buildType, budget,
                productTypes, powerSupply, hddType, hardDriveCount);

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
        public void RecommendationManager_GenerateBuildKey_ReturnsBuildKeyOfValues()
        {
            // Arrange

            // Act

            // Assert

        }

        [TestMethod]
        public void RecommendationManager_GenerateComponentList_ReturnsEmptyComponentList()
        {
            // Arrange

            // Act

            // Assert

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
