using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * Random Access Memory class that invokes the IComonent interface.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    /// <summary>
    /// Random Access Memory class of type IComponent.
    /// </summary>
    public class RAM : IComponent
    {
        #region "Field Declarations, get; set;"
        public readonly int MIN_LIST_SIZE = 1;
        public readonly int MIN_INDEX = 0;

        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<byte[]> ProductImages { get; set; }
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
            ProductImages = new List<byte[]>();
            Color = new List<string>();
            Timing = new List<int>();
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

            if (index >= MIN_INDEX && ProductImages.Count >= MIN_LIST_SIZE
                && index <= endOfList)
            {
                ProductImages.RemoveAt(index);
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
