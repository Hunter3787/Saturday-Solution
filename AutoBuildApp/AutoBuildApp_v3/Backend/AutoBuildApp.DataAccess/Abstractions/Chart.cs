using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess.Abstractions
{
    ///will be treating this as a base class
    /// for now.
    class Chart
    {
        /// <summary>
        /// XLabel represents the
        /// X Name Tick in the chart/ graph.
        /// </summary>
        public string XLabel { get; set; }
        /// <summary>
        /// Legend represents essentially 
        /// the Z value in the chart.
        /// </summary>
        public string Legend { get; set; }
        /// <summary>
        /// YValue represent the corresponding
        /// value to the X-Value.
        /// </summary>
        public int YValue { get; set; }


        public Chart()
        {
            XLabel = " ";
            Legend = " ";
            YValue = 0;
        }

        public Chart(string xLabel, int yValue, string legend)
        {
            this.XLabel = xLabel;
            this.YValue = yValue;
            this.Legend = legend;

        }

    }
}
