using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.WebCrawler
{
    public class Product
    {
        public string ImageUrl { get; set; }
        public bool Availability { get; set; }
        public string Company { get; set; }
        public string Url { get; set; }
        public string ModelNumber { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public string ManufacturerName { get; set; }
        public double Price { get; set; }
        public double TotalRating { get; set; }
        public int TotalNumberOfReviews { get; set; }
        public Dictionary<string, string> Specs { get; set; }
        public List<Review> Reviews { get; set; }

        public Product()
        {

        }

        public Product(string imageUrl, bool availability, string company, string url, string modelNumber, string name, 
            string productType, string manufacturerName, double totalRating, int totalNumberOfReviews,
            double price, Dictionary<string, string> specs, List<Review> reviews)
        {
            ImageUrl = imageUrl;
            Availability = availability;
            Company = company;
            Url = url;
            ModelNumber = modelNumber;
            Name = name;
            ProductType = productType;
            ManufacturerName = manufacturerName;
            TotalRating = totalRating;
            TotalNumberOfReviews = totalNumberOfReviews;
            Price = price;
            Specs = specs;
            Reviews = reviews;
        }
    }
}
