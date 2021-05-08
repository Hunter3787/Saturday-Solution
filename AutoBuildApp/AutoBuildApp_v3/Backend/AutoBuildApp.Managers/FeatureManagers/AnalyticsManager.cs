using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels;
using AutoBuildApp.DomainModels.Abstractions;
using AutoBuildApp.DomainModels.Enumerations;
using AutoBuildApp.Logging;
using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using System;
using System.Collections.Generic;



namespace AutoBuildApp.Managers.FeatureManagers
{
    public class AnalyticsManager
    { // this is to access the principleUser on the thread to update its identity and 
        // principle after they are authenticated 

        private AnalyticsDAO _uadDAO;
        private List<string> _allowedRoles; //specify rles

        #region logegr 

        // Creates the local instance for the logger
        private LoggingProducerService _logger;

        #endregion


        public AnalyticsManager(string ConnectiontString)
        {
            try
            {
                 _uadDAO = new AnalyticsDAO(ConnectiontString);
                 _allowedRoles = new List<string>()
                { RoleEnumType.SystemAdmin };

                _logger = LoggingProducerService.GetInstance;

            }

            catch (ArgumentNullException)
            {
                if (ConnectiontString == null)
                {
                    var expectedParamName = "NULL OBJECT PROVIDED";
                    throw new ArgumentNullException(expectedParamName);
                }
            }

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
        public AnalyticsDataDTO GetChartData(int graphType)
        {
            AnalyticsDataDTO dataDTO = new AnalyticsDataDTO();
            ResponseUAD responseUAD = new ResponseUAD();

            string notAuthorized = AuthorizationResultType.NotAuthorized.ToString();

            //Initial Authorization Check
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {

              dataDTO.Result = notAuthorized;
                dataDTO.SuccessFlag = false;
                _logger.LogWarning("Unauthorized Access");
                return dataDTO;

            }
            responseUAD = _uadDAO.GetGraphData((DBViews)graphType);
            if (responseUAD.ResponseString.Equals(notAuthorized))
            {

                dataDTO.SuccessFlag = false;
                dataDTO.Result = responseUAD.ResponseString;



                return dataDTO;
            }
            if (!responseUAD.IsSuccessful|| responseUAD.GetChartDatas == null)
            {
                dataDTO.SuccessFlag = responseUAD.ResponseBool;
                dataDTO.Result = responseUAD.ResponseString;
                _logger.LogWarning($" analytics retriaval failed: {responseUAD.ResponseString}");

                return dataDTO;
            }
            else //if(responseUAD.ResponseBool)
            {
                IList<ChartData> GetChartDatas = responseUAD.GetChartDatas;


                dataDTO.SuccessFlag = responseUAD.ResponseBool;
                dataDTO.Result = responseUAD.ResponseString;

                Charts analyticsChart = new Charts();
                #region GRAPHS
                analyticsChart =
                     new Charts(
                        //"The Number Of Accounts Held Amongst Account Types",
                        XTitle: responseUAD.XTitle,
                        YTitle: responseUAD.YTitle,
                        legendTitle: responseUAD.LegendTitle,
                        chartDatas: GetChartDatas,
                        chartType: ChartType.Bar);
                #endregion
                dataDTO.analyticChartsRequisted = analyticsChart;
                return dataDTO;

            }
        }












    }
}
