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
    /// Static class service that takes in a build to return a components list.
    /// </summary>
    public static class CreateICompListService
    {
        /// <summary>
        /// Generate a list of Compnents from a Build.
        /// </summary>
        /// <param name="build"></param>
        /// <returns></returns>
        public static List<IComponent> CreateComponentList(IBuild build)
        {
            // Null check for early dismissal.
            if (build == null)
                return null;

            // Set components list for on method completion.
            var compList = new List<IComponent>();

            // For each loop using the properties of the build class type
            // to iterate through each dynamic property.
            foreach (var element in build.GetType().GetProperties())
            {
                // Stores the value (class) of each property. 
                var item = element.GetValue(build);

                // Check that the item is of the list type and not null.
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
