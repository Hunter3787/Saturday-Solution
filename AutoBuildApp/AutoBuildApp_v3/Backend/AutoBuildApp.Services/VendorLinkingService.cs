using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Models.WebCrawler;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoBuildApp.Services.FeatureServices;

namespace AutoBuildApp.Services
{
    public class VendorLinkingService
    {
        private VendorLinkingDAO _vendorLinkingDAO;
        private CommonResponseService commonResponseService = new CommonResponseService();
        public VendorLinkingService(string connectionString)
        {
            _vendorLinkingDAO = new VendorLinkingDAO(connectionString);
        }

        public CollectionCommonResponse<ConcurrentDictionary<string, HashSet<string>>> PopulateVendorsProducts()
        {
            // Call the DAO method and return the system code along with the collection
            SystemCodeWithCollection<ConcurrentDictionary<string, HashSet<string>>> daoResponse = _vendorLinkingDAO.PopulateVendorsProducts();

            // Initialize a Common Response object that has a collection field
            CollectionCommonResponse<ConcurrentDictionary<string, HashSet<string>>> commonResponse = new CollectionCommonResponse<ConcurrentDictionary<string, HashSet<string>>>();

            // Pass in the response code and the common response object and set the bool and the string fields
            commonResponseService.SetCommonResponse(daoResponse.Code, commonResponse);

            // Save the collection to the common response object
            commonResponse.GenericCollection = daoResponse.GenericCollection;

            return commonResponse;
        }

        public CommonResponse AddProductToVendorListOfProducts(AddProductDTO product)
        {
            // Call the DAO method and return the system code
            AutoBuildSystemCodes daoResponseCode = _vendorLinkingDAO.AddProductToVendorListOfProducts(product);

            // Initialize a common response object
            CommonResponse commonResponse = new CommonResponse();

            // Pass in the response code and the common response object and set the bool and the string fields
            //      The third parameter is for a custom success message
            commonResponseService.SetCommonResponse(daoResponseCode, commonResponse, ResponseStringGlobals.SUCCESSFUL_ADDITION);

            return commonResponse;
        }
        public CommonResponse EditProductInVendorListOfProducts(AddProductDTO product)
        {
            // Call the DAO method and return the system code
            AutoBuildSystemCodes daoResponseCode = _vendorLinkingDAO.EditProductInVendorListOfProducts(product);

            // Initialize a common response object
            CommonResponse commonResponse = new CommonResponse();

            // Pass in the response code and the common response object and set the bool and the string fields
            //      The third parameter is for a custom success message for when the response code is success
            commonResponseService.SetCommonResponse(daoResponseCode, commonResponse, ResponseStringGlobals.SUCCESSFUL_MODIFICATION);

            return commonResponse;
        }

        public CommonResponse DeleteProductFromVendorList(string modelNumber)
        {
            // Call the DAO method and return the system code
            AutoBuildSystemCodes daoResponseCode = _vendorLinkingDAO.DeleteProductFromVendorList(modelNumber);

            // Initialize a common response object
            CommonResponse commonResponse = new CommonResponse();

            // Pass in the response code and the common response object and set the bool and the string fields
            //      The third parameter is for a custom success message for when the response code is success
            commonResponseService.SetCommonResponse(daoResponseCode, commonResponse, ResponseStringGlobals.SUCCESSFUL_DELETION);

            return commonResponse;
        }

        public CollectionCommonResponse<List<AddProductDTO>> GetAllProductsByVendor(GetProductByFilterDTO filters)
        {
            // Call the DAO method and return the system code along with the collection
            SystemCodeWithCollection<List<AddProductDTO>> daoResponse = _vendorLinkingDAO.GetVendorProductsByFilter(filters);

            // Initialize a Common Response object that has a collection field
            CollectionCommonResponse<List<AddProductDTO>> commonResponse = new CollectionCommonResponse<List<AddProductDTO>>();

            // Pass in the response code and the common response object and set the bool and the string fields
            commonResponseService.SetCommonResponse(daoResponse.Code, commonResponse);

            // Save the collection to the common response object
            commonResponse.GenericCollection = daoResponse.GenericCollection;

            return commonResponse;
        }
        public CollectionCommonResponse<List<string>> GetAllModelNumbers()
        {
            // Call the DAO method and return the system code along with the collection
            SystemCodeWithCollection<List<string>> daoResponse = _vendorLinkingDAO.GetAllModelNumbers();

            // Initialize a Common Response object that has a collection field
            CollectionCommonResponse<List<string>> commonResponse = new CollectionCommonResponse<List<string>>();

            // Pass in the response code and the common response object and set the bool and the string fields
            commonResponseService.SetCommonResponse(daoResponse.Code, commonResponse);

            // Save the collection to the common response object
            commonResponse.GenericCollection = daoResponse.GenericCollection;

            return commonResponse;
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

            return storeIn;
        }
    }
}
