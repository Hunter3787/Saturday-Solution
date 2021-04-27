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

        string Legend { get; set; }

        ChartType chartType { get; set; } 
        int XScale { get; set; }
        int YScale { get; set; }

        public IList<ChartData> ChartDatas;

        public Charts()
        {
            ChartTitle = ChartTitlesType.NONE.ToString();
            YAxisTitle = ChartTitlesType.Y_TITLE.ToString();
            XAxisTitle = ChartTitlesType.X_TITLE.ToString();
            Legend     = ChartTitlesType.LEGEND.ToString();
            XScale = 2;
            YScale = 2;
            chartType = ChartType.NONE;
            ChartDatas = new List<ChartData>();

        }

        // mandatory constructor parameter:

        public Charts(
            string XTitle,
            string YTitle,
            string legendTitle,
            IList<ChartData> chartDatas,
            ChartType chartType)
        {
            ChartTitle = YTitle + " PER" + XTitle + " BY "+ legendTitle;
            YAxisTitle = XTitle;
            XAxisTitle = YTitle;
            Legend = legendTitle;
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
