using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;

/**
 * Water cooler class for the AutoBuild App.
 * Implements the IComponent and ICooler interfaces.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class WaterCooler : Component, IComponent, ICooler
    {
        #region "Field Declarations: get; set;"
        public string FanRPM { get; set; }
        public string NoiseVolume { get; set; }
        public List<string> CompatableSockets { get; set; }
        public bool Fanless { get; set; }
        #endregion

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public WaterCooler() : base()
        { 
            CompatableSockets = new List<string>();
        }
    }
}
