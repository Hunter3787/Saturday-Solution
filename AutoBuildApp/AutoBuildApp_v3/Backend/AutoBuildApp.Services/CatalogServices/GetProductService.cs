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
        private readonly string[] suffix = {"bytes", "KB", "MB", "GB", "TB", "PB"};
        private string _efficiency = "Efficiency";
        private string _formFactor = "Form Factor";
        private string _cpuSocketType = "CPU Socket Type";
        private string _maxMemorySupport = "Maximum Memory Supported";
        private string _chipset = "Chipset";
        private string _wattage = "Maximum Power";
        private string _modularity = "Modular";
        private string _capacity = "Capacity";
        private string _casLatency = "CAS Latency";
        private string _heatSpreader = "Heat Spreader";
        private string _errCorrection = "ECC";
        private string _voltage = "Voltage";
        private string _timing = "Timing";
        private string _speed = "Speed";
        private string _type = "Type";
        private string _cache = "Cache";
        



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
                output.ModelNumber = toCreate.ModelNumber;
                output.AddImage(toCreate.ImageURL);

                //FillComponentSpecs(toCreate, output);                
            }
            catch (ArgumentException ex)
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
                        if (specDictionary.ContainsKey(_capacity))
                        {

                        }
                        if (specDictionary.ContainsKey(_cache))
                        {

                        }
                        break;
                    case ProductType.RAM:
                        if (specDictionary.ContainsKey(_capacity))
                        {
                            var tempString = specDictionary[_capacity].Split();


                        }
                        if (specDictionary.ContainsKey(_casLatency))
                        {

                        }
                        if (specDictionary.ContainsKey(_errCorrection))
                        {

                        }
                        if (specDictionary.ContainsKey(_heatSpreader))
                        {

                        }
                        if (specDictionary.ContainsKey(_voltage))
                        {

                        }
                        if (specDictionary.ContainsKey(_speed))
                        {

                        }
                        if (specDictionary.ContainsKey(_timing))
                        {

                        }
                        if (specDictionary.ContainsKey(_type))
                        {

                        }

                        break;
                    case ProductType.PSU:
                        if (specDictionary.ContainsKey(_wattage))
                        {
                            ((PowerSupplyUnit)component).Wattage = int.Parse(specDictionary[_wattage]);
                        }
                        if (specDictionary.ContainsKey(_efficiency))
                        {
                            ((PowerSupplyUnit)component).EfficiencyRating = specDictionary[_efficiency];
                        }
                        //if (specDictionary.ContainsKey(_modularity))
                        //{
                        //    ((PowerSupplyUnit)component).PsuModulartiy = (PSUModularity)Enum.Parse(typeof(PSUModularity), specDictionary[_modularity]);
                        //}
                        break;
                    case ProductType.Motherboard:
                        if (specDictionary.ContainsKey(_formFactor))
                        {
                            ((Motherboard)component).MoboForm = specDictionary[_formFactor];
                        }
                        if (specDictionary.ContainsKey(_cpuSocketType))
                        {
                            ((Motherboard)component).Socket = specDictionary[_cpuSocketType];
                        }
                        if (specDictionary.ContainsKey(_maxMemorySupport))
                        {
                            ((Motherboard)component).MaxMemory = specDictionary[_maxMemorySupport];
                        }
                        if (specDictionary.ContainsKey(_chipset))
                        {
                            ((Motherboard)component).Chipset = specDictionary[_chipset];
                        }
                        //MemoryType memType = (MemoryType)Enum.Parse(typeof(MemoryType), specDictionary["Memory Standard"]);
                        //((Motherboard)component).MaxMemoryType = ;
                        break;
                    case ProductType.Cooler:
                        // TODO: Not yet implemented. No data in DB.
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

        private void Size
        #endregion
    }
}
