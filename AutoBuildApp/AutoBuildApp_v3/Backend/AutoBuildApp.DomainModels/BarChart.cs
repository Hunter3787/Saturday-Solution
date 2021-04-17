using AutoBuildApp.DataAccess.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DomainModels
{
    public class BarChart
    {
        public IList<ChartData> ChartDatas;
        int BarSpace { get; set; }

    }
}
