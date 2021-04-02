using System;
using AutoBuildApp.Models.Products;
using System.Collections.Generic;

namespace AutoBuildApp.Models.Interfaces
{
    /// <summary>
    /// Generic build type interface. 
    /// </summary>
    public interface IBuild
    {
        public List<IHardDrive> HardDrive { get; set; }
        public ComputerCase Case { get; set; }
        public Motherboard Mobo { get; set; }
        public PowerSupplyUnit Psu { get; set; }
        public GPU Gpu { get; set; }
        public CPU Cpu { get; set; }
        public RAM Ram { get; set; }
        public ICooler CPUCooler { get; set; }
        public List<IComponent> Peripheral { get; set; }

        public bool AddHardDrive(IHardDrive add);
        public bool RemoveHardDrive(IHardDrive remove);
        public bool AddPeripheral(IComponent add);
        public bool RemovePeripheral(IComponent remove);
        public double GetTotalCost();
    }
}
