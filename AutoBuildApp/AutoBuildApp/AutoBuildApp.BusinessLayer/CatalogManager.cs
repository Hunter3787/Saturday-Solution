using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.ServiceLayer;
using System;
using AutoBuildApp.Loggg;
//using Microsoft.Extensions.Options;
//using Microsoft.Extensions.Logging;
using Moq;

namespace AutoBuildApp.BusinessLayer
{
    public class CatalogManager
    {
        private readonly CatalogService service;

        ILogger logger = new LoggingService();

        public CatalogManager(String connectionString)
        {
            service = new CatalogService(connectionString);
        }

        public String SaveProductToUser(string product, UserAccount user)
        {
            return service.LoginUser(user);
        }

        public String SearchProduct(string product)
        {
            
        }

        public void GetProductDetails(string product) 
        { 
        
        }

        public bool SearchStringEmpty(string searchString) 
        {
            return searchString == null || searchString == " ";
        }
    }
}
