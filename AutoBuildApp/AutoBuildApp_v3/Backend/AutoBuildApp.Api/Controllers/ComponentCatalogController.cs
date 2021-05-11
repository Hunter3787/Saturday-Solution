using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Logging;
using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBuildApp.Api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class ComponentCatalogController : ControllerBase
    {
        // This will start the logging consumer manager in the background so that logs may be sent to the DB.
        private LoggingProducerService _logger = LoggingProducerService.GetInstance;

        private ComponentCatalogDAO _componentCatalogDAO;
        private ComponentCatalogService _componentCatalogService;
        private ComponentCatalogManager _componentCatalogManager;
        public ComponentCatalogController()
        {
            // Initializes each layer and passes into the respective layer
            _componentCatalogDAO = new ComponentCatalogDAO(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
            _componentCatalogService = new ComponentCatalogService(_componentCatalogDAO);
            _componentCatalogManager = new ComponentCatalogManager(_componentCatalogService);
        }

        [HttpGet]
        public IActionResult GetAllProductsByFilter(string filtersString, string order, int minimumPrice, int maximumPrice)
        {
            // Takes the filters and the order from the front end and converts it into a GetProductByFilterDTO
            CommonResponseWithObject<ProductByFilterDTO> dtoCommonResponse = _componentCatalogManager.ConvertToGetProductByFilterDTO(filtersString, order, minimumPrice, maximumPrice);

            // If dtoCommonResponse is false, the request failed to convert to ProductByFilterDTO
            if (!dtoCommonResponse.IsSuccessful)
            {
                _logger.LogWarning("FiltersString was null. GetAllProductsByVendor failed.");
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }

            // Call the manager's GetAllProductsByVendor function
            CommonResponseWithObject<List<CatalogProductDTO>> commonResponse = _componentCatalogManager.GetAllProductsByFilter(dtoCommonResponse.GenericObject);

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
