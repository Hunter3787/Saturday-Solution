using AutoBuildApp.Models.Enumerations;
using System.Collections.Generic;
/**
 * Abstract IComponent interface that should enforce
 * the minimum details to define a speicfic component. 
 */
namespace AutoBuildApp.Models.Interfaces
{
    /// <summary>
    /// Interface representation of Components/Products for the AutoBuildApp.
    /// </summary>
    public interface IComponent
    {
        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<byte[]> ProductImage { get; set; }
        public double Price { get; set; }
        public double Budget { get; set; }

        /// <summary>
        /// Interface implementation of AddImage method.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public bool AddImage(byte[] image);

        /// <summary>
        /// Interface implementation of RemoveImage method.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RemoveImage(int index);
    }
}
