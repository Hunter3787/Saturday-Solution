﻿using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Services;

/// <summary>
/// Utilizes the Managers namespace for the Auto-build app. 
/// </summary>
namespace AutoBuildApp.Managers
{
    /// <summary>
    /// Class that functions as the manager to the recommendation tool
    /// for the AutoBuildApp.
    /// </summary>
    public class RecommendationManager : ArgumentOutOfRangeException
    {
        // Local constant for minimum budget.
        private const double MAX_BUDGET = double.MaxValue;
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
        /// <param name="budget">Double value representing the budget.</param>
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
            RecommendBuilds(BuildType buildType, double budget,
                List<IComponent> peripherals, PSUModularity psuType,
                    HardDriveType hddType, int hddCount)
        {
            if ( budget < MIN_BUDGET )
                return null;

            IBuild build = BuildFactory.CreateBuild(buildType);

            if ( peripherals != null )
                build.Peripheral = peripherals;

            if (!BudgetComponents(build, buildType, budget))
                return null;

            



            return null;
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
        private void GenerateBuildKey(BuildType type, double budget)
        {

        }

        private void GenerateComponentList()
        {

        }

        private bool BudgetComponents(IBuild build, BuildType type, double budget)
        {
            // Early exit and failure criteria.
            if (budget < MIN_BUDGET)
                return false;

            double tempBudget;

            // If budget is not set price is no object.
            if (budget == 0)
                tempBudget = MAX_BUDGET;
            else
                tempBudget = budget;

            GenerateBuildKey(type, tempBudget);






            BudgetPortionService portionService = new BudgetPortionService();

            return true;
        }

        private void RemoveOverBudgetItems()
        {

        }

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
