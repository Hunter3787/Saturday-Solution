using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;

/// <summary>
/// Computer case model, dimensions are are in
/// Millimeters to be converted later.
/// </summary>
namespace AutoBuildApp.Models.Products
{
    public class ComputerCase : Component
    {
        #region "Field Declarations: get; set;"
        public List<string> MoboFormSupport { get; set; }
        public TowerType TowerType { get; set; }
        public int ExpansionSlots { get; set; }
        public bool PsuShroud { get; set; }
        public string SidePanel { get; set; }
        public List<string> Color { get; set; }
        public int TwoInchDriveBays { get; set; }
        public int ThreeInchDriveBays { get; set; }
        public List<string> FrontPanel { get; set; }
        public int MaxGPULength { get; set; }
        public int MaxPSULength { get; set; }
        public List<double> Dimensions { get; set; }
        #endregion

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ComputerCase() : base()
        {
            ProductType = ProductType.Case;
            MoboFormSupport = new List<string>();
            Color = new List<string>();
            FrontPanel = new List<string>();
            Dimensions = new List<double>();
        }

        #region "Form Factor Add/Remove"
        /// <summary>
        /// Add a form factor to the supported form factor list.
        /// </summary>
        /// <param name="input">string of Type Enumeration</param>
        /// <returns>Boolean</returns>
        public bool AddFormFactorSupport(string input)
        {
            if (input == null || MoboFormSupport.Contains(input))
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
        public bool RemoveFormFactorSupport(string toRemove)
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
    }
}
