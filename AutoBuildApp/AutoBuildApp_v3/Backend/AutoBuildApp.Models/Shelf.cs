using System;
using System.Collections.Generic;
using AutoBuildApp.Models.Products;

/**
 * Shelf class to carry the shelf name 
 * and the list of components through the system.
 * @Author Nick Marshall-Eminger
 */
namespace AutoBuildApp.Models
{
    public class Shelf
    {
        public List<Component> ComponentList { get; set; }
        public string ShelfName { get; set; }

        public Shelf()
        {
            ComponentList = new List<Component>();
        }

    }
}
