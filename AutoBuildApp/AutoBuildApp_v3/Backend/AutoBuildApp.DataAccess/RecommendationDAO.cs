using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Enumerations;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System;
/**
* This Data Access Object will handle collection and transformation of 
* infromation coming from the database to be usable inside the Builder.
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
                Dictionary<ProductType, List<IComponent>> output = new();

                using( var command = new SqlCommand())
                {
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;

                    // send query

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                        }
                    }
                    command.ExecuteNonQuery();
                    command.Transaction.Commit();

                }



                Dictionary<ProductType, double> querystuff =
                    new Dictionary<ProductType, double>();

                foreach (var item in input)
                    querystuff.Add(item.ProductType, item.Budget);


                return output;
            }
        }
    }
}
