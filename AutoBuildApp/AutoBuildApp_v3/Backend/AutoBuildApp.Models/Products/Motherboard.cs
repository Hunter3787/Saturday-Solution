using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * Motherboard class that Implements the IComponent interface.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class Motherboard : Component, Interfaces.Component
    {
        #region "Field Declarations: get; set;"
        public string Socket { get; set; }
        public MoboFormFactor MoboForm{ get; set; }
        public string Chipset { get; set; }
        public string MaxMemory { get; set; }
        public MemoryType MaxMemoryType { get; set; }
        public List<string> SupportedMemory { get; set; }
        public List<string> Colors { get; set; }
        public int PCIEXSixTeenSlots { get; set; }
        public int PCIEXEightSlots { get; set; }
        public int PCIEXFourSlots { get; set; }
        public int PCIEXOneSlots { get; set; }
        public int PCISlots { get; set; }
        public List<string> M2Slots { get; set; }
        public int MSataSlots { get; set; }
        public List<string> OnboardEthernet { get; set; }
        public int SataSixPorts { get; set; }
        public string OnboardVideo { get; set; }
        public int USBTwoHeadersCount { get; set; }
        public int GenOneUSBThreeCount { get; set; }
        public int GenTwoUSBThreeCount { get; set; }
        public int GenTwoXTwoUSBThreeCount { get; set; }
        public bool ErrCorrctCodeSupport { get; set; }
        public bool WirelessNetworking { get; set; }
        public bool RaidSupport { get; set; }
        #endregion

        /// <summary>
        /// Default constructor with no initializatoin.
        /// </summary>
        public Motherboard() : base()
        {
            SupportedMemory = new List<string>();
            Colors = new List<string>();
            M2Slots = new List<string>();
            OnboardEthernet = new List<string>();
       }

        #region "Supported Memory Add/Remove"
        /// <summary>
        /// Add supported memory to the Motherboard SupportedMemory List.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddSupportedMemory(string input)
        {
            ProductGuard.Exists(SupportedMemory, nameof(SupportedMemory));
            ProductGuard.IsNotEmpty(input, nameof(input));

            SupportedMemory.Add(input);
            return true;
        }

        /// <summary>
        /// Locate the index of a string representation of the Supported Memory.
        /// Upon location will call the index variation to remove the element from the list.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>Boolean</returns>
        public bool RemoveSupportedMemory(string toRemove)
        {
            ProductGuard.Exists(SupportedMemory, nameof(SupportedMemory));
            ProductGuard.IsNotEmpty(toRemove, nameof(toRemove));
            ProductGuard.ContainsElement(SupportedMemory, toRemove, nameof(toRemove));

            return RemoveSupportedMemory(SupportedMemory.IndexOf(toRemove));
        }

        /// <summary>
        /// Removes the element from the Supported Memory list at the passed index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RemoveSupportedMemory(int index)
        {
            ProductGuard.Exists(SupportedMemory, nameof(SupportedMemory));
            ProductGuard.IsInRange(SupportedMemory, index, nameof(SupportedMemory));

            SupportedMemory.RemoveAt(index);
            return true;
        }
        #endregion

        #region "Color Add/Remove"
        /// <summary>
        /// Add a string representation of a color to the Motherboard color list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddColor(string input)
        {
            ProductGuard.Exists(Colors, nameof(Colors));
            ProductGuard.IsNotEmpty(input, nameof(input));

            Colors.Add(input);
            return true;
        }

        /// <summary>
        /// String method to locate the index of the passed string.
        /// Will call index variation to remove the element upon location.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>Boolean</returns>
        public bool RemoveColor(string toRemove)
        {
            ProductGuard.Exists(Colors, nameof(Colors));
            ProductGuard.IsNotEmpty(toRemove, nameof(toRemove));
            ProductGuard.ContainsElement(Colors, toRemove, nameof(toRemove));

            return RemoveColor(Colors.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove a color from the Motheboard color list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveColor(int index)
        {
            ProductGuard.Exists(Colors, nameof(Colors));
            ProductGuard.IsInRange(Colors, index, nameof(Colors));

            Colors.RemoveAt(index);
            return true;
        }
        #endregion

        #region "M2 Slots Add/Remove"
        /// <summary>
        /// Add a string representation of the M2 Slot specifications to the M2 list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddM2Slot(string input)
        {
            ProductGuard.Exists(M2Slots, nameof(M2Slots));
            ProductGuard.IsNotEmpty(input, nameof(input));

            M2Slots.Add(input);
            return true;
        }

        /// <summary>
        /// Locate and remove the string representation of the M2 slot specification
        /// from the M2Slots list.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>Boolean</returns>
        public bool RemoveM2Slot(string toRemove)
        {
            ProductGuard.Exists(M2Slots, nameof(M2Slots));
            ProductGuard.IsNotEmpty(toRemove, nameof(toRemove));
            ProductGuard.ContainsElement(M2Slots, toRemove, nameof(toRemove));

            return RemoveM2Slot(M2Slots.IndexOf(toRemove));
        }

        /// <summary>
        /// Removes the element at the passed index from the M2Slots list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveM2Slot(int index)
        {
            ProductGuard.Exists(M2Slots, nameof(M2Slots));
            ProductGuard.IsInRange(M2Slots, index, nameof(M2Slots));

            M2Slots.RemoveAt(index);
            return true;
        }
        #endregion

        #region "Onboard Ethernet Add/Remove"
        /// <summary>
        /// Adds string representation of the Onboard Ethernet specification to
        /// the OnboardEthernet list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddOnboardEthernet(string input)
        {
            ProductGuard.Exists(OnboardEthernet, nameof(OnboardEthernet));
            ProductGuard.IsNotEmpty(input, nameof(input));

            OnboardEthernet.Add(input);
            return true;
        }

        /// <summary>
        /// Locates and removes the string representation of the Onboard Ethernet
        /// specification from the OnboardEthernet list.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>Boolean</returns>
        public bool RemoveOnboardEthernet(string toRemove)
        {
            ProductGuard.Exists(OnboardEthernet, nameof(OnboardEthernet));
            ProductGuard.IsNotEmpty(toRemove, nameof(toRemove));
            ProductGuard.ContainsElement(OnboardEthernet, toRemove, nameof(OnboardEthernet));

            return RemoveOnboardEthernet(OnboardEthernet.IndexOf(toRemove));
        }

        /// <summary>
        /// Removes the element at the passed index from the OnboardEthernet
        /// list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveOnboardEthernet(int index)
        {
            ProductGuard.Exists(OnboardEthernet, nameof(OnboardEthernet));
            ProductGuard.IsInRange(OnboardEthernet, index, nameof(OnboardEthernet));

            OnboardEthernet.RemoveAt(index);
            return true;
        }
        #endregion
    }
}