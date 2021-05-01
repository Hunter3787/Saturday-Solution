using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.DataAccess.Abstractions;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Services.FactoryServices;
using AutoBuildApp.Models.Products;

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
        private ProductDAO _productDao;

        /// <summary>
        /// Constructor that takes an injection of a data access object.
        /// </summary>
        /// <param name="acessObject">Data Access Object</param>
        public GetProductService(ProductDAO acessObject)
        {
            _productDao = acessObject;
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

            // Call DAO for rows
            List<ProductEntity> entities = _productDao.GetEntities(components);

            // Parse rows by product type
            foreach(var entity in entities)
            {
                // Interface : PCI-Express 3.0 x4 or NVMe; Form Factor : M.2  = NVMe harddrive
                // Interface : SATA III, SATA 6.0/gb; Form Factor : 2.5" = SSD
                // Interface : SATA III, SATA 6.0/gb; Form Factor : 3.5" = SATA Drive
                // 


            }

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

        public IComponent GetComponent(string pid)
        {
            return null;
        }
    }
}