using AutoBuildApp.DataAccess.Abstractions;
using AutoBuildApp.DomainModels.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DomainModels.Abstractions
{
    public class Charts
    {

        string ChartTitle { get; set; }
        string YAxisTitle { get; set; }
        string XAxisTitle { get; set; }

        ChartType chartType { get; set; } 
        int XScale { get; set; }
        int YScale { get; set; }

        public IList<ChartData> ChartDatas;

        public Charts()
        {
            ChartTitle = "ChartTitle";
            YAxisTitle = "YAxisTitle";
            XAxisTitle = "XAxisTitle";
            XScale = 2;
            YScale = 2;
            chartType = ChartType.NONE;
            ChartDatas = new List<ChartData>();

        }

        // mandatory constructor parameter:

        public Charts(
            string title,
            string XTitle,
            string YTitle,
            List<ChartData> chartDatas,
            ChartType chartType)
        {
            ChartTitle = title;
            YAxisTitle = XTitle;
            XAxisTitle = YTitle;
            XScale = 2;
            YScale = 2;
            this.chartType = chartType;
            ChartDatas = chartDatas;

        }



        /// <summary>
        /// ToString() method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"ChartTitle : {ChartTitle}\n" +
                $"YAxisTitle : { YAxisTitle }\n" +
                $"XAxisTitle : { XAxisTitle}\n" +
                $"XScale : { XScale}\n" +
                $"YScale : {YScale}\n.";
        }

    }
}
