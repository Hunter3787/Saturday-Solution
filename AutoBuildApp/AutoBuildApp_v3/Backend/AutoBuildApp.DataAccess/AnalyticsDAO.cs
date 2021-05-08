
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.DataTransferObjects;
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
    public class AnalyticsDAO
    {
        private List<string> _allowedRoles; //specify rles

        public string ConnectionString { get; set; }

        #region CONSTANTS 
        public const string X_VALUE = "X_Value";
        public const string Y_VALUE = "Y_Value";
        public const string LEGEND  = "Legend";

        public const string Y_TITLE = "YTitle";
        public const string X_TITLE = "XTitle";
        public const string LEGEND_TITLE = "LegendTitle";



        #endregion

        // Creates the local instance for the logger

        //private LoggingProducerService _logger = LoggingProducerService.GetInstance;


        public AnalyticsDAO(string connection)
        {
            try
            {
                // set the claims needed for this method call
                ConnectionString = connection;

                _allowedRoles = new List<string>(){ RoleEnumType.SystemAdmin };
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

        public ResponseUAD GetGraphData(DBViews graphViewType)
        {
              ResponseUAD responseUAD = new ResponseUAD();

            int count = Enum.GetValues(typeof(DBViews)).Length;
            if ((int)graphViewType >= count || (int)graphViewType <= 0 )
            {
                responseUAD.ResponseString = "InValid Graph Specified";

                responseUAD.IsSuccessful = false;
                //_logger.LogInformation( AuthorizationResultType.NOT_AUTHORIZED.ToString());
                return responseUAD;
            }


            ChartData chartData;

            if (!AuthorizationCheck.IsAuthorized(_allowedRoles))
            {
                responseUAD.ResponseString = AuthorizationResultType.NotAuthorized.ToString();
                //_logger.LogInformation( AuthorizationResultType.NOT_AUTHORIZED.ToString());
                return responseUAD;
            }
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                // naming convention View_ name of VIEW
                string View_AccountsByAccountType = $"SELECT * FROM {graphViewType.ToString()};";
                using (SqlCommand command = new SqlCommand(View_AccountsByAccountType, conn))
                {
                    try
                    {
                        command.Transaction = conn.BeginTransaction();

                        #region SQL related

                        // https://learning.oreilly.com/library/view/adonet-in-a/0596003617/ch04s05.html
                        command.CommandTimeout = TimeSpan.FromSeconds(TimeoutLengths.TIMEOUT_LONG).Seconds;
                        // 1) Create a Command, and set its CommandType property to StoredProcedure.
                        command.CommandType = CommandType.Text;
                        // 2) Set the CommandText to the name of the stored procedure.
                        command.CommandText = View_AccountsByAccountType;

                        #endregion SQL related

                        using (var reader = command.ExecuteReader())
                        {
                            // checking if there are rows
                            if (!reader.HasRows) // use the bang!!!!!!! 
                            {
                                // if no rows then no data being retrieved 
                                responseUAD.ResponseString = "No Data At The Moment";
                                responseUAD.IsSuccessful = false;
                                return responseUAD;
                            }
                            else
                            {
                                //READ AND STORE All THE ORDINALS YOU NEED
                                int X_Value_ord = reader.GetOrdinal(X_VALUE); // no magic 
                                int Y_Value_ord = reader.GetOrdinal(Y_VALUE);
                                int Legend_ord = reader.GetOrdinal(LEGEND);


                                int XTitle_ord = reader.GetOrdinal(X_TITLE);
                                int YTitle_ord = reader.GetOrdinal(Y_TITLE);
                                int LegendTitle_ord = reader.GetOrdinal(LEGEND_TITLE);

                                bool flag = true;
                                while (reader.Read()) // reading the rows
                                {
                                    if (flag) // I only want these columns read once
                                    {
                                        responseUAD.XTitle = (string)reader[XTitle_ord];
                                        responseUAD.YTitle = (string)reader[YTitle_ord];
                                        responseUAD.LegendTitle = (string)reader[LegendTitle_ord];
                                        flag = false;
                                        //Console.WriteLine("{0} - {1} - {2}",
                                        //    _responseUAD.XTitle,
                                        //    _responseUAD.YTitle,
                                        //    _responseUAD.LegendTitle);

                                    }
                                    chartData =
                                        new ChartData(
                                            (object)reader[X_Value_ord],
                                            (object)reader[Y_Value_ord],
                                            (object)reader[Legend_ord]);
                                    // and adding those datas to the chartsdata list 
                                    responseUAD.GetChartDatas.Add(chartData);
                                }
                            }

                        }
                        // commit the changes to the DB. 
                        command.Transaction.Commit();
                    }
                    catch(SqlException e)
                    {
                        command.Transaction.Rollback();
                        if (!conn.State.Equals(ConnectionState.Open))
                        {
                            responseUAD.ConnectionState = false;
                        }
                        //Console.WriteLine("SqlException.GetType: {0}", e.GetType());
                        //Console.WriteLine("SqlException.Source: {0}", e.Source);
                        //Console.WriteLine("SqlException.ErrorCode: {0}", e.ErrorCode);
                        //Console.WriteLine("SqlException.Message: {0}", e.Message);

                    }
                }
            }
            // finally set the success into the respose and return the common response 
            responseUAD.ResponseString = "Successful Data retrieval";
            responseUAD.IsSuccessful = true;
            return responseUAD;
        }




    }
}
