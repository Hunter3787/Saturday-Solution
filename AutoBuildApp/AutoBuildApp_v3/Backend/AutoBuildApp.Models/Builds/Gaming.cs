﻿using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Products;

namespace AutoBuildApp.Models.Builds
{
    public class Gaming : IBuild
    {
        private const int MIN_VALUE = 0;
        private const int INCREMENT_VALUE = 1;

        public List<IHardDrive> HardDrive { get; set; }
        public ComputerCase Case { get; set; }
        public Motherboard Mobo { get; set; }
        public PowerSupplyUnit Psu { get; set; }
        public GPU Gpu { get; set; }
        public CPU Cpu { get; set; }
        public RAM Ram { get; set; }
        public ICooler CPUCooler { get; set; }
        public List<IComponent> Peripheral { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Gaming()
        {
        }


        #region "IHardDrive List Add/Remove/Delete methods"
        /// <summary>
        /// Add hard drive to the list, if hard drive is already present
        /// increment the quantity.
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        public bool AddHardDrive(IHardDrive add)
        {
            if (add is null)
                return false;

            if (HardDrive.Contains(add))
            {
                var index = HardDrive.IndexOf(add);
                HardDrive[index].Quantity += INCREMENT_VALUE;
            }
            else
                HardDrive.Add(add);

            return true;
        }

        /// <summary>
        /// Remove IHardDrive elemnt from the hard drive list,
        /// else decrement the count.
        /// </summary>
        /// <param name="remove"></param>
        /// <returns></returns>
        public bool RemoveHardDrive(IHardDrive remove)
        {
            if (remove is null || !HardDrive.Contains(remove))
                return false;

            var success = false;
            var index = HardDrive.IndexOf(remove);


            if (HardDrive[index].Quantity > MIN_VALUE)
                HardDrive[index].Quantity -= INCREMENT_VALUE;
            else
                HardDrive.Remove(remove);

            return success;
        }

        /// <summary>
        /// Completely remove selected element from the List.
        /// </summary>
        /// <param name="delete"></param>
        /// <returns></returns>
        public bool DeleteHardDrive(IHardDrive delete)
        {
            if (delete is null || !HardDrive.Contains(delete))
                return false;

            return HardDrive.Remove(delete);
        }

        #endregion

        #region "IComponent Periperhals List Add/Remove/Delete methods"
        /// <summary>
        /// Add hard drive to the list, if hard drive is already present
        /// increment the quantity.
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        public bool AddPeripheral(IComponent add)
        {
            if (add is null)
                return false;

            if (Peripheral.Contains(add))
            {
                var index = Peripheral.IndexOf(add);
                Peripheral[index].Quantity += INCREMENT_VALUE;
            }
            else
                Peripheral.Add(add);

            return true;
        }

        /// <summary>
        /// Remove IHardDrive elemnt from the hard drive list,
        /// else decrement the count.
        /// </summary>
        /// <param name="remove"></param>
        /// <returns></returns>
        public bool RemovePeripheral(IComponent remove)
        {
            if (remove is null || !Peripheral.Contains(remove))
                return false;

            var success = false;
            var index = Peripheral.IndexOf(remove);


            if (Peripheral[index].Quantity > MIN_VALUE)
                Peripheral[index].Quantity -= INCREMENT_VALUE;
            else
                Peripheral.Remove(remove);

            return success;
        }

        /// <summary>
        /// Completely remove selected element from the List.
        /// </summary>
        /// <param name="delete"></param>
        /// <returns></returns>
        public bool DeletePeripheral(IComponent delete)
        {
            if (delete is null || !Peripheral.Contains(delete))
                return false;

            return Peripheral.Remove(delete);
        }
        #endregion
    }
}
