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
         List<IHardDrive> HardDrive { get; set; }
         ComputerCase Case { get; set; }
         Motherboard Mobo { get; set; }
         PowerSupplyUnit Psu { get; set; }
         GPU Gpu { get; set; }
         CPU Cpu { get; set; }
         RAM Ram { get; set; }
         ICooler CPUCooler { get; set; }
         List<IComponent> Peripheral { get; set; }

         bool AddHardDrive(IHardDrive add);
         bool RemoveHardDrive(IHardDrive remove);
         bool AddPeripheral(IComponent add);
         bool RemovePeripheral(IComponent remove);
         double GetTotalCost();
    }
}
