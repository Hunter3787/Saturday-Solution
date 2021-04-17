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

        int XScale { get; set; }
        int YScale { get; set; }


        /// <summary>
        /// ToString() method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "this is base class";
        }

    }
}
