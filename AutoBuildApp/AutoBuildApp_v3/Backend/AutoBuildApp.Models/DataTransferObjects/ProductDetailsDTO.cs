using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.DataTransferObjects
{
    public class ProductDetailsDTO
    {
        public string ImageUrl { get; set; }
        public string ProductType { get; set; }
        public string ModelNumber {get; set;}
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public Dictionary<string, string> Specs { get; set; }
        public Dictionary<string, ProductVendorDetailsDTO> VendorInformation { get; set; }

    }
}
