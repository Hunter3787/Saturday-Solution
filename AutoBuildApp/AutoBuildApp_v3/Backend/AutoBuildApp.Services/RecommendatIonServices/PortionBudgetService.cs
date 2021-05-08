using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Services.FactoryServices;
using AutoBuildApp.Models.Interfaces;

/**
 * The Budget Portion Service designed to portion out the total budget
 * to each component passed through in the IComponent list.
 * @Author Nick Marshall-Eminger, assissted by Sirage El-jawhari.
 */
namespace AutoBuildApp.Services.RecommendationServices
{
    public class PortionBudgetService
    {
        /// <summary>
        /// Defaut Constructor.
        /// </summary>
        public PortionBudgetService()
        {

        }

        /// <summary>
        /// Portion out to each component how much of the
        /// total budget each component should recieve in percentages.
        /// </summary>
        /// <param name="input">List of IComoponents</param>
        /// <param name="type">BuildType</param>
        /// <param name="budget">Double</param>
        /// <returns>List of IComponents</returns>
        public List<IComponent> PortionOutBudget
            (List<IComponent> input, BuildType type, double budget)
        {
            var outputList = new List<IComponent>(input);
            var budgetWeights = KeyFactory.CreateKey(type);
            
            if (budget < RecServiceGlobals.MIN_BUDGET || input == null)
            {
                return outputList;
            }

            foreach (var key in budgetWeights.Keys)
            {
                bool found = false;
                foreach (var part in input)
                {
                    if (part == null)
                    {
                        continue;
                    }
                    else if (part.ProductType == key)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    budgetWeights.Remove(key);
                }
            }

            double remainingWeight = 0;
            foreach (var key in budgetWeights.Keys)
            {
                remainingWeight += budgetWeights[key];
            }

            remainingWeight = Math.Round(remainingWeight, 2, MidpointRounding.AwayFromZero);

            foreach(var component in outputList)
            {
                var t = component.ProductType;
                component.Budget = (budgetWeights[t] / remainingWeight) * budget;
            }

            return outputList;
        }
    }
}
