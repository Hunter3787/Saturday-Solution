using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Products;

namespace AutoBuildApp.Services.FactoryServices
{
    public class HardDriveFactory
    {
        public HardDriveFactory()
        {
        }

        public IHardDrive CreateHardDrive(HardDriveType driveType)
        {
            switch (driveType)
            {
                case HardDriveType.NVMe:
                    return new NVMeDrive();
                case HardDriveType.SSD:
                    return new SolidStateDrive();
                case HardDriveType.None:
                case HardDriveType.Hybrid:
                    return new SATADrive();
                default:
                    return new SATADrive(driveType);
            }
        }
    }
}
