using AutoBuildApp.DataAccess.Abstractions;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class ResponseUAD : CommonResponse
    {

        public IList<ChartData> GetNumAccountsPerRole { get; set; }
        public IList<ChartData> GetUsePerComponent { get; set; }
        public IList<ChartData> GetRegPerMonthByUserType { get; set; }

        public IList<ChartData> GetAvgSessDurPerRole { get; set; }
        public IList<ChartData> GetPageViewPerMonth{ get; set; }
        public bool ConnectionState { get; set; }

        public bool IsAuthorized { get; set; }

        public ResponseUAD()
        {

            ResponseString = " ";
            ResponseBool = false;
            ConnectionState = false;
            // bar graph 1:
            GetNumAccountsPerRole = new List<ChartData>();
            GetUsePerComponent = new List<ChartData>();
            GetRegPerMonthByUserType = new List<ChartData>();
            GetAvgSessDurPerRole = new List<ChartData>();
            GetPageViewPerMonth = new List<ChartData>();
        }


        public override string ToString()
        {
            return $"\nResponse String {ResponseString }\n" +
                $"Success Bool {ResponseBool}\n" +
                $"Connection bool {ConnectionState}\n" +
                $"IsAuthorized {IsAuthorized}\n";

        }

    }
}
