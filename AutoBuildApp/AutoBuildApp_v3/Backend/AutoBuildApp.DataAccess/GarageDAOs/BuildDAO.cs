using AutoBuildApp.Models.Interfaces;
using System.Collections.Generic;

/**
* This Data Access Object will handle the database
* operations with regard to Builds, such as "add", "remove", "delete".
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.DataAccess
{
    public class BuildDAO
    {
        private readonly string _connectionString;

        public BuildDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IBuild GetBuildByID(int id)
        {




            return null;
        }

        public List<IBuild> GetListOfBuilds(string user)
        {
            List<IBuild> outputList = new List<IBuild>();

            return outputList;
        }

        public bool InsertBuild(IBuild build, string buildName, string user)
        {
            return false;
        }

        public bool DeleteBuild(int id)
        {
            return false;
        }

        public bool ModifyBuild(IBuild incoming, int buildID)
        {
            return false;
        }
    }
}
