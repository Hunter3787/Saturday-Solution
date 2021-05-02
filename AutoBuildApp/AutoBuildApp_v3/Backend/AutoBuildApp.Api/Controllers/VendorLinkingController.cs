using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
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
            _vendorClaims = _claimsFactory.GetClaims(RoleEnumType.VENDOR_ROLE);
        }
            // Initializes the DAO that will be used for review ratings.

            // This will start the logging consumer manager in the background so that logs may be sent to the DB.
            //private LoggingConsumerManager _loggingConsumerManager = new LoggingConsumerManager();

            //private LoggingProducerService _logger = LoggingProducerService.GetInstance;

            /// <summary>
            /// This class will show no contend if fetch Options is made.
            /// </summary>
            /// <returns>will return a page of no content to the view.</returns>
            //[HttpOptions]
            //public IActionResult PreflightRoute()
            //{
            //    _logger.LogInformation("HttpOptions was called");
            //    return NoContent();
            //}

        [HttpPost]
        public IActionResult AddProductToVendorListOfProducts(AddProductDTO product)
        {
            Console.WriteLine("image url = " + product.ImageUrl);
            bool result = _vendorLinkingManager.AddProductToVendorListOfProducts(product);
            return Ok();
        }

        [HttpPut]
        public IActionResult EditProductInVendorListOfProducts(AddProductDTO product)
        {
            Console.WriteLine("image url2 = " + product.ImageUrl);

            bool result = _vendorLinkingManager.EditProductInVendorListOfProducts(product);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllProductsByVendor(string filtersString, string order)
        {
            Console.WriteLine("order = " + order);
            GetProductByFilterDTO filters = null;
            if (filtersString != null)
            {
                filters = new GetProductByFilterDTO();
                filters.PriceOrder = order;
                filtersString = filtersString.Replace("?filtersString=", "");
                string[] SeparatedFilters = filtersString.Split(',');
                foreach (string f in SeparatedFilters)
                {
                    filters.FilteredListOfProducts.Add(f, true);
                }
            }
            if (!_threadPrinciple.Identity.IsAuthenticated)
            {

                // Add action logic here
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            if (!AuthorizationService.CheckPermissions(_vendorClaims.Claims()))
            {

                // Add action logic here
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
            Console.WriteLine("here");
            var productsByVendor = _vendorLinkingManager.GetAllProductsByVendor(filters);

            return Ok(productsByVendor);
        }
            /// <summary>
            /// This method will get all reviews from the DB.
            /// </summary>
            /// <returns>returns the status of OK as well as teh list of reviews.</returns>
            /// 

        //    [HttpGet]
        //public IActionResult GetAllProductsByVendor()
        //{
        //    if (!_threadPrinciple.Identity.IsAuthenticated)
        //    {

        //        // Add action logic here
        //        return new StatusCodeResult(StatusCodes.Status401Unauthorized);
        //    }
        //    if (!AuthorizationService.CheckPermissions(_vendorClaims.Claims()))
        //    {

        //        // Add action logic here
        //        return new StatusCodeResult(StatusCodes.Status403Forbidden);
        //    }
        //    var productsByVendor = _vendorLinkingManager.GetAllProductsByVendor("new egg");
        //    return Ok(productsByVendor);
        //}





    }
}
