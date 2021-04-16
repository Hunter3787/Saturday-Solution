using AutoBuildApp.DataAccess.Abstractions;
using AutoBuildApp.DomainModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DomainModels
{
    public class LineChart : Charts
    {
        public IList<ChartData> ChartDatas;

        public LineChart() : base()
        {

        }

    }
}
