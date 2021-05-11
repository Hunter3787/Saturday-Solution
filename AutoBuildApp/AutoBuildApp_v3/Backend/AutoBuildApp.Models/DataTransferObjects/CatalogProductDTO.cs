using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.DataTransferObjects
{
    public class CatalogProductDTO
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ModelNumber { get; set; }
        public double Price { get; set; }
        public string ProductType { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}
