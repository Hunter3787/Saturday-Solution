
using AutoBuildApp.Logging;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Models.WebCrawler;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBuildApp.Managers.FeatureManagers
{
    public class VendorLinkingManager
    {
        private List<string> _allowedRoles;
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> vendorsProducts;
        private VendorLinkingService _vendorLinkingService;

        public VendorLinkingManager(string connectionString)
        {
            _vendorLinkingService = new VendorLinkingService(connectionString);
            CommonResponseWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> response = _vendorLinkingService.PopulateVendorsProducts();
            //Todo: fix cache
            vendorsProducts = _vendorLinkingService.PopulateVendorsProducts().GenericObject;
            _allowedRoles = new List<string>()
            {
                RoleEnumType.SystemAdmin,
                RoleEnumType.VendorRole
            };
        }

        public CommonResponseWithObject<AddProductDTO> ConvertFormToProduct(IFormCollection formData)
        {
            // Initialize a common response object
            CommonResponseWithObject<AddProductDTO> commonResponse = new CommonResponseWithObject<AddProductDTO>();

            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();
                commonResponse.ResponseBool = false;

                return commonResponse;
            }
            
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

                // Create the product and set all the values
                AddProductDTO product = new AddProductDTO();
                product.ModelNumber = ModelNumber;
                product.Name = Name;
                product.ImageUrl = formData["imageUrl"];
                product.Availability = Convert.ToBoolean(formData["availability"]);
                product.Url = Url;
                product.Price = Convert.ToDouble(Price);

                commonResponse.GenericObject = product;
                commonResponse.ResponseBool = true;
                commonResponse.ResponseString = "Successfully created an AddProductDTO.";

                return commonResponse;
            }
            catch (Exception ex)
            {
                if(ex is FormatException)
                {
                    _logger.LogWarning(ex.Message);
                    commonResponse.ResponseString = "One or more fields were empty.";
                }

                else if(ex is NullReferenceException)
                {
                    _logger.LogWarning(ex.Message);
                    commonResponse.ResponseString = "Parameter was null.";
                }
                else
                {
                    _logger.LogWarning("An error occurred in vendor linking manager.");
                    commonResponse.ResponseString = "An error occurred.";
                }

                commonResponse.ResponseBool = false;

                return commonResponse;
            }
        }

        public CommonResponseWithObject<GetProductByFilterDTO> ConvertToGetProductByFilterDTO(string filtersString, string order)
        {
            // Initialize a common response object
            CommonResponseWithObject<GetProductByFilterDTO> commonResponse = new CommonResponseWithObject<GetProductByFilterDTO>();

            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();
                commonResponse.ResponseBool = false;

                return commonResponse;
            }

            // If the filters string is null, return null.
            if (filtersString == null)
            {
                _logger.LogWarning("Filters string is null in vendor linking manager.");
                commonResponse.ResponseBool = false;
                commonResponse.ResponseString = "Filters string is null.";

                return commonResponse;
            }

            // If order is null, set it to a default order.
            if(order == null)
            {
                order = "price_asc";
                _logger.LogWarning("No order was specified. Setting to price_asc as default.");
            }

            // Assign filter dictionary to the response's object's dictionary
            Dictionary<string, bool> filtersDictionary = commonResponse.GenericObject.FilteredListOfProducts;

            // Assign the GetProductByFilterDTO's price order
            commonResponse.GenericObject.PriceOrder = order;

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

            commonResponse.ResponseBool = true;
            commonResponse.ResponseString = "Successfully converted to a GetProductByFilterDTO.";

            return commonResponse;
        }

        public async Task<CommonResponse> AddProductToVendorListOfProducts(AddProductDTO product, IFormFile photo)
        {
            // Initialize a common response object
            CommonResponse commonResponse = new CommonResponse();

            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();
                commonResponse.ResponseBool = false;

                return commonResponse;
            }

            // Get the current principal on the thread
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string vendor = _threadPrinciple.Identity.Name;
            
            string modelNumber = product.ModelNumber;

            // If photo is null, return
            if(photo == null)
            {
                _logger.LogWarning("Image was not chosen. AddProductToVendorListOfProducts manager call failed.");
                commonResponse.ResponseString = "Image was not chosen.";
                commonResponse.ResponseBool = false;

                return commonResponse;
            }

            // If vendor doesn't exist, add it to our dictionary with an initialized dictionary
            if (!vendorsProducts.ContainsKey(vendor))
            {
                ConcurrentDictionary<string, byte> dictionary = new ConcurrentDictionary<string, byte>();
                vendorsProducts.TryAdd(vendor, dictionary);
            }

            // If the model number already exists for the current vendor, return false since it's a duplicate
            if (vendorsProducts[vendor].ContainsKey(modelNumber))
            {
                _logger.LogWarning("This vendor already has this model number. AddProductToVendorListOfProducts manager call failed.");
                commonResponse.ResponseString = "This vendor already has this model number. ";
                commonResponse.ResponseBool = false;

                return commonResponse;
            }

            // Uploads image to the location and saves the path to the product's imageUrl field
            product.ImageUrl = await _vendorLinkingService.UploadImage("", photo);
            _logger.LogInformation("Successfully uploaded the image.");

            CommonResponse serviceResponse = _vendorLinkingService.AddProductToVendorListOfProducts(product);

            // If the database call was successful, we can add that product to our dictionary cache
            if(serviceResponse.ResponseBool)
            {
                vendorsProducts[vendor].TryAdd(modelNumber, 0);
            }

            return serviceResponse;
        }

        public async Task<CommonResponse> EditProductInVendorListOfProducts(AddProductDTO product, IFormFile photo)
        {
            // Initialize a common response object
            CommonResponse commonResponse = new CommonResponse();

            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();
                commonResponse.ResponseBool = false;

                return commonResponse;
            }
            
            // If a photo is selected to edit, update the image and the image path.
            if (photo != null)
            {
                // Uploads image to the location and saves the path to the product's imageUrl field.
                product.ImageUrl = await _vendorLinkingService.UploadImage("", photo);
                _logger.LogInformation("Successfully edited the image.");
            }

            return _vendorLinkingService.EditProductInVendorListOfProducts(product);
        }

        public CommonResponse DeleteProductFromVendorList(string modelNumber)
        {
            // Initialize a common response object
            CommonResponse commonResponse = new CommonResponse();

            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();
                commonResponse.ResponseBool = false;

                return commonResponse;
            }
            
            // Check if model number is null or empty
            if (String.IsNullOrEmpty(modelNumber))
            {
                _logger.LogWarning("User inputted a null model number. DeleteProductFromVendorList manager call failed.");
                commonResponse.ResponseString = "Model number is null.";
                commonResponse.ResponseBool = false;

                return commonResponse;
            }

            return _vendorLinkingService.DeleteProductFromVendorList(modelNumber);
        }

        public CommonResponseWithObject<List<AddProductDTO>> GetAllProductsByVendor(GetProductByFilterDTO filters)
        {
            // Initialize a common response object
            CommonResponseWithObject<List<AddProductDTO>> commonResponse = new CommonResponseWithObject<List<AddProductDTO>>();

            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();
                commonResponse.ResponseBool = false;

                return commonResponse;
            }

            return _vendorLinkingService.GetAllProductsByVendor(filters);
        }

        public CommonResponseWithObject<List<string>> GetAllModelNumbers()
        {
            CommonResponseWithObject<List<string>> commonResponse = new CommonResponseWithObject<List<string>>();

            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();
                commonResponse.ResponseBool = false;

                return commonResponse;
            }
            
            return _vendorLinkingService.GetAllModelNumbers();
        }

    }
}
