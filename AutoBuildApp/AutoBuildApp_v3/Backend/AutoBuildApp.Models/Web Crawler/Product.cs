﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.Web_Crawler
{
    public class Product
    {
        public bool Availability { get; set; }
        public string Company { get; set; }
        public string Url { get; set; }
        public string ModelNumber { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public string ManufacturerName { get; set; }
        public Dictionary<string, string> Specs { get; set; }

        public Product(bool availability, string company, string url, string modelNumber, string name, string productType, string manufacturerName, Dictionary<string, string> specs)
        {
            Availability = availability;
            Company = company;
            Url = url;
            ModelNumber = modelNumber;
            Name = name;
            ProductType = productType;
            ManufacturerName = manufacturerName;
            Specs = specs;
        }
    }
}