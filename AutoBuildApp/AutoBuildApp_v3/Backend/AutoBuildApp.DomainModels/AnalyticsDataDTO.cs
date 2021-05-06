using AutoBuildApp.DomainModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DomainModels
{
    public class AnalyticsDataDTO
    {
        public bool SuccessFlag { get; set; }
        public string Result { get; set; }

        public Charts analyticChartsRequisted { get; set; }


        public AnalyticsDataDTO()
        {
            SuccessFlag = false;
            Result = "";
            analyticChartsRequisted = new Charts();
        }


        public override string ToString()
        {
            
            return analyticChartsRequisted.ToString();
        }
    }
}
