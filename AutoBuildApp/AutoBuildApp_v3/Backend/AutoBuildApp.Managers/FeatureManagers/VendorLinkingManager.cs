﻿
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Models.WebCrawler;
using AutoBuildApp.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoBuildApp.Managers.FeatureManagers
{
    public class VendorLinkingManager
    {
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private readonly ConcurrentDictionary<string, HashSet<string>> VendorsProducts;
        private VendorLinkingService _vendorLinkingService;


        public VendorLinkingManager(string connectionString)
        {
            _vendorLinkingService = new VendorLinkingService(connectionString);
            VendorsProducts = _vendorLinkingService.PopulateVendorsProducts().GenericCollection;
        }

        public AddProductDTO ConvertFormToProduct(IFormCollection formData)
        {
            // This try catch catches a format exception or a null reference exception
            try
            {
                string ModelNumber = formData["modelNumber"];
                string Name = formData["name"];
                string Price = formData["price"];
                string Url = formData["url"];

                if (formData["modelNumber"] == "Model Number" || String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Price) || String.IsNullOrEmpty(Url))
                {
                    throw new FormatException("Invalid input");
                } 
                AddProductDTO product = new AddProductDTO();
                product.ModelNumber = ModelNumber;
                product.Name = Name;
                product.ImageUrl = formData["imageUrl"];
                product.Availability = Convert.ToBoolean(formData["availability"]);
                product.Url = Url;
                product.Price = Convert.ToDouble(Price);

                _logger.LogInformation("Successfully created an AddProductDTO.");
                return product;
            }
            catch (Exception ex)
            {
                if(ex is FormatException)
                {
                    _logger.LogWarning(ex.Message);
                }

                if(ex is NullReferenceException)
                {
                    _logger.LogWarning(ex.Message);
                }

                _logger.LogWarning("Returning a null product.");
                return null;
            }
        }

        public GetProductByFilterDTO ConvertToGetProductByFilterDTO(string filtersString, string order)
        {
            GetProductByFilterDTO filters = null;

            // If the filters string is null, return null.
            if(filtersString == null)
            {
                _logger.LogWarning("Filters string is null. Returning a null GetProductByFilterDTO.");
                return null;
            }

            // If order is null, set it to a default order.
            if(order == null)
            {
                order = "price_asc";
                _logger.LogWarning("No order was specified. Setting to price_asc as default.");
            }

            filters = new GetProductByFilterDTO();
            filters.PriceOrder = order;

            // The front end passes the whole query parameter over. This gets rid of "?filtersString" with "".
            filtersString = filtersString.Replace("?filtersString=", "");

            // The front end passes the string of filters which is comma separated. This separates them and stores each filter into a string in a string[].
            string[] SeparatedFilters = filtersString.Split(',');

            foreach (string Filter in SeparatedFilters)
            {
                // Only add filters to the dictionary if the dictionary doesn't already contain it. 
                if (!filters.FilteredListOfProducts.ContainsKey(Filter))
                {
                    filters.FilteredListOfProducts.Add(Filter, true);
                }
            }
            
            return filters;
        }

        public async Task<CommonResponse> AddProductToVendorListOfProducts(AddProductDTO product, IFormFile photo)
        {
            string Vendor = "danny";
            CommonResponse response = new CommonResponse();

            if(photo == null)
            {
                _logger.LogWarning("Image was not chosen. AddProductToVendorListOfProducts manager call failed.");
                response.ResponseString = "Image was not chosen.";
                response.ResponseBool = false;
                return response;
            }

            // If vendor doesn't exist, add it to our dictionary.
            if (!VendorsProducts.ContainsKey(Vendor))
            {
                HashSet<string> HashSet = new HashSet<string>();
                VendorsProducts.TryAdd(Vendor, HashSet);
            }

            // If the model number already exists for the current vendor, return false since it's a duplicate.
            if (VendorsProducts[Vendor].Contains(product.ModelNumber))
            {
                _logger.LogWarning("This vendor already has this model number. AddProductToVendorListOfProducts manager call failed.");
                response.ResponseString = "This vendor already has this model number. ";
                response.ResponseBool = false;

                return response;
            }

            // Uploads image to the location and saves the path to the product's imageUrl field.
            product.ImageUrl = await _vendorLinkingService.UploadImage("", photo);
            _logger.LogInformation("Successfully uploaded the image in AddProductToVendorListOfProducts.");

            return _vendorLinkingService.AddProductToVendorListOfProducts(product);
        }

        public async Task<CommonResponse> EditProductInVendorListOfProducts(AddProductDTO product, IFormFile photo)
        {
            // If a photo is selected, update the image and the image path.
            if(photo != null)
            {
                // Uploads image to the location and saves the path to the product's imageUrl field.
                product.ImageUrl = await _vendorLinkingService.UploadImage("", photo);
                _logger.LogInformation("Successfully edited the image.");
            }

            _logger.LogInformation("Successfully uploaded the image in AddProductToVendorListOfProducts.");
            return _vendorLinkingService.EditProductInVendorListOfProducts(product);
        }

        public CommonResponse DeleteProductFromVendorList(string modelNumber)
        {
            //log
            return _vendorLinkingService.DeleteProductFromVendorList(modelNumber);
        }

        public CollectionCommonResponse<List<AddProductDTO>> GetAllProductsByVendor(GetProductByFilterDTO filters)
        {
            //log
            return _vendorLinkingService.GetAllProductsByVendor(filters);
        }

        public CollectionCommonResponse<List<string>> GetAllModelNumbers()
        {
            //log
            return _vendorLinkingService.GetAllModelNumbers();
        }

    }
}
