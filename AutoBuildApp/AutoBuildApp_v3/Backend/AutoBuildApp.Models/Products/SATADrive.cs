using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * A classic SATA hard drive that implements the IHardDrive and IComponent
 * interfaces.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class SATADrive : HardDrive, IComponent
    {
        #region "Field Declarations: get; set;"
        public HardDriveType DriveType { get; set; }
        #endregion

        public SATADrive() : base()
        {
        }

        public SATADrive(HardDriveType hardDriveType) : base()
        {
            ProductImageStrings = new List<string>();
            DriveType = hardDriveType;
        }        
    }
}
