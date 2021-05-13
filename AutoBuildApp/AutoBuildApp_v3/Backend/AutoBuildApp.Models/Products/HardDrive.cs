using AutoBuildApp.Models.Interfaces;

/**
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class HardDrive : Component, IComponent, IHardDrive
    {
        public string Capacity { get; set; }
        public string Cache { get; set; }

        public HardDrive() : base()
        {
        }
    }
}
