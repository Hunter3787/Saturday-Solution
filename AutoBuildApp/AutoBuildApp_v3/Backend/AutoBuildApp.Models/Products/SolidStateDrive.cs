using System;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    public class SolidStateDrive : IHardDrive, IComponent
    {
        public HardDriveType hardDriveType { get; set; }
        public string modelNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ProductType productType { get; set => throw new NotImplementedException(); }
        public string productName { get; set; }
        public string manufacturerName { get; set; }
        public int quantity { get; set; }

        public SolidStateDrive()
        {
        }
    }
}
