using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.DomainModels;
using AutoBuildApp.DomainModels.Abstractions;
using AutoBuildApp.DomainModels.Enumerations;
using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using System.Collections.Generic;
using System;
using AutoBuildApp.Logging;
using AutoBuildApp.Models.Enumerations;

namespace AutoBuildApp.Api.Controllers
{

    [EnableCors("CorsPolicy")] // applying the cors policy so that the client can

    [Route("[controller]")] // the route
    // make a call the this controller 
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;

        // Creates the local instance for the logger
        private LoggingProducerService _logger;

        // instantiate the consumer. 

        private AnalyticsManager _analyticsManager;
        private List<string> _allowedRoles; //specify roles
        public AnalyticsController()
        {
            #region retrieving the roles for Authorization check:
            _allowedRoles = new List<string>()
            { RoleEnumType.SystemAdmin };

            #endregion


            #region getting the connection string and passing to the loginmanager
            // created a connection manager to access the connection strings in 
            // 1) the app settings .json file
            ConnectionManager conString = ConnectionManager.connectionManager;
            // 2) passing in the name I assigned my connection string 
            string connection = conString.GetConnectionStringByName("MyConnection");
            // Console.WriteLine($"connection string passed in controller: {connection} ");
            //3) connection string passed to the analytics manager
            #endregion


            _analyticsManager = new AnalyticsManager(connection);
            _logger = LoggingProducerService.GetInstance;

            _logger.LogInformation("Analytics page is called BY USER", Models.EventType.ViewPageEvent,
               PageIDType.AnalyticsPage.ToString(), Thread.CurrentPrincipal.Identity.Name.ToString());


            _logger.LogInformation("Analytics page is called PASSING NULL USER", Models.EventType.ViewPageEvent,
                PageIDType.AnalyticsPage.ToString(), "");


        }

        /*
         * There are two cases to consider:
         * A request coming through from:
         * 1) user is logged in BUT do not have permissions -> 
         * status code:  403 (don’t have permissions to do what they requested)
         * 
         * 
         * 
         * 2) user is NOT logged in and are not authorized -> 
         * status code: 401. here comes in the REDIRECT URL.
         * They are directed to the login page and later 
         * to the page intended (if they are aurthorized)
         * 
         */

        public IActionResult Index()
        {

            if (!_threadPrinciple.Identity.IsAuthenticated)
            {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                // Add action logic here
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
            return Ok("Good to Continue");

        }



        [HttpGet]
        public IActionResult RetrieveGraphs(int graphType)
        {
            //return Ok("good job");
            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                 _logger.LogWarning("Unauthorized Access!!!");

                _logger.LogWarning("StatusCodes.Status403Forbidden");

                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            AnalyticsDataDTO dataDTO  = _analyticsManager.GetChartData(graphType);
           

            if (dataDTO.Result.Equals(AuthorizationResultType.NotAuthorized.ToString()))
            {
                 _logger.LogWarning("Unauthorized Access Attempted");
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
            if (!dataDTO.SuccessFlag)
            {
                
                    // _logger.LogWarning(result.result);
                    var result = dataDTO.Result;
                return Ok(result);
                    //return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            return Ok(dataDTO.analyticChartsRequisted);


        }

    }
}
