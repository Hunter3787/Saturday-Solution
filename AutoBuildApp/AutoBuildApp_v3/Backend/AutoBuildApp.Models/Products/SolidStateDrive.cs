using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * Solid state drive class impliments the IHardDrive and IComponent
 * interfaces. Representing a hard drive of the solid state type.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class SolidStateDrive : HardDrive, IComponent
    {

        #region "Field Declarations: get; set;"
        public HardDriveType DriveType { get; set; }
        #endregion

        public SolidStateDrive() : base()
        {
            ProductType = ProductType.SSD;
            DriveType = HardDriveType.SSD;
        }
    }
}
