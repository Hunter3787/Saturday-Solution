using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Logging;
using AutoBuildApp.Managers;
using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.WebCrawler;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBuildApp.Api.Controllers
{
    public class ProductDetailsController : ControllerBase
    {
        private LoggingConsumerManager _loggingConsumerManager = new LoggingConsumerManager();
        private LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private List<string> _allowedRoles;
        private ProductDetailsManager _productDetailsManager;

        public ProductDetailsController()
        {
            _allowedRoles = new List<string>()
            {
                RoleEnumType.SystemAdmin,
                RoleEnumType.VendorRole
            };

            _productDetailsManager = new ProductDetailsManager(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
        }

        [HttpGet("/ProductDetails/{modelNumber}")]
        public IActionResult GetProductByModelNumber(string modelNumber)
        {
            // Call the manager's GetProductByModelNumber function
            CommonResponseWithObject<ProductDetailsDTO> commonResponse = _productDetailsManager.GetProductByModelNumber(modelNumber);

            // If commonResponse is false, the request failed
            if (!commonResponse.ResponseBool)
            {
                _logger.LogWarning("GetProductByModelNumber failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            _logger.LogInformation("GetProductByModelNumber succeeded.");
            return Ok(commonResponse.GenericObject);
        }
    }
}
