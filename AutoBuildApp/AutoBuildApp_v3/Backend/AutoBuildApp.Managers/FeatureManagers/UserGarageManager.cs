using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.DataAccess;
using System.Collections.Generic;

/**
* User Garage Manager class that directs 
* operations with regards to the user garage.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Managers
{
    public class UserGarageManager
    {
        private BuildDAO _buildDAO;

        public UserGarageManager(string connectionString)
        {
            _buildDAO = new BuildDAO(connectionString);
        }

        public bool AddBuild(IBuild build)
        {
            return false;
        }

        public bool CopyBuildToGarage(string buildID)
        {
            return false;
        }

        public bool DeleteBuild(string buildID)
        {
            return false;
        }

        public List<IBuild> GetBuilds(string id, string order)
        {
            List<IBuild> outputList = new List<IBuild>();

            return outputList;
        }

        public bool PublishBuild(IBuild buildID)
        {
            return false;
        }

        public bool ModifyBuild(IBuild build)
        {
            return false;
        }

        public bool AddShelf()
        {
            return false;
        }

        public bool DeleteShelf(string shelfID)
        {
            return false;
        }

        public bool AddToShelf(IComponent item, string shelfID)
        {
            return false;
        }

        public bool CopyToShelf(IComponent item, string originID, string destinationID)
        {
            return false;
        }

        public bool RemoveFromShelf(int itemIndex, string shelfID)
        {
            return false;
        }

        public bool ModifyCount(int count, int itemIndex, string shelfID)
        {
            return false;
        }

        public List<IComponent> GetShelf(string shelfID)
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
