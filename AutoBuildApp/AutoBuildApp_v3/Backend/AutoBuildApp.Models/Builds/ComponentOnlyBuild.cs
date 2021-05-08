using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Products;

namespace AutoBuildApp.Models.Builds
{
    public class ComponentOnlyBuild
    {
        public string BuildName;
        public List<Component> HardDrives { get; set; }
        public Component Case { get; set; }
        public Component Mobo { get; set; }
        public Component Psu { get; set; }
        public Component Gpu { get; set; }
        public Component Cpu { get; set; }
        public Component Ram { get; set; }
        public Component CPUCooler { get; set; }
        public List<Component> Peripherals { get; set; }
        public List<string> ImageStrings { get; set; }

        public ComponentOnlyBuild()
        {
            HardDrives = new List<Component>();
            Case = new Component();
            Mobo = new Component();
            Psu = new Component();
            Gpu = new Component();
            Cpu = new Component();
            Ram = new Component();
            CPUCooler = new Component();
            Peripherals = new List<Component>();
            ImageStrings = new List<string>();
        }

        #region "IHardDrive List Add/Remove/Delete methods"
        /// <summary>
        /// Add hard drive to the list, if hard drive is already present
        /// increment the quantity.
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        public bool AddHardDrive(Component add)
        {
            if (add == null)
            {
                return false;
            }

            if (HardDrives == null)
            {
                HardDrives = new List<Component>();
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
        public bool RemoveHardDrive(Component remove)
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
        public bool DeleteHardDrive(Component delete)
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
        public bool AddPeripheral(Component add)
        {
            if (add == null)
            {
                return false;
            }

            if (Peripherals == null)
            {
                Peripherals = new List<Component>();
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
        public bool RemovePeripheral(Component remove)
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
        public bool DeletePeripheral(Component delete)
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
                foreach (Component hdd in HardDrives)
                {
                    total += hdd.GetTotalcost();
                }
            }

            if (Peripherals != null)
            {
                foreach (Component peri in Peripherals)
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
