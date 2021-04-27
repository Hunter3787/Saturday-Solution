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
        private VendorLinkingDAO _vendorLinkingDAO;

        public VendorLinkingService(string connectionString)
        {
            _vendorLinkingDAO = new VendorLinkingDAO(connectionString);
        }
        public bool AddProductToVendorListOfProducts(AddProductDTO product)
        {
            return _vendorLinkingDAO.AddProductToVendorListOfProducts(product);
        }
        public bool EditProductInVendorListOfProducts(AddProductDTO product)
        {
            return _vendorLinkingDAO.EditProductInVendorListOfProducts(product);
        }

        public bool DeleteProductFromVendorList(string modelNumber)
        {
            return _vendorLinkingDAO.DeleteProductFromVendorList(modelNumber);
        }
        public List<AddProductDTO> GetAllProductsByVendor(GetProductByFilterDTO filters)
        {
            return _vendorLinkingDAO.GetProductsByFilter(filters, 1);
            //return _vendorLinkingDAO.GetAllProductsByVendor(" ");
        }
        public List<string> GetAllModelNumbers()
        {
            return _vendorLinkingDAO.GetAllModelNumbers();
        }

        public List<AddProductDTO> GetAllProductsByVendor(string companyName)
        {
            return _vendorLinkingDAO.GetAllProductsByVendor(companyName);
        }
    }
}
