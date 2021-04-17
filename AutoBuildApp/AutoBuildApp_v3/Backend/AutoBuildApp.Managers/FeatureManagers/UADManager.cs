using AutoBuildApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Managers.FeatureManagers
{
    public class UADManager
    { // this is to access the principleUser on the thread to update its identity and 
        // principle after they are authenticated 

        private UadDAO _uadDAO;


        public UADManager(string _cnnctString)
        {
            _uadDAO = new UadDAO(_cnnctString);

        }





    }
}
