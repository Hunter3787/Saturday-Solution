using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * Central Processing unit class that implments the IComponent interface.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class CentralProcUnit : Component, IComponent
    {
        #region "Field Declarations, get; set;"
        public int CoreCount { get; set; }
        public string CoreClock { get; set; }
        public string BoostClock { get; set; }
        public double PowerDraw { get; set; }
        public string Series { get; set; }
        public string MicrorArchitecture { get; set; }
        public string CoreFamily { get; set; }
        public string Socket { get; set; }
        public string IntegratedGraphics { get; set; }
        public string MaxRam { get; set; }
        public bool ErrCorrectionCodeSupport { get; set; }
        public string Packaging { get; set; }
        public List<string> L1Cache { get; set; }
        public List<string> L2Cache { get; set; }
        public List<string> L3Cache { get; set; }
        public string Lithograph { get; set; }
        public string HyperThreading { get; set; }
        #endregion

        /// <summary>
        /// Default consturctor.
        /// </summary>
        public CentralProcUnit() : base()
        {
            L1Cache = new List<string>();
            L2Cache = new List<string>();
            L3Cache = new List<string>();
        }
    }
}
