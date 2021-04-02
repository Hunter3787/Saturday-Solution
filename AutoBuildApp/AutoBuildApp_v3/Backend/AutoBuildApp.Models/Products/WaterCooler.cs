using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    public class WaterCooler : IComponent
    {
        public WaterCooler()
        {
        }

        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<byte[]> ProductImage { get; set; }
        public double Price { get; set; }
        public double Budget { get; set; }

        #region "Interface Implementations"
        public bool AddImage(byte[] image)
        {
            throw new NotImplementedException();
        }

        public bool RemoveImage(int index)
        {
            throw new NotImplementedException();
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
