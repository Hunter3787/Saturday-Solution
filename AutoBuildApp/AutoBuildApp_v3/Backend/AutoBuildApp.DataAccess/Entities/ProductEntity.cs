using System.Collections.Generic;

/**
 * Entity to carry a single products elements from a query.
 * Storing the product table, and specs table.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Entities
{
    public class ProductEntity
    {
        // Product table elements
        public string Type { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }

        // Specifications table elements
        public Dictionary<string, string> Specs { get; set; }
    }
}
