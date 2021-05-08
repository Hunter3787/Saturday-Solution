using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.DataTransferObjects
{
    ///will be treating this as a base class
    /// for now.
    public class ChartData
    {
        /// <summary>
        /// XLabel represents the
        /// X Name Tick in the chart/ graph.
        /// </summary>
        public object XLabel { get; set; }

        /// <summary>
        /// XLabel represents the
        /// X Name Tick in the chart/ graph.
        /// </summary>
        public object YValue { get; set; }


        /// <summary>
        /// Legend represents essentially 
        /// the Z value in the chart.
        /// </summary>
        public object Legend { get; set; }
        

        public ChartData()
        {
            XLabel = (string)" ";
            Legend = (string)" ";
            YValue = (string)" ";
        }

        public ChartData(object xLabel, object yValue, object legend)
        {
            // here we check the data type and cast them into thier intended type
            try
            {

                if (Object.ReferenceEquals(xLabel.GetType(), typeof(System.String)))
                {
                    this.XLabel = (string)xLabel;
                }
                else
                {
                    this.XLabel = (int)xLabel;
                }

                if (Object.ReferenceEquals(yValue.GetType(), typeof(System.String)))
                {
                    this.YValue = (string)yValue;
                }
                else
                {
                    this.YValue = (int)yValue;
                }

                if (Object.ReferenceEquals(legend.GetType(), typeof(System.String)))
                {
                    this.Legend = (string)legend;
                }
                else
                {
                    this.Legend = (int)legend;
                }

            }
            catch (ArgumentNullException)
            {
                // handling the null paramteres case
                if (xLabel == null || yValue == null || legend == null)
                {

                    var expectedParamName = "NULL OBJECT PROVIDED";
                    throw new ArgumentNullException(expectedParamName);

                }
            }
        }

        public override string ToString()
        {

            return
                $"\nXLabel : {XLabel}\n" +
                $"YValue : {YValue}\n" +
                $"Legend : {Legend }\n";
        }

    }
}
