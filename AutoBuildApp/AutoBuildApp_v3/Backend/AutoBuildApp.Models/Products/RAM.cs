using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    /// <summary>
    /// Random Access Memory class of type IComponent.
    /// </summary>
    public class RAM : IComponent
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
        public List<string> Color { get; set; }
        public string FirstWordLat { get; set; }
        public string CASLat { get; set; }
        public double Voltage { get; set; }
        public List<int> Timing { get; set; }
        public string ErrCorrctionCode { get; set; }
        public string Registered { get; set; }
        public bool HeatSpreader { get; set; }
        public double Budget { get; set; }
        #endregion

        public RAM()
        {
            ProductImage = new List<byte[]>();
            Color = new List<string>();
            Timing = new List<int>();
        }

        /// <summary>
        /// Random Access Memory constructor to initialize all fields on creation.
        /// </summary>
        /// <param name="productType"></param>
        /// <param name="modelNumber"></param>
        /// <param name="productName"></param>
        /// <param name="manufacturerName"></param>
        /// <param name="quantity"></param>
        /// <param name="productImage"></param>
        /// <param name="price"></param>
        /// <param name="formFactor"></param>
        /// <param name="color"></param>
        /// <param name="firstWordLat"></param>
        /// <param name="cASLat"></param>
        /// <param name="voltage"></param>
        /// <param name="timing"></param>
        /// <param name="errCorrctionCode"></param>
        /// <param name="registered"></param>
        /// <param name="heatSpreader"></param>
        public RAM(ProductType productType, string modelNumber, string productName,
            string manufacturerName, int quantity, List<byte[]> productImage,
                double price, string formFactor, List<string> color,
                    string firstWordLat, string cASLat, double voltage,
                        List<int> timing, string errCorrctionCode,
                            string registered, bool heatSpreader)
        {
            ProductType = productType;
            ModelNumber = modelNumber;
            ProductName = productName;
            ManufacturerName = manufacturerName;
            Quantity = quantity;
            ProductImage = productImage;
            Price = price;
            FormFactor = formFactor;
            Color = color;
            FirstWordLat = firstWordLat;
            CASLat = cASLat;
            Voltage = voltage;
            Timing = timing;
            ErrCorrctionCode = errCorrctionCode;
            Registered = registered;
            HeatSpreader = heatSpreader;
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
