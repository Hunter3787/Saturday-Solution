using AutoBuildApp.Logging;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.VendorLinking;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class ComponentCatalogDAO
    {
        private string _connectionString;

        // Creates the local instance for the logger
        private readonly LoggingProducerService _logger;

        public ComponentCatalogDAO(string connectionString)
        {
            _connectionString = connectionString;
            _logger = LoggingProducerService.GetInstance;
        }

        public virtual SystemCodeWithObject<List<CatalogProductDTO>> GetVendorProductsByFilter(ProductByFilterDTO product)
        {
            // Initialize the system code response object
            SystemCodeWithObject<List<CatalogProductDTO>> response = new SystemCodeWithObject<List<CatalogProductDTO>>();

            // Product null check
            if (product == null)
            {
                response.Code = AutoBuildSystemCodes.NullValue;
                response.GenericObject = null;

                return response;
            }

            List<CatalogProductDTO> allProducts = new List<CatalogProductDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();

                        string sp_GetAllProductsByFilter = "GetAllProductsByFilter";
                        adapter.InsertCommand = new SqlCommand(sp_GetAllProductsByFilter, connection, transaction);
                        adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                        adapter.InsertCommand.CommandText = sp_GetAllProductsByFilter;

                        // Creates a dynamic data table to be passed into the stored procedure
                        DataTable pair = new DataTable();

                        // Create columns in the table
                        DataColumn column = new DataColumn();
                        column.ColumnName = "productType";
                        column.DataType = typeof(string);
                        pair.Columns.Add(column);

                        column = new DataColumn();

                        column.ColumnName = "FilterOn";
                        column.DataType = typeof(bool);
                        pair.Columns.Add(column);

                        // Create all the rows
                        DataRow row;
                        foreach (var filter in product.FilteredListOfProducts)
                        {
                            row = pair.NewRow();
                            row["productType"] = filter.Key;
                            row["FilterOn"] = filter.Value;
                            pair.Rows.Add(row);
                        }

                        // Add the necessary parameters for the stored procedure
                        var param = new SqlParameter[4];
                        param[0] = adapter.InsertCommand.Parameters.AddWithValue("@Filterlist", pair);
                        param[1] = adapter.InsertCommand.Parameters.AddWithValue("@ORDER", product.PriceOrder);
                        param[2] = adapter.InsertCommand.Parameters.AddWithValue("@MINVALUE", product.MinimumPrice);
                        param[3] = adapter.InsertCommand.Parameters.AddWithValue("@MAXVALUE", product.MaximumPrice);

                        // Execute the stored procedure and read each row and create an AddProductDTO
                        using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CatalogProductDTO productInfo = new CatalogProductDTO();

                                productInfo.Name = (string)reader["productName"];
                                productInfo.ImageUrl = (string)reader["imageURL"];
                                //productInfo.Availability = (bool)reader["productStatus"];
                                //productInfo.Company = (string)reader["vendorName"];
                                //productInfo.Url = (string)reader["VendorLinkURL"];
                                productInfo.ModelNumber = (string)reader["modelNumber"];
                                productInfo.ProductType = (string)reader["ProductType"];
                                productInfo.AverageRating = Decimal.ToDouble((decimal)reader["average"]);
                                productInfo.TotalReviews = (int)reader["totalReviews"];
                                productInfo.Price = Decimal.ToDouble((decimal)reader["minPrice"]);

                                allProducts.Add(productInfo);
                            }
                        }

                        transaction.Commit();

                        response.Code = AutoBuildSystemCodes.Success;
                        response.GenericObject = allProducts;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        // Passes the exception number to a handler which returns an AutoBuildSystemCode
                        response.Code = SqlExceptionHandler.GetCode(ex.Number);
                        response.GenericObject = null;

                    }
                }
            }

            return response;
        }
    }
}
