using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    public class GPU : IComponent
    {
        #region "Field Declarations, get; set;"
        private const int MIN_LIST_SIZE = 1;
        private const int MIN_INDEX = 0;

        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public List<byte[]> ProductImage { get; set; }
        public double Price { get; set; }
        public string Chipset { get; set; }
        public string Memory { get; set; }
        public string CoreClock { get; set; }
        public string BoostClock { get; set; }
        public string EffctvMemoryClcok { get; set; }
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
        public double Budget { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion

        /// <summary>
        /// Graphic Processing Unit Default Constructor.
        /// </summary>
        public GPU()
        {
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
