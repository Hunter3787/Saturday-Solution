using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Enumerations;

/// <summary>
/// Utilizes the Managers namespace for the Auto-build app. 
/// </summary>
namespace AutoBuildApp.Managers
{
    /// <summary>
    /// Class that functions as the manager to the recommendation tool
    /// for the AutoBuildApp.
    /// </summary>
    public class RecommendationManager
    {
        // Local constant for minimum budget.
        private const double MIN_BUDGET = 0.0;

        private double _budget;
        private BuildType _buildType;


        #region "Constructors"
        /// <summary>
        /// RecommendationManager default constructor.
        /// </summary>
        public RecommendationManager()
        {
            // Initialize budget to 0.
            _budget = 0;
            // Initialize buildType to none.
            _buildType = BuildType.None;

        }

        /// <summary>
        /// Constructor that takes in a BuildType(Enumeration)
        /// and a budget amount(double). 
        /// </summary>
        /// <param name="buildType"></param>
        /// <param name="budget"></param>
        /** Speculating this is not necessary */
        public RecommendationManager(BuildType buildType, double budget)
        {
            // If budget is below 0 set to 0.
            if (budget < MIN_BUDGET)
                _budget = MIN_BUDGET;
            // If budget is positive or 0 set it. 
            else
                _budget = budget;
            // Assign Enum BuildType to private variable.
            _buildType = buildType;
        }
        #endregion

        /// <summary>
        /// Attempts to return a build recommendation using only a budget and
        /// BuildType enumeration. If no budget is present the budget will be
        /// unlimited. 
        /// </summary>
        /// <param name="buildType"></param>
        /// <param name="budget"></param>
        /// <returns></returns>
        public Dictionary<int, List<IComponent>>
            RecommendBuilds(BuildType buildType, double budget)
        {
            return null;
        }


        public Dictionary<int, List<IComponent>>
            RecommendBuilds(BuildType buildType, double budget,
                List<ProductType> peripherals, PowerSupplyType psuType,
                    HardDriveType hddType, int hddCount)
        {
            return null;
        }

        public List<IComponent> RecommendUpgrade()
        {
            return null;
        }

        public Dictionary<int,List<IComponent>> RecommendUpgrades()
        {
            return null;
        }


        public bool SaveRecommendation()
        {
            return false;
        }


        public List<IComponent> RequestComponentsByType()
        {
            return null;
        }

        public void RequestSavedBuilds()
        {

        }

        #region "Private Methods"
        private void GenerateBuildKey()
        {

        }

        private void GenerateComponentList()
        {

        }

        private void BudgetCompopnents()
        {

        }

        private void RemoveOverBudgetItems()
        {

        }

        private void ScoreComponent()
        {

        }

        private void CompareBuilds()
        {

        }
        #endregion
    }
}
