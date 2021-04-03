using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Services.FactoryServices;
using AutoBuildApp.Services.RecommendationServices;

/**
 * Recommendation Manager includes business logic
 * and directs the recommendation process.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Managers
{
    /// <summary>
    /// Class that functions as the manager to the recommendation tool
    /// for the AutoBuildApp.
    /// </summary>
    public class RecommendationManager : ArgumentOutOfRangeException
    {
        // Local constant for minimum budget.
        public readonly double MAX_BUDGET = double.MaxValue;
        public readonly double MIN_BUDGET = 0.0;
        public readonly int MIN_INDEX = 0;
        public readonly int MIN_INTEGER_VALUE = 0;

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
            else
                _budget = budget;

            // Assign Enum BuildType to private variable.
            _buildType = buildType;
        }
        #endregion

        #region "Build Recommendations"
        /// <summary>
        /// Recommend a computer build based off user defined parameters.
        /// </summary>
        /// <param name="buildType">Type of Build based on Enum.</param>
        /// <param name="principal">Double value representing the budget.</param>
        /// <param name="peripherals">Dictionary of product types and how many
        /// of each would be requested.(Optional)</param>
        /// <param name="psuType">Power supply unit type requested by
        /// the user.(Optional)</param>
        /// <param name="hddType">Hard drive type
        /// prioritized by user. (Optional)</param>
        /// <param name="hddCount">Number of hard drives that the user
        /// would like to be in their final build.(Optional)</param>
        /// <returns>A list of IBuild representing the recommended builds.</returns>
        public List<IBuild>
            RecommendBuilds(BuildType buildType, double principal,
                List<IComponent> peripherals, PSUModularity psuType,
                    HardDriveType hddType, int hddCount)
        {
            // Early escapes.
            if ( principal < MIN_BUDGET || hddCount < MIN_INTEGER_VALUE )
                return null;

            double budget = principal;
            // Buid factor passses type and returns a specific build.
            IBuild build = BuildFactory.CreateBuild(buildType);

            // If peripherals were selected we remove their
            // cost from the total budget.
            if (peripherals != null)
            {
                build.Peripherals = peripherals;
                foreach (IComponent extras in build.Peripherals)
                    budget -= extras.GetTotalcost();
            }

            // Early kick out if budget has been reduced too low for items
            // by peripheral selection.
            if (budget <= MIN_BUDGET && principal > MIN_BUDGET)
                return null;

            // Advanced settings to be implemented. 
            if (hddType != HardDriveType.None ||
                hddCount > MIN_INTEGER_VALUE || psuType != PSUModularity.None)
            {
                return null;
            }
            else
            {
                // Create component list using service. 
                var compList = CreateICompListService.CreateComponentList(build);
                var budgetedList =
                    BudgetPortionService.BudgetComponents(compList, buildType, budget);

                // Get elements from DB by using the budgeted List

                // 


                

                return null;
            }
        }
        #endregion

        #region "Upgrade Recommendations"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="components"></param>
        /// <param name="buildType"></param>
        /// <param name="budget"></param>
        /// <returns></returns>
        public List<IComponent> RecommendUpgrade(List<IComponent> components,
            BuildType buildType, double budget)
        {

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedBuild"></param>
        /// <returns></returns>
        public List<IBuild> RecommendUpgrades(IBuild savedBuild)
        {
            return null;
        }
        #endregion

        #region "Private Methods"

        private void ScoreComponent()
        {

        }

        private void ComponentCompare()
        {

        }


        private void BuildCompare()
        {

        }
        #endregion

        #region "Out of Scope"
        // Migrating to user garage.
        public bool SaveRecommendation()
        {
            return false;
        }

        // Migrating to controller.
        public List<IComponent> RequestComponentsByType()
        {
            return null;
        }

        // Migrating to controller.
        public void RequestSavedBuilds()
        {

        }
        #endregion
    }
}
