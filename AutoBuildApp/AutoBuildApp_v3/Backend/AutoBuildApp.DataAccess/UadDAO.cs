using AutoBuildApp.DataAccess.Abstractions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class UadDAO
    {
        private ResponseUAD _responseUAD;
        private ChartData _chartData;
        public string ConnectionString { get; set; }

        public UadDAO(string connection)
        {
            try
            {
                // set the claims needed for this method call
                ConnectionString = connection;
            }
            catch (ArgumentNullException)
            {
                if (connection == null)
                {
                    var expectedParamName = "NULL OBJECT PROVIDED";
                    throw new ArgumentNullException(expectedParamName);
                }
            }
        }

        public ResponseUAD GetAnalyticData()
        {
            _responseUAD = new ResponseUAD();
            


            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                Console.WriteLine($"\tRetreiving Graph Data \n");
                conn.Open();
                // naming convention SP_ name of procudrure
                string SP_RereiveGraphData = "RereiveGraphData";
                using (SqlCommand command = new SqlCommand(SP_RereiveGraphData, conn))
                {
                    command.Transaction = conn.BeginTransaction();
                    #region SQL related

                    // https://learning.oreilly.com/library/view/adonet-in-a/0596003617/ch04s05.html
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    // 1) Create a Command, and set its CommandType property to StoredProcedure.
                    command.CommandType = CommandType.StoredProcedure;
                    // 2) Set the CommandText to the name of the stored procedure.
                    command.CommandText = SP_RereiveGraphData;
                    
                    # endregion SQL related

                    var _reader = command.ExecuteReader();

                    Console.WriteLine($"Has reader rows?{_reader.HasRows}");
                    if (!_reader.HasRows) // use the bang!!!!!!! 
                    {
                        _responseUAD.ResponseString = "No Data At The Moment";
                    }
                    else if (_reader.HasRows) // just else is enough 
                    {
                        _responseUAD.ResponseString = "There Exists Data!";
                    }

                    //READ AND STORE ALL THE ORDINALS YOU NEED
                    int X_Value = _reader.GetOrdinal("X_Value");
                    int Y_Value = _reader.GetOrdinal("Y_Value");
                    int Legend  = _reader.GetOrdinal("Legend");

                    while( _reader.Read())
                    {
                        // each time generate 
                        _chartData = new ChartData();

                        _chartData.XLabel = (string)_reader[X_Value];
                        _chartData.Legend = (string)_reader[Legend];
                        _chartData.YValue = (int)_reader[Y_Value];

                    }

                }
            }
            _responseUAD.SuccessBool = true;
            _responseUAD.connectionState = true;
            return _responseUAD;
        }
















    }
}
