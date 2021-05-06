using AutoBuildApp.DataAccess;
using AutoBuildApp.Logging;
using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.WebCrawler;
using AutoBuildApp.Security.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Services.FeatureServices
{
    public class ProductDetailsService
    {
        private List<string> _allowedRoles;
        private ProductDetailsDAO _productDetailsDAO;
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private CommonResponseService commonResponseService = new CommonResponseService();
        public ProductDetailsService(string connectionString)
        {
            _productDetailsDAO = new ProductDetailsDAO(connectionString);
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

            // Call the DAO method and return the system code
            SystemCodeWithObject<ProductDetailsDTO> daoResponse = _productDetailsDAO.GetProductByModelNumber(modelNumber);

            // Pass in the response code and the common response object and set the bool and the string fields
            commonResponseService.SetCommonResponse(daoResponse.Code, commonResponse);

            // Save the collection to the common response object
            commonResponse.GenericObject = daoResponse.GenericObject;

            return commonResponse;
        }
    }
}
