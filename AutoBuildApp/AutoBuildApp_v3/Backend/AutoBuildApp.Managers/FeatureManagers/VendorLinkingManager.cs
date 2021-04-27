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

        private VendorLinkingService _vendorLinkingService;


        public VendorLinkingManager(string connectionString)
        {
            _vendorLinkingService = new VendorLinkingService(connectionString);
        }

        // NULL CHECKS AND LOGGING

        public bool AddProductToVendorListOfProducts(AddProductDTO product)
        {
            return _vendorLinkingService.AddProductToVendorListOfProducts(product);
        }

        public bool EditProductInVendorListOfProducts(AddProductDTO product)
        {
            return _vendorLinkingService.EditProductInVendorListOfProducts(product);
        }

        public bool DeleteProductFromVendorList(string modelNumber)
        {
            return _vendorLinkingService.DeleteProductFromVendorList(modelNumber);
        }

        public List<AddProductDTO> GetAllProductsByVendor(GetProductByFilterDTO filters)
        {
            return _vendorLinkingService.GetAllProductsByVendor(filters);
        }

        public List<string> GetAllModelNumbers()
        {
            return _vendorLinkingService.GetAllModelNumbers();
        }

        public List<AddProductDTO> GetAllProductsByVendor(string companyName)
        {
            return _vendorLinkingService.GetAllProductsByVendor(companyName);
        }
    }
}
