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
        private const int MIN_LIST_SIZE = 1;
        private const int MIN_INDEX = 0;

        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<byte[]> ProductImage { get; set; }
        public double Price { get; set; }
        public List<FormFactor> MotherboardSupport { get; set; }
        public TowerType TowerType { get; set; }
        public int ExpansionSlots { get; set; }
        public bool PsuShroud { get; set; }
        public string SidePanel { get; set; }
        public List<string> Color { get; set; }
        public int TwoInchDriveBays { get; set; }
        public int ThreeInchDriveBays { get; set; }
        public List<string> FrontPanel { get; set; }
        public int MaxGPULength { get; set; }
        public double[,,] Dimension { get; set; }
        #endregion

        /// <summary>
        /// Default constructor with no initilization.
        /// </summary>
        public ComputerCase()
        {

        }

        /// <summary>
        /// ComputerCase constructor to initialize all fields on creation.
        /// </summary>
        /// <param name="productType"></param>
        /// <param name="towerType"></param>
        /// <param name="modelNumber"></param>
        /// <param name="productName"></param>
        /// <param name="manufacturerName"></param>
        /// <param name="quantity"></param>
        /// <param name="motherboardSupport"></param>
        /// <param name="expansionSlots"></param>
        /// <param name="psuShroud"></param>
        /// <param name="sidePanel"></param>
        /// <param name="color"></param>
        /// <param name="twoInchBays"></param>
        /// <param name="threeInchBays"></param>
        /// <param name="frontPanel"></param>
        /// <param name="maxGPULength"></param>
        /// <param name="dimension"></param>
        public ComputerCase(ProductType productType, TowerType towerType,
            string modelNumber, string productName, string manufacturerName,
                int quantity, List<FormFactor> motherboardSupport, int expansionSlots,
                    bool psuShroud, string sidePanel, List<string> color, int twoInchDriveBays,
                        int threeInchDriveBays, List<string> frontPanel, int maxGPULength,
                            double[,,] dimension)
        {
            this.ProductType = productType;
            this.TowerType = towerType;
            this.ModelNumber = modelNumber;
            this.ProductName = productName;
            this.ManufacturerName = manufacturerName;
            this.Quantity = quantity;
            this.MotherboardSupport = motherboardSupport;
            this.ExpansionSlots = expansionSlots;
            this.PsuShroud = psuShroud;
            this.SidePanel = sidePanel;
            this.Color = color;
            this.TwoInchDriveBays = twoInchDriveBays;
            this.ThreeInchDriveBays = threeInchDriveBays;
            this.FrontPanel = frontPanel;
            this.MaxGPULength = maxGPULength;
            this.Dimension = dimension;
        }


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

            if(index >= MIN_INDEX && ProductImage.Count >= MIN_LIST_SIZE
                && index <= endOfList)
            {
                ProductImage.RemoveAt(index);
                success =  true;
            }

            return success;
        }
        #endregion
    }
}
