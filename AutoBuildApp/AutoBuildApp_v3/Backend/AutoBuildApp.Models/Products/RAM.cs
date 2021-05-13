using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;

/**
 * Random Access Memory class that invokes the IComonent interface.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    /// <summary>
    /// Random Access Memory class of type IComponent.
    /// </summary>
    public class RAM : Component, IComponent
    {
        #region "Field Declarations: get; set;"
        public int NumOfModules { get; set; }
        public int ModuleCapacity { get; set; }
        public string FormFactor { get; set; }
        public List<string> Color { get; set; }
        public string FirstWordLat { get; set; }
        public string CASLat { get; set; }
        public double Voltage { get; set; }
        public List<int> Timing { get; set; }
        public string ErrCorrctionCode { get; set; }
        public string Registered { get; set; }
        public bool HeatSpreader { get; set; }
        #endregion

        public RAM() : base()
        {
            Color = new List<string>();
            Timing = new List<int>();
        }
    }
}
