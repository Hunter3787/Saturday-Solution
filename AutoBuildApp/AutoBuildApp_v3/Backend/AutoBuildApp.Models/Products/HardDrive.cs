using System;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

namespace AutoBuildApp.Models.Products
{
    public class HardDrive : Component, IHardDrive
    {
        public string Capacity { get; set; }
        public string Cache { get; set; }

        public HardDrive() : base()
        {
        }
    }
}
