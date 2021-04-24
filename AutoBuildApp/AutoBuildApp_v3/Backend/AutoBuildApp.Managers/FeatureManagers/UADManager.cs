using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Managers.FeatureManagers
{
    public class UADManager
    { // this is to access the principleUser on the thread to update its identity and 
        // principle after they are authenticated 

        private UadDAO _uadDAO;

        private ResponseUAD _responseUAD;


        public UADManager(string _cnnctString)
        {
            _uadDAO = new UadDAO(_cnnctString);

        }
        /// here in the manager we need to get the data 
        /// from the UAD DAO and populate into graphs with actual values.
        /// 


        ///the controller will call the getAll method here that
        /// will get all data
        /// 
        
        public IList<Charts> getAllChartData()
        {
            var data  = _uadDAO.GetAllAnalytics();
            /// so here is the work part, I already know what to populate
            /// into the charts but since they are all of type charts I can 
            /// just leave the actual chart drawing in the from and 
            /// just send back the list of chart data within the list of charts
            /// its easier. data is data, objects are onjects so yeah.


            return null;
        }

    }
}
