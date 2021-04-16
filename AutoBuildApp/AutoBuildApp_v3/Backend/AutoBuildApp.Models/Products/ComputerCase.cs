using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/// <summary>
/// Computer case model, dimensions are are in
/// Millimeters to be converted later.
/// </summary>
namespace AutoBuildApp.Models.Products
{
    public class ComputerCase : IComponent
    {
        #region "Field Declarations, get; set;"
        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<byte[]> ProductImages { get; set; }
        public double Price { get; set; }
        public double Budget { get; set; }
        public List<MoboFormFactor> MoboFormSupport { get; set; }
        public TowerType TowerType { get; set; }
        public int ExpansionSlots { get; set; }
        public bool PsuShroud { get; set; }
        public string SidePanel { get; set; }
        public List<string> Color { get; set; }
        public int TwoInchDriveBays { get; set; }
        public int ThreeInchDriveBays { get; set; }
        public List<string> FrontPanel { get; set; }
        public int MaxGPULength { get; set; }
        public List<double> Dimensions { get; set; }
        #endregion

        /// <summary>
        /// Default constructor with no initilization.
        /// </summary>
        public ComputerCase()
        {
            ProductImages = new List<byte[]>();
            MoboFormSupport = new List<MoboFormFactor>();
            Color = new List<string>();
            FrontPanel = new List<string>();
            Dimensions = new List<double>();
        }

        #region "Form Factor Add/Remove"
        /// <summary>
        /// Add a form factor to the supported form factor list.
        /// </summary>
        /// <param name="input">MoboFormFactor of Type Enumeration</param>
        /// <returns>Boolean</returns>
        public bool AddFormFactorSupport(MoboFormFactor input)
        {
            if (MoboFormSupport == null || MoboFormSupport.Contains(input))
            {
                return false;
            }

            MoboFormSupport.Add(input);
            return true;
        }

        /// <summary>
        /// Passes an Enum of motherboard form factor to locate
        /// and remove from the list.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>Boolean</returns>
        public bool RemoveFormFactorSupport(MoboFormFactor toRemove)
        {
            if (MoboFormSupport == null || !MoboFormSupport.Contains(toRemove))
            {
                return false;
            }
                
            return RemoveFormFactorSupport(MoboFormSupport.IndexOf(toRemove));
        }

        /// <summary>
        /// Remove Motherboard form factor support.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveFormFactorSupport(int index)
        {
            if (index > MoboFormSupport.Count || index < ProductGlobals.MIN_INDEX)
            {
                return false;
            }
                
            MoboFormSupport.RemoveAt(index);
            return true;
        }
        #endregion

        #region "Color Add/Remove"
        /// <summary>
        /// Add a string representation of a color to the computer case color list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddColor(string input)
        {
            if (Color == null
                || string.IsNullOrWhiteSpace(input)
                || Color.Contains(input))
            {
                return false;
            }

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
            if (Color == null
                || string.IsNullOrWhiteSpace(toRemove)
                || !Color.Contains(toRemove))
            {
                return false;
            }

            return RemoveColor(Color.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove a color from the computer case color list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveColor(int index)
        { 
            if (Color == null
                || index > Color.Count
                || index < ProductGlobals.MIN_INDEX)
            {
                return false;
            }

            Color.RemoveAt(index);
            return true;
        }
        #endregion

        #region "Front Panel Add/Remove"
        /// <summary>
        /// Add a string representation of a front panel to the computer case
        /// front panel list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddFrontPanel(string input)
        {
            if (FrontPanel == null
                || string.IsNullOrWhiteSpace(input)
                || FrontPanel.Contains(input))
            {
                return false;
            }

            FrontPanel.Add(input);
            return true;
        }

        /// <summary>
        /// String method to locate the index of the passed string.
        /// Will call index variation to remove the element upon location.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>Boolean</returns>
        public bool RemoveFrontPanel(string toRemove)
        {
            if (FrontPanel == null
                || string.IsNullOrWhiteSpace(toRemove)
                || !FrontPanel.Contains(toRemove))
            {
                return false;
            }

            return RemoveFrontPanel(FrontPanel.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove a front panel element from the computer case
        /// front panel list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveFrontPanel(int index)
        {
            if (FrontPanel == null
                || index > FrontPanel.Count
                || index < ProductGlobals.MIN_INDEX)
            {
                return false;
            }

            FrontPanel.RemoveAt(index);
            return true;
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
            
            ProductImages.Add(image);
            return true;
        }

        /// <summary>
        /// Removes an image from the byte array at the provided index.
        /// </summary>
        /// <param name="index">Position of the image intended to be deleted.</param>
        /// <returns></returns>
        public bool RemoveImage(int index)
        {
            if (ProductImages == null)
                return false;

            var success = false;
            var endOfList = ProductImages.Count - 1;

            if(index >= ProductGlobals.MIN_INDEX && ProductImages.Count >= ProductGlobals.MIN_LIST_SIZE
                && index <= endOfList)
            {
                ProductImages.RemoveAt(index);
                success =  true;
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
