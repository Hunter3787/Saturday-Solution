using AutoBuildApp.DataAccess.Abstractions;
using AutoBuildApp.DomainModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DomainModels
{
    class BarChart : Charts
    {
        public IList<ChartData> ChartDatas;
        int BarSpace { get; set; }

    }
}
