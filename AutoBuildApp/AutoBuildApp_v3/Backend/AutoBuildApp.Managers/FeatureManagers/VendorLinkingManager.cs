using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Models.WebCrawler;
using AutoBuildApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Managers.FeatureManagers
{
    public class VendorLinkingManager
    {
        //private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;

        private VendorLinkingService _vendorLinkingService = new VendorLinkingService();




        // NULL CHECKS AND LOGGING

        public bool AddProductToVendorListOfProducts(AddProductDTO product)
        {
            return _vendorLinkingService.AddProductToVendorListOfProducts(product);
        }

        public bool EditProductInVendorListOfProducts(AddProductDTO product)
        {
            return _vendorLinkingService.EditProductInVendorListOfProducts(product);
        }

        public List<AddProductDTO> GetAllProductsByVendor(string companyName)
        {
            return _vendorLinkingService.GetAllProductsByVendor(companyName);
        }
    }
}
