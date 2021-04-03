using AutoBuildApp.Models.Enumerations;
using System.Collections.Generic;
/**
 * Abstract IComponent interface that should enforce
 * the minimum details to define a speicfic component.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Interfaces
{
    /// <summary>
    /// Interface representation of Components/Products for the AutoBuildApp.
    /// </summary>
    public interface IComponent
    {
        ProductType ProductType { get; set; }
        string ModelNumber { get; set; }
        string ProductName { get; set; }
        string ManufacturerName { get; set; }
        int Quantity { get; set; }
        List<byte[]> ProductImages { get; set; }
        double Price { get; set; }
        double Budget { get; set; }

        /// <summary>
        /// Interface implementation of AddImage method.
        /// </summary>
        /// <param name="image"></param>
        /// <returns>Boolean</returns>
        bool AddImage(byte[] image);

        /// <summary>
        /// Interface implementation of RemoveImage method.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        bool RemoveImage(int index);

        /// <summary>
        /// Compute the total cost based of quanitity and price.
        /// </summary>
        /// <returns>Double</returns>
        double GetTotalcost();
    }
}
