using System;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    public class GPU : IComponent
    {
        public GPU()
        {
        }

        public string ModelNumber { get; set; }
        public string productType { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
    }
}
