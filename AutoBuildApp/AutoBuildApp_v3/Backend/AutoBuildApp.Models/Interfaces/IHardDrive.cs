using System;
using AutoBuildApp.Models.Enumerations;

namespace AutoBuildApp.Models.Interfaces
{
    public interface IHardDrive : IComponent
    {
        HardDriveType HardDrive { get; set; }
    }
}
