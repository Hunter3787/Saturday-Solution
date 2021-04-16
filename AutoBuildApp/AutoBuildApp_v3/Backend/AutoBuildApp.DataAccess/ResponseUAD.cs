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
            connectionState = false;

        }


        public string SuccessString { get; set; }
        public string FailureString { get; set; }

        public string ResponseString { get; set; }
        public bool SuccessBool { get; set; }
        public bool connectionState { get; set; }



        public override string ToString()
        {
            return $"\nResponse String {ResponseString }\n" +
                $"Success string {SuccessString}\nFailure String {FailureString}\n" +
                $"Success Bool {SuccessBool}\n" +
                $"Connection bool {connectionState}\n";

        }

    }
}
