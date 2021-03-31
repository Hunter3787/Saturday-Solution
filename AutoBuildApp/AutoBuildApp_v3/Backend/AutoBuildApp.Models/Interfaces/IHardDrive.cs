using System;
using AutoBuildApp.Models.Enumerations;

namespace AutoBuildApp.Models.Interfaces
{
    public interface IHardDrive : IComponent
    {
        public HardDriveType HardDrive { get; set; }
    }
}
