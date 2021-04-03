using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.Web_Crawler
{
    public class Product
    {
        public string Url { get; set; }
        public string ModelNumber { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public string ManufacturerName { get; set; }
        public Dictionary<string, string> Specs { get; set; }

        public Product(string url, string modelNumber, string name, string productType, string manufacturerName)
        {
            Url = url;
            ModelNumber = modelNumber;
            Name = name;
            ProductType = productType;
            ManufacturerName = manufacturerName;
        }
    }
}
