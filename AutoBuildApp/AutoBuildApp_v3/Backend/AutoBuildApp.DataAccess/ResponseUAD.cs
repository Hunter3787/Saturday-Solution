using AutoBuildApp.DataAccess.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class ResponseUAD : CommonResponse
    {

        public IList<ChartData> _chartDatas;





        public ResponseUAD()
        {

            ResponseString = " ";
            SuccessString = " ";
            FailureString = " ";
            SuccessBool = false;
            ConnectionState = false;
            // bar graph 1:
            GetNumAccountsPerRole = new List<ChartData>();
            GetUsePerComponent = new List<ChartData>();
            GetRegPerMontthByUserType = new List<ChartData>();
            GetAvgSessDurPerRole = new List<ChartData>();
            GetPageViewPerMonth = new List<ChartData>();
        }

        public IList<ChartData> GetNumAccountsPerRole { get; set; }


        public IList<ChartData> GetUsePerComponent { get; set; }


        public IList<ChartData> GetRegPerMontthByUserType { get; set; }



        public IList<ChartData> GetAvgSessDurPerRole { get; set; }


        public IList<ChartData> GetPageViewPerMonth{ get; set; }




        public string SuccessString { get; set; }
        public string FailureString { get; set; }

        public string ResponseString { get; set; }
        public bool SuccessBool { get; set; }
        public bool ConnectionState { get; set; }



        public override string ToString()
        {
            return $"\nResponse String {ResponseString }\n" +
                $"Success string {SuccessString}\nFailure String {FailureString}\n" +
                $"Success Bool {SuccessBool}\n" +
                $"Connection bool {ConnectionState}\n";

        }

    }
}
