using AutoBuildApp.Logging;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Managers.FeatureManagers
{
    public class ComponentCatalogManager
    {
        private ComponentCatalogService _componentCatalogService;

        // Creates the local instance for the logger
        private readonly LoggingProducerService _logger;
        public ComponentCatalogManager(ComponentCatalogService componentCatalogService)
        {
            _componentCatalogService = componentCatalogService;
            _logger = LoggingProducerService.GetInstance;
        }

        public CommonResponseWithObject<ProductByFilterDTO> ConvertToGetProductByFilterDTO(string filtersString, string order, int minimumPrice, int maximumPrice)
        {
            // Initialize a common response object
            CommonResponseWithObject<ProductByFilterDTO> commonResponse = new CommonResponseWithObject<ProductByFilterDTO>();

            // If the filters string is null, return null.
            if (filtersString == null)
            {
                _logger.LogWarning("Filters string is null in vendor linking manager.");
                commonResponse.IsSuccessful = false;
                commonResponse.ResponseString = "Filters string is null.";

                return commonResponse;
            }

            // If order is null, set it to a default order.
            if (order == null)
            {
                order = "price_asc";
                _logger.LogWarning("No order was specified. Setting to price_asc as default.");
            }

            // Assign filter dictionary to the response's object's dictionary
            Dictionary<string, bool> filtersDictionary = commonResponse.GenericObject.FilteredListOfProducts;

            // Assign the GetProductByFilterDTO's price order
            commonResponse.GenericObject.PriceOrder = order;

            // Assign the GetProductByFilterDTO's minimum price
            commonResponse.GenericObject.MinimumPrice = minimumPrice;

            // Assign the GetProductByFilterDTO's maximum price
            if (maximumPrice == 0)
            {
                commonResponse.GenericObject.MaximumPrice = Int32.MaxValue;
            }
            else
            {
                commonResponse.GenericObject.MaximumPrice = maximumPrice;
            }

            // The front end passes the whole query parameter over. This gets rid of "?filtersString" with "".
            filtersString = filtersString.Replace("?filtersString=", "");

            // The front end passes the string of filters which is comma separated. This separates them and stores each filter into a string in a string[].
            string[] separatedFilters = filtersString.Split(',');

            // Iterate through the filters and add to the dictionary
            foreach (string filter in separatedFilters)
            {
                // Only add filters to the dictionary if the dictionary doesn't already contain it. 
                if (!filtersDictionary.ContainsKey(filter))
                {
                    filtersDictionary.Add(filter, true);
                }
            }

            commonResponse.IsSuccessful = true;
            commonResponse.ResponseString = "Successfully converted to a GetProductByFilterDTO.";

            return commonResponse;
        }

        public CommonResponseWithObject<List<CatalogProductDTO>> GetAllProductsByFilter(ProductByFilterDTO filters)
        {
            CommonResponseWithObject<List<CatalogProductDTO>> commonResponse = _componentCatalogService.GetAllProductsByFilter(filters);

            return commonResponse;
        }
    }
}
