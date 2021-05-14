using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * Power supply unit (PSU) class that implements the IComponent interface.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    /// <summary>
    /// Power Supply Unit class to represent the computers power source.
    /// </summary>
    public class PowerSupplyUnit : Component, IComponent
    {
        #region "Field Declarations: get; set;"
        public string FormFactor { get; set; }
        public int Wattage { get; set; }
        public int Length { get; set; }
        public string EfficiencyRating { get; set; }
        public bool Fanless { get; set; }
        public PSUModularity PsuModulartiy { get; set; }
        public int EPSConnectors { get; set; }
        public int SataConnectors { get; set; }
        public int MolexConnectors { get; set; }
        public int SixPlusTwoConnectors { get; set; }
        #endregion

        /// <summary>
        /// PowerSupplyUnit default constructor.
        /// </summary>
        public PowerSupplyUnit() : base()
        {
            ProductType = ProductType.PSU;
        }
    }
}
