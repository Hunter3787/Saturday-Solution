using Models.Products.Interfaces;

namespace ProductModels.Models
{
    public class CPU : IComponent
    {
        public string modelNumber { get; set; }
        public string productType { get; set; }
        public string productName { get; set; }
        public string manufacturerName { get; set; }

        public CPU()
        {

        }

    }
}
