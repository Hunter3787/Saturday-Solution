using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * Solid state drive class impliments the IHardDrive and IComponent
 * interfaces. Representing a hard drive of the solid state type.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class SolidStateDrive : HardDrive, Interfaces.Component
    {

        #region "Field Declarations: get; set;"
        public HardDriveType DriveType { get; set; }
        #endregion

        public SolidStateDrive() : base()
        {
            DriveType = HardDriveType.SSD;
        }
    }
}
