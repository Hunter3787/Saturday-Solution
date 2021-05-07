using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * Monitor class for the AutoBuild App that implements 
 * the IComponent interface.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class Monitor : Interfaces.Component
    {
        #region "Field Declarations: get; set;"
        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<string> ProductImageStrings { get; set; }
        public double Price { get; set; }
        public double Budget { get; set; }
        public double ScreenSize { get; set; }
        public string Resolution { get; set; }
        public double RefreshRate { get; set; }
        public double ResponseTime { get; set; }
        public List<string> Colors { get; set; }
        public int Brightness { get; set; }
        public bool Widescreen { get; set; }
        public bool CurvedScreen { get; set; }
        public string HDRTier { get; set; }
        public List<string> InterfacePorts { get; set; }
        public List<string> FrameSyncList { get; set; }
        public Dictionary<string, int> DisplayPortCounts { get; set; }
        public bool Speakers { get; set; }
        public string ViewingAngle { get; set; }
        #endregion

        public Monitor()
        {
            ProductImageStrings = new List<string>();
            Colors = new List<string>();
            InterfacePorts = new List<string>();
            FrameSyncList = new List<string>();
            DisplayPortCounts = new Dictionary<string, int>();
        }

        #region "Color Add/Remove"
        /// <summary>
        /// Add a string representation of a color to the monitor color list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddColor(string input)
        {
            ProductGuard.Exists(Colors, nameof(Colors));
            ProductGuard.IsNotEmpty(input, nameof(input));

            if (Colors.Contains(input))
            {
                return false;
            }

            Colors.Add(input);
            return true;
        }

        /// <summary>
        /// String method to locate the index of the passed string.
        /// Will call index variation to remove the element upon location.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>Boolean</returns>
        public bool RemoveColor(string toRemove)
        {
            ProductGuard.Exists(Colors, nameof(Colors));
            ProductGuard.IsNotEmpty(toRemove, nameof(toRemove));
            ProductGuard.ContainsElement(Colors, toRemove, nameof(Colors));

            return RemoveColor(Colors.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove a color from the monitor color list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveColor(int index)
        {
            ProductGuard.Exists(Colors, nameof(Colors));
            ProductGuard.IsInRange(Colors, index, nameof(Colors));

            Colors.RemoveAt(index);
            return true;
        }
        #endregion

        #region "I/O port Add/Remove"
        /// <summary>
        /// Add a string representation of a interface ports to the monitor
        /// InterfacePorts list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddInterfacePort(string input)
        {
            ProductGuard.Exists(InterfacePorts, nameof(InterfacePorts));
            ProductGuard.IsNotEmpty(input, nameof(input));

            Colors.Add(input);
            return true;
        }

        /// <summary>
        /// String method to locate the index of the passed string.
        /// Will call index variation to remove the element upon location.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>Boolean</returns>
        public bool RemoveInterfacePort(string toRemove)
        {
            ProductGuard.Exists(InterfacePorts, nameof(Colors));
            ProductGuard.IsNotEmpty(toRemove, nameof(toRemove));
            ProductGuard.ContainsElement(InterfacePorts, toRemove, nameof(InterfacePorts));

            return RemoveInterfacePort(InterfacePorts.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove an element from the monitor interface port list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveInterfacePort(int index)
        {
            ProductGuard.Exists(Colors, nameof(Colors));
            ProductGuard.IsInRange(Colors, index, nameof(Colors));

            InterfacePorts.RemoveAt(index);
            return true;
        }
        #endregion

        #region "Frame sync Add/Remove"
        /// <summary>
        /// Add a string representation of a interface port to the monitor
        /// InterfacePorts list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddFrameSync(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || FrameSyncList.Contains(input))
                return false;

            FrameSyncList.Add(input);
            return true;
        }

        /// <summary>
        /// String method to locate the index of the passed string.
        /// Will call index variation to remove the element upon location.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>Boolean</returns>
        public bool RemoveFrameSync(string toRemove)
        {
            if (FrameSyncList == null || string.IsNullOrWhiteSpace(toRemove) || !FrameSyncList.Contains(toRemove))
                return false;

            return RemoveFrameSync(FrameSyncList.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove an element from the monitor interface port list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveFrameSync(int index)
        {
            if (FrameSyncList == null || index > FrameSyncList.Count || index < ProductGlobals.MIN_INDEX)
                return false;

            FrameSyncList.RemoveAt(index);
            return true;
        }
        #endregion

        #region "Display port count Add/Remove"
        /// <summary>
        /// Add a key value pair representing a display port type (key)
        /// and how many (value).
        /// </summary>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns>Boolean</returns>
        public bool AddDisplayPort(string input, int value)
        {
            if (string.IsNullOrWhiteSpace(input)
                || value < ProductGlobals.MIN_VALUE
                || DisplayPortCounts.ContainsKey(input))
            {
                return false;
            }

            DisplayPortCounts.Add(input, value);
            return true;
        }

        /// <summary>
        /// Set display port count based on a string key value.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetDisplayPortCount(string input, int value)
        {
            if (DisplayPortCounts == null
                || string.IsNullOrWhiteSpace(input)
                || value < ProductGlobals.MIN_VALUE)
            {
                return false;
            }

            // If display port does not yet contain the key, call add for convenience.
            if (!DisplayPortCounts.ContainsKey(input))
            {
                return AddDisplayPort(input, value);
            }

            DisplayPortCounts[input] = value;
            return true;
        }

        /// <summary>
        /// Removes the designated key and its associated value from the dictionary.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>Boolean</returns>
        public bool RemoveDisplayPort(string toRemove)
        {
            if (DisplayPortCounts == null
                || string.IsNullOrWhiteSpace(toRemove)
                || !DisplayPortCounts.ContainsKey(toRemove))
            {
                return false;
            }

            return DisplayPortCounts.Remove(toRemove);
        }
        #endregion

        #region "Interface Implementations"
        /// <summary>
        /// Adds an image from a byte array to the component.
        /// </summary>
        /// <param name="image">Byte Array representing an image.</param>
        /// <returns></returns>
        public bool AddImage(string location)
        {
            ProductGuard.Exists(ProductImageStrings, nameof(ProductImageStrings));
            ProductGuard.IsNotEmpty(location, nameof(location));

            ProductImageStrings.Add(location);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool RemoveImage(string location)
        {
            ProductGuard.Exists(ProductImageStrings, nameof(ProductImageStrings));
            ProductGuard.IsNotEmpty(location, nameof(location));
            ProductGuard.ContainsElement(ProductImageStrings, location, nameof(location));

            var index = ProductImageStrings.IndexOf(location);
            return RemoveImage(index);
        }

        /// <summary>
        /// Removes an image from the byte array at the provided index.
        /// </summary>
        /// <param name="index">Position of the image intended to be deleted.</param>
        /// <returns></returns>
        public bool RemoveImage(int index)
        {
            ProductGuard.Exists(ProductImageStrings, nameof(ProductImageStrings));
            ProductGuard.IsInRange(ProductImageStrings, index, nameof(ProductImageStrings));

            ProductImageStrings.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Total cost of components based on quantity and price.
        /// </summary>
        /// <returns>Double</returns>
        public double GetTotalcost()
        {
            return Price * Quantity;
        }
        #endregion
    }
}
