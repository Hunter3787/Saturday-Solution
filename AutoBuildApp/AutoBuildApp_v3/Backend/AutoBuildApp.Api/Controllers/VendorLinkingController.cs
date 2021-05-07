using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Logging;
using AutoBuildApp.Managers;
using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Models.WebCrawler;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBuildApp.Api.Controllers
{
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

        public VendorLinkingController()
        {
            _allowedRoles = new List<string>()
            { 
                RoleEnumType.SystemAdmin,
                RoleEnumType.VendorRole
            };

            _vendorLinkingManager = new VendorLinkingManager(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
         
        }

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
                //ContentResult result = new ContentResult();
                //result.StatusCode = StatusCodes.Status400BadRequest;
                //result.ContentType = "text/plain";
                //result.Content = "yooo";
                ////return HttpStatusCodeResult(403, "hey");
                ////return Content(new StatusCodeResult(StatusCodes.Status400BadRequest), "hey");
                //return result;
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
            CommonResponseWithObject<GetProductByFilterDTO> dtoCommonResponse = _vendorLinkingManager.ConvertToGetProductByFilterDTO(filtersString, order);

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
