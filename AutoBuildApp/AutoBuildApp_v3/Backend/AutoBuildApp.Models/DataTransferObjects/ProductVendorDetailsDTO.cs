using AutoBuildApp.Models.WebCrawler;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.DataTransferObjects
{
    public class ProductVendorDetailsDTO
    {
        public bool Availability { get; set; }
        public string Url { get; set; }
        public string ListingName { get; set; }
        public double Price { get; set; }
        public List<Review> Reviews { get; set; }

    }
}
