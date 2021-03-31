using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Products;

namespace AutoBuildApp.Models.Builds
{
    public class Gaming : IBuild
    {
        public Gaming()
        {
        }

        public List<IHardDrive> HardDrive { get; set; }
        public ComputerCase ComputerCase { get; set; }
        public Motherboard Motherboard { get; set; }
        public PowerSupplyUnit Psu { get; set; }
        public GPU Gpu { get; set; }
        public CPU Cpu { get; set; }
        public RAM Ram { get; set; }
        public ComputerCase Case { get; set; }
        public Motherboard Mobo { get; set; }
    }
}
