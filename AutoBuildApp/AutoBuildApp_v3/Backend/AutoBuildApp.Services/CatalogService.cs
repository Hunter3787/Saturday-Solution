using AutoBuildApp.Models.DTO;
using System.Collections.Generic;
using AutoBuildApp.Managers;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.Users;
using AutoBuildApp.Models.Catalog;

namespace AutoBuildApp.Services
{
    public class CatalogService
    {
        private CatalogGateway _gateway;
        public CatalogService(CatalogGateway gateway) 
        {
            _gateway = gateway;
        }

        public ISet<IResult> Search(string searchString, string resultType) {
            return _gateway.SearchForProduct(searchString, resultType);
        }

        public bool SaveProductToUser(string product, UserAccount user) 
        {
            return _gateway.SaveComponentToUser(product, user);
        }

        public ProductDetails GetProductDetails(string product)
        {
            return _gateway.GetComponentDetails(product);
        }
    }
}
