using AutoBuildApp.Models.Enumerations;

/**
 * IHardDrive interface for AuotBuild App
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Interfaces
{
    public interface IHardDrive : IComponent
    {
        HardDriveType DriveType { get; set; }
        string Capacity { get; set; }
        string Cache { get; set; }
    }
}
