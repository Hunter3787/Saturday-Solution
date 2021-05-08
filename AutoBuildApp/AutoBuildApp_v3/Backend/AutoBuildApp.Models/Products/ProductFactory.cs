using System.ComponentModel;
using AutoBuildApp.Models.Enumerations;

namespace AutoBuildApp.Models.Products
{
    public class ProductFactory
    {
        public Component CreateComponent(ProductType productType)
        {
            switch (productType)
            {
                case ProductType.Case:
                    return new Component();
                case ProductType.Cooler:
                    return new WaterCooler();
                case ProductType.CPU:
                    return new CentralProcUnit();
                case ProductType.Fan:
                    return new Fan();
                case ProductType.GPU:
                    return new GraphicsProcUnit();
                case ProductType.Motherboard:
                    return new Motherboard();
                case ProductType.PSU:
                    return new PowerSupplyUnit();
                case ProductType.RAM:
                    return new RAM();
                case ProductType.SSD:
                case ProductType.HDD:
                    return new HardDrive();
                default:
                    throw new InvalidEnumArgumentException(productType + " not implemented.");
            }
        }
    }
}
