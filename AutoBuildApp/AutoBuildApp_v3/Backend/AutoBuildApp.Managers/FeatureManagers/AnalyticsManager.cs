using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels;
using AutoBuildApp.DomainModels.Abstractions;
using AutoBuildApp.DomainModels.Enumerations;
using AutoBuildApp.Logging;
using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AutoBuildApp.Managers.FeatureManagers
{
    public class AnalyticsManager
    { // this is to access the principleUser on the thread to update its identity and 
        // principle after they are authenticated 

        private AnalyticsDAO _uadDAO;
        //specifies the roles that the analytics controller allows 
        private List<string> _allowedRoles;

        #region logegr 

        // Creates the local instance for the logger
        private LoggingProducerService _logger;

        #endregion


        public AnalyticsManager(string connectiontString)
        {
            try
            {
                 _uadDAO = new AnalyticsDAO(connectiontString);
                // instantiation of the allowed roles
                 _allowedRoles = new List<string>()
                { RoleEnumType.SystemAdmin };

                // get instance of logger singleton
                _logger = LoggingProducerService.GetInstance;

            }

            catch (ArgumentNullException)
            {
                if (connectiontString == null)
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

        /// <summary>
        /// the GetChartData cals the DAO and passes throught the graphtype for retrieval
        /// </summary>
        /// <param name="graphType"></param>
        /// <returns></returns>
        public AnalyticsDataDTO GetChartData(int graphType)
        {
            // instantiation of the DTO that will be sent back to the controller
            AnalyticsDataDTO dataDTO = new AnalyticsDataDTO();
            // instantiation of the response which will be expected by the DAO
            ResponseUAD responseUAD = new ResponseUAD();

            // expected string in case of not authorized
            string notAuthorized = AuthorizationResultType.NotAuthorized.ToString();

            //Initial Authorization Check in the manager
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                // if not authorized send back :not authorized" with success flag False
                dataDTO.Result = notAuthorized;
                dataDTO.SuccessFlag = false;
                // log that failure 
                _logger.LogWarning(AuthorizationResultType.NotAuthorized.ToString() + "made by"
                    + Thread.CurrentPrincipal.Identity.Name);
                // return the dto with fail state back
                return dataDTO;

            }
            // if authorized go ahead and call the DAO for graph data 
            responseUAD = _uadDAO.GetGraphData((DBViews)graphType);
            if (responseUAD.ResponseString.Equals(notAuthorized))
            {
                dataDTO.SuccessFlag = false;
                dataDTO.Result = responseUAD.ResponseString;
                return dataDTO;
            }
            // if the response from the DAO does not contain data 
            if (!responseUAD.IsSuccessful|| responseUAD.GetChartDatas == null)
            {
                // update the DTO to have the results from the responseDAO
                dataDTO.SuccessFlag = responseUAD.IsSuccessful;
                dataDTO.Result = responseUAD.ResponseString;
                // log the fail
                _logger.LogWarning($" analytics retriaval failed: {responseUAD.ResponseString}");
                // return the dAO
                return dataDTO;
            }
            else // else everything went good
            {

                IList<ChartData> GetChartDatas = responseUAD.GetChartDatas;
                dataDTO.SuccessFlag = responseUAD.IsSuccessful;
                dataDTO.Result = responseUAD.ResponseString;
                // lets go ahead and store the charts datas into a list from the response from the UAD
                Charts analyticsChart = new Charts();
                #region GRAPHS
                analyticsChart =
                     new Charts(
                        // here we case between the toe objects 
                        XTitle: responseUAD.XTitle,
                        YTitle: responseUAD.YTitle,
                        legendTitle: responseUAD.LegendTitle,
                        chartDatas: GetChartDatas,
                        chartType: ChartType.Bar);
                #endregion
                dataDTO.analyticChartsRequisted = analyticsChart;
                //let us log that success of data retrieval and by the user on the thread.
                _logger.LogInformation(
                   Thread.CurrentPrincipal.Identity.Name,
                    Models.EventType.AdminDataRetrievalEvent,
                    PageIDType.AnalyticsPage.ToString(), 
                    Thread.CurrentPrincipal.Identity.Name.ToString());
                // finnally return the dTo to the controller 
                return dataDTO;

            }
        }
    }
}
