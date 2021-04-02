
using AutoBuildApp.Models.Enumerations;
using System.Collections.Generic;

namespace AutoBuildApp.Services.FactoryServices
{
    public static class KeyFactory
    {
        public static Dictionary<ProductType, double> CreateKey(BuildType buildType)
        {
            switch (buildType)
            {
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
                        { ProductType.MotherBoard, .08 }
                    };

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
                        { ProductType.MotherBoard, .08 }
                    };

                default:
                    return new Dictionary<ProductType, double>
                    {
                        { ProductType.HDD, .1 },
                        { ProductType.GPU, .35 },
                        { ProductType.CPU, .25 },
                        { ProductType.Case, .05 },
                        { ProductType.Cooler, .03 },
                        { ProductType.RAM, .08 },
                        { ProductType.PSU, .06 },
                        { ProductType.MotherBoard, .08 }
                    };
            }
        }
    }
}