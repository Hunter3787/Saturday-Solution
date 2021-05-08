using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;

/**
 * Fan class of ICooler implimenting both IComponent and ICooler.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models.Products
{
    public class Fan : Component, IComponent, ICooler
    {

        #region "Field Declarations: get; set;"
        public List<string> Colors { get; set; }
        public string FanRPM { get; set; }
        public string NoiseVolume { get; set; }
        public List<string> CompatableSockets { get; set; }
        public int FanSize { get; set; }
        public bool Fanless { get; set; }
        public bool WaterCooling { get; set; }
        #endregion

        public Fan() : base()
        {
            Colors = new List<string>();
            CompatableSockets = new List<string>();
        }

        #region "Color Add/Remove"
        /// <summary>
        /// Add a string representation of a color to the fan color list.
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
        /// Index method to remove a color from the fan color list.
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

        #region "Compatable Socket Add/Remove"
        /// <summary>
        /// Add a string representation of a color to the fan color list.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Boolean</returns>
        public bool AddCompatableSocket(string input)
        {
            ProductGuard.Exists(CompatableSockets, nameof(CompatableSockets));
            ProductGuard.IsNotEmpty(input, nameof(input));

            CompatableSockets.Add(input);
            return true;
        }

        /// <summary>
        /// String method to locate the index of the passed string.
        /// Will call index variation to remove the element upon location.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>Boolean</returns>
        public bool RemoveCompatableSocket(string toRemove)
        {
            ProductGuard.Exists(CompatableSockets, nameof(CompatableSockets));
            ProductGuard.IsNotEmpty(toRemove, nameof(toRemove));
            ProductGuard.ContainsElement(CompatableSockets, toRemove, nameof(toRemove));

            return RemoveCompatableSocket(CompatableSockets.IndexOf(toRemove));
        }

        /// <summary>
        /// Index method to remove a color from the fan color list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Boolean</returns>
        public bool RemoveCompatableSocket(int index)
        {
            ProductGuard.Exists(CompatableSockets, nameof(CompatableSockets));
            ProductGuard.IsInRange(CompatableSockets, index, nameof(CompatableSockets));

            CompatableSockets.RemoveAt(index);
            return true;
        }
        #endregion
    }
}
