﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Products;

namespace AutoBuildApp.Models.Builds
{
    public class Build : IBuild, IEquatable<Build>
    {
        public double TotalCost;
        public string BuildName;
        public List<HardDrive> HardDrives { get; set; }
        public ComputerCase Case { get; set; }
        public Motherboard Mobo { get; set; }
        public PowerSupplyUnit Psu { get; set; }
        public GraphicsProcUnit Gpu { get; set; }
        public CentralProcUnit Cpu { get; set; }
        public RAM Ram { get; set; }
        public ICooler CPUCooler { get; set; }
        public List<IComponent> Peripherals { get; set; }
        public List<string> ImageStrings { get; set; }

        public Build()
        {            
            HardDrives = new List<HardDrive>();
            Case = new ComputerCase();
            Mobo = new Motherboard();
            Psu = new PowerSupplyUnit();
            Gpu = new GraphicsProcUnit();
            Cpu = new CentralProcUnit();
            Ram = new RAM();
            CPUCooler = new Fan();
            Peripherals = new List<IComponent>();
            ImageStrings = new List<string>();
        }

        #region "HardDrive List Add/Remove/Delete methods"
        /// <summary>
        /// Add hard drive to the list, if hard drive is already present
        /// increment the quantity.
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        public bool AddHardDrive(HardDrive add)
        {
            if (add == null)
            {
                return false;
            }

            if (HardDrives == null)
            {
                HardDrives = new List<HardDrive>();
            }

            if (HardDrives.Contains(add))
            {
                var index = HardDrives.IndexOf(add);
                HardDrives[index].Quantity += BuildGlobals.INCREMENT_VALUE;
            }
            else
            {
                HardDrives.Add(add);
            }

            return true;
        }

        /// <summary>
        /// Remove HardDrive elemnt from the hard drive list,
        /// else decrement the count.
        /// </summary>
        /// <param name="remove"></param>
        /// <returns></returns>
        public bool RemoveHardDrive(HardDrive remove)
        {
            if (remove == null
                || HardDrives == null
                || !HardDrives.Contains(remove))
            {
                return false;
            }

            var success = false;
            var index = HardDrives.IndexOf(remove);

            if (HardDrives[index].Quantity > BuildGlobals.MIN_VALUE)
            {
                HardDrives[index].Quantity -= BuildGlobals.INCREMENT_VALUE;
            }
            else
            {
                HardDrives.Remove(remove);
            }

            return success;
        }

        /// <summary>
        /// Completely remove selected element from the List.
        /// </summary>
        /// <param name="delete"></param>
        /// <returns></returns>
        public bool DeleteHardDrive(HardDrive delete)
        {
            if (delete == null
                || HardDrives == null
                || !HardDrives.Contains(delete))
            {
                return false;
            }

            return HardDrives.Remove(delete);
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
            if (add == null)
            {
                return false;
            }

            if (Peripherals == null)
            {
                Peripherals = new List<IComponent>();
            }

            if (Peripherals.Contains(add))
            {
                var index = Peripherals.IndexOf(add);
                Peripherals[index].Quantity += BuildGlobals.INCREMENT_VALUE;
            }
            else
            {
                Peripherals.Add(add);
            }

            return true;
        }

        /// <summary>
        /// Remove HardDrive elemnt from the hard drive list,
        /// else decrement the count.
        /// </summary>
        /// <param name="remove"></param>
        /// <returns></returns>
        public bool RemovePeripheral(IComponent remove)
        {
            if (remove == null
                || Peripherals == null
                || !Peripherals.Contains(remove))
            {
                return false;
            }

            var success = false;
            var index = Peripherals.IndexOf(remove);

            if (Peripherals[index].Quantity > BuildGlobals.MIN_VALUE)
            {
                Peripherals[index].Quantity -= BuildGlobals.INCREMENT_VALUE;
            }
            else
            {
                Peripherals.Remove(remove);
            }

            return success;
        }

        /// <summary>
        /// Completely remove selected element from the List.
        /// </summary>
        /// <param name="delete"></param>
        /// <returns></returns>
        public bool DeletePeripheral(IComponent delete)
        {
            if (delete == null
                || Peripherals == null
                || !Peripherals.Contains(delete))
            {
                return false;
            }

            return Peripherals.Remove(delete);
        }
        #endregion

