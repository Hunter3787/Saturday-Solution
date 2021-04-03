using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DataAccess.Abstractions
{

    /// <summary>
    ///Chart interface  
    ///
    /// </summary>
    public interface IGraph
    {
        String ChartTitle { get; set; }
        String YAxisTitle { get; set; }

        String XAxisTitle { get; set; }

        int XScale { get; set; }
        int YScale { get; set; }

        /// <summary>
        /// ToString() method
        /// </summary>
        /// <returns></returns>
        string ToString();




    }

    class Line : IGraph
    {
        public string ChartTitle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string YAxisTitle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string XAxisTitle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int XScale { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int YScale { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    //class Bar : IGraph
    //{

    //}
}
