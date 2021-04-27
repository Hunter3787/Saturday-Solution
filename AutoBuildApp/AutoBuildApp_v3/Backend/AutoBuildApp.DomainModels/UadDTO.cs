using AutoBuildApp.DomainModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DomainModels
{
    public class UadDTO
    {
        public string result { get; set; }

        public IList<Charts> analyticChartsRequisted { get; set; }


        public UadDTO()
        {
            result = " ";
            analyticChartsRequisted = new List<Charts>();
        }

    }
}
