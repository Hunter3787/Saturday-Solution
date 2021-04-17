using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * Graphics Processing Unit class for use with AutoBuild App
 * that implements the IComponent interface.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class GPU : IComponent
    {
        #region "Field Declarations: get; set;"
        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public List<string> ProductImageStrings { get; set; }
        public double Price { get; set; }
        public string Chipset { get; set; }
        public string Memory { get; set; }
        public string MemoryType { get; set; }
        public string CoreClock { get; set; }
        public string BoostClock { get; set; }
        public string EffectiveMemClock { get; set; }
        public string Interface { get; set; }
        public string Color { get; set; }
        public string FrameSync { get; set; }
        public double PowerDraw { get; set; }
        public int Length { get; set; }
        public int Quantity { get; set; }
        public int DVIPorts { get; set; }
        public int HDMIPorts { get; set; }
        public int MiniHDMIPorts { get; set; }
        public int DisplayPortPorts { get; set; }
        public int MiniDisplayPortPorts { get; set; }
        public int ExpansionSlotWidth { get; set; }
        public int Cooling { get; set; }
        public string ExternalPower { get; set; }
        public double Budget { get; set; }
        #endregion

        /// <summary>
        /// Graphic Processing Unit Default Constructor.
        /// </summary>
        public GPU()
        {
            ProductImageStrings = new List<string>();
        }

        #region "Interface Implementations"
        /// <summary>
        /// Adds an image from a byte array to the component.
        /// </summary>
        /// <param name="image">Byte Array representing an image.</param>
        /// <returns></returns>
        public bool AddImage(string location)
        {
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
