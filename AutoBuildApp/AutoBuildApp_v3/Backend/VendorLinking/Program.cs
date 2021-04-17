using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Models.WebCrawler;
using System;
using System.Collections.Generic;

namespace VendorLinking
{
    class Program
    {
        static void Main(string[] args)
        {
            VendorLinkingManager manager = new VendorLinkingManager();
            //Product f = new Product(true, "company", "url", "DBDO40JF0", "3060 super", "gpu", "nvidia", "5", "55", "$345.34", null, null);
            //manager.AddProductToVendorListOfProducts(f);

            List<AddProductDTO> list = manager.GetAllProductsByVendor("new egg");
            foreach(var v in list)
            {
                Console.WriteLine(v.ModelNumber);
            }
        }
    }
}
