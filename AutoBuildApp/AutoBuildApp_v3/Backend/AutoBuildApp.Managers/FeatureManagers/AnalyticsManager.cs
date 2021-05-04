using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels;
using AutoBuildApp.DomainModels.Abstractions;
using AutoBuildApp.DomainModels.Enumerations;
using AutoBuildApp.Models;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using System.Collections.Generic;



namespace AutoBuildApp.Managers.FeatureManagers
{
    public class AnalyticsManager
    { // this is to access the principleUser on the thread to update its identity and 
        // principle after they are authenticated 

        private AnalyticsDAO _uadDAO;
        private List<string> _allowedRoles; //specify rles
        

        public AnalyticsManager(string ConnectiontString)
        {
            _uadDAO = new AnalyticsDAO(ConnectiontString);
            _allowedRoles = new List<string>()
            { RoleEnumType.SystemAdmin };
        }

        //public IList<Charts> GetCharts(ResponseUAD responseUAD, DBViews specifiedChart = DBViews.none)
        //{
        //    IList<Charts> analyticCharts = new List<Charts>();

        //    if( specifiedChart == DBViews.NumberOfAccountTypes)
        //    {

        //        ///for Bar 1 
        //        #region BAR GRAPH 1
        //        analyticCharts.Add(
        //             new Charts(
        //                //"The Number Of Accounts Held Amongst Account Types",
        //                ChartTitlesType.AccountTypes.ToString(),
        //                ChartTitlesType.NumberOFAccounts.ToString(),
        //                ChartTitlesType.None.ToString(),
        //                responseUAD.GetNumAccountsPerRole,
        //                chartType: ChartType.Bar));
        //        #endregion


        //    }
        //    else if( specifiedChart == DBViews.VisitsPerFeature)
        //    {

        //        #region BAR GRAPH 2
        //        // for Bar 2 :
        //        analyticCharts.Add(
        //        new Charts(
        //               //"Percentage usage of Autobuild components, usage by visits",
        //               ChartTitlesType.NumberOfVisits.ToString(),
        //               ChartTitlesType.AutobuildComponent.ToString(),
        //                ChartTitlesType.None.ToString(),
        //               responseUAD.GetUsePerComponent,
        //                chartType: ChartType.Bar)
        //        );
        //        #endregion

        //    }
        //    else if ( specifiedChart == DBViews.none)
        //    {
        //        return null;
        //    }






        //    #region BAR GRAPH 3
        //    // for bar graph 3:
        //    // add it to the list:
        //    analyticCharts.Add(
        //    new Charts(
        //           // "Average session duration of user by account type",
        //           ChartTitlesType.AccountTypes.ToString(),
        //           ChartTitlesType.TimeSpentPerHour.ToString(),
        //            ChartTitlesType.None.ToString(),
        //           responseUAD.GetAvgSessDurPerRole,
        //           chartType: ChartType.Line));
        //    #endregion


        //    #region LINE CHART 1:
        //    // for Line Chart 1:
        //    // add it to the list:
        //    analyticCharts.Add(
        //    new Charts(
        //          //  "Number os registrations that took place per month by account type",
        //          ChartTitlesType.Month.ToString(),
        //          ChartTitlesType.NumberOfRegistrations.ToString(),
        //            ChartTitlesType.AccountTypes.ToString(),
        //           responseUAD.GetRegPerMonthByUserType,
        //           chartType: ChartType.Line));

        //    #endregion


        //    #region LINE CHART 2:
        //    // for Line Chart 2:
        //    analyticCharts.Add(
        //    new Charts(
        //            // "Number of views that took place by month per AutoBuild Component",
        //            ChartTitlesType.Month.ToString(),
        //            ChartTitlesType.NumberOfVisits.ToString(),
        //            ChartTitlesType.AutobuildComponent.ToString(),
        //           responseUAD.GetPageViewPerMonth,
        //           chartType: ChartType.Line));

        //    #endregion

        //    return analyticCharts;
        //}

        // TURN INTO ASYNC 
        public void GetChartData(int GraphType)
        {
            AnalyticsDataDTO dataDTO = new AnalyticsDataDTO();


            ResponseUAD responseUAD = new ResponseUAD();
            //Initial Authorization Check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                var result = AuthorizationResultType.NotAuthorized.ToString();
                return;

            }
            responseUAD = _uadDAO.GetGraphData((DBViews)GraphType);
            //from dao
            if (!responseUAD.IsAuthorized)
            {
                var result = AuthorizationResultType.NotAuthorized.ToString();
                return;

            }

            if (responseUAD.ResponseBool == false || responseUAD.GetChartDatas == null)
            {
                return ;
            }
            if (responseUAD.GetChartDatas != null)
            {

                Charts analyticsChart = new Charts();
                #region GRAPHS
                analyticsChart =
                     new Charts(
                        //"The Number Of Accounts Held Amongst Account Types",
                        XTitle: responseUAD.XTitle,
                        YTitle: responseUAD.YTitle,
                        legendTitle: responseUAD.LegendTitle,
                        chartDatas: responseUAD.GetChartDatas,
                        chartType: ChartType.Bar);
                #endregion

                return;

            }



        }












    }
}
