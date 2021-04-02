using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * Fan class of ICooler implimenting both IComponent and ICooler.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class Fan : IComponent, ICooler
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
        public double Budget { get; set; }
        public List<string> Color { get; set; }
        public string FanRPM { get; set; }
        public string NoiseVolume { get; set; }
        public List<string> CompatableSocket { get; set; }
        public bool Fanless { get; set; }
        public string WaterCooling { get; set; }
        #endregion

        public Fan()
        {
            ProductImages = new List<byte[]>();
            Color = new List<string>();
            CompatableSocket = new List<string>();
        }

        #region "Color Add/Remove"
        /// <summary>
        /// Add a string representation of a color to the fan color list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddColor(string input)
        {
            if (Color == null || string.IsNullOrWhiteSpace(input) || Color.Contains(input))
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
            if (Color == null || string.IsNullOrWhiteSpace(toRemove) || !Color.Contains(toRemove))
                return false;

            return RemoveColor(Color.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove a color from the fan color list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveColor(int index)
        {
            if (Color == null || index > Color.Count || index < MIN_INDEX)
                return false;

            Color.RemoveAt(index);
            return true;
        }
        #endregion

        #region "Compatable Socket Add/Remove"
        /// <summary>
        /// Add a string representation of a color to the fan color list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddCompatableSocket(string input)
        {
            if (CompatableSocket == null || string.IsNullOrWhiteSpace(input) || CompatableSocket.Contains(input))
                return false;

            CompatableSocket.Add(input);
            return true;
        }

        /// <summary>
        /// String method to locate the index of the passed string.
        /// Will call index variation to remove the element upon location.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>Boolean</returns>
        public bool RemoveCompatableSocket(string toRemove)
        {
            if (CompatableSocket == null || string.IsNullOrWhiteSpace(toRemove) || !CompatableSocket.Contains(toRemove))
                return false;

            return RemoveCompatableSocket(CompatableSocket.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove a color from the fan color list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveCompatableSocket(int index)
        {
            if (CompatableSocket == null || index > CompatableSocket.Count || index < MIN_INDEX)
                return false;

            CompatableSocket.RemoveAt(index);
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
