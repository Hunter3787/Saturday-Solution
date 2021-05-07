using AutoBuildApp.Logging;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Services.FeatureServices;
using System;
using System.Collections.Generic;

namespace AutoBuildApp.Managers.FeatureManagers
{
    /// <summary>
    /// This class will consist of the methods used to validate
    /// business requirements that can be seen in the BRD.
    /// </summary>
    public class ProductDetailsManager
    {
        private List<string> _allowedRoles;
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private ProductDetailsService _productDetailsService;

        /// <summary>
        /// This default constructor to initalize the service.
        /// </summary>
        /// <param name="connectionString">sql database string to be able to connect to database.</param>
        public ProductDetailsManager(string connectionString)
        {
            _productDetailsService = new ProductDetailsService(connectionString);
            _allowedRoles = new List<string>()
            {
                RoleEnumType.SystemAdmin,
                RoleEnumType.VendorRole
            };
        }

        /// <summary>
        /// This method checks to see if the model number is null before passing it to the service layer
        /// </summary>
        /// <param name="modelNumber">takes in a model number as a string</param>
        /// <returns>retruns a common response object with a ProductDetailsDTO</returns>
        public CommonResponseWithObject<ProductDetailsDTO> GetProductByModelNumber(string modelNumber)
        {
            // Initialize a common response object
            CommonResponseWithObject<ProductDetailsDTO> commonResponse = new CommonResponseWithObject<ProductDetailsDTO>();

            // Check if model number is null or empty
            if (String.IsNullOrEmpty(modelNumber))
            {
                _logger.LogWarning("User inputted a null model number. GetProductByModelNumber manager call failed.");
                commonResponse.ResponseString = "Model number is null.";
                commonResponse.IsSuccessful = false;

                return commonResponse;
            }

            return _productDetailsService.GetProductByModelNumber(modelNumber);
        }

        /// <summary>
        /// This method checks to see if the model number is null before passing it to the service layer
        /// </summary>
        /// <param name="modelNumber">takes in a model number as a string</param>
        /// <returns>retruns a common response object</returns>
        public CommonResponse AddEmailToEmailListForProduct(string modelNumber)
        {
            // Initialize a common response object
            CommonResponseWithObject<ProductDetailsDTO> commonResponse = new CommonResponseWithObject<ProductDetailsDTO>();

            // Check if model number is null or empty
            if (String.IsNullOrEmpty(modelNumber))
            {
                _logger.LogWarning("User inputted a null model number. GetProductByModelNumber manager call failed.");
                commonResponse.ResponseString = "Model number is null.";
                commonResponse.IsSuccessful = false;

                return commonResponse;
            }

            return _productDetailsService.AddEmailToEmailListForProduct(modelNumber);
        }
    }
}
