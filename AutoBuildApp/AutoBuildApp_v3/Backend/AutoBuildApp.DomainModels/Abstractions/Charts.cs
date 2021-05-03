using AutoBuildApp.DataAccess.Abstractions;
using AutoBuildApp.DomainModels.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DomainModels.Abstractions
{
    public class Charts
    {

        public string ChartTitle { get; set; }
        public string YAxisTitle { get; set; }
        public string XAxisTitle { get; set; }

        public string Legend { get; set; }

        public ChartType chartType { get; set; }

        public int XScale { get; set; }
        public int YScale { get; set; }

        public IList<ChartData> ChartDatas { get; set; }

        public Charts()
        {
            this.ChartTitle = ChartTitlesType.NONE.ToString();
            YAxisTitle = ChartTitlesType.Y_TITLE.ToString();
            XAxisTitle = ChartTitlesType.X_TITLE.ToString();
            Legend = ChartTitlesType.LEGEND.ToString();
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
            ChartTitle = YTitle + " PER " + XTitle + " BY " + legendTitle;
            YAxisTitle = XTitle;
            XAxisTitle = YTitle;
            Legend = legendTitle;
            XScale = 2;
            YScale = 2;
            this.chartType = chartType;
            ChartDatas = new List<ChartData>();
            ChartDatas = chartDatas;

        }



        /// <summary>
        /// ToString() method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            string ret = " ";

            foreach( var elem in ChartDatas)
            {
                ret += $"{elem.ToString()}\n";
            }

            return
               $"ChartTitle : {this.ChartTitle}\n" +
               $"YAxisTitle : {this.YAxisTitle }\n" +
               $"XAxisTitle : { this.XAxisTitle}\n" +
               $"XScale : { this.XScale}\n" +
               $"YScale : {this.YScale}\n." +
               $"Points: {ret}\n";
        }

    }
}
