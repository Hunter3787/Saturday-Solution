using AutoBuildApp.DataAccess;
using AutoBuildApp.Logging;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Services.FeatureServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Services
{
    public class ComponentCatalogService
    {
        private ComponentCatalogDAO _componentCatalogDAO;

        // Creates the local instance for the logger
        private readonly LoggingProducerService _logger;
        private CommonResponseService commonResponseService = new CommonResponseService();

        public ComponentCatalogService(ComponentCatalogDAO componentCatalogDAO)
        {
            _componentCatalogDAO = componentCatalogDAO;
            _logger = LoggingProducerService.GetInstance;
        }

        public CommonResponseWithObject<List<CatalogProductDTO>> GetAllProductsByFilter(ProductByFilterDTO filters)
        {
            // Initialize a common response object that has a collection field
            CommonResponseWithObject<List<CatalogProductDTO>> commonResponse = new CommonResponseWithObject<List<CatalogProductDTO>>();

            // Call the DAO method and return the system code along with the collection
            SystemCodeWithObject<List<CatalogProductDTO>> daoResponse = _componentCatalogDAO.GetVendorProductsByFilter(filters);

            commonResponseService.SetCommonResponse(daoResponse.Code, commonResponse);

            commonResponse.GenericObject = daoResponse.GenericObject;
            return commonResponse;
        }
    }
}
