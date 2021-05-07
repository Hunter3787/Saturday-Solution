using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;
using System.Collections.Generic;
/**
 * Domain model to represent user preferences when requesting a bild.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.DomainModels
{
    public class RecommenderReqParams
    {
        public BuildType Build { get; set; }
        public double Budget { get; set; }
        public List<IComponent> PeripheralsList { get; set; }
        public PSUModularity Psu { get; set; }
        public HardDriveType HddType { get; set; }
        public int HddCount { get; set; }

        public RecommenderReqParams()
        {
            PeripheralsList = new List<IComponent>();
        }
    }
}
