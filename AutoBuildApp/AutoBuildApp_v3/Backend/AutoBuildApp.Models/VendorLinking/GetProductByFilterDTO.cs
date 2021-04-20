using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.VendorLinking
{
    public class GetProductByFilterDTO
    {
        public Dictionary<string, bool> FilteredListOfProducts { get; private set; } = new Dictionary<string, bool>();
        public string PriceOrder { get; private set; }
    }
}
