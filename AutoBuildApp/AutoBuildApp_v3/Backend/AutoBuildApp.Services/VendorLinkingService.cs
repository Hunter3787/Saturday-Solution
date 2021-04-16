using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Models.WebCrawler;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Services
{
    public class VendorLinkingService
    {
        private VendorLinkingDAO _vendorLinkingDAO = new VendorLinkingDAO();

        public bool AddProductToVendorListOfProducts(AddProductDTO product)
        {
            return _vendorLinkingDAO.AddProductToVendorListOfProducts(product);
        }
        public bool EditProductInVendorListOfProducts(AddProductDTO product)
        {
            return _vendorLinkingDAO.EditProductInVendorListOfProducts(product);
        }
        public List<AddProductDTO> GetAllProductsByVendor(string companyName)
        {
            return _vendorLinkingDAO.GetAllProductsByVendor(companyName);
        }
    }
}
