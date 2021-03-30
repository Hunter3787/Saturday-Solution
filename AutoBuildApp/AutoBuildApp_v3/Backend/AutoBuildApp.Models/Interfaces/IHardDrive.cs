using System;
using AutoBuildApp.Models.Enumerations;

namespace AutoBuildApp.Models.Interfaces
{
    public interface IHardDrive
    {
        public HardDriveType hardDriveType { get; set; }
    }
}
