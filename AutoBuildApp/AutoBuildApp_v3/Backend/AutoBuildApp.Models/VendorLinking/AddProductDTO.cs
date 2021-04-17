using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.VendorLinking
{
    public class AddProductDTO
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool Availability { get; set; }
        public string Company { get; set; }
        public string Url { get; set; }
        public string ModelNumber { get; set; }
        public string Price { get; set; }
    }
}
