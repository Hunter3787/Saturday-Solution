using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers;
using AutoBuildApp.Models.Users;
using AutoBuildApp.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBuildApp.Apis.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class CatalogController : Controller
    {
        // Initializes the DAO that will be used for review ratings.
        private readonly CatalogGateway _catalogGateway = new CatalogGateway("Server = localhost; Database = DB; Trusted_Connection = True;");

        public IActionResult SearchForProduct(string componentName)
        {
            SearchService service = new SearchService(_catalogGateway);

            CatalogManager manager = new CatalogManager(service);

            return manager.SearchForComponent(componentName);
        }

        public IActionResult SaveProductToUser(string productName, UserAccount user)
        {
            CatalogManager manager = new CatalogManager(_catalogGateway);

            return manager.SaveComponentToUserAccount(productName, user);
        }

        public IActionResult GetProductDetails(string productName) 
        {
            CatalogManager manager = new CatalogManager(_catalogGateway);

            return manager.GetComponentDetails(productName);
        }
    }
}