        #region Image Add/Delete
        /// <summary>
        /// Adds an image from a byte array to the component.
        /// </summary>
        /// <param name="image">Byte Array representing an image.</param>
        /// <returns></returns>
        public bool AddImage(string location)
        {
            ProductGuard.Exists(ImageStrings, nameof(ImageStrings));
            ProductGuard.IsNotEmpty(location, nameof(location));

            ImageStrings.Add(location);
            return true;
        }

        /// <summary>
        /// String method to locate the index of the passed string.
        /// Will call index variation to remove the element upon location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool RemoveImage(string location)
        {
            ProductGuard.Exists(ImageStrings, nameof(ImageStrings));
            ProductGuard.IsNotEmpty(location, nameof(location));
            ProductGuard.ContainsElement(ImageStrings, location, nameof(location));

            var index = ImageStrings.IndexOf(location);
            return RemoveImage(index);
        }

        /// <summary>
        /// Removes an image from the byte array at the provided index.
        /// </summary>
        /// <param name="index">Position of the image intended to be deleted.</param>
        /// <returns></returns>
        public bool RemoveImage(int index)
        {
            ProductGuard.Exists(ImageStrings, nameof(ImageStrings));
            ProductGuard.IsInRange(ImageStrings, index, nameof(ImageStrings));

            ImageStrings.RemoveAt(index);
            return true;
        }
        #endregion

        /// <summary>
        /// Computes total cost of all components and returns as a double.
        /// </summary>
        /// <returns>Double</returns>
        public double GetTotalCost()
        {
            double total = 0;

            if (HardDrives != null)
            {
                foreach (HardDrive hdd in HardDrives)
                {
                    total += hdd.GetTotalcost();
                }
            }

            if (Peripherals != null)
            {
                foreach (IComponent peri in Peripherals)
                {
                    total += peri.GetTotalcost();
                }
            }

            if (CPUCooler != null)
            {
                total += CPUCooler.GetTotalcost();
            }

            if (Ram != null)
            {
                total += Ram.GetTotalcost();
            }

            if (Cpu != null)
            {
                total += Cpu.GetTotalcost();
            }

            if (Gpu != null)
            {
                total += Gpu.GetTotalcost();
            }

            if (Psu != null)
            {
                total += Psu.GetTotalcost();
            }

            if (Mobo != null)
            {
                total += Mobo.GetTotalcost();
            }

            if (Case != null)
            {
                total += Case.GetTotalcost();
            }

            // This version returns true on tests however results in an error
            // "Missing Compiler required member 'microsoft.csharp.runtimebinder..."
            //// Set components list for on method completion.
            //var compList = new List<IComponent>();

            //// For each loop using the properties of the build class type
            //// to iterate through each dynamic property.
            //foreach (var element in this.GetType().GetProperties())
            //{
            //    // Stores the value (class) of each property. 
            //    var item = element.GetValue(this);

            //    // Check that the item is of the list type and not null.
            //    if (item is IList && item != null)
            //        // Used the dynamic cast to assure the compiler that the item
            //        // is in fact of the expected type of List<IComponent>.
            //        foreach (var component in (dynamic)item)
            //            compList.Add(component);
            //    else
            //        if (item != null)
            //        compList.Add((IComponent)item);
            //}

            //foreach (var item in compList)
            //    total += item.GetTotalcost();

            return total;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Build))
            {
                return false;
            }

            Build temp = (Build)obj;

            if (BuildName == temp.BuildName)
                if (HardDrives.SequenceEqual(temp.HardDrives))
                if(Case == temp.Case)
                if(Mobo == temp.Mobo)
                if(Psu == temp.Psu)
                if(Gpu == temp.Gpu)
                if(Cpu == temp.Cpu)
                if(Ram == temp.Ram)
                if(CPUCooler == temp.CPUCooler)
                if(Peripherals.SequenceEqual(temp.Peripherals))
                if(ImageStrings.SequenceEqual(temp.ImageStrings))
            {
                return true;
            }

            return false;
        }

        public bool Equals(Build other)
        {
            if (other == null)
            {
                return false;
            }

            if (BuildName == other.BuildName
                && HardDrives.SequenceEqual(other.HardDrives)
                && Case == other.Case
                && Mobo == other.Mobo
                && Psu == other.Psu
                && Gpu == other.Gpu
                && Cpu == other.Cpu
                && Ram == other.Ram
                && CPUCooler == other.CPUCooler
                && Peripherals.SequenceEqual(other.Peripherals)
                && ImageStrings.SequenceEqual(other.ImageStrings)
                )
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

 
    }
}
