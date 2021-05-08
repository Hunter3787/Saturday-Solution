using System;
using System.Collections.Generic;
using System.Linq;
using AutoBuildApp.Models.Products;

/**
 * Shelf class to carry the shelf name 
 * and the list of components through the system.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models
{
    public class Shelf : IEquatable<Shelf>
    {
        public List<Component> ComponentList { get; set; }
        public string ShelfName { get; set; }

        public Shelf()
        {
            ComponentList = new List<Component>();
        }

        /// <summary>
        /// Override to the default Equals method.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (!(obj is Shelf))
            {
                return false;
            }
            else if (ShelfName == ((Shelf)obj).ShelfName
                && ComponentList.SequenceEqual(((Shelf)obj).ComponentList))
            {
                return true;
            }

            return false;
        }

        public bool Equals(Shelf other)
        {
            if(other == null)
            {
                return false;
            }

            return ShelfName == other.ShelfName
                && ComponentList.SequenceEqual(other.ComponentList);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
