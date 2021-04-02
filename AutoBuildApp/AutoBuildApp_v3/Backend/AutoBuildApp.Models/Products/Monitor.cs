using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    public class Monitor : IComponent
    {
        #region "Field Declarations, get; set;"
        private const int MIN_LIST_SIZE = 1;
        private const int MIN_INDEX = 0;
        private const int MIN_VALUE = 0;

        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<byte[]> ProductImage { get; set; }
        public double Price { get; set; }
        public double Budget { get; set; }
        public double ScreenSize { get; set; }
        public string Resolution { get; set; }
        public double RefreshRate { get; set; }
        public double ResponseTime { get; set; }
        public List<string> Color { get; set; }
        public int Brightness { get; set; }
        public bool Widescreen { get; set; }
        public bool CurvedScreen { get; set; }
        public string HDRTier { get; set; }
        public List<string> InterfacePort { get; set; }
        public List<string> FrameSync { get; set; }
        public Dictionary<string, int> DisplayPortCount { get; set; }
        public bool Speakers { get; set; }
        public string ViewingAngle { get; set; }
        #endregion

        public Monitor()
        {
        }

        #region "Color Add/Remove"
        /// <summary>
        /// Add a string representation of a color to the monitor color list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddColor(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || Color.Contains(input))
                return false;

            Color.Add(input);
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
            if (string.IsNullOrWhiteSpace(toRemove) || !Color.Contains(toRemove))
                return false;

            return RemoveColor(Color.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove a color from the monitor color list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveColor(int index)
        {
            if (index > Color.Count || index < MIN_INDEX)
                return false;

            Color.RemoveAt(index);
            return true;
        }
        #endregion

        #region "Interface port Add/Remove"
        /// <summary>
        /// Add a string representation of a interface port to the monitor
        /// InterfacePorts list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddInterfacePort(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || InterfacePort.Contains(input))
                return false;

            Color.Add(input);
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
            if (string.IsNullOrWhiteSpace(toRemove) || !InterfacePort.Contains(toRemove))
                return false;

            return RemoveInterfacePort(InterfacePort.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove an element from the monitor interface port list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveInterfacePort(int index)
        {
            if (index > InterfacePort.Count || index < MIN_INDEX)
                return false;

            InterfacePort.RemoveAt(index);
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
            if (string.IsNullOrWhiteSpace(input) || FrameSync.Contains(input))
                return false;

            FrameSync.Add(input);
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
            if (string.IsNullOrWhiteSpace(toRemove) || !FrameSync.Contains(toRemove))
                return false;

            return RemoveFrameSync(FrameSync.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove an element from the monitor interface port list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveFrameSync(int index)
        {
            if (index > FrameSync.Count || index < MIN_INDEX)
                return false;

            FrameSync.RemoveAt(index);
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
            if (string.IsNullOrWhiteSpace(input) || value < MIN_VALUE ||
                    DisplayPortCount.ContainsKey(input))
                return false;

            DisplayPortCount.Add(input, value);
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

            if (string.IsNullOrWhiteSpace(input) || value < MIN_VALUE)
                return false;

            // If display port does not yet contain the key, call add for convenience.
            if (!DisplayPortCount.ContainsKey(input))
                return AddDisplayPort(input, value);

            DisplayPortCount[input] = value;
            return true;
        }

        /// <summary>
        /// Removes the designated key and its associated value from the dictionary.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>Boolean</returns>
        public bool RemoveDisplayPort(string toRemove)
        {
            if (string.IsNullOrWhiteSpace(toRemove) ||
                    !DisplayPortCount.ContainsKey(toRemove))
                return false;
            return DisplayPortCount.Remove(toRemove);
        }
        #endregion

        #region "Interface Implementations"
        /// <summary>
        /// Adds an image from a byte array to the component.
        /// </summary>
        /// <param name="image">Byte Array representing an image.</param>
        /// <returns></returns>
        public bool AddImage(byte[] image)
        {
            if (image == null)
                return false;

            ProductImage.Add(image);
            return true;
        }

        /// <summary>
        /// Removes an image from the byte array at the provided index.
        /// </summary>
        /// <param name="index">Position of the image intended to be deleted.</param>
        /// <returns></returns>
        public bool RemoveImage(int index)
        {
            var success = false;
            var endOfList = ProductImage.Count - 1;

            if (index >= MIN_INDEX && ProductImage.Count >= MIN_LIST_SIZE
                && index <= endOfList)
            {
                ProductImage.RemoveAt(index);
                success = true;
            }

            return success;
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
