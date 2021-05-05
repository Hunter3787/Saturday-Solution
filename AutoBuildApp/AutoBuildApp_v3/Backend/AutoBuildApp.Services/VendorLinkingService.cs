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
using System.Linq;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security;

namespace AutoBuildApp.Services
{
    public class VendorLinkingService
    {
        private List<string> _allowedRoles;
        private VendorLinkingDAO _vendorLinkingDAO;
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private CommonResponseService commonResponseService = new CommonResponseService();
        public VendorLinkingService(string connectionString)
        {
            _vendorLinkingDAO = new VendorLinkingDAO(connectionString);
            _allowedRoles = new List<string>()
            {
                RoleEnumType.SYSTEM_ADMIN,
                RoleEnumType.VENDOR_ROLE
            };
        }

        public CommonResponseWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> PopulateVendorsProducts()
        {
            // Initialize a common response object
            CommonResponseWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> commonResponse = new CommonResponseWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>>();

            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.ResponseBool = false;
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();

                return commonResponse;
            }

            // Call the DAO method and return the system code along with the collection
            SystemCodeWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> daoResponse = _vendorLinkingDAO.PopulateVendorsProducts();

            // Initialize a Common Response object that has a collection field

            // Pass in the response code and the common response object and set the bool and the string fields
            commonResponseService.SetCommonResponse(daoResponse.Code, commonResponse);

            // Save the collection to the common response object
            commonResponse.GenericObject = daoResponse.GenericObject;

            return commonResponse;
        }

        public CommonResponse AddProductToVendorListOfProducts(AddProductDTO product)
        {
            // Initialize a common response object
            CommonResponse commonResponse = new CommonResponse(); 
            
            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.ResponseBool = false;
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();

                return commonResponse;
            }

            // Call the DAO method and return the system code
            AutoBuildSystemCodes daoResponseCode = _vendorLinkingDAO.AddProductToVendorListOfProducts(product);

            // Pass in the response code and the common response object and set the bool and the string fields
            //      The third parameter is for a custom success message
            commonResponseService.SetCommonResponse(daoResponseCode, commonResponse, ResponseStringGlobals.SUCCESSFUL_ADDITION);

            return commonResponse;
        }
        public CommonResponse EditProductInVendorListOfProducts(AddProductDTO product)
        {
            // Initialize a common response object
            CommonResponse commonResponse = new CommonResponse();

            //Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.ResponseBool = false;
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();

                return commonResponse;
            }

            // Call the DAO method and return the system code
            AutoBuildSystemCodes daoResponseCode = _vendorLinkingDAO.EditProductInVendorListOfProducts(product);

            // Pass in the response code and the common response object and set the bool and the string fields
            //      The third parameter is for a custom success message for when the response code is success
            commonResponseService.SetCommonResponse(daoResponseCode, commonResponse, ResponseStringGlobals.SUCCESSFUL_MODIFICATION);

            return commonResponse;
        }

        public CommonResponse DeleteProductFromVendorList(string modelNumber)
        {

            // Initialize a common response object
            CommonResponse commonResponse = new CommonResponse();

            //Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.ResponseBool = false;
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();

                return commonResponse;
            }

            // Call the DAO method and return the system code
            AutoBuildSystemCodes daoResponseCode = _vendorLinkingDAO.DeleteProductFromVendorList(modelNumber);

            // Pass in the response code and the common response object and set the bool and the string fields
            //      The third parameter is for a custom success message for when the response code is success
            commonResponseService.SetCommonResponse(daoResponseCode, commonResponse, ResponseStringGlobals.SUCCESSFUL_DELETION);

            return commonResponse;
        }

        public CommonResponseWithObject<List<AddProductDTO>> GetAllProductsByVendor(GetProductByFilterDTO filters)
        {
            // Initialize a common response object that has a collection field
            CommonResponseWithObject<List<AddProductDTO>> commonResponse = new CommonResponseWithObject<List<AddProductDTO>>();

            //Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.ResponseBool = false;
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();

                return commonResponse;
            }

            // Call the DAO method and return the system code along with the collection
            SystemCodeWithObject<List<AddProductDTO>> daoResponse = _vendorLinkingDAO.GetVendorProductsByFilter(filters);

            // Pass in the response code and the common response object and set the bool and the string fields
            commonResponseService.SetCommonResponse(daoResponse.Code, commonResponse);

            // Save the collection to the common response object
            commonResponse.GenericObject = daoResponse.GenericObject;

            return commonResponse;
        }
        public CommonResponseWithObject<List<string>> GetAllModelNumbers()
        {
            // Initialize a Common Response object that has a collection field
            CommonResponseWithObject<List<string>> commonResponse = new CommonResponseWithObject<List<string>>();

            //Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.ResponseBool = false;
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();

                return commonResponse;
            }
            
            // Call the DAO method and return the system code along with the collection
            SystemCodeWithObject<List<string>> daoResponse = _vendorLinkingDAO.GetAllModelNumbers();

            // Pass in the response code and the common response object and set the bool and the string fields
            commonResponseService.SetCommonResponse(daoResponse.Code, commonResponse);

            // Save the collection to the common response object
            commonResponse.GenericObject = daoResponse.GenericObject;

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
