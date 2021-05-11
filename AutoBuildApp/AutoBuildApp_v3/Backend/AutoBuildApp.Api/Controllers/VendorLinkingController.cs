using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Logging;
using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoBuildApp.Api.Controllers
{
    /// <summary>
    /// This class will be the controller of VendorLinking and call methods from the front end.
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class VendorLinkingController : ControllerBase
    {
        // This will start the logging consumer manager in the background so that logs may be sent to the DB.
        private LoggingConsumerManager _loggingConsumerManager = new LoggingConsumerManager();
        private LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private List<string> _allowedRoles;
        private VendorLinkingManager _vendorLinkingManager;
        private VendorLinkingService _vendorLinkingService;
        private VendorLinkingDAO _vendorLinkingDAO;

        /// <summary>
        /// The default constructor to initalize the controller. Initializes and sets the allowed roles and creates a VendorLinkingManager object.
        /// </summary>
        public VendorLinkingController()
        {
            _allowedRoles = new List<string>()
            {
                RoleEnumType.VendorRole
            };

            // Initializes each layer and passes into the respective layer
            _vendorLinkingDAO = new VendorLinkingDAO(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
            _vendorLinkingService = new VendorLinkingService(_vendorLinkingDAO);
            _vendorLinkingManager = new VendorLinkingManager(_vendorLinkingService);
        }

        /// <summary>
        /// This method will be used to add products to the vendor's list of products.
        /// </summary>
        /// <param name="formData">takes in a IFormCollection object which reads from FormData.</param>
        /// <param name="photo">takes in an image file from the FormData.</param>
        /// <returns>returns a status code result.</returns>
        [HttpPost]
        public async Task<IActionResult> AddProductToVendorListOfProducts(IFormCollection formData, IFormFile photo)
        {
            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            // Takes the form data from javascript and converts it to an AddProductDTO.
            CommonResponseWithObject<AddProductDTO> dtoCommonResponse = _vendorLinkingManager.ConvertFormToProduct(formData);

            // If DTOCommonResponse is false, the request failed to convert the form to the product
            if (!dtoCommonResponse.IsSuccessful)
            {
                _logger.LogWarning("AddProductToVendorListOfProducts failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            CommonResponse commonResponse = await _vendorLinkingManager.AddProductToVendorListOfProducts(dtoCommonResponse.GenericObject, photo);

            // If commonResponse is false, the request failed
            if (!commonResponse.IsSuccessful)
            {
                _logger.LogWarning("AddProductToVendorListOfProducts failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            _logger.LogInformation("EditProductInVendorListOfProducts succeeded.");
            return Ok();
        }

        /// <summary>
        /// This method will be used to edit products in the vendor's list of products.
        /// </summary>
        /// <param name="formData">takes in a IFormCollection object which reads from FormData.</param>
        /// <param name="photo">takes in an image file from the FormData.</param>
        /// <returns>returns a status code result.</returns>
        [HttpPut]
        public async Task<IActionResult> EditProductInVendorListOfProducts(IFormCollection formData, IFormFile photo)
        {
            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            // Takes the form data from the front end and converts it to an AddProductDTO.
            CommonResponseWithObject<AddProductDTO> dtoCommonResponse = _vendorLinkingManager.ConvertFormToProduct(formData);

            // If DTOCommonResponse is false, the request failed to convert the form to the product
            if (!dtoCommonResponse.IsSuccessful)
            {
                _logger.LogWarning("EditProductInVendorListOfProducts failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            CommonResponse commonResponse = await _vendorLinkingManager.EditProductInVendorListOfProducts(dtoCommonResponse.GenericObject, photo);

            // If commonResponse is false, the request failed
            if (!commonResponse.IsSuccessful)
            {
                _logger.LogWarning("EditProductInVendorListOfProducts failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            _logger.LogInformation("EditProductInVendorListOfProducts succeeded.");
            return Ok();
        }


        /// <summary>
        /// This method will be used to delete products from the vendor's list of products.
        /// </summary>
        /// <param name="modelNumber">takes in the model number as a string</param>
        /// <returns>returns a status code result.</returns>
        [HttpDelete]
        public IActionResult DeleteProductFromVendorList(string modelNumber)
        {
            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            // Call the manager's DeleteProductFromVendorList function
            CommonResponse commonResponse = _vendorLinkingManager.DeleteProductFromVendorList(modelNumber);

            // If commonResponse is false, the request failed
            if (!commonResponse.IsSuccessful)
            {
                _logger.LogWarning("DeleteProductFromVendorList failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            _logger.LogInformation("DeleteProductFromVendorList succeeded.");
            return Ok();
        }

        /// <summary>
        /// This method will be used to get all model numbers from the database.
        /// </summary>
        /// <returns>returns a status code result.</returns>
        [HttpGet("modelNumbers")]
        public IActionResult GetAllModelNumbers()
        {
            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            // Call the manager's GetAllModelNumbers function
            CommonResponseWithObject<List<string>> commonResponse = _vendorLinkingManager.GetAllModelNumbers();

            // If commonResponse is false, the request failed
            if (!commonResponse.IsSuccessful)
            {
                _logger.LogWarning("GetAllModelNumbers failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            _logger.LogInformation("GetAllModelNumbers succeeded.");
            return Ok(commonResponse.GenericObject);
        }

        /// <summary>
        /// This method will be used to delete products from the vendor's list of products.
        /// </summary>
        /// <param name="filtersString">takes in filters as a string which will be used to filter the product types that the user wants</param>
        /// <param name="order">takes in order as a string which will be used to order the products</param>
        /// <returns>returns a status code result.</returns>
        [HttpGet]
        public IActionResult GetAllProductsByVendor(string filtersString, string order)
        {
            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            // Takes the filters and the order from the front end and converts it into a GetProductByFilterDTO
            CommonResponseWithObject<ProductByFilterDTO> dtoCommonResponse = _vendorLinkingManager.ConvertToGetProductByFilterDTO(filtersString, order);

            // If dtoCommonResponse is false, the request failed to convert to ProductByFilterDTO
            if (!dtoCommonResponse.IsSuccessful)
            {
                _logger.LogWarning("FiltersString was null. GetAllProductsByVendor failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            // Call the manager's GetAllProductsByVendor function
            CommonResponseWithObject<List<AddProductDTO>> commonResponse = _vendorLinkingManager.GetAllProductsByVendor(dtoCommonResponse.GenericObject);

            // If commonResponse is false, the request failed
            if (!commonResponse.IsSuccessful)
            {
                _logger.LogWarning("ProductsByVendor was null. GetAllProductsByVendor failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            _logger.LogInformation("GetAllProductsByVendor succeeded.");
            return Ok(commonResponse.GenericObject);
        }
    }
}
