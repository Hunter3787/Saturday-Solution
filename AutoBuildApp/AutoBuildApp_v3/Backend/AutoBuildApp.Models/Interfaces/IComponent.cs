using AutoBuildApp.Models.Enumerations;

/**
 * Abstract IComponent interface that should enforce
 * the minimum details to define a speicfic component. 
 */
namespace AutoBuildApp.Models.Interfaces
{
    public interface IComponent
    {
        public ProductType productType { get; set; }
        public string modelNumber { get; set; }
        public string productName { get; set; }
        public string manufacturerName { get; set; }
        public int quantity { get; set; }
    }
}
