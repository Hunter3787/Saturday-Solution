using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Products;

/**
 * Currently a duplicate of gaming build. Not yet implemented.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Builds
{
    public class WordProcessing : IBuild
    {
        public List<IHardDrive> HardDrives { get; set; }
        public ComputerCase Case { get; set; }
        public Motherboard Mobo { get; set; }
        public PowerSupplyUnit Psu { get; set; }
        public GraphicsProcUnit Gpu { get; set; }
        public CentralProcUnit Cpu { get; set; }
        public RAM Ram { get; set; }
        public ICooler CPUCooler { get; set; }
        public List<Interfaces.Component> Peripherals { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WordProcessing()
        {
            HardDrives = new List<IHardDrive>();
            Case = new ComputerCase();
            Mobo = new Motherboard();
            Psu = new PowerSupplyUnit();
            Gpu = new GraphicsProcUnit();
            Cpu = new CentralProcUnit();
            Ram = new RAM();
            CPUCooler = new Fan();
            Peripherals = new List<Interfaces.Component>();
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
            if (add == null)
            {
                return false;
            }

            if (HardDrives == null)
            {
                HardDrives = new List<IHardDrive>();
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
        /// Remove IHardDrive elemnt from the hard drive list,
        /// else decrement the count.
        /// </summary>
        /// <param name="remove"></param>
        /// <returns></returns>
        public bool RemoveHardDrive(IHardDrive remove)
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
        public bool DeleteHardDrive(IHardDrive delete)
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
        public bool AddPeripheral(Interfaces.Component add)
        {
            if (add == null)
            {
                return false;
            }

            if (Peripherals == null)
            {
                Peripherals = new List<Interfaces.Component>();
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
        /// Remove IHardDrive elemnt from the hard drive list,
        /// else decrement the count.
        /// </summary>
        /// <param name="remove"></param>
        /// <returns></returns>
        public bool RemovePeripheral(Interfaces.Component remove)
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
        public bool DeletePeripheral(Interfaces.Component delete)
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

        /// <summary>
        /// Computes total cost of all components and returns as a double.
        /// </summary>
        /// <returns>Double</returns>
        public double GetTotalCost()
        {
            double total = 0;

            if (HardDrives != null)
            {
                foreach (IHardDrive hdd in HardDrives)
                {
                    total += hdd.GetTotalcost();
                }
            }

            if (Peripherals != null)
            {
                foreach (Interfaces.Component peri in Peripherals)
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
    }
}
