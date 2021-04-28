using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AutoBuildApp.DataAccess
{
    public class ShelfDAO
    {
        private readonly string _connectionString;

        public ShelfDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool InsertShelf()
        {
            return false;
        }

        public bool RemoveShelf()
        {
            return false;
        }

        public bool AddComponent(IComponent item)
        {
            return false;
        }

        public bool RemoveComponent(int id)
        {
            return false;
        }

        public bool ModifyShelf(int id)
        {
            return false;
        }

        public List<IComponent> GetComponentsByShelf(int id)
        {
            List<IComponent> outputList = new List<IComponent>();

            return outputList;
        }

        public IComponent GetComponentByID(int id)
        {
            return null;
        }
    }    
}
