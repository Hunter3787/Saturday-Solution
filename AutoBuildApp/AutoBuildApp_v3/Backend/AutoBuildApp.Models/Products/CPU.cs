using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    public class CPU : IComponent
    {
        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<byte[]> ProductImage { get; set; }

        public CPU()
        {

        }

        public bool AddImage(byte[] image)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveImage(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}
