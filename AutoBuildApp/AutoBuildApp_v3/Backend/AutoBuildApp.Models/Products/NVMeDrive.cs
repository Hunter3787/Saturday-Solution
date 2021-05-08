using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * NVMe internal hard drive class that implements the IHardDrive 
 * and IComponent interfaces. 
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class NVMeDrive : HardDrive, Interfaces.Component
    {
        #region "Field Declarations: get; set;"
        public HardDriveType DriveType { get; set; }
        public string FormFactor { get; set; }
        public string Interface { get; set; }
        public bool NVMe { get; set; }
        #endregion

        /// <summary>
        /// Default constructor with no initialization.
        /// </summary>
        public NVMeDrive() : base()
        {
            NVMe = true;
            DriveType = HardDriveType.NVMe;
        }
    }
}
