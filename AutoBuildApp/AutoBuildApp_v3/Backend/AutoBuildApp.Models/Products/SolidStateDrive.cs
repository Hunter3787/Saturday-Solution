using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    public class SolidStateDrive : IHardDrive, IComponent
    {
        public HardDriveType HardDriveType { get; set; }
        public string ModelNumber { get; set; }
        public ProductType ProductType { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<byte[]> ProductImage { get; set; }
        public double Price { get; set; }
        public double Budget { get; set; }
        public HardDriveType HardDrive { get; set; }

        public SolidStateDrive()
        {
        }

        public bool AddImage(byte[] image)
        {
            throw new NotImplementedException();
        }

        public bool RemoveImage(int index)
        {
            throw new NotImplementedException();
        }
    }
}
