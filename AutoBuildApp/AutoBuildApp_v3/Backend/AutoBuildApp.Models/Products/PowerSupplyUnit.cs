using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    public class PowerSupplyUnit : IComponent
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
        public string FormFactor { get; set; }
        public int Wattage { get; set; }
        public int Length { get; set; }
        public string EfficiencyRating { get; set; }
        public bool Fanless { get; set; }
        public PSUModularity PsuType { get; set; }
        public int EPSConnectors { get; set; }
        public int SataConnectors { get; set; }
        public int MolexConnectors { get; set; }
        public int SixPlusTwoConnectors { get; set; }
        #endregion

        /// <summary>
        /// PowerSupplyUnit default constructor.
        /// </summary>
        public PowerSupplyUnit()
        {

        }

        /// <summary>
        /// PowerSupplyUnit 
        /// </summary>
        /// <param name="productType"></param>
        /// <param name="modelNumber"></param>
        /// <param name="productName"></param>
        /// <param name="manufacturerName"></param>
        /// <param name="quantity"></param>
        /// <param name="productImage"></param>
        /// <param name="price"></param>
        /// <param name="formFactor"></param>
        /// <param name="wattage"></param>
        /// <param name="length"></param>
        /// <param name="efficiencyRating"></param>
        /// <param name="fanless"></param>
        /// <param name="psuType"></param>
        /// <param name="ePSConnectors"></param>
        /// <param name="sataConnectors"></param>
        /// <param name="molexConnectors"></param>
        /// <param name="sixPlusTwoConnectors"></param>
        public PowerSupplyUnit(ProductType productType, string modelNumber,
            string productName, string manufacturerName, int quantity,
                List<byte[]> productImage, double price, string formFactor,
                    int wattage, int length, string efficiencyRating, bool fanless,
                        PSUModularity psuType, int ePSConnectors, int sataConnectors,
                            int molexConnectors, int sixPlusTwoConnectors)
        {
            ProductType = productType;
            ModelNumber = modelNumber;
            ProductName = productName;
            ManufacturerName = manufacturerName;
            Quantity = quantity;
            ProductImage = productImage;
            Price = price;
            FormFactor = formFactor;
            Wattage = wattage;
            Length = length;
            EfficiencyRating = efficiencyRating;
            Fanless = fanless;
            PsuType = psuType;
            EPSConnectors = ePSConnectors;
            SataConnectors = sataConnectors;
            MolexConnectors = molexConnectors;
            SixPlusTwoConnectors = sixPlusTwoConnectors;
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

            if (index >= MIN_INDEX && ProductImage.Count >= MIN_LIST_SIZE
                && index <= endOfList)
            {
                ProductImage.RemoveAt(index);
                success = true;
            }

            return success;
        }
        #endregion
    }
}
