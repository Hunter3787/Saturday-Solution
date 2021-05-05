using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * Graphics Processing Unit class for use with AutoBuild App
 * that implements the IComponent interface.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class GraphicsProcUnit : Component, IComponent
    {
        #region "Field Declarations: get; set;"
        public string Chipset { get; set; }
        public string Memory { get; set; }
        public string MemoryType { get; set; }
        public string CoreClock { get; set; }
        public string BoostClock { get; set; }
        public string EffectiveMemClock { get; set; }
        public string Interface { get; set; }
        public string Color { get; set; }
        public string FrameSync { get; set; }
        public double PowerDraw { get; set; }
        public int Length { get; set; }
        public int DVIPorts { get; set; }
        public int HDMIPorts { get; set; }
        public int MiniHDMIPorts { get; set; }
        public int DisplayPortPorts { get; set; }
        public int MiniDisplayPortPorts { get; set; }
        public int ExpansionSlotWidth { get; set; }
        public int Cooling { get; set; }
        public string ExternalPower { get; set; }
        #endregion

        /// <summary>
        /// Graphic Processing Unit Default Constructor.
        /// </summary>
        public GraphicsProcUnit() : base()
        {
        }
    }
}
