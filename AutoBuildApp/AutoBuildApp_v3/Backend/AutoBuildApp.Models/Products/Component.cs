using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    public class Component : IComponent
    {
        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<string> ProductImageStrings { get; set; }
        public double Price { get; set; }
        public double Budget { get; set; }

        public Component()
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
            ProductGuard.Exists(ProductImageStrings, nameof(ProductImageStrings));
            ProductGuard.IsNotEmpty(location, nameof(location));

            ProductImageStrings.Add(location);
            return true;
        }

        /// <summary>
        /// String method to locate the index of the passed string.
        /// Will call index variation to remove the element upon location.
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

        #region Overridden methods
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Component))
            {
                return false;
            }

            if (ProductType == ((Component)obj).ProductType
                && ModelNumber == ((Component)obj).ModelNumber
                && ProductName == ((Component)obj).ProductName
                && ManufacturerName == ((Component)obj).ManufacturerName
                && Price == ((Component)obj).Price)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
