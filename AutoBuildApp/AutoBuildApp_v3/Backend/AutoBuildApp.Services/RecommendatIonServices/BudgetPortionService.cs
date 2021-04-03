using System;
using AutoBuildApp.Models.Interfaces;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Services.FactoryServices;

/**
 * The Budget Portion Service designed to portion out the total budget
 * to each component passed through in the IComponent list.
 * @Author Nick Marshall-Eminger, assissted by Sirage El-jawhari.
 */
namespace AutoBuildApp.Services.RecommendationServices
{
    public static class BudgetPortionService
    {
        public static readonly double MAX_BUDGET = double.MaxValue;
        public static readonly double MIN_BUDGET = 0.0;
        public static readonly int MIN_INDEX = 0;
        public static readonly int MIN_INTEGER_VALUE = 0;

        public static List<IComponent>
            BudgetComponents(List<IComponent> input,
                BuildType type, double budget)
        {
            // Early exit and failure criteria.
            if (budget < MIN_BUDGET || input == null)
                return null;

            double tempBudget;
            // If budget is not set price is no object.
            if (budget == 0)
                tempBudget = MAX_BUDGET;
            else
                tempBudget = budget;
            // Dictionary of key value pairs representing
            // the weights of each component.
            var budgetWeights = KeyFactory.CreateKey(type);
            // Initial key reiterated to allow for comparison
            // when items are missing.
            var inital = KeyFactory.CreateKey(type);
            var outputList = input;

            foreach (var key in budgetWeights.Keys)
            {
                bool found = false;
                foreach (var part in input)
                {
                    if (part == null)
                        continue; 

                    if (part.ProductType == key)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    budgetWeights.Remove(key);
            }

            double remainingWeight = 0;
            foreach (var key in budgetWeights.Keys)
                remainingWeight += budgetWeights[key];

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
