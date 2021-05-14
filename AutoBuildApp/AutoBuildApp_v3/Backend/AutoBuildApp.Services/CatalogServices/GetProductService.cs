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
        private readonly List<string> _supportedMemoryArray = new List<string>
        {
            "5333(OC)", "5133(OC)", "5000(OC)", "4800(OC)", "4600(OC)",
            "4500(OC)", "4400(OC)", "4400", "4267(OC)", "4133(OC)", "4133",
            "4000(OC)", "4000", "3866(OC)", "3800", "3733(OC)", "3600(OC)",
            "3600", "3466(OC)", "3400(OC)", "3333(OC)", "3200", "3000", "2933",
            "2800", "2666", "2400", "2133", "1866", "1600"
        };
        private readonly string _efficiency = "Efficiency";
        private readonly string _formFactor = "Form Factor";
        private readonly string _cpuSocketType = "CPU Socket Type";
        private readonly string _maxMemorySupport = "Maximum Memory Supported";
        private readonly string _chipset = "Chipset";
        private readonly string _wattage = "Maximum Power";
        private readonly string _modularity = "Modular";
        private readonly string _capacity = "Capacity";
        private readonly string _casLatency = "CAS Latency";
        private readonly string _heatSpreader = "Heat Spreader";
        private readonly string _errCorrection = "ECC";
        private readonly string _voltage = "Voltage";
        private readonly string _timing = "Timing";
        private readonly string _speed = "Speed";
        private readonly string _type = "Type";
        private readonly string _cache = "Cache";
        private readonly string _memoryStandard = "Memory Standard";
        private readonly string _coreCount = "# of Cores";
        private readonly string _socketType = "CPU Socket Type";
        private readonly string _L1cache = "L1 Cache";
        private readonly string _L2cache = "L2 Cache";
        private readonly string _L3cache = "L3 Cache";
        private readonly string _coreClock = "Operating Frequency";
        private readonly string _boostClock = "Max Turbo Frequency";
        private readonly string _powerDraw = "Thermal Design Power";
        private readonly string _microarchitecture = "Core Name";



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
                    #region Case = Computer Case
                    case ProductType.Case:
                        break;
                    #endregion
                    #region Case = Fan
                    case ProductType.Fan:
                        // TODO: Not yet implemented. No data in DB.
                        break;
                    #endregion
                    #region Case = Graphical Processing Unit
                    case ProductType.GPU:
                        break;
                    #endregion
                    #region Case = Central Processing Unit
                    case ProductType.CPU:
                        //if (specDictionary.ContainsKey(_coreCount))
                        //{
                        //    var tempString = specDictionary[_coreCount].ToLower();
                        //    var coreCount = 0;

                        //    if (tempString.Contains("quad"))
                        //    {
                        //        coreCount = 4;
                        //    }
                        //    if (t)

                        //    ((CentralProcUnit)component).CoreCount = coreCount;
                        //}

                        //if (specDictionary.ContainsKey[])
                        //{
                            
                        //}


                        //if (specDictionary.ContainsKey[])
                        //{

                        //}

                        //if (specDictionary.ContainsKey[])
                        //{

                        //}

                        //if (specDictionary.ContainsKey[])
                        //{

                        //}

                        //if (specDictionary.ContainsKey[])
                        //{

                        //}

                        //if (specDictionary.ContainsKey[])
                        //{

                        //}

                        //if (specDictionary.ContainsKey[])
                        //{

                        //}

                        //if (specDictionary.ContainsKey[])
                        //{

                        //}


                        //    ((CentralProcUnit)component).CoreClock;
                        //    ((CentralProcUnit)component).BoostClock;
                        //    ((CentralProcUnit)component).CoreCount;
                        //    ((CentralProcUnit)component).L1Cache;
                        //    ((CentralProcUnit)component).L2Cache;
                        //    ((CentralProcUnit)component).L3Cache;
                        //    ((CentralProcUnit)component).PowerDraw;
                        //    ((CentralProcUnit)component).Socket;
                        //    ((CentralProcUnit)component).MicrorArchitecture;
                        
                        break;
                    #endregion
                    #region Case = Spinning Hard Drive
                    case ProductType.HDD:
                        if (specDictionary.ContainsKey(_capacity))
                        {
                            ((SATADrive)component).Capacity = specDictionary[_capacity];
                        }
                        if (specDictionary.ContainsKey(_cache))
                        {
                            ((SATADrive)component).Capacity = specDictionary[_cache];
                        }
                        break;
                    #endregion
                    #region Case = Solid State Drive
                    case ProductType.SSD:
                        // In gigabytes.
                        if (specDictionary.ContainsKey(_capacity))
                        {
                            ((SolidStateDrive)component).Capacity = specDictionary[_capacity];
                        }
                        if (specDictionary.ContainsKey(_cache))
                        {
                            ((SolidStateDrive)component).Cache = specDictionary[_cache];
                        }
                        break;
                    #endregion
                    #region Case = RAM
                    case ProductType.RAM:
                        if (specDictionary.ContainsKey(_capacity))
                        {
                            var tempString = specDictionary[_capacity].Split(' ');

                            if(tempString.Length > 1)
                            {
                                var modCount = tempString[1].TrimStart('(');
                                var modCapacity = tempString[3].Remove(tempString[3].Length - 3);

                                ((RAM)component).NumOfModules = int.Parse(modCount);
                                ((RAM)component).ModuleCapacity = int.Parse(modCapacity);
                            }
                            else
                            {
                                var modCapacity = tempString[0].Remove(tempString[0].Length - 2);

                                ((RAM)component).NumOfModules = 1;
                                ((RAM)component).ModuleCapacity = int.Parse(modCapacity);
                            }

                        }

                        if (specDictionary.ContainsKey(_casLatency))
                        {
                            ((RAM)component).CASLat = specDictionary[_casLatency];
                        }

                        if (specDictionary.ContainsKey(_errCorrection))
                        {
                            ((RAM)component).ErrCorrctionCode = specDictionary[_errCorrection];
                        }

                        if (specDictionary.ContainsKey(_heatSpreader))
                        {
                            ((RAM)component).HeatSpreader = bool.Parse(specDictionary[_heatSpreader]);
                        }

                        if (specDictionary.ContainsKey(_voltage))
                        {
                            var tempString = specDictionary[_voltage];
                            var voltage = tempString.Remove(tempString.Length - 1);

                            ((RAM)component).Voltage = double.Parse(voltage);
                        }

                        if (specDictionary.ContainsKey(_speed))
                        {
                            ((RAM)component).Speed = specDictionary[_speed];
                        }

                        if (specDictionary.ContainsKey(_timing))
                        {
                            string[] splitArray = specDictionary[_timing].Split('-');
                            List<int> intList = new List<int>();

                            foreach(string toParse in splitArray)
                            {
                                intList.Add(int.Parse(toParse));
                            }

                            ((RAM)component).Timing = intList;
                        }

                        if (specDictionary.ContainsKey(_type))
                        {
                            ((RAM)component).FormFactor = specDictionary[_type];
                        }

                        break;
                    #endregion
                    #region Case = Power Supply Unit
                    case ProductType.PSU:
                        if (specDictionary.ContainsKey(_wattage))
                        {
                            ((PowerSupplyUnit)component).Wattage = int.Parse(specDictionary[_wattage]);
                        }
                        if (specDictionary.ContainsKey(_efficiency))
                        {
                            ((PowerSupplyUnit)component).EfficiencyRating = specDictionary[_efficiency];
                        }
                        if (specDictionary.ContainsKey(_modularity))
                        {
                            PSUModularity modularity;
                            var toConvert = specDictionary[_modularity].ToLower();

                            if(toConvert == "non-modular")
                            {
                                modularity = PSUModularity.NonModular;
                            }
                            else if (toConvert == "semi-modular")
                            {
                                modularity = PSUModularity.SemiModular;
                            }
                            else if(toConvert == "full modular")
                            {
                                modularity = PSUModularity.FullyModular;
                            }
                            else
                            {
                                modularity = PSUModularity.None;
                            }

                            ((PowerSupplyUnit)component).PsuModulartiy = modularity;
                        }
                        break;
                    #endregion
                    #region Case = Motherboard
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

                        if (specDictionary.ContainsKey(_memoryStandard))
                        {
                            var memTypeString = specDictionary[_memoryStandard];
                            MemoryType memType;
                            var supportedRam = memTypeString.Split();

                            if (memTypeString.Contains(MemoryType.DDR.ToString()))
                            {
                                memType = MemoryType.DDR;
                            }
                            else if (memTypeString.Contains(MemoryType.DDR2.ToString()))
                            {
                                memType = MemoryType.DDR2;
                            }
                            else if (memTypeString.Contains(MemoryType.DDR3.ToString()))
                            {
                                memType = MemoryType.DDR3;
                            }
                            else if (memTypeString.Contains(MemoryType.DDR4.ToString()))
                            {
                                memType = MemoryType.DDR4;
                            }
                            else
                            {
                                memType = MemoryType.None;
                            }

                            ((Motherboard)component).MaxMemoryType = memType;

                            foreach (string toCheck in supportedRam)
                            {
                                if (_supportedMemoryArray.Contains(toCheck))
                                {
                                    ((Motherboard)component).AddSupportedMemory(toCheck);
                                }
                            }
                        }

                        break;
                    #endregion
                    #region Case = Cooler
                    case ProductType.Cooler:
                        // TODO: Not yet implemented. No data in DB.
                        break;
                    #endregion
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
