using System;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    public class SolidStateDrive : IHardDrive, IComponent
    {
        public HardDriveType HardDriveType { get; set; }
        public string ModelNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ProductType ProductType { get; set => throw new NotImplementedException(); }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }

        public SolidStateDrive()
        {
        }
    }
}
