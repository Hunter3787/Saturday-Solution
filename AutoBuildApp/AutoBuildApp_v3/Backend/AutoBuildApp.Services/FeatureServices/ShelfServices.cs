using System.Collections.Generic;
using AutoBuildApp.Models.Interfaces;

/**
* The ShelfServices will provide the shelf specific operatons.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Services
{
    public class ShelfServices
    {
        public ShelfServices()
        {
        }

        public bool AddShelf()
        {
            return false;
        }

        public bool DeleteShelf()
        {
            return false;
        }

        public bool ChangeShelfName()
        {
            return false;
        }

        public bool AddToShelf()
        {
            return false;
        }

        public bool RemoveFromShelf()
        {
            return false;
        }

        public bool ModifyShelf()
        {
            return false;
        }

        public bool ChangeQuantity()
        {
            return false;
        }

        public List<IComponent> GetShelf()
        {
            List<IComponent> outputList = new List<IComponent>();

            return outputList;
        }

        public IComponent GetComponent()
        {
            IComponent output = null;

            return output;
        }
    }
}