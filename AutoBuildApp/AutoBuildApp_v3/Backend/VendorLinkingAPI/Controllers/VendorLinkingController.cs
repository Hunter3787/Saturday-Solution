using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Models.VendorLinking;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendorLinkingAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class VendorLinkingController : Controller
    {
        private VendorLinkingManager _vendorLinkingManager = new VendorLinkingManager();
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
            bool result = _vendorLinkingManager.AddProductToVendorListOfProducts(product);
            return Ok();
        }

        [HttpPost]
        public IActionResult EditProductInVendorListOfProducts(AddProductDTO product)
        {
            bool result = _vendorLinkingManager.EditProductInVendorListOfProducts(product);
            return Ok();
        }
        /// <summary>
        /// This method will get all reviews from the DB.
        /// </summary>
        /// <returns>returns the status of OK as well as teh list of reviews.</returns>
        [HttpGet]
        public IActionResult GetAllProductsByVendor()
        {
            var productsByVendor = _vendorLinkingManager.GetAllProductsByVendor("new egg");
            return Ok(productsByVendor);
        }





    }
}
