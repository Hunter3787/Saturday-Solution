using AutoBuildApp.DataAccess;
using AutoBuildApp.DataAccess.Abstractions;
using AutoBuildApp.DomainModels;
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
        private UadDTO _uadDTO;
        private ClaimsFactory _claimsFactory = new ConcreteClaimsFactory();

        private  List<string> _allowedRoles; //specify rles
        

        public UADManager(string _cnnctString)
        {
           
            _uadDAO = new UadDAO(_cnnctString);
            _responseUAD = new ResponseUAD();

            _uadDTO = new UadDTO();

            _allowedRoles = new List<string>()
            { RoleEnumType.SYSTEM_ADMIN };


        }
       
        ///// <summary>
        ///// method to check permissions needed per authorization service.
        ///// </summary>
        ///// <returns></returns>
        //public bool IsAuthorized(List<string> _allowedRoles)
        //{
        //    foreach (string role in _allowedRoles)
        //    {
        //        IClaims _claims = _claimsFactory.GetClaims(role);
        //    // FIRST LINE OF DEFENCE 
          
        //        if (AuthorizationService.CheckPermissions(_claims.Claims()))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public UadDTO GetAllChartData()
        {
            //request hits the manager
           
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                _uadDTO.result = AuthorizationResultType.NOT_AUTHORIZED.ToString();
                return _uadDTO;

            }



           _responseUAD = _uadDAO.GetAllAnalytics();

            //from dao
            if (!_responseUAD.IsAuthorized)
            {
                _uadDTO.result = AuthorizationResultType.NOT_AUTHORIZED.ToString();
                return _uadDTO;

            }


            if(_responseUAD.SuccessBool == false)
            {
                _uadDTO.SuccessFlag = false;
                _uadDTO.result = _responseUAD.ResponseString;
                return _uadDTO;
            }
            if(GetCharts(_responseUAD) != null)
            {

                /// time to populate the charts into the list:
                /// 
                _uadDTO.SuccessFlag = true;

                _uadDTO.analyticChartsRequisted = GetCharts(_responseUAD);

                return _uadDTO;

            }

            return _uadDTO;

        }

        /// <summary>
        /// this method will be triggered upon 
        /// </summary>
        /// <param name="responseUAD"></param>
        /// <returns></returns>
        public IList<Charts> GetCharts(ResponseUAD responseUAD)
        {
            IList<Charts> analyticCharts = new List<Charts>();

            ///for Bar 1 
            #region BAR GRAPH 1
            analyticCharts.Add(
                 new Charts(
                    //"The Number Of Accounts Held Amongst Account Types",
                    ChartTitlesType.ACCOUNT_TYPES.ToString(),
                    ChartTitlesType.NUMBER_OF_ACCOUNTS.ToString(),
                    ChartTitlesType.NONE.ToString(),
                    responseUAD.GetNumAccountsPerRole,
                    chartType: ChartType.BARCHART));
            #endregion


            #region BAR GRAPH 2
            // for Bar 2 :
            analyticCharts.Add(
            new Charts(
                   //"Percentage usage of Autobuild components, usage by visits",
                   ChartTitlesType.NUMBER_OF_VISITS.ToString(),
                   ChartTitlesType.AUTOBUILD_COMPONENT.ToString(),
                    ChartTitlesType.NONE.ToString(),
                   responseUAD.GetUsePerComponent,
                    chartType: ChartType.BARCHART)
            );
            #endregion


            #region BAR GRAPH 3
            // for bar graph 3:
            // add it to the list:
            analyticCharts.Add(
            new Charts(
                   // "Average session duration of user by account type",
                   ChartTitlesType.ACCOUNT_TYPES.ToString(),
                   ChartTitlesType.TIME_SPENT_PER_HOUR.ToString(),
                    ChartTitlesType.NONE.ToString(),
                   responseUAD.GetAvgSessDurPerRole,
                   chartType: ChartType.LINECHART));
            #endregion


            #region LINE CHART 1:
            // for Line Chart 1:
            // add it to the list:
            analyticCharts.Add(
            new Charts(
                  //  "Number os registrations that took place per month by account type",
                  ChartTitlesType.MONTH.ToString(),
                  ChartTitlesType.NUMBER_OF_REGISTRATIONS.ToString(),
                    ChartTitlesType.ACCOUNT_TYPES.ToString(),
                   responseUAD.GetRegPerMonthByUserType,
                   chartType: ChartType.LINECHART));

            #endregion


            #region LINE CHART 2:
            // for Line Chart 2:
            analyticCharts.Add(
            new Charts(
                    // "Number of views that took place by month per AutoBuild Component",
                    ChartTitlesType.MONTH.ToString(),
                    ChartTitlesType.NUMBER_OF_VISITS.ToString(),
                    ChartTitlesType.AUTOBUILD_COMPONENT.ToString(),
                   responseUAD.GetPageViewPerMonth,
                   chartType: ChartType.LINECHART));

            #endregion

            return analyticCharts;
        }

    }
}
