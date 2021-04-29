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
    public class ProductDAO : IProductDAO
    {
        // Connection String
        private readonly string _connectionString;

        // SQL Query string for products by type.
        private readonly string _productQueryByType
            = "SELECT * FROM Products WHERE ProductType = @productType;";
        // SQL Query string to get specification by model.
        private readonly string _specsQueryByModel
            = "SELECT * FROM Specs_Table WHERE model = @modelNumber;";

        private readonly string _allProductQuery = $"SELECT * FROM Products;";
        private readonly string _productTypeString = "@productType";
        private readonly string _modelNumberString = "@modelNumber";
        
        #region Constructor
        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="connectionString"></param>
        public ProductDAO(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        /// <summary>
        /// Get all products from the product table.
        /// </summary>
        /// <returns>List of ProductEntity objects for further manipulation.</returns>
        public List<ProductEntity> GetAllProductEntities()
        {
            List<ProductEntity> entitiesList = new List<ProductEntity>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.CommandTimeout = DAOGlobals.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;
                    command.CommandText = _allProductQuery;

                    using (SqlDataReader outerReader = command.ExecuteReader())
                    {
                        while (outerReader.Read())
                        {
                            // Create new dictionary to store the queried specs.
                            Dictionary<string, string> specsDictionary = new Dictionary<string, string>();

                            ProductEntity tempEntity = new ProductEntity()
                            {
                                Type = (string)outerReader[ProductTableCollumns.PRODUCT_COLUMN_TYPE],
                                Name = (string)outerReader[ProductTableCollumns.PRODUCT_COLUMN_NAME],
                                Manufacturer = (string)outerReader[ProductTableCollumns.PRODUCT_COLUMN_MANUFACTURER],
                                Model = (string)outerReader[ProductTableCollumns.PRODUCT_COLUMN_MODEL]
                            };



                            using (SqlDataReader innerReader = command.ExecuteReader())
                            {

                            }

                            // For each element in the specs table add
                            // the spec and details.


                            tempEntity.Specs = specsDictionary;

                            entitiesList.Add(tempEntity);
                            outerReader.NextResult();
                        }

                        
                    }
                }
            }

            return entitiesList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toFind"></param>
        /// <returns></returns>
        public List<ProductEntity> GetEntities(List<IComponent> toFind)
        {
            List<ProductEntity> entitiesList = new List<ProductEntity>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var command = new SqlCommand())
                {
                    command.Transaction = conn.BeginTransaction();
                    command.Connection = conn;
                    command.CommandTimeout = DAOGlobals.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;



                }
            }

            return entitiesList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ProductEntity> GetEntitiesByType(ProductType type)
        {
            List<ProductEntity> entitiesList = new List<ProductEntity>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Transaction = conn.BeginTransaction();
                    command.Connection = conn;
                    command.CommandTimeout = DAOGlobals.TIMEOUT_SHORT;
                    command.CommandType = CommandType.Text;


                    var parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter(_productTypeString, type);

                    
                    command.Transaction.Commit();
                }
            }

            return entitiesList;
        }




        /* Likely not what I want or need */
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
