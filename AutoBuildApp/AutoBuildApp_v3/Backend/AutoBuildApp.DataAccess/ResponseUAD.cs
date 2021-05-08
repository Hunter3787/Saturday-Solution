using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace AutoBuildApp.DataAccess
{
    public class ResponseUAD : CommonResponse
    {
        public IList<ChartData> GetChartDatas { get; set; }
        public string XTitle { get; set; }
        public string YTitle { get; set; }
        public string LegendTitle { get; set; }

        public bool ConnectionState { get; set; }

        public ResponseUAD()
        {

            ResponseString = " ";
            IsSuccessful = false;
            ConnectionState = true;
            GetChartDatas = new List<ChartData>();
            LegendTitle = " ";
            XTitle = " ";
            YTitle = " ";
        }


        public override string ToString()
        {
            return 
                $"\nResponse String {ResponseString }\n" +
                $"Success Bool {IsSuccessful}\n" +
                $"Connection bool {ConnectionState}\n";

        }

    }
}
