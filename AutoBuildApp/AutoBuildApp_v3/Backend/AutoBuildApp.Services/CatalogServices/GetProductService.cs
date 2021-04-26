using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.DataAccess.Abstractions;
using AutoBuildApp.DataAccess.Entities;

/**
 * Getter that uses a data access object to call the and return,
 * from a database, a list or dictionary of products. This service
 * turns the returned query into concrete classes.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Services.CatalogServices
{
    public class GetProductService
    {
        private readonly IProductDAO _dao;

        /// <summary>
        /// Constructor that takes an injection of a data access object.
        /// </summary>
        /// <param name="acessObject">Data Access Object</param>
        public GetProductService(IProductDAO acessObject)
        {
            _dao = acessObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        public Dictionary<ProductType, List<IComponent>> GetProductDictionary(
            List<IComponent> components)
        {
            Dictionary<ProductType, List<IComponent>> outputDictionary
                = new Dictionary<ProductType, List<IComponent>>();

            // call DAO for rows

            // Parse rows by product type

            // Tokenize strings to fit the product models.

            // place into dictionary.


            return outputDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<IComponent> GetComponents(ProductType type)
        {
            List<IComponent> output = new List<IComponent>();


            return output;
        }
    }
}