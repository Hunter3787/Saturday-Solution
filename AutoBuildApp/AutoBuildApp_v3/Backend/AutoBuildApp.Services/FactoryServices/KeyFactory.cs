using AutoBuildApp.Models.Enumerations;
using System.Collections.Generic;

/**
 * A factory to return percentages of the budget that should be allocated
 * to a build by component. These are static keys that can be re-evaluated.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Services.FactoryServices
{
    public static class KeyFactory
    {
        public static Dictionary<ProductType, double> CreateKey(BuildType buildType)
        {
            switch (buildType)
            {
                // Repeated for now
                case BuildType.GraphicArtist:
                    return new Dictionary<ProductType, double>
                    {
                        { ProductType.HDD, .1 },
                        { ProductType.GPU, .35 },
                        { ProductType.CPU, .25 },
                        { ProductType.Case, .05 },
                        { ProductType.Cooler, .03 },
                        { ProductType.RAM, .08 },
                        { ProductType.PSU, .06 },
                        { ProductType.Motherboard, .08 }
                    };
                //Repated for now.
                case BuildType.WordProcessing:
                    return new Dictionary<ProductType, double>
                    {
                        { ProductType.HDD, .1 },
                        { ProductType.GPU, .35 },
                        { ProductType.CPU, .25 },
                        { ProductType.Case, .05 },
                        { ProductType.Cooler, .03 },
                        { ProductType.RAM, .08 },
                        { ProductType.PSU, .06 },
                        { ProductType.Motherboard, .08 }
                    };
                //Default is gaming build.
                default:
                    return new Dictionary<ProductType, double>
                    {
                        {ProductType.SSD, .09 },
                        //{ ProductType.HDD, .09 },
                        { ProductType.GPU, .30 },
                        { ProductType.CPU, .25 },
                        { ProductType.Case, .08 },
                        //{ ProductType.Cooler, .03 },
                        { ProductType.RAM, .10 },
                        { ProductType.PSU, .09 },
                        { ProductType.Motherboard, .09 }
                    };
            }
        }
    }
}