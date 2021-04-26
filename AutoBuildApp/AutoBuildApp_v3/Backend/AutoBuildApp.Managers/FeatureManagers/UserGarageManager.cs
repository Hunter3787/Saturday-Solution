using AutoBuildApp.Models.Interfaces;

/// <summary>
/// "Autobuild User Garage" manager class
/// to be built and handle management of user
/// garage functions.
///
/// "Coming soon"
/// </summary>
namespace AutoBuildApp.Managers
{
    public class UserGarageManager
    {
        public UserGarageManager()
        { 
        }

        public bool SaveBuild(IBuild build)
        {
            if (build == null)
            {
                return false;
            }
                
            return true;
        }

        public bool SaveToShelf(IComponent item, string shelfID)
        {
            if (item == null || shelfID == null)
            {
                return false;
            }
                
            return true;
        }
    }
}
