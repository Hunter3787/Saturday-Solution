using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.WebCrawler;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Services;
using AutoBuildApp.Services.FeatureServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Managers.FeatureManagers
{
    public class ProductDetailsManager
    {
        private List<string> _allowedRoles;
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private ProductDetailsService _productDetailsService;

        public ProductDetailsManager(string connectionString)
        {
            _productDetailsService = new ProductDetailsService(connectionString);
            _allowedRoles = new List<string>()
            {
                RoleEnumType.SystemAdmin,
                RoleEnumType.VendorRole
            };
        }

        public CommonResponseWithObject<ProductDetailsDTO> GetProductByModelNumber(string modelNumber)
        {
            // Initialize a common response object
            CommonResponseWithObject<ProductDetailsDTO> commonResponse = new CommonResponseWithObject<ProductDetailsDTO>();

            // Check if model number is null or empty
            if (String.IsNullOrEmpty(modelNumber))
            {
                _logger.LogWarning("User inputted a null model number. GetProductByModelNumber manager call failed.");
                commonResponse.ResponseString = "Model number is null.";
                commonResponse.ResponseBool = false;

                return commonResponse;
            }

            return _productDetailsService.GetProductByModelNumber(modelNumber);
        }
    }
}
