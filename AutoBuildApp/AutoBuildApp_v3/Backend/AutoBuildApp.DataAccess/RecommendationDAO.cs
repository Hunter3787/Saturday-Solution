using AutoBuildApp.DataAccess.Abstractions;
using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.DataAccess.Entities;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
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
    public class RecommendationDAO : IProductDAO
    {
        // Connection String
        private readonly string _connectionString;

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="connectionString"></param>
        public RecommendationDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ProductEntity> GetAllProductEntities()
        {
            throw new NotImplementedException();
        }

        public List<ProductEntity> GetEntities(List<System.ComponentModel.IComponent> toFind)
        {
            throw new NotImplementedException();
        }

        public List<ProductEntity> GetEntitiesByType(ProductType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a list of components under the budgeted allocation.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Dictionary<ProductType, List<IComponent>> GetProductsByList(
            List<IComponent> input)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Dictionary<ProductType, List<IComponent>> output
                    = new Dictionary<ProductType, List<IComponent>>();

                var stored = "Search_ProductBudget";
                using ( var command = new SqlCommand())
                {
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = stored;

                    DataTable pair = new DataTable();
                    DataColumn column = new DataColumn();
                    column.ColumnName = "productType";
                    column.DataType = typeof(string);
                    pair.Columns.Add(column);
                    column.ColumnName = "productPrice";
                    column.DataType = typeof(double);
                    pair.Columns.Add(column);

                    DataRow row;
                    foreach (var elements in input)
                    {
                        row = pair.NewRow();
                        row["productType"] = elements.ProductType;
                        row["productPrice"] = elements.Budget;
                        pair.Rows.Add(row);
                    }

                    SqlParameter param = command
                        .Parameters
                        .AddWithValue("@TYPEBUDGET", pair);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows == false)
                            return null;

                        while (reader.Read())
                        {
                            var key = reader.GetString(0);
                            var value = reader.GetFloat(1);
                        }
                    }
                    command.Transaction.Commit();
                }

                return output;
            }
        }
    }
}
