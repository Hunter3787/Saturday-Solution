using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * Central Processing unit class that implments the IComponent interface.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class CPU : IComponent
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
        public int CoreCount { get; set; }
        public string CoreClock { get; set; }
        public string BoostClock { get; set; }
        public double PowerDraw { get; set; }
        public string Series { get; set; }
        public string MicrorArchitecture { get; set; }
        public string CoreFamily { get; set; }
        public string Socket { get; set; }
        public string IntegratedGraphics { get; set; }
        public string MaxRam { get; set; }
        public string ErrCorrectionCodeSupport { get; set; }
        public string Packaging { get; set; }
        public List<string> L1Cache { get; set; }
        public List<string> L2Cache { get; set; }
        public List<string> L3Cache { get; set; }
        public string Lithograph { get; set; }
        public string HyperThreading { get; set; }
        public double Budget { get; set; }
        #endregion

        /// <summary>
        /// Default consturctor.
        /// </summary>
        public CPU()
        {
            ProductImages = new List<byte[]>();
            L1Cache = new List<string>();
            L2Cache = new List<string>();
            L3Cache = new List<string>();
        }

        #region "Interface Implementations"
        /// <summary>
        /// Adds an image from a byte array to the component.
        /// </summary>
        /// <param name="image">Byte Array representing an image.</param>
        /// <returns>Success of addition from object.</returns>
        public bool AddImage(byte[] image)
        {
            if (image == null)
                return false;

            ProductImages.Add(image);
            return true;
        }

        /// <summary>
        /// Removes an 
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Success of removal from object.</returns>
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
