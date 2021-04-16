using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * Water cooler class for the AutoBuild App.
 * Implements the IComponent and ICooler interfaces.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class WaterCooler : IComponent, ICooler
    {
        #region "Field Declarations, get; set;"
        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<string> ProductImageStrings { get; set; }
        public double Price { get; set; }
        public double Budget { get; set; }
        public string FanRPM { get; set; }
        public string NoiseVolume { get; set; }
        public List<string> CompatableSocket { get; set; }
        public bool Fanless { get; set; }
        #endregion

        public WaterCooler()
        {
            ProductImageStrings = new List<string>();
            CompatableSocket = new List<string>();
        }

        #region "Interface Implementations"
        /// <summary>
        /// Adds an image from a byte array to the component.
        /// </summary>
        /// <param name="location">Byte Array representing an image.</param>
        /// <returns></returns>
        public bool AddImage(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return false;
            }
                
            ProductImageStrings.Add(location);
            return true;
        }


        public bool RemoveImage(string location)
        {

            if (ProductImageStrings == null
                || string.IsNullOrWhiteSpace(location)
                || !ProductImageStrings.Contains(location))
            {
                return false;
            }

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
            if (ProductImageStrings == null)
                return false;

            var success = false;
            var endOfList = ProductImageStrings.Count - 1;

            if (index >= ProductGlobals.MIN_INDEX
                && ProductImageStrings.Count >= ProductGlobals.MIN_LIST_SIZE
                && index <= endOfList)
            {
                ProductImageStrings.RemoveAt(index);
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
