using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.VendorLinking;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoBuildApp.Services.FeatureServices;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security;
using AutoBuildApp.Logging;

namespace AutoBuildApp.Services
{
    /// <summary>
    /// This class will handle the data transferrance from the manager
    /// to the DAO entity object.
    /// </summary>
    public class VendorLinkingService
    {
        public List<string> _allowedRoles;
        private VendorLinkingDAO _vendorLinkingDAO;
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private CommonResponseService commonResponseService = new CommonResponseService();

        /// <summary>
        /// This default constructor to initalize the service. Initializes and sets the allowed roles and creates a VendorLinkingDAO object
        /// </summary>
        /// <param name="connectionString">sql database string to be able to connect to database.</param>
        public VendorLinkingService(VendorLinkingDAO vendorLinkingDAO)
        {
            _vendorLinkingDAO = vendorLinkingDAO;
            _allowedRoles = new List<string>()
            {
                RoleEnumType.SystemAdmin,
                RoleEnumType.VendorRole
            };
        }

        /// <summary>
        /// This method will be used to populate the vendors products (cache)
        /// </summary>
        /// <returns>returns a common response object with a concurrent dictionary of string and concurrent dictionoary of string and byte.</returns>
        public virtual CommonResponseWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> PopulateVendorsProducts()
        {
            // Initialize a common response object
            CommonResponseWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> commonResponse = new CommonResponseWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>>();

            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.IsSuccessful = false;
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

        /// <summary>
        /// This method will be used to check authorization, handle the DAO's response, and then pass the parameter to the DAO
        /// </summary>
        /// <param name="product">passes in an AddProductDTO which will be passed to the DAO</param>
        /// <returns>returns a common response object</returns>
        public virtual CommonResponse AddProductToVendorListOfProducts(AddProductDTO product)
        {
            // Initialize a common response object
            CommonResponse commonResponse = new CommonResponse(); 
            
            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.IsSuccessful = false;
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();

                return commonResponse;
            }

            // Call the DAO method and return the system code
            SystemCodeWithObject<int> daoResponse = _vendorLinkingDAO.AddProductToVendorListOfProducts(product);

            // If the rows returned is 0, change the code to no change occurred.
            if (daoResponse.GenericObject == 0)
            {
                daoResponse.Code = AutoBuildSystemCodes.NoChangeOccurred;
            }

            // Pass in the response code and the common response object and set the bool and the string fields
            //      The third parameter is for a custom success message
            commonResponseService.SetCommonResponse(daoResponse.Code, commonResponse, ResponseStringGlobals.SUCCESSFUL_ADDITION);

            return commonResponse;
        }

        /// <summary>
        /// This method will be used to check authorization, handle the DAO's response, and then pass the parameter to the DAO
        /// </summary>
        /// <param name="product">passes in an AddProductDTO which will be passed to the DAO</param>
        /// <returns>returns a common response object</returns>
        public virtual CommonResponse EditProductInVendorListOfProducts(AddProductDTO product)
        {
            // Initialize a common response object
            CommonResponse commonResponse = new CommonResponse();

            //Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.IsSuccessful = false;
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();

                return commonResponse;
            }

            // Call the DAO method and return the system code
            SystemCodeWithObject<int> daoResponse = _vendorLinkingDAO.EditProductInVendorListOfProducts(product);

            // If the rows returned is 0, change the code to no change occurred.
            if (daoResponse.GenericObject == 0)
            {
                daoResponse.Code = AutoBuildSystemCodes.NoChangeOccurred;
            }

            // Pass in the response code and the common response object and set the bool and the string fields
            //      The third parameter is for a custom success message for when the response code is success
            commonResponseService.SetCommonResponse(daoResponse.Code, commonResponse, ResponseStringGlobals.SUCCESSFUL_MODIFICATION);

            return commonResponse;
        }

        /// <summary>
        /// This method will be used to check authorization, handle the DAO's response, and then pass the parameter to the DAO
        /// </summary>
        /// <param name="modelNumber">passes in string model number which will be deleted</param>
        /// <returns>returns a common response object</returns>
        public virtual CommonResponse DeleteProductFromVendorList(string modelNumber)
        {

            // Initialize a common response object
            CommonResponse commonResponse = new CommonResponse();

            //Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.IsSuccessful = false;
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();

                return commonResponse;
            }

            // Call the DAO method and return the system code
            SystemCodeWithObject<int> daoResponse = _vendorLinkingDAO.DeleteProductFromVendorList(modelNumber);

            // If the rows returned is 0, change the code to no change occurred.
            if (daoResponse.GenericObject == 0)
            {
                daoResponse.Code = AutoBuildSystemCodes.NoChangeOccurred;
            }

            // Pass in the response code and the common response object and set the bool and the string fields
            //      The third parameter is for a custom success message for when the response code is success
            commonResponseService.SetCommonResponse(daoResponse.Code, commonResponse, ResponseStringGlobals.SUCCESSFUL_DELETION);

            return commonResponse;
        }

        /// <summary>
        /// This method will be used to check authorization, handle the DAO's response and to pass a GetProductByFilterDTO to the DAO
        /// </summary>
        /// <param name="filters">passes in a GetProductByFilterDTO which will be passed to the DAO</param>
        /// <returns>returns a common response object</returns>
        public virtual CommonResponseWithObject<List<AddProductDTO>> GetAllProductsByVendor(ProductByFilterDTO filters)
        {
            // Initialize a common response object that has a collection field
            CommonResponseWithObject<List<AddProductDTO>> commonResponse = new CommonResponseWithObject<List<AddProductDTO>>();

            //Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.IsSuccessful = false;
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

        /// <summary>
        /// This method will be used to check authorization, and to handle the DAO's response 
        /// </summary>
        /// <param name="filters">passes in a GetProductByFilterDTO which will be passed to the DAO</param>
        /// <returns>returns a common response object</returns>
        public virtual CommonResponseWithObject<List<string>> GetAllModelNumbers()
        {
            // Initialize a Common Response object that has a collection field
            CommonResponseWithObject<List<string>> commonResponse = new CommonResponseWithObject<List<string>>();

            //Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.IsSuccessful = false;
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

        /// <summary>
        /// This method will be used to upload an image
        /// </summary>
        /// <param name="username">passes in a username which is used to determine which user to save the information to</param>
        /// <param name="file">passes in a formfile which is the file to be saved</param>
        /// <returns>returns a string for the file path</returns>
        public virtual async Task<string> UploadImage(string username, IFormFile file)
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
