using AutoBuildApp.DataAccess;
using AutoBuildApp.Logging;
using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;
using System.Collections.Generic;

namespace AutoBuildApp.Services.FeatureServices
{
    /// <summary>
    /// This class will handle the data transferrance from the manager
    /// to the DAO entity object.
    /// </summary>
    public class ProductDetailsService
    {
        private List<string> _allowedRoles;
        private ProductDetailsDAO _productDetailsDAO;
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private CommonResponseService commonResponseService = new CommonResponseService();

        /// <summary>
        /// This default constructor to initalize the service. Creates a VendorLinkingDAO object
        /// </summary>
        /// <param name="connectionString">sql database string to be able to connect to database.</param>
        public ProductDetailsService(ProductDetailsDAO productDetailsDAO)
        {
            _productDetailsDAO = productDetailsDAO;
        }

        /// <summary>
        /// This method will be used to handle the DAO's response, and then pass the parameter to the DAO
        /// </summary>
        /// <param name="modelNumber">passes in a model number which will be passed to the DAO</param>
        /// <returns>returns a common response object</returns>
        public virtual CommonResponseWithObject<ProductDetailsDTO> GetProductByModelNumber(string modelNumber)
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

        /// <summary>
        /// This method will be used to handle the DAO's response, and then pass the parameter to the DAO
        /// </summary>
        /// <param name="modelNumber">passes in a model number which will be passed to the DAO</param>
        /// <returns>returns a common response object</returns>
        public virtual CommonResponse AddEmailToEmailListForProduct(string modelNumber)
        {
            // Initialize a common response object
            CommonResponse commonResponse = new CommonResponse();

            // Call the DAO method and return the system code
            SystemCodeWithObject<int> daoResponse = _productDetailsDAO.AddEmailToEmailListForProduct(modelNumber);

            // Pass in the response code and the common response object and set the bool and the string fields
            //      The third parameter is for a custom success message for when the response code is success
            commonResponseService.SetCommonResponse(daoResponse.Code, commonResponse, ResponseStringGlobals.SUCCESSFUL_MODIFICATION);

            return commonResponse;
        }
    }
}
