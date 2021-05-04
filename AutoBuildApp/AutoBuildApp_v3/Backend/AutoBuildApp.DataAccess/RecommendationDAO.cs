using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Enumerations;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System;

/**
* This Data Access Object will handle collection of 
* infromation from the database to be used inside the Recommender.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.DataAccess
{

    /// <summary>
    /// Class to access the database while
    /// using the Recommendation tool.
    /// </summary>
    public class RecommendationDAO
    {
        private readonly string _connectionString;

        public RecommendationDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Get a list of components under the budgeted allocation.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Dictionary<ProductType, List<IComponent>>
            GetComponentDictionary(List<IComponent> input)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Dictionary<ProductType, List<IComponent>> output = new Dictionary<ProductType, List<IComponent>>();

                var stored = "Search_ProductBudget";
                using ( var command = new SqlCommand())
                {
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = stored;
                    string productType = "productType";
                    string productPrice = "productPrice";


                    DataTable pair = new DataTable();
                    DataColumn column = new DataColumn();
                    column.ColumnName = productType;
                    column.DataType = typeof(string);
                    pair.Columns.Add(column);

                    column = new DataColumn();
                    column.ColumnName = productPrice;
                    column.DataType = typeof(double);
                    pair.Columns.Add(column);

                    DataRow row;
                    //foreach (var elements in input)
                    //{
                    //    row = pair.NewRow();
                    //    row[productType] = elements.ProductType;
                    //    row[productPrice] = elements.Budget;
                    //    pair.Rows.Add(row);
                    //}

                     row = pair.NewRow();
                        row[productType] = "cpu";
                        row[productPrice] = 200.00;
                        pair.Rows.Add(row);
                    
                    var param = new SqlParameter[1];
                    param[0] = command
                        .Parameters
                        .AddWithValue("@PRODUCTS", pair);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //int type = reader.GetOrdinal(productType);
                        //int price = reader.GetOrdinal(productPrice);

                        string URL = "VendorLinkURL";
                        int VendorLinkURL = reader.GetOrdinal(URL);
                        int pPrice = reader.GetOrdinal(productPrice);

                        if (reader.HasRows == false)
                            return null;

                        while (reader.Read())
                        {
                            //var key = (string)reader[type];
                            //var value = (float)reader[price];
                            var key = (string)reader[VendorLinkURL];
                            var value = (System.Decimal)reader[pPrice];

                            Console.WriteLine($" the existing components:\n" +
                                $"type: {key } , current value:{value }\n");
                        }
                    }
                    command.Transaction.Commit();
                }

                return output;
            }
        }
    }
}
