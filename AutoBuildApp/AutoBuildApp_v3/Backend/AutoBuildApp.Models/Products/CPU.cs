using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    public class CPU : IComponent
    {
        #region "Field Declarations, get; set;"
        private const int MIN_LIST_SIZE = 1;
        private const int MIN_INDEX = 0;

        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<byte[]> ProductImage { get; set; }
        public double Price { get; set; }
        public int CoreCount { get; set; }
        public string CoreClock { get; set; }
        public string BoostClock { get; set; }
        public double PowerDraw { get; set; }
        public string Series { get; set; }
        public string MicrorArchitecture { get; set; }
        public string CoreFamily { get; set; }
        public string Socket { get; set; }
        public string IntegratedGraphics { get; set; }
        public string MaxRam { get; set; }
        public string ErrCorrectionCodeSupport { get; set; }
        public string Packaging { get; set; }
        public List<string> L1Cache { get; set; }
        public List<string> L2Cache { get; set; }
        public List<string> L3Cache { get; set; }
        public string Lithograph { get; set; }
        public string HyperThreading { get; set; }
        #endregion

        /// <summary>
        /// Default consturctor.
        /// </summary>
        public CPU()
        {

        }

        /// <summary>
        /// Central Processing Unit class constructor that accepts all
        /// parameters for initialization. 
        /// </summary>
        /// <param name="productType">Enumeration of ProductType</param>
        /// <param name="modelNumber">String representation of products model number.</param>
        /// <param name="productName">String representation of product name.</param>
        /// <param name="manufacturerName">String representation of product manufacturer(maker).</param>
        /// <param name="quantity">Integer value of how many of the defined component
        /// this class represents.</param>
        /// <param name="productImage">List of byte arrays representing associated images.</param>
        /// <param name="coreCount">Integer value representing the cores
        /// count of processor.</param>
        /// <param name="coreClock"></param>
        /// <param name="boostClock"></param>
        /// <param name="powerDraw"></param>
        /// <param name="series"></param>
        /// <param name="microrArchitecture"></param>
        /// <param name="coreFamily"></param>
        /// <param name="socket"></param>
        /// <param name="integratedGraphics"></param>
        /// <param name="maxRam"></param>
        /// <param name="errCorrectionCodeSupport"></param>
        /// <param name="packaging"></param>
        /// <param name="l1Cache"></param>
        /// <param name="l2Cache"></param>
        /// <param name="l3Cache"></param>
        /// <param name="lithograph"></param>
        /// <param name="hyperThreading"></param>
        public CPU(ProductType productType, string modelNumber, string productName,
            string manufacturerName, int quantity, List<byte[]> productImage,
                int coreCount, string coreClock, string boostClock, string powerDraw,
                    string series, string microrArchitecture, string coreFamily,
                        string socket, string integratedGraphics, string maxRam,
                            string errCorrectionCodeSupport, string packaging,
                                List<string> l1Cache, List<string> l2Cache,
                                    List<string> l3Cache, string lithograph,
                                        string hyperThreading)
        {
            ProductType = productType;
            ModelNumber = modelNumber;
            ProductName = productName;
            ManufacturerName = manufacturerName;
            Quantity = quantity;
            ProductImage = productImage;
            CoreCount = coreCount;
            CoreClock = coreClock;
            BoostClock = boostClock;
            PowerDraw = powerDraw;
            Series = series;
            MicrorArchitecture = microrArchitecture;
            CoreFamily = coreFamily;
            Socket = socket;
            IntegratedGraphics = integratedGraphics;
            MaxRam = maxRam;
            ErrCorrectionCodeSupport = errCorrectionCodeSupport;
            Packaging = packaging;
            L1Cache = l1Cache;
            L2Cache = l2Cache;
            L3Cache = l3Cache;
            Lithograph = lithograph;
            HyperThreading = hyperThreading;
        }

        #region "Interface Implementations"
        /// <summary>
        /// Adds an image from a byte array to the component.
        /// </summary>
        /// <param name="image">Byte Array representing an image.</param>
        /// <returns>Success of addition from object.</returns>
        public bool AddImage(byte[] image)
        {
            if (image == null)
                return false;

            ProductImage.Add(image);
            return true;
        }

        /// <summary>
        /// Removes an 
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Success of removal from object.</returns>
        public bool RemoveImage(int index)
        {
            var success = false;
            var endOfList = ProductImage.Count - 1;

            if (index >= MIN_INDEX && ProductImage.Count >= MIN_LIST_SIZE
                && index <= endOfList)
            {
                ProductImage.RemoveAt(index);
                success = true;
            }

            return success;
        }
        #endregion
    }
}
