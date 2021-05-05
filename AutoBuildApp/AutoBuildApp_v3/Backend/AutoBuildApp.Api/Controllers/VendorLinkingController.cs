using AutoBuildApp.Api.HelperFunctions;
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
        private ClaimsFactory _claimsFactory = new ConcreteClaimsFactory();
        private IClaims _vendorClaims;

        ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;

        private VendorLinkingManager _vendorLinkingManager = new VendorLinkingManager(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));

        public VendorLinkingController()
        {
            _vendorClaims = _claimsFactory.GetClaims(RoleEnumType.VendorRole);
        }
        // Initializes the DAO that will be used for review ratings.

        // This will start the logging consumer manager in the background so that logs may be sent to the DB.
        private LoggingConsumerManager _loggingConsumerManager = new LoggingConsumerManager();

        private LoggingProducerService _logger = LoggingProducerService.GetInstance;

        /// <summary>
        /// This class will show no contend if fetch Options is made.
        /// </summary>
        /// <returns>will return a page of no content to the view.</returns>

        [HttpPost]
        public async Task<IActionResult> AddProductToVendorListOfProducts(IFormCollection formData, IFormFile photo)
        {
            // Takes the form data from javascript and converts it to an AddProductDTO.
            AddProductDTO Product = _vendorLinkingManager.ConvertFormToProduct(formData);

            // Product is null if a format exception was thrown
            if (Product == null)
            {
                _logger.LogWarning("AddProductToVendorListOfProducts failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            CommonResponse Result = await _vendorLinkingManager.AddProductToVendorListOfProducts(Product, photo);

            if (!Result.ResponseBool)
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
            // Takes the form data from the front end and converts it to an AddProductDTO.
            AddProductDTO product = _vendorLinkingManager.ConvertFormToProduct(formData);

            // Product is null if a format exception was thrown
            if (product == null)
            {
                _logger.LogWarning("EditProductInVendorListOfProducts failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            CommonResponse result = await _vendorLinkingManager.EditProductInVendorListOfProducts(product, photo);

            if (!result.ResponseBool)
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
            CommonResponse result = _vendorLinkingManager.DeleteProductFromVendorList(modelNumber);

            if(!result.ResponseBool)
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
            CollectionCommonResponse<List<string>> modelNumbers = _vendorLinkingManager.GetAllModelNumbers();

            // ModelNumbers is null when an SQL exception occurs.
            if (modelNumbers == null)
            {
                _logger.LogWarning("GetAllModelNumbers failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            _logger.LogInformation("GetAllModelNumbers succeeded.");
            return Ok(modelNumbers.GenericCollection);
        }

        [HttpGet]
        public IActionResult GetAllProductsByVendor(string FiltersString, string Order)
        {
            #region Authentication stuff?
            //if (!_threadPrinciple.Identity.IsAuthenticated)
            //{

            //    // Add action logic here
            //    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            //}
            //if (!AuthorizationService.checkPermissions(_vendorClaims.Claims()))
            //{

            //    // Add action logic here
            //    return new StatusCodeResult(StatusCodes.Status403Forbidden);
            //}
            #endregion

            // Takes the filters and the order from the front end and converts it into a GetProductByFilterDTO.
            GetProductByFilterDTO Filters = _vendorLinkingManager.ConvertToGetProductByFilterDTO(FiltersString, Order);

            // Filters is null when FiltersString is null.
            if (Filters == null)
            {
                _logger.LogWarning("FiltersString was null. GetAllProductsByVendor failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            CollectionCommonResponse<List<AddProductDTO>> ProductsByVendor = _vendorLinkingManager.GetAllProductsByVendor(Filters);

            // ProductsByVendor is null when an SQL exception occurs.
            if(ProductsByVendor == null)
            {
                _logger.LogWarning("ProductsByVendor was null. GetAllProductsByVendor failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            _logger.LogInformation("GetAllProductsByVendor succeeded.");
            return Ok(ProductsByVendor.GenericCollection);
        }
    }
}
