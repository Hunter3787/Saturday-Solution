using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Services.FactoryServices;
using AutoBuildApp.Services.RecommendationServices;
using AutoBuildApp.Services.CatalogServices;
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
        private readonly ProductDAO _dao;
        private readonly IClaims _unregistered;

        #region "Constructors"
        /// <summary>
        /// Constructor that requires a connection string to start.
        /// </summary>
        /// <param name="_connectionString"></param>
        public RecommendationManager(string connectionString)
        {
            //Generate claims
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            _unregistered = claimsFactory.GetClaims(RoleEnumType.UNREGISTERED_ROLE);

            _connectionString = connectionString;
            _dao = new ProductDAO(_connectionString);
        }
        #endregion

        #region "Build Recommendations"
        /// <summary>
        /// Recommend a computer build based off user defined parameters.
        /// </summary>
        /// <param name="requestedType">Type of Build based on Enum.</param>
        /// <param name="initialBudget">Double value representing the budget.</param>
        /// <param name="peripherals">Dictionary of product types and how many
        /// of each would be requested.(Optional)</param>
        /// <param name="psuType">Power supply unit type requested by
        /// the user.(Optional)</param>
        /// <param name="hddType">Hard drive type
        /// prioritized by user. (Optional)</param>
        /// <param name="hddCount">Number of hard drives that the user
        /// would like to be in their final build.(Optional)</param>
        /// <returns>A list of IBuild representing the recommended builds.</returns>
        public List<IBuild> RecommendBuilds(
            BuildType requestedType,
            double initialBudget,
            List<IComponent> peripherals,
            PSUModularity psuType,
            HardDriveType hddType,
            int hddCount)
        {
            #region Guards
            if (!AuthorizationService.CheckPermissions(_unregistered.Claims()))
            {
                throw new UnauthorizedAccessException("Unauthorized user");
            }
            
            if (initialBudget < RecBusinessGlobals.MIN_BUDGET)
            {
                throw new ArgumentOutOfRangeException("Budget too low.");
            }
            else if(hddCount < RecBusinessGlobals.MIN_INTEGER_VALUE)
            {
                throw new ArgumentOutOfRangeException("Invalid arguement.");
            }
            #endregion

            #region Initializations
            Dictionary<ProductType, List<IComponent>> products = new Dictionary<ProductType, List<IComponent>>();
            Dictionary<IComponent, int> scores = new Dictionary<IComponent, int>();
            //ComponentComparisonService comparator = new ComponentComparisonService();
            GetComponentsService getter = new GetComponentsService(_dao);
            PortionBudgetService portioner = new PortionBudgetService();
            HardDriveFactory driveFacorty = new HardDriveFactory();
            BuildParsingService parser = new BuildParsingService();
            BuildFactory buildFactory = new BuildFactory();
            List<IBuild> buildRecommendations = new List<IBuild>();
            IBuild prototype = buildFactory.CreateBuild(requestedType);
            double adjustedBudget = initialBudget;
            #endregion

            // Business Rule
            if (initialBudget == RecBusinessGlobals.MIN_BUDGET)
            {
                adjustedBudget = RecBusinessGlobals.MAX_BUDGET;
            }
            else if (peripherals != null)
            {
                foreach (IComponent extras in peripherals)
                {
                    adjustedBudget -= extras.GetTotalcost();
                }
            }

            if (adjustedBudget <= RecBusinessGlobals.MIN_BUDGET)
            {
                return buildRecommendations;
            }

            #region Advanced option initialization
            if (psuType != PSUModularity.None)
            {
                prototype.Psu.PsuType = psuType;
            }

            if(hddType != HardDriveType.None
                && hddCount > RecBusinessGlobals.MIN_INTEGER_VALUE)
            {
                var hddToAdd = driveFacorty.CreateHardDrive(hddType);
                prototype.AddHardDrive(hddToAdd);
            }
            #endregion

            var componentList = parser.CreateComponentList(prototype);
            var portionedList = portioner.PortionOutBudget(
                componentList,
                requestedType,
                adjustedBudget);

            products = getter.GetProductDictionary(portionedList);
            ScoreProductDictionary(products, scores, requestedType);

            // Business rule to create 5 builds and return them all.
            for (int i = 0; i < RecBusinessGlobals.BUILDS_TO_RETURN; i++)
            {
                IBuild build = buildFactory.CreateBuild(requestedType);
                // Add preselected peripherals.
                build.Peripherals = peripherals;


                // Get the highest scored mother board.
                // Find compatable CPU, RAM, and Case
                // Find in stock GPU. 
                // Pick PSU based off selection if applicable that supports GPU.
                // Add hard drives of type and number. (Compatable with Mobo.)
                // Find cooler of the selected type. Default is fan.
                



                buildRecommendations.Add(build);
            }



            // Add a function to builds to total their score
            // via the scoring tool for sorting.

            return buildRecommendations;
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

        #region Private Methods
        /// <summary>
        /// Scores the dictionary of products and stores them in a key value paring
        /// based off the build type requested. 
        /// </summary>
        /// <param name="products">Dictionary of products to score.</param>
        /// <param name="scores">Dictionary to hold product scores.</param>
        /// <param name="type">Requested build type.</param>
        private void ScoreProductDictionary(
            Dictionary<ProductType, List<IComponent>> products,
            Dictionary<IComponent, int> scores,
            BuildType type
            )
        {
            ComponentScoringService scorer = new ComponentScoringService();

            foreach (ProductType key in products.Keys)
            {
                foreach (IComponent component in products[key])
                {
                    if (!scores.ContainsKey(component))
                    {
                        var score = scorer.ScoreComponent(component, type);
                        scores.Add(component, score);
                    }
                }
            }
        }
        #endregion
    }
}
