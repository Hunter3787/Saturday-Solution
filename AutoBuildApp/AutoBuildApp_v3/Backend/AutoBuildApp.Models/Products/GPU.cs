using System;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    public class GPU : IComponent
    {
        public GPU()
        {
        }

        public string modelNumber { get; set; }
        public string productType { get; set; }
        public string productName { get; set; }
        public string manufacturerName { get; set; }
    }
}
