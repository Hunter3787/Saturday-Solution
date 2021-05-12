using System.Collections.Generic;

/**
 * Entity to carry a single products elements from a query.
 * Storing the product table, and specs table.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.DataAccess.Entities
{
    public class ProductEntity
    {
        // Product table elements
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public string Manufacturer { get; set; }
        public string ImageURL { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }

        // Specifications table elements
        public Dictionary<string, string> Specs { get; set; }
    }
}
