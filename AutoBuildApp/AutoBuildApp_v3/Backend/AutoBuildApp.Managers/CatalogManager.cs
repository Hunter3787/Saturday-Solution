using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.Models.Catalog;
using AutoBuildApp.Models.Users;
using AutoBuildApp.Services;
using System;
using System.Collections.Generic;

namespace AutoBuildApp.Managers
{
    public class CatalogManager
    {
        CatalogService _catalogService;
        //ILogger logger = new LoggingService();

        public CatalogManager(CatalogService service)
        {
            _catalogService = service;
        }

        // Feature: Search for a component in the catalog
        // Requirements:
        public ISet<IResult> SearchForComponent(string searchString) {
            if (String.IsNullOrWhiteSpace(searchString))
            {
                throw new Exception("The search string is empty");
            }
            else 
            {
                return _catalogService.Search(searchString, "Component");
            }
        }
        // Feature: Save a component to a user account
        // Requirements:
        //      The user has to exist. 
        public bool SaveProductToUserAccount(string component, UserAccount user) 
        {
            // If the user exists or the component does not exist, stop. 
            if (user == null || String.IsNullOrWhiteSpace(component))
            {
                return false;
            }

            // Return true if the component is saved to user
            return _catalogService.SaveProductToUser(component, user);
        }

        // Feature: Get
        public ProductDetails GetProductDetails(string component) {
            var details = _catalogService.GetProductDetails(component);

            return details;
        }
    }
}
