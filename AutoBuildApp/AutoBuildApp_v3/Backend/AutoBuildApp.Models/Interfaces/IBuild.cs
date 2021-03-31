using System;
using AutoBuildApp.Models.Products;
using System.Collections.Generic;

namespace AutoBuildApp.Models.Interfaces
{
    public interface IBuild
    {
        public List<IHardDrive> HardDrive { get; set; }
        public ComputerCase Case { get; set; }
        public Motherboard Mobo { get; set; }
        public PowerSupplyUnit Psu { get; set; }
        public GPU Gpu { get; set; }
        public CPU Cpu { get; set; }
        public RAM Ram { get; set; }
    }
}
