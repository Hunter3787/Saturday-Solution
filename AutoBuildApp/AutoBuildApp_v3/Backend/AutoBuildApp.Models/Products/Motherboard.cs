using System.Collections.Generic;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.Interfaces;

/**
 * Motherboard class that Implements the IComponent interface.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class Motherboard : IComponent
    {
        #region "Field Declarations, get; set;"
        public readonly int MIN_LIST_SIZE = 1;
        public readonly int MIN_INDEX = 0;

        public ProductType ProductType { get; set; }
        public string ModelNumber { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public int Quantity { get; set; }
        public List<byte[]> ProductImages { get; set; }
        public double Price { get; set; }
        public double Budget { get; set; }
        public string Socket { get; set; }
        public MoboFormFactor MoboForm{ get; set; }
        public string Chipset { get; set; }
        public string MaxMemory { get; set; }
        public MemoryType MaxMemoryType { get; set; }
        public List<string> SupportedMemory { get; set; }
        public List<string> Color { get; set; }
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
        public Motherboard()
        {
            ProductImages = new List<byte[]>();
            SupportedMemory = new List<string>();
            Color = new List<string>();
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
            if (string.IsNullOrWhiteSpace(input) || SupportedMemory.Contains(input))
                return false;

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
            if (string.IsNullOrWhiteSpace(toRemove) || !SupportedMemory.Contains(toRemove))
                return false;

            return RemoveSupportedMemory(SupportedMemory.IndexOf(toRemove));
        }

        /// <summary>
        /// Removes the element from the Supported Memory list at the passed index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RemoveSupportedMemory(int index)
        {
            if (index > SupportedMemory.Count || index < MIN_INDEX)
                return false;

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
            if (string.IsNullOrWhiteSpace(input) || Color.Contains(input))
                return false;

            Color.Add(input);
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
            if (Color == null || string.IsNullOrWhiteSpace(toRemove) || !Color.Contains(toRemove))
                return false;

            return RemoveColor(Color.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove a color from the Motheboard color list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveColor(int index)
        {
            if (Color == null || index > Color.Count || index < MIN_INDEX)
                return false;

            Color.RemoveAt(index);
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
            if (string.IsNullOrWhiteSpace(input) || M2Slots.Contains(input))
                return false;

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
            if (M2Slots == null || string.IsNullOrWhiteSpace(toRemove) || !M2Slots.Contains(toRemove))
                return false;

            return RemoveM2Slot(M2Slots.IndexOf(toRemove));
        }

        /// <summary>
        /// Removes the element at the passed index from the M2Slots list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveM2Slot(int index)
        {
            if (M2Slots == null || index > M2Slots.Count || index < MIN_INDEX)
                return false;

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
            if (OnboardEthernet == null || string.IsNullOrWhiteSpace(input) || OnboardEthernet.Contains(input))
                return false;

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
            if (OnboardEthernet == null || string.IsNullOrWhiteSpace(toRemove) || !OnboardEthernet.Contains(toRemove))
                return false;

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
            if (OnboardEthernet == null || index > OnboardEthernet.Count || index < MIN_INDEX)
                return false;

            OnboardEthernet.RemoveAt(index);
            return true;
        }
        #endregion

        #region "Interface Implementations"
        /// <summary>
        /// Adds an image from a byte array to the component.
        /// </summary>
        /// <param name="image">Byte Array representing an image.</param>
        /// <returns></returns>
        public bool AddImage(byte[] image)
        {
            if (image == null)
                return false;

            ProductImages.Add(image);
            return true;
        }

        /// <summary>
        /// Removes an image from the byte array at the provided index.
        /// </summary>
        /// <param name="index">Position of the image intended to be deleted.</param>
        /// <returns></returns>
        public bool RemoveImage(int index)
        {
            if (ProductImages == null)
                return false;

            var success = false;
            var endOfList = ProductImages.Count - 1;

            if (index >= MIN_INDEX && ProductImages.Count >= MIN_LIST_SIZE
                && index <= endOfList)
            {
                ProductImages.RemoveAt(index);
                success = true;
            }

            return success;
        }
        /// <summary>
        /// Total cost of components based on quantity and price.
        /// </summary>
        /// <returns>Double</returns>
        public double GetTotalcost()
        {
            return Price * Quantity;
        }
        #endregion
    }
}