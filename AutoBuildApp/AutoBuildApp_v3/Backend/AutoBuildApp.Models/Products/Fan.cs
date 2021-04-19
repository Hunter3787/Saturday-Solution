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

        #region "Field Declarations: get; set;"
        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<string> ProductImageStrings { get; set; }
        public double Price { get; set; }
        public double Budget { get; set; }
        public List<string> Colors { get; set; }
        public string FanRPM { get; set; }
        public string NoiseVolume { get; set; }
        public List<string> CompatableSockets { get; set; }
        public int FanSize { get; set; }
        public bool Fanless { get; set; }
        public bool WaterCooling { get; set; }
        #endregion

        public Fan()
        {
            ProductImageStrings = new List<string>();
            Colors = new List<string>();
            CompatableSockets = new List<string>();
        }

        #region "Color Add/Remove"
        /// <summary>
        /// Add a string representation of a color to the fan color list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddColor(string input)
        {
            ProductGuard.Exists(Colors, nameof(Colors));
            ProductGuard.IsNotEmpty(input, nameof(input));

            Colors.Add(input);
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
            ProductGuard.Exists(Colors, nameof(Colors));
            ProductGuard.IsNotEmpty(toRemove, nameof(toRemove));
            ProductGuard.ContainsElement(Colors, toRemove, nameof(toRemove));

            return RemoveColor(Colors.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove a color from the fan color list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveColor(int index)
        {
            ProductGuard.Exists(Colors, nameof(Colors));
            ProductGuard.IsInRange(Colors, index, nameof(Colors));

            Colors.RemoveAt(index);
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
            ProductGuard.Exists(CompatableSockets, nameof(CompatableSockets));
            ProductGuard.IsNotEmpty(input, nameof(input));

            CompatableSockets.Add(input);
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
            ProductGuard.Exists(CompatableSockets, nameof(CompatableSockets));
            ProductGuard.IsNotEmpty(toRemove, nameof(toRemove));
            ProductGuard.ContainsElement(CompatableSockets, toRemove, nameof(toRemove));

            return RemoveCompatableSocket(CompatableSockets.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove a color from the fan color list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveCompatableSocket(int index)
        {
            ProductGuard.Exists(CompatableSockets, nameof(CompatableSockets));
            ProductGuard.IsInRange(CompatableSockets, index, nameof(CompatableSockets));

            CompatableSockets.RemoveAt(index);
            return true;
        }
        #endregion

        #region "Interface Implementations"
        /// <summary>
        /// Adds an image from a byte array to the component.
        /// </summary>
        /// <param name="image">Byte Array representing an image.</param>
        /// <returns></returns>
        public bool AddImage(string location)
        {
            ProductGuard.Exists(ProductImageStrings, nameof(ProductImageStrings));
            ProductGuard.IsNotEmpty(location, nameof(location));

            ProductImageStrings.Add(location);
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool RemoveImage(string location)
        {
            ProductGuard.Exists(ProductImageStrings, nameof(ProductImageStrings));
            ProductGuard.IsNotEmpty(location, nameof(location));
            ProductGuard.ContainsElement(ProductImageStrings, location, nameof(location));

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
            ProductGuard.Exists(ProductImageStrings, nameof(ProductImageStrings));
            ProductGuard.IsInRange(ProductImageStrings, index, nameof(ProductImageStrings));

            ProductImageStrings.RemoveAt(index);
            return true;
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
