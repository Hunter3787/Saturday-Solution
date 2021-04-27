using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels.Abstractions;
using AutoBuildApp.DomainModels.Enumerations;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
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

        private ClaimsFactory _claimsFactory = new ConcreteClaimsFactory();

        public UADManager(string _cnnctString)
        {
           
            _uadDAO = new UadDAO(_cnnctString);
            _AnalyticCharts = new List<Charts>();

        }
       
        /// <summary>
        /// method to check permissions needed per authorization service.
        /// </summary>
        /// <returns></returns>
        public bool isAuthorized()
        {
            IClaims _admin = _claimsFactory.GetClaims(RoleEnumType.SENIOR_ADMIN);
            // FIRST LINE OF DEFENCE 
            if (!AuthorizationService.checkPermissions(_admin.Claims()))
            {
                return false;
            }
            return true;

        }

        public string getAllChartData()
        {

           if (!isAuthorized())
           {
                AuthorizationResultType.NOT_AUTHORIZED.ToString();
           }
           if(!_responseUAD.IsAuthorized)
           {
                AuthorizationResultType.NOT_AUTHORIZED.ToString();
           }

           _responseUAD  = _uadDAO.GetAllAnalytics();

            if(_responseUAD.SuccessBool == false)
            {
                // in the case data is not being retrieved
                return _responseUAD.ResponseString;
            }
            /// time to populate the charts into the list:
            GetCharts(_responseUAD);
            return ("");

        }

        /// <summary>
        /// this method will be triggered upon 
        /// </summary>
        /// <param name="responseUAD"></param>
        /// <returns></returns>
        public IList<Charts> GetCharts(ResponseUAD responseUAD)
        {
            _responseUAD = responseUAD;


            ///for Bar 1 
            #region BAR GRAPH 1
            Charts mycharts =
                new Charts(
                    //"The Number Of Accounts Held Amongst Account Types",
                    ChartTitlesType.ACCOUNT_TYPES.ToString(),
                    ChartTitlesType.NUMBER_OF_ACCOUNTS.ToString(),
                    ChartTitlesType.NONE.ToString(),
                    _responseUAD.GetNumAccountsPerRole,
                    chartType: ChartType.BARCHART);
            // add it to the list:
            _AnalyticCharts.Add(mycharts);
            #endregion


            #region BAR GRAPH 2
            // for Bar 2 :
            new Charts(
                   //"Percentage usage of Autobuild components, usage by visits",
                   ChartTitlesType.NUMBER_OF_VISITS.ToString(),
                   ChartTitlesType.AUTOBUILD_COMPONENT.ToString(),
                    ChartTitlesType.NONE.ToString(),
                   _responseUAD.GetUsePerComponent,
                    chartType: ChartType.BARCHART);
            // add it to the list:
            _AnalyticCharts.Add(mycharts);
            #endregion


            #region BAR GRAPH 3
            // for bar graph 3:
            new Charts(
                   // "Average session duration of user by account type",
                   ChartTitlesType.ACCOUNT_TYPES.ToString(),
                   ChartTitlesType.TIME_SPENT_PER_HOUR.ToString(),
                    ChartTitlesType.NONE.ToString(),
                   _responseUAD.GetAvgSessDurPerRole,
                   chartType: ChartType.LINECHART);
            // add it to the list:
            _AnalyticCharts.Add(mycharts);
            #endregion


            #region LINE CHART 1:
            // for Line Chart 1:
            new Charts(
                  //  "Number os registrations that took place per month by account type",
                  ChartTitlesType.MONTH.ToString(),
                  ChartTitlesType.NUMBER_OF_REGISTRATIONS.ToString(),
                    ChartTitlesType.ACCOUNT_TYPES.ToString(),
                   _responseUAD.GetRegPerMonthByUserType,
                   chartType: ChartType.LINECHART);
            // add it to the list:
            _AnalyticCharts.Add(mycharts);

            #endregion


            #region LINE CHART 2:
            // for Line Chart 2:
            new Charts(
                    // "Number of views that took place by month per AutoBuild Component",
                    ChartTitlesType.MONTH.ToString(),
                    ChartTitlesType.NUMBER_OF_VISITS.ToString(),
                    ChartTitlesType.AUTOBUILD_COMPONENT.ToString(),
                   _responseUAD.GetPageViewPerMonth,
                   chartType: ChartType.LINECHART);
            // add it to the list:
            _AnalyticCharts.Add(mycharts);

            #endregion


            return _AnalyticCharts;
        }

    }
}
