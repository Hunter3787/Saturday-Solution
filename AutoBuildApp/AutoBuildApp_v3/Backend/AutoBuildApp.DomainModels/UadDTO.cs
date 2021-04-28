using AutoBuildApp.DomainModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DomainModels
{
    public class UadDTO
    {
        public bool SuccessFlag { get; set; }
        public string result { get; set; }

        public IList<Charts> analyticChartsRequisted { get; set; }


        public UadDTO()
        {
            SuccessFlag = false;
            result = " ";
            analyticChartsRequisted = new List<Charts>();
        }


        public override string ToString()
        {
            string ret = "";
            foreach (var elem in analyticChartsRequisted)
            {
                ret += $" elem. \n: {elem.ToString()} ";

            }

            return ret;
        }
    }
}
