using System;
using AutoBuildApp.Models.Products;
using System.Collections.Generic;

/**
 * IBuild Interface for AutoBuild App
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Interfaces
{
    /// <summary>
    /// Generic build type interface. 
    /// </summary>
    public interface IBuild
    {
        List<IHardDrive> HardDrives { get; set; }
        ComputerCase Case { get; set; }
        Motherboard Mobo { get; set; }
        PowerSupplyUnit Psu { get; set; }
        GraphicsProcUnit Gpu { get; set; }
        CentralProcUnit Cpu { get; set; }
        RAM Ram { get; set; }
        ICooler CPUCooler { get; set; }
        List<IComponent> Peripherals { get; set; }

        /// <summary>
        /// Add a hard drive component to the hard drive list.
        /// </summary>
        /// <param name="add"></param>
        /// <returns>Bool</returns>
        bool AddHardDrive(IHardDrive add);
        /// <summary>
        /// Remove a hard drive component from the hard drive list.
        /// </summary>
        /// <param name="remove"></param>
        /// <returns></returns>
        bool RemoveHardDrive(IHardDrive remove);
        /// <summary>
        /// Add an IComponent to the peripeheral list.
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        bool AddPeripheral(IComponent add);
        /// <summary>
        /// Remove an IComponetn from the peripheral list.
        /// </summary>
        /// <param name="remove"></param>
        /// <returns></returns>
        bool RemovePeripheral(IComponent remove);
        /// <summary>
        /// The sum of entire build.
        /// </summary>
        /// <returns></returns>
        double GetTotalCost();
    }
}
