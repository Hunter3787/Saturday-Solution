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

        private List<Charts> _AnalyticCharts;

        public UADManager(string _cnnctString)
        {
            _uadDAO = new UadDAO(_cnnctString);
            _AnalyticCharts = new List<Charts>();

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

            ///
            /// will populate each charts object
            /// then will add it to the _analiticCharts 
            /// thislist of chart object will be sent back 
            /// and the front will populate the charts based off
            /// of the chart type and data. 
            return null;
        }

    }
}
