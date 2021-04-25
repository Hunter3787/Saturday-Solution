using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess.Abstractions
{
    ///will be treating this as a base class
    /// for now.
    public class ChartData
    {
        /// <summary>
        /// XLabel represents the
        /// X Name Tick in the chart/ graph.
        /// </summary>
        public string XLabelString { get; set; }

        /// <summary>
        /// XLabel represents the
        /// X Name Tick in the chart/ graph.
        /// </summary>
        public float XLabelInt { get; set; }

        /// <summary>
        /// Legend represents essentially 
        /// the Z value in the chart.
        /// </summary>
        public object Legend { get; set; }
        /// <summary>
        /// YValue represent the corresponding
        /// value to the X-Value.
        /// </summary>
        public string YValueString { get; set; }

        /// <summary>
        /// YValue represent the corresponding
        /// value to the X-Value.
        /// </summary>
        public int YValueInt { get; set; }


        public ChartData()
        {
            XLabelInt = 0;
            XLabelString = " ";
            Legend = " ";
            YValueInt = 0;
            YValueString = " ";
        }

        public ChartData(string xLabel, int yValue, string legend)
        {
            this.XLabelString = xLabel;
            this.YValueInt = yValue;
            this.Legend = legend;

        }
        public override string ToString()
        {

            return $"\tXLabelInt : { XLabelInt}\n\tXLabelString : {XLabelString}\n" +
                $"\tYValueInt : {YValueInt}\n\tYValueString : {YValueString} \n" +
                $"\tLegend : {  Legend } ";
        }

    }
}
