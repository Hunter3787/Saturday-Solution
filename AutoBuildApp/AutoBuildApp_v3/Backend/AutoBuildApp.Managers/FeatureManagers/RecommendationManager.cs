using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Services.FactoryServices;
using AutoBuildApp.Services.RecommendationServices;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security;

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
    public class RecommendationManager
    {
        // Local constant for minimum budget.
        private readonly string _connectionString;
        private RecommendationDAO _dao;
        private PortionBudgetService _portionService;
        private ClaimsFactory _claimsFactory;
        private IClaims _basicUser;
        private IClaims _unregistered;

        #region "Constructors"
        /// <summary>
        /// Constructor that requires a connection string to start.
        /// </summary>
        /// <param name="_connectionString"></param>
        public RecommendationManager(string connectionString)
        {
            //Generate claims
            _claimsFactory = new ConcreteClaimsFactory();
            _basicUser = _claimsFactory.GetClaims(RoleEnumType.BASIC_ROLE);
            _unregistered = _claimsFactory.GetClaims(RoleEnumType.UNREGISTERED_ROLE);

            _connectionString = connectionString;
            _dao = new RecommendationDAO(_connectionString);
        }
        #endregion

        #region "Build Recommendations"
        /// <summary>
        /// Recommend a computer build based off user defined parameters.
        /// </summary>
        /// <param name="buildType">Type of Build based on Enum.</param>
        /// <param name="initial">Double value representing the budget.</param>
        /// <param name="peripherals">Dictionary of product types and how many
        /// of each would be requested.(Optional)</param>
        /// <param name="psuType">Power supply unit type requested by
        /// the user.(Optional)</param>
        /// <param name="hddType">Hard drive type
        /// prioritized by user. (Optional)</param>
        /// <param name="hddCount">Number of hard drives that the user
        /// would like to be in their final build.(Optional)</param>
        /// <returns>A list of IBuild representing the recommended builds.</returns>
        public List<IBuild> RecommendBuilds(BuildType buildType, double initial,
                    List<IComponent> peripherals, PSUModularity psuType,
                    HardDriveType hddType, int hddCount)
        {
            if (!AuthorizationService.checkPermissions(_unregistered.Claims()))
            {
                throw new UnauthorizedAccessException("Unauthorized user");
            }

            //if ( initial < MIN_BUDGET || hddCount < MIN_INTEGER_VALUE)
            //{
            //    return null;
            //}


            //double budget = initial;
            //IBuild build = BuildFactory.CreateBuild(buildType);

            //if (peripherals != null)
            //{
            //    build.Peripherals = peripherals;
            //    foreach (IComponent extras in build.Peripherals)
            //        budget -= extras.GetTotalcost();
            //}

            //// Business Rule
            //if (budget <= MIN_BUDGET && initial > MIN_BUDGET)
            //{
            //    return null;
            //}

            //// Advanced settings to be implemented. 
            //if (hddType != HardDriveType.None ||
            //    hddCount > MIN_INTEGER_VALUE || psuType != PSUModularity.None)
            //{
            //    return null;
            //}
            //else
            //{
            //    // Create component list using service. 
            //    var compList = BuildParsingService.CreateComponentList(build);
            //    var budgetedList =
            //        _portionService.PortionComponentList(compList, buildType, budget);




            //    // recieve elements, for loop passing each component and return an int.

            //    return null;
            //}
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
        public List<IBuild> RecommendUpgrades(IBuild savedBuild,
            BuildType buildType, double budget)
        {


            return null;
        }
        #endregion
    }
}
