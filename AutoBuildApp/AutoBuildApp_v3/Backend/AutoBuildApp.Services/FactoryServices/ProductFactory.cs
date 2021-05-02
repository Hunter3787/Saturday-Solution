using System;
using System.ComponentModel;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Products;

namespace AutoBuildApp.Services.FactoryServices
{
    public class ProductFactory
    {
        public IComponent CreateComponent(ProductType productType)
        {
            switch (productType)
            {
                case ProductType.Case:
                    return (IComponent)new ComputerCase();
                case ProductType.Cooler:
                    return (IComponent)new WaterCooler();
                case ProductType.CPU:
                    return (IComponent)new CentralProcUnit();
                case ProductType.Fan:
                    return (IComponent)new Fan();
                case ProductType.GPU:
                    return (IComponent)new GraphicsProcUnit();
                case ProductType.MotherBoard:
                    return (IComponent)new Motherboard();
                case ProductType.PSU:
                    return (IComponent)new PowerSupplyUnit();
                case ProductType.RAM:
                    return (IComponent)new RAM();
                default:
                    throw new InvalidEnumArgumentException(productType + " not implemented.");
            }
        }
    }
}
