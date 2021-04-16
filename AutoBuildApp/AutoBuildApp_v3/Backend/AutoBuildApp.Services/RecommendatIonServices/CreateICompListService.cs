using System;
using System.Collections;
using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;

/**
 * Parses an IBuild object to return an ICompnent list.
 * Null elements will not be added to the IComponent list.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Services.RecommendationServices
{
    /// <summary>
    /// Class to parse elements from an IBuild.
    /// </summary>
    public class IBuildParsingService
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public IBuildParsingService()
        { 
        }

        /// <summary>
        /// Generate a list of Compnents from a Build.
        /// </summary>
        /// <param name="build"></param>
        /// <returns></returns>
        public static List<IComponent> CreateComponentList(IBuild build)
        {
            if (build == null)
            {
                throw new ArgumentException("No paramater passed.");
            }

            var compList = new List<IComponent>();

            // For each loop using the properties of the build class type
            // to iterate through each dynamic property.
            foreach (var element in build.GetType().GetProperties())
            {
                var item = element.GetValue(build);

                if (item is IList && item != null)
                    // Used the dynamic cast to assure the compiler that the item
                    // is in fact of the expected type of List<IComponent>.
                    foreach (var component in (dynamic)item)
                        compList.Add(component);
                else
                    if (item != null)
                    compList.Add((IComponent)item);
            }

            return compList;
        }
    }
}
