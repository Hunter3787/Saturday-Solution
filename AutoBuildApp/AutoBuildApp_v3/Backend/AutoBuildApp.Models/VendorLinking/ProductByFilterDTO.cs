using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.VendorLinking
{
    public class ProductByFilterDTO
    {
        public Dictionary<string, bool> FilteredListOfProducts { get; set; } = new Dictionary<string, bool>();
        public string PriceOrder { get; set; }
    }
}
