using System;
using AutoBuildApp.Models.Products;
using System.Collections.Generic;

namespace AutoBuildApp.Models.Interfaces
{
    public interface IBuild
    {
        public List<IHardDrive> hardDrive { get; set; }
        public Motherboard motherboard { get; set; }
        public PowerSupplyUnit psu { get; set; }
        public GPU gpu { get; set; }
        public CPU cpu { get; set; }
        public RAM ram { get; set; }
    }
}
