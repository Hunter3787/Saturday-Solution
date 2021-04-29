using AutoBuildApp.DataAccess.Abstractions;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
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

        private ClaimsFactory _claimsFactory = new ConcreteClaimsFactory();


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

        /// 
        /*
        public ResponseUAD GetAnalyticData()
        {

        ChartData chartData;
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

                    #endregion SQL related

                    using (var _reader = command.ExecuteReader())
                    {


                        Console.WriteLine($"Has reader rows?{_reader.HasRows}");
                        if (!_reader.HasRows) // use the bang!!!!!!! 
                        {
                            _responseUAD.ResponseString = "No Data At The Moment";
                            _responseUAD.SuccessBool = false;
                        }
                        else if (_reader.HasRows) // just else is enough 
                        {
                            _responseUAD.ResponseString = "There Exists Data!";
                            _responseUAD.SuccessBool = true;
                        }

                        //READ AND STORE ALL THE ORDINALS YOU NEED
                        int X_Value = _reader.GetOrdinal("X_Value");
                        int Y_Value = _reader.GetOrdinal("Y_Value");
                        int Legend = _reader.GetOrdinal("Legend");

                        while (_reader.Read())
                        {
                            // each time generate 
                            chartData = new ChartData();

                            chartData.XLabelString = (string)_reader[X_Value];
                            chartData.Legend = (string)_reader[Legend];
                            chartData.YValueInt = (int)_reader[Y_Value];

                        }

                        _reader.Close();
                    }

                    command.Dispose();
                    conn.Close();
                }
            }
            _responseUAD.SuccessBool = true;
            _responseUAD.ConnectionState = true;
            return _responseUAD;
        }

        */
        ///

        /// <summary>
        /// method to check permissions needed per authorization service.
        /// </summary>
        /// <returns></returns>
        public bool IsAuthorized()
        {

            IClaims _admin = _claimsFactory.GetClaims(RoleEnumType.SENIOR_ADMIN);

            // FIRST LINE OF DEFENCE 
            if (!AuthorizationService.checkPermissions(_admin.Claims()))
            {
                return false;
            }
            return true;

        }


        /// <summary>
        /// this methos is an attempt at getting multiple result sets for the graphs so I 
        /// dont have to make trip for each of the graphs it will all be available. 
        /// </summary>
        /// <returns></returns>
        public ResponseUAD GetAllAnalytics()
        {

            _responseUAD = new ResponseUAD();
            ChartData chartData;

            if (!IsAuthorized())
            {
                _responseUAD.IsAuthorized = false;
                return _responseUAD;
            }

            _responseUAD.IsAuthorized = true;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                Console.WriteLine($"\tRetreiving Graph Data \n");
                conn.Open();
                // naming convention SP_ name of procudrure
                string SP_GetAllAnalytics = "GetAllAnalytics";
                using (SqlCommand command = new SqlCommand(SP_GetAllAnalytics, conn))
                {
                    command.Transaction = conn.BeginTransaction();

                    #region SQL related

                    // https://learning.oreilly.com/library/view/adonet-in-a/0596003617/ch04s05.html
                    command.CommandTimeout = TimeSpan.FromSeconds(DAOGlobals.TIMEOUT_LONG).Seconds;
                    // 1) Create a Command, and set its CommandType property to StoredProcedure.
                    command.CommandType = CommandType.StoredProcedure;
                    // 2) Set the CommandText to the name of the stored procedure.
                    command.CommandText = SP_GetAllAnalytics;

                    #endregion SQL related


                    string X_Value = "X_Value";
                    string Y_Value = "Y_Value";
                    string Legend = "Legend";

                    using (var _reader = command.ExecuteReader())
                    {
                        if (!_reader.HasRows) // use the bang!!!!!!! 
                            {
                                _responseUAD.ResponseString = "No Data At The Moment";
                                _responseUAD.SuccessBool = false;
                                _reader.Close();
                            }
                        else
                            { 
                            //READ AND STORE ALL THE ORDINALS YOU NEED
                            int X_Value_ord = _reader.GetOrdinal(X_Value); // no magic 
                            int Y_Value_ord = _reader.GetOrdinal(Y_Value);
                            int Legend_ord = _reader.GetOrdinal(Legend);

                            /// BAR 1: 
                            /// Bar graph 1: The number of accounts (Y Axis) held
                            /// amongst account types(Label: Vendor, Admin,
                            /// Basic, Devoloper) * no legends used
                            /// 

                            while (_reader.Read())
                            {
                                chartData = 
                                    new ChartData(
                                        (string)_reader[X_Value_ord],
                                        (int)_reader[Y_Value_ord],
                                        (string)_reader[Legend_ord]
                                        );
                                // going to take the data! 
                                Console.WriteLine($" {chartData.ToString()}");
                                _responseUAD.GetNumAccountsPerRole.Add(chartData);
                            }

                            /// Summary:
                            ///     Advances the data reader to the next result, when reading the results of batch
                            ///     Transact-SQL statements.
                            ///
                            /// Returns:
                            ///     true if there are more result sets; otherwise false.
                            _reader.NextResult();


                            /// BAR 2: 
                            /// ar 2: Percentage usage of Autobuild components 
                            /// Y - Axis: usage can be time, or page visits maybe, X - Axis:
                            /// Label is the Autobuild component,*No legends used

                            while (_reader.Read())
                            {
                                // each time generate 
                                chartData = new ChartData(
                                    (int)_reader[X_Value],
                                    (int)_reader[Y_Value],
                                    (string)_reader[Legend]
                                    );
                                // going to take the data! 
                                _responseUAD.GetUsePerComponent.Add(chartData);
                            }

                            _reader.NextResult();

                            /// LINE CHART 1:
                            /// Line chart 1: Number of registrations (Y Axis) that
                            /// took place every month(x -axis) and the label show
                            /// the type of user
                            /// 

                            while (_reader.Read())
                            {
                                // each time generate 
                                chartData = new ChartData(
                                    (int)_reader[X_Value],
                                    (int)_reader[Y_Value],
                                    (string)_reader[Legend]
                                    );
                                // going to take the data! 
                                _responseUAD.GetRegPerMonthByUserType.Add(chartData);
                            }


                            _reader.NextResult();

                            /// BAR 3:
                            /// ar Graph 3: Average Session duration(yaxis) 
                            /// of user BY ROLE(shown as labels) *no legends used
                            /// 

                            while (_reader.Read())
                            {
                                // each time generate 
                                chartData = new ChartData(
                                    (string)_reader[X_Value],
                                    (int)_reader[Y_Value],
                                    (string)_reader[Legend]);
                                // going to take the data! 
                                _responseUAD.GetAvgSessDurPerRole.Add(chartData);
                            }

                            _reader.NextResult();

                            /// LINE GRAPH 2:
                            /// Most frequently viewed Autobuild view per
                            /// month.Number of views as Y - axis , Time on X - axis, and legends are
                            /// the Autobuild views

                            while (_reader.Read())
                            {
                                // each time generate 
                                chartData = new ChartData(
                                    (int)_reader[X_Value],
                                    (int)_reader[Y_Value],
                                    (int)_reader[Legend]);
                                // going to take the data! 
                                _responseUAD.GetPageViewPerMonth.Add(chartData);
                            }
                        }

                        _reader.Close();
                    }
                    // commit the changes to the DB. 
                    command.Transaction.Commit();
                    command.Dispose();
                    conn.Close();
                }
            }
            _responseUAD.SuccessBool = true;
            _responseUAD.ConnectionState = true;
            return _responseUAD;
        }








    }
}
