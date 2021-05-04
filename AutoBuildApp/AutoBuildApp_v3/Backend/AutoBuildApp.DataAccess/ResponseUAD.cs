using AutoBuildApp.Models.DataTransferObjects;
using System.Collections.Generic;

namespace AutoBuildApp.Models
{
    public class ResponseUAD : CommonResponse
    {
        public IList<ChartData> GetChartDatas { get; set; }
        public string XTitle { get; set; }
        public string YTitle { get; set; }
        public string LegendTitle { get; set; }

        public bool ConnectionState { get; set; }
        public bool IsAuthorized { get; set; }

        public ResponseUAD()
        {

            ResponseString = " ";
            ResponseBool = false;
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
                $"Success Bool {ResponseBool}\n" +
                $"Connection bool {ConnectionState}\n" +
                $"IsAuthorized {IsAuthorized}\n";

        }

    }
}
