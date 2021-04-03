using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.Models.Users;
using AutoBuildApp.Services;
using System;
using System.Collections.Generic;

namespace AutoBuildApp.Managers
{
    public class CatalogManager
    { 
        private readonly CatalogGateway gateway;

        ILogger logger = new LoggingService();

        public CatalogManager(String connectionString)
        {
            gateway = new CatalogGateway(connectionString);
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
                var searchService = new SearchService();
                return searchService.Search(searchString, "Component");
            }
        }

        // Feature: Save a component to a user account
        // Requirements:
        //      The user has to exist. 
        public bool SaveComponentToUserAccount(string component, UserAccount user) 
        {
            // If the user exists or the component does not exist, stop. 
            if (user == null || String.IsNullOrWhiteSpace(component))
            {
                return false;
            }

            // Return true if the component is saved to user
            return gateway.SaveComponentToUser(component, user);
        }

        // Feature: Get
        public ComponentDetails GetComponentDetails(string component) {
            var details =  gateway.GetComponentDetails(component);
        }
    }
}
