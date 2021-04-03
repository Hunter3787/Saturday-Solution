using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DataAccess.Entities
{
    /// <summary>
    /// this class is responsible for holding possible
    /// permutations of return types for dashboard.
    /// https://stackoverflow.com/questions/18620353/making-a-liststring-string-string-in-c-sharp-with-three-flexible-elements-in
    /// </summary>
    public class DashboardValues
    {
        public string xLabel { get; set; }
        public string legend { get; set; }
        public int yValue { get; set; }
        public DashboardValues()
        {
            xLabel = " ";
            legend = " ";
            yValue = 0;
        }
        // think of this as (x,y,z)
        public DashboardValues(string xLabel, int yValue, string legend)
        {
            this.xLabel = xLabel;
            this.yValue = yValue;
            this.legend = legend;

        }
    }
}
