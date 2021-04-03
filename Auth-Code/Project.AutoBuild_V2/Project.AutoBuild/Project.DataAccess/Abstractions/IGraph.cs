using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DataAccess.Abstractions
{
    #region this is the I graph interface for Dashboard not yet implemented

    /// <summary>
    ///Chart interface for the Bar charts and line graphs
    /// </summary>
    public interface IGraph
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
    #endregion
}
