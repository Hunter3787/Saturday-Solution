using System;
using AutoBuildApp.Models.Interfaces;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Services.FactoryServices;

/**
 * The Budget Portion Service designed to portion out the total budget
 * to each component passed through in the IComponent list.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Services.RecommendatIonServices
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
            var budgetWeight = KeyFactory.CreateKey(type);

            foreach (var test in budgetWeight.Keys)
            {
                bool found = false;
                foreach (var part in input)
                {
                    if (part.ProductType == test)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    budgetWeight.Remove(test);
            }

            // Stuck



            return null;
        }
    }
}
