using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Logging;
using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Services.FeatureServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoBuildApp.Api.Controllers
{
    /// <summary>
    /// This class will be the controller of the Product Details and call methods from the front end.
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private LoggingConsumerManager _loggingConsumerManager = new LoggingConsumerManager();
        private LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private ProductDetailsManager _productDetailsManager;
        private ProductDetailsService _productDetailsService;
        private ProductDetailsDAO _productDetailsDAO;

        public ProductDetailsController()
        {
            // Set each respective layer to their constructor and pass in its respective layers to the parameter
            _productDetailsDAO = new ProductDetailsDAO(ConnectionManager.connectionManager.GetConnectionStringByName(ControllerGlobals.ADMIN_CREDENTIALS_CONNECTION));
            _productDetailsService = new ProductDetailsService(_productDetailsDAO);
            _productDetailsManager = new ProductDetailsManager(_productDetailsService);
        }

        /// <summary>
        /// This method will be used to fetch post new Build Posts to the DB.
        /// </summary>
        /// <param name="modelNumber">takes in a model number as a string</param>
        /// <returns>returns a status code result.</returns>
        [HttpGet("{modelNumber}")]
        public IActionResult GetProductByModelNumber(string modelNumber)
        {
            // Call the manager's GetProductByModelNumber function
            CommonResponseWithObject<ProductDetailsDTO> commonResponse = _productDetailsManager.GetProductByModelNumber(modelNumber);

            // If commonResponse is false, the request failed
            if (!commonResponse.IsSuccessful)
            {
                _logger.LogWarning("GetProductByModelNumber failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            _logger.LogInformation("GetProductByModelNumber succeeded.");
            return Ok(commonResponse.GenericObject);
        }

        /// <summary>
        /// This method will be used to update the email list by adding the email in the database for a particular product
        /// </summary>
        /// <param name="modelNumber">takes in a model number as a string</param>
        /// <returns>returns a status code result.</returns>
        [HttpPost("EmailSubmit/{modelNumber}")]
        public IActionResult AddEmailToEmailListForProduct(string modelNumber)
        {
            CommonResponse commonResponse = _productDetailsManager.AddEmailToEmailListForProduct(modelNumber);

            // If commonResponse is false, the request failed
            if (!commonResponse.IsSuccessful)
            {
                _logger.LogWarning("AddEmailToEmailListForProduct failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            _logger.LogInformation("AddEmailToEmailListForProduct succeeded.");
            return Ok();
        }
    }
}
