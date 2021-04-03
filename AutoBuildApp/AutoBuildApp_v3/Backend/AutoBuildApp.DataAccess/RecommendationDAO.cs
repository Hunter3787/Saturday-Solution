using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Enumerations;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
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
        public Dictionary<ProductType, List<IComponent>> GetBudgetedList(List<IComponent> input)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using( var command = new SqlCommand())
                {

                    using (SqlDataReader reader = command.ExecuteReader()){
                        while (reader.Read())
                        {

                        }
                    }
                    command.ExecuteNonQuery();

                    command.Transaction.Commit();

                    return null;
                }
            }


                //Dictionary<ProductType, double> querystuff =
                //    new Dictionary<ProductType, double>();

            //foreach (var item in input)
            //    querystuff.Add(item.ProductType, item.Budget);


        }
    }
}
