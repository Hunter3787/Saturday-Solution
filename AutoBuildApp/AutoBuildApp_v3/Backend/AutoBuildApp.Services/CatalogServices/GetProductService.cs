using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Services.FactoryServices;
using AutoBuildApp.Models.Products;
using System;

/**
 * Getter that uses a data access object to call the and return,
 * from a database, a list or dictionary of products. This service
 * turns the returned query into concrete classes.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Services
{
    public class GetProductService
    {
        private ProductDAO _productDao;
        private ProductFactory _productFactory;

        /// <summary>
        /// Constructor that takes an injection of a data access object.
        /// </summary>
        /// <param name="acessObject">Data Access Object</param>
        public GetProductService(ProductDAO acessObject)
        {
            _productFactory = new ProductFactory();
            _productDao = acessObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        public Dictionary<ProductType, List<IComponent>> GetComponentDictionary(List<IComponent> components)
        {
            Dictionary<ProductType, List<IComponent>> outputDictionary = new Dictionary<ProductType, List<IComponent>>();
            List<ProductEntity> entities = _productDao.GetEntities(components);

            // Parse rows by product type
            foreach(var entity in entities)
            {
                var toInsert = CreateIComponent(entity);

                if(toInsert != null)
                {
                    if (!outputDictionary.ContainsKey(toInsert.ProductType))
                    {
                        List<IComponent> componentList = new List<IComponent>();
                        componentList.Add(toInsert);
                        outputDictionary.Add(toInsert.ProductType, componentList);
                    }
                    else
                    {
                        outputDictionary[toInsert.ProductType].Add(toInsert);
                    }
                }
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
        public List<IComponent> GetComponentList(ProductType type)
        {
            List<IComponent> output = new List<IComponent>();


            return output;
        }

        public IComponent GetComponent(string pid)
        {
            return null;
        }

        #region Private methods
        /// <summary>
        /// Helper method to generate an IComponent from a product entity.
        /// </summary>
        /// <param name="toCreate"></param>
        /// <returns></returns>
        private IComponent CreateIComponent(ProductEntity toCreate)
        {
            int defaultQuantity = 1;

            IComponent output;

            try
            {
                var productType = (ProductType)Enum.Parse(typeof(ProductType), toCreate.ProductType);
                output = _productFactory.CreateComponent(productType);

                output.ProductType = productType;
                output.Quantity = defaultQuantity;
                output.ManufacturerName = toCreate.Manufacturer;
                output.ProductName = toCreate.ProductName;
                output.Price = toCreate.Price;
                output.ModelNumber = toCreate.Model;
                output.AddImage(toCreate.ImageURL);

                //FillComponentSpecs(toCreate, output);                
            }
            catch (ArgumentException)
            {
                // Log
                output = null;
            }

            return output;
        }

        /// <summary>
        /// Fill the components specs based on the passed entity and component.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="component"></param>
        private void FillComponentSpecs(ProductEntity entity, IComponent component)
        {
            var specDictionary = entity.Specs;
            var type = component.ProductType;

            try
            {
                switch (type)
                {
                    case ProductType.Case:
                        break;
                    case ProductType.Fan:
                        break;
                    case ProductType.GPU:
                        break;
                    case ProductType.CPU:
                        break;
                    case ProductType.HDD:
                        break;
                    case ProductType.SSD:
                        break;
                    case ProductType.RAM:
                        break;
                    case ProductType.PSU:
                        ((PowerSupplyUnit)component).Wattage = int.Parse(specDictionary["Maximum Power"]);

                        break;
                    case ProductType.Motherboard:
                        ((Motherboard)component).MoboForm = specDictionary["Form Factor"];
                        ((Motherboard)component).Socket = specDictionary["CPU Socket Type"];
                        ((Motherboard)component).MaxMemory = specDictionary["Maximum Memory Supported"];
                        //MemoryType memType = (MemoryType)Enum.Parse(typeof(MemoryType), specDictionary["Memory Standard"]);
                        //((Motherboard)component).MaxMemoryType = ;
                        ((Motherboard)component).Chipset = specDictionary["Chipset"];
                        break;
                    case ProductType.Cooler:
                        break;
                    default:
                        break;
                }


            }
            catch (Exception)
            {
                // Log
            }
        }
        #endregion
    }
}