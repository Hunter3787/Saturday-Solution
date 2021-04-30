using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Models.WebCrawler;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AutoBuildApp.Services
{
    public class VendorLinkingService
    {
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;
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

        public async Task<string> UploadImage(string username, IFormFile file)
        {
            string storeIn = "";

            // Checks to see if the file is empty.
            if (file.Length > 0)
            {
                var currentDirectory = Directory.GetCurrentDirectory().ToString();

                // Specify a location to store the file.
                storeIn = $@"/assets/_{ DateTime.UtcNow.ToString("yyyyMMdd_hh_mm_ss_ms")}.jpg";

                var path = Path.GetFullPath(Path.Combine(currentDirectory, $@"..\..\FrontEnd{storeIn}"));

                // Save the file to the path.
                using (var stream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    await file.CopyToAsync(stream);
                }
            }

            _logger.LogInformation("Successfully uploaded image to specified location.");
            return storeIn;
        }
    }
}
