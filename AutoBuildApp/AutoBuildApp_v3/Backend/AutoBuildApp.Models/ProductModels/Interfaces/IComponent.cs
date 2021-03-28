/**
 * Abstract IComponent interface that should enforce
 * the minimum details to be entered for a component.
 */
namespace Models.Products.Interfaces
{
    public interface IComponent
    {
        public string modelNumber { get; set; }
        public string productType { get; set; }
        public string productName { get; set; }
        public string manufacturerName { get; set; }
    }
}
