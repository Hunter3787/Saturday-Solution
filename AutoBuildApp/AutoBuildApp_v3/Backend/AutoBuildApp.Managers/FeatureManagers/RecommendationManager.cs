using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Services.FactoryServices;
using AutoBuildApp.Services.RecommendationServices;
using AutoBuildApp.Services;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security;
using AutoBuildApp.Models.Builds;
using AutoBuildApp.Models.Products;

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
            _unregistered = claimsFactory.GetClaims(RoleEnumType.UnregisteredRole);

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
        public Build RecommendBuilds(
            BuildType requestedType,
            double initialBudget,
            List<IComponent> peripherals,
            PSUModularity psuType,
            HardDriveType hddType,
            int hddCount)
        {
            #region Guards
            //if (!AuthorizationService.CheckPermissions(_unregistered.Claims()))
            //{
            //    throw new UnauthorizedAccessException("Unauthorized user");
            //}
            
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
            Dictionary<ProductType, List<KeyValuePair<IComponent, int>>> scores = new Dictionary<ProductType, List<KeyValuePair<IComponent, int>>>();
            //ComponentComparisonService comparator = new ComponentComparisonService();
            GetProductService getter = new GetProductService(_dao);
            PortionBudgetService portioner = new PortionBudgetService();
            HardDriveFactory driveFacorty = new HardDriveFactory();
            BuildParsingService parser = new BuildParsingService();
            BuildFactory buildFactory = new BuildFactory();
            List<Build> buildRecommendations = new List<Build>();
            Build prototype = buildFactory.CreateBuild(requestedType);
            double adjustedBudget = initialBudget;
            #endregion

            // Business Rule
            if (initialBudget == RecBusinessGlobals.MIN_BUDGET)
            {
                adjustedBudget = RecBusinessGlobals.MAX_BUDGET;
            }
            else if (peripherals != null)
            {
                foreach (Component extras in peripherals)
                {
                    adjustedBudget -= extras.GetTotalcost();
                }
            }

            //if (adjustedBudget <= RecBusinessGlobals.MIN_BUDGET)
            //{
            //    return buildRecommendations;
            //}

            #region Advanced option initialization
            if (psuType != PSUModularity.None)
            {
                //prototype.Psu.PsuType = psuType;
            }

            if(hddType != HardDriveType.None
                && hddCount > RecBusinessGlobals.MIN_INTEGER_VALUE)
            {
                //var hddToAdd = driveFacorty.CreateHardDrive(hddType);
                //prototype.AddHardDrive(hddToAdd);
            }
            #endregion

            prototype.AddHardDrive(new HardDrive() { ProductType = ProductType.SSD });

            var componentList = parser.CreateComponentList(prototype);
            componentList.RemoveAt(7);

            var portionedList = portioner.PortionOutBudget(
                componentList,
                requestedType,
                adjustedBudget);

            products = getter.GetComponentDictionary(portionedList);
            ScoreProductDictionary(products, scores, requestedType);
            SortScoreDictionary(scores);

            //SortByPriceDesc(products);
            int lastMotherboardLocation = -1;
            // Business rule to create 5 builds and return them all.
            for (int i = 0; i < 5; i++)
            {
                Build build = buildFactory.CreateBuild(requestedType);
                // Add preselected peripherals.
                build.Peripherals = peripherals;

                Motherboard chosenMobo = null;
                CentralProcUnit chosenCPU = null;
                RAM chosenRAM = null;
                ComputerCase chosenCase = null;
                GraphicsProcUnit chosenGPU = null;
                PowerSupplyUnit chosenPSU = null;
                HardDrive chosenHD = null;

                int iMobo;
                for (iMobo = lastMotherboardLocation + 1; iMobo < products[ProductType.Motherboard].Count; iMobo++)
                {
                    // Start with the highest scored motherboard
                    chosenMobo = (Motherboard)products[ProductType.Motherboard][iMobo];
                    lastMotherboardLocation = iMobo;

                    // If there isn't a motherboard form factor, try the next highest scored motherboard
                    if(chosenMobo.MoboForm == null)
                    {
                        continue;
                    }

                    // Find a compatible CPU
                    chosenCPU = FindCompatibleCPU(chosenMobo, scores[ProductType.CPU]);

                    // If there is no compatible CPU, try the next highest scored motherboard
                    if(chosenCPU == null)
                    {
                        continue;
                    }

                    // Find compatible RAM
                    chosenRAM = FindCompatibleRAM(chosenMobo, scores[ProductType.RAM]);

                    // If there is no compatible RAM, try the next highest scored motherboard
                    if(chosenRAM == null)
                    {
                        continue;
                    }

                    // Find compatible case with gpu and psu
                    int iCase;
                    for (iCase = 0; iCase < products[ProductType.Case].Count; iCase++)
                    {
                        chosenCase = FindCompatibleCase(chosenMobo, scores[ProductType.Case]);
                        if(chosenCase == null)
                        {
                            continue;
                        }

                        // Find compatible gpu
                        chosenGPU = FindCompatibleGPU(chosenCase, scores[ProductType.GPU]);

                        // If there is no compatible gpu, try the next highest scored case
                        if(chosenGPU == null)
                        {
                            continue;
                        }

                        // Find compatible psu
                        chosenPSU = FindCompatiblePSU(chosenCase, scores[ProductType.PSU]);

                        if(chosenPSU == null)
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // If it went through all cases, that means we didn't find a compatible case
                    if(iCase == products[ProductType.Case].Count)
                    {
                        continue;
                    }

                    build.Mobo = chosenMobo;
                    build.Cpu = chosenCPU;
                    build.Ram = chosenRAM;
                    build.Case = chosenCase;
                    build.Gpu = chosenGPU;
                    build.Psu = chosenPSU;

                    build.AddHardDrive((SolidStateDrive)products[ProductType.SSD][0]);

                    build.TotalCost += build.Mobo.Price;
                    build.TotalCost += build.Cpu.Price;
                    build.TotalCost += build.Ram.Price;
                    build.TotalCost += build.Case.Price;
                    build.TotalCost += build.Gpu.Price;
                    build.TotalCost += build.Psu.Price;
                    build.TotalCost += build.HardDrives[0].Price;
                    
                    // If we get to the bottom, we know everything else passed
                    break;
                }


                if(iMobo == products[ProductType.Motherboard].Count)
                {
                    // Case where no motherboard is compatible..
                    // AKA no build was created successfully
                }


                // Get the highest scored mother board.
                // Find compatable CPU, RAM, and Case
                // Find in stock GPU. 
                // Pick PSU based off selection if applicable that supports GPU.
                // Add hard drives of type and number. (Compatable with Mobo.)
                // Find cooler of the selected type. Default is fan.




                buildRecommendations.Add(build);
            }

            int mostExpensiveBuildIndex = 0;
            for(int i = 0; i < buildRecommendations.Count; i++)
            {
                if(buildRecommendations[i].TotalCost > buildRecommendations[mostExpensiveBuildIndex].TotalCost)
                {
                    mostExpensiveBuildIndex = i;
                }
            }

            Console.WriteLine(buildRecommendations);



            // Add a function to builds to total their score
            // via the scoring tool for sorting.

            return buildRecommendations[mostExpensiveBuildIndex];
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
        public List<Build> RecommendUpgrades(Build savedBuild,
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
            Dictionary<ProductType, List<KeyValuePair<IComponent, int>>> scores,
            BuildType type
            )
        {
            ComponentScoringService scorer = new ComponentScoringService();

            foreach(ProductType key in products.Keys)
            {
                if(!scores.ContainsKey(key))
                {
                    scores.Add(key, new List<KeyValuePair<IComponent, int>>());
                }
                foreach(Component component in products[key])
                {
                    KeyValuePair<IComponent, int> tempComponentAndScore = new KeyValuePair<IComponent, int>(component, 0);
                    if(!scores[key].Contains(tempComponentAndScore))
                    {
                        var score = scorer.ScoreComponent(tempComponentAndScore.Key, type);

                        KeyValuePair<IComponent, int> componentAndScore = new KeyValuePair<IComponent, int>(component, score);

                        scores[key].Add(componentAndScore);
                    }
                }
            }
            //foreach (ProductType key in products.Keys)
            //{
            //    foreach (Component component in products[key])
            //    {
            //        if (!scores.ContainsKey(component))
            //        {
            //            var score = scorer.ScoreComponent(component, type);
            //            scores.Add(component, score);
            //        }
            //    }
            //}
        }
        #endregion

        private void SortScoreDictionary(Dictionary<ProductType, List<KeyValuePair<IComponent, int>>> scores)
        {
            foreach(var keyValue in scores)
            {
                keyValue.Value.Sort((x, y) => y.Value.CompareTo(x.Value));
            }
        }

        private void SortByPriceDesc(Dictionary<ProductType, List<IComponent>> products)
        {
            foreach(var keyValue in products)
            {
                keyValue.Value.Sort(delegate (IComponent x, IComponent y)
                {
                    return y.Price.CompareTo(x.Price);
                });
            }
        }

        private CentralProcUnit FindCompatibleCPU(Motherboard motherboard, List<KeyValuePair<IComponent, int>> potentialCPUs)
        {
            if(motherboard.Socket == null)
            {
                return null;
            }

            List<KeyValuePair<IComponent, int>> f = new List<KeyValuePair<IComponent, int>>();
            CentralProcUnit bestCPU = null;
            foreach (KeyValuePair<IComponent, int> keyValue in potentialCPUs)
            {
                bestCPU = (CentralProcUnit)keyValue.Key;
                if (motherboard.Socket == bestCPU.Socket)
                {
                    f.Add(keyValue);
                    //return bestCPU;
                }
            }
            return f.Count == 0 ? null : (CentralProcUnit)f[0].Key;
        }

        private RAM FindCompatibleRAM(Motherboard motherboard, List<KeyValuePair<IComponent, int>> potentialRAM)
        {
            // Quick check to see if 
            if(motherboard.SupportedMemory.Count == 0)
            {
                return null;
            }

            List<KeyValuePair<IComponent, int>> f = new List<KeyValuePair<IComponent, int>>();
            RAM bestRAM = null;
            foreach (KeyValuePair<IComponent, int> keyValue in potentialRAM)
            {
                bestRAM = (RAM)keyValue.Key;
                if(bestRAM.Speed == null)
                {
                    continue;
                }
                foreach(var memorySpeed in motherboard.SupportedMemory)
                {
                    if(bestRAM.Speed.Contains(memorySpeed) && !f.Contains(keyValue))
                    {
                        return bestRAM;
                    }
                }

            }

            return null;
            return f.Count == 0 ? null : (RAM)f[0].Key;
        }

        private ComputerCase FindCompatibleCase(Motherboard motherboard, List<KeyValuePair<IComponent, int>> potentialCases)
        {
            if (motherboard.MoboForm == null)
            {
                return null;
            }

            List<KeyValuePair<IComponent, int>> f = new List<KeyValuePair<IComponent, int>>();
            ComputerCase bestCase = null;
            foreach (KeyValuePair<IComponent, int> keyValue in potentialCases)
            {
                bestCase = (ComputerCase)keyValue.Key;
                if (bestCase.MoboFormSupport.Count == 0)
                {
                    continue;
                }
                foreach (var formFactor in bestCase.MoboFormSupport)
                {
                    if(formFactor.Equals(motherboard.MoboForm))
                    {
                        return bestCase;
                    }
                }

            }
            return null;
            Console.WriteLine(f);
            return f.Count == 0 ? null : (ComputerCase)f[0].Key;
        }

        private GraphicsProcUnit FindCompatibleGPU(ComputerCase _case, List<KeyValuePair<IComponent, int>> potentialGPUs)
        {

            List<KeyValuePair<IComponent, int>> f = new List<KeyValuePair<IComponent, int>>();
            GraphicsProcUnit bestGPU = null;
            foreach (KeyValuePair<IComponent, int> keyValue in potentialGPUs)
            {
                bestGPU = (GraphicsProcUnit)keyValue.Key;
                if (bestGPU.Length <= _case.MaxGPULength)
                {
                    return bestGPU;
                }
            }
            return null;
            //Console.WriteLine(f);
            //return f.Count == 0 ? null : (ComputerCase)f[0].Key;
        }

        private PowerSupplyUnit FindCompatiblePSU(ComputerCase _case, List<KeyValuePair<IComponent, int>> potentialPSUs)
        {

            List<KeyValuePair<IComponent, int>> f = new List<KeyValuePair<IComponent, int>>();
            PowerSupplyUnit bestPSU = null;
            foreach (KeyValuePair<IComponent, int> keyValue in potentialPSUs)
            {
                bestPSU = (PowerSupplyUnit)keyValue.Key;
                if (bestPSU.Length <= _case.MaxPSULength)
                {
                    return bestPSU;
                }
            }
            return null;
            //Console.WriteLine(f);
            //return f.Count == 0 ? null : (ComputerCase)f[0].Key;
        }

    }

}

