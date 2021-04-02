using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Products;

namespace AutoBuildApp.Models.Builds
{
    public class WordProcessing : IBuild
    {
        public WordProcessing()
        {
        }

        public List<IHardDrive> HardDrive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ComputerCase Case { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Motherboard Mobo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public PowerSupplyUnit Psu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GPU Gpu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public CPU Cpu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public RAM Ram { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICooler CPUCooler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<IComponent> Peripheral { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool AddHardDrive(IHardDrive add)
        {
            throw new NotImplementedException();
        }

        public bool AddPeripheral(IComponent add)
        {
            throw new NotImplementedException();
        }

        public double GetTotalCost()
        {
            throw new NotImplementedException();
        }

        public bool RemoveHardDrive(IHardDrive remove)
        {
            throw new NotImplementedException();
        }

        public bool RemovePeripheral(IComponent remove)
        {
            throw new NotImplementedException();
        }
    }
}
