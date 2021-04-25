﻿using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * NVMe internal hard drive class that implements the IHardDrive 
 * and IComponent interfaces. 
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class NVMeDrive : IHardDrive, IComponent
    {
        #region "Field Declarations: get; set;"
        public HardDriveType DriveType { get; set; }
        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<string> ProductImageStrings { get; set; }
        public double Price { get; set; }
        public double Budget { get; set; }
        public string Capacity { get; set; }
        public string Cache { get; set; }
        public string FormFactor { get; set; }
        public string Interface { get; set; }
        public bool NVMe { get; set; }
        #endregion

        /// <summary>
        /// Default constructor with no initialization.
        /// </summary>
        public NVMeDrive()
        {
            ProductImageStrings = new List<string>();
            NVMe = true;
            DriveType = HardDriveType.NVMe;
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
