using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.WebCrawler;
using AutoBuildApp.Security.Enumerations;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class ProductDetailsDAO
    {
        private List<string> _allowedRoles;
        private string _connectionString;

        public ProductDetailsDAO(string connectionString)
        {
            _connectionString = connectionString;
            _allowedRoles = new List<string>()
            {
                RoleEnumType.SystemAdmin,
                RoleEnumType.VendorRole
            };
        }

        public SystemCodeWithObject<ProductDetailsDTO> GetProductByModelNumber(string modelNumber)
        {
            // Initialize the system code response object with a Product as the generic object
            SystemCodeWithObject<ProductDetailsDTO> response = new SystemCodeWithObject<ProductDetailsDTO>();

            // Initialize the Product
            ProductDetailsDTO product = new ProductDetailsDTO();

            // Initialize the vendor information dictionary
            Dictionary<string, ProductVendorDetailsDTO> vendorInformation = new Dictionary<string, ProductVendorDetailsDTO>();
            product.VendorInformation = vendorInformation;

            // Initialize the product specs dictionary
            Dictionary<string, string> productSpecs = new Dictionary<string, string>();
            product.Specs = productSpecs;

            // Total ratings for a product
            int totalRating = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();

                        string sql =
                            "select * from products where modelNumber = @MODELNUMBER;" +
                            "select productSpecs, productSpecsValue from products p inner join Products_Specs ps on p.productID = ps.productID where modelNumber = @MODELNUMBER;" +
                            "select vendorName from vendorclub v inner join Vendor_Product_Junction vpj on v.vendorID = vpj.vendorID where productid = (select productid from products where modelNUmber = @MODELNUMBER)" +
                            "select * from Vendor_Product_Reviews_Junction vprj inner join vendorclub v on vprj.vendorID = v.vendorID where  productid = (select productID from products where modelNumber = @MODELNUMBER);" +
                            "select * from Vendor_Product_Junction vpj inner join vendorclub v on vpj.vendorid = v.vendorid where productid = (select productId from products where modelnumber = @MODELNUMBER)";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = modelNumber;

                        using (SqlDataReader reader = adapter.InsertCommand.ExecuteReader())
                        {
                            // Get basic product information
                            while (reader.Read())
                            {
                                product.ProductName = (string)reader["productName"];
                                product.ImageUrl = (string)reader["imageUrl"];
                                product.ProductType = (string)reader["productType"];
                                product.ModelNumber = modelNumber;
                            }

                            // Now get the product's specs
                            reader.NextResult();

                            while (reader.Read())
                            {
                                // Add an entry into the product specs dictionary
                                productSpecs.Add((string)reader["productSpecs"], (string)reader["productSpecsValue"]);
                            }

                            // Now get all vendors that have the product
                            reader.NextResult();

                            while(reader.Read())
                            {
                                // Add an entry into dictionary with vendor name and an initialized productVendorDetailsDTO
                                vendorInformation.Add((string)reader["vendorName"], new ProductVendorDetailsDTO());
                            }

                            // Now get the product reviews per vendor
                            reader.NextResult();

                            while (reader.Read())
                            {
                                string currentVendor = (string)reader["vendorName"];

                                // Set currentVendorsReviews equal to the list of reviews for the vendor
                                if (vendorInformation[currentVendor].Reviews == null)
                                {
                                    // Initialize list of reviews if there currently doesn't exist a list of reviews
                                    vendorInformation[currentVendor].Reviews = new List<Review>();
                                }

                                // Instantiate review and populate it
                                Review review = new Review();
                                review.ReviewerName = (string)reader["reviewerName"];
                                review.StarRating = (string)reader["reviewStarRating"];
                                review.Content = (string)reader["reviewContent"];
                                review.Date = (string)reader["reviewDate"];

                                vendorInformation[currentVendor].Reviews.Add(review);

                                // Add to total rating
                                totalRating += Int32.Parse(review.StarRating);

                                // Increment total reviews
                                product.TotalReviews++;
                            }

                            // Now get the vendor product information
                            reader.NextResult();

                            while (reader.Read())
                            {
                                string currentVendor = (string)reader["vendorName"];
                                ProductVendorDetailsDTO productVendorDetailsDTO = vendorInformation[currentVendor];

                                if(productVendorDetailsDTO == null)
                                {
                                    productVendorDetailsDTO = new ProductVendorDetailsDTO();
                                }

                                // Set individual product vendor values  
                                productVendorDetailsDTO.Availability = (bool)reader["productStatus"];
                                productVendorDetailsDTO.ListingName = (string)reader["listingName"];
                                productVendorDetailsDTO.Url = (string)reader["vendorLinkUrl"];
                                productVendorDetailsDTO.Price = Decimal.ToDouble(reader["productPrice"] == DBNull.Value ? 0 : (decimal)reader["productPrice"]);
                            }

                            // Sets the average rating
                            product.AverageRating = (Convert.ToDouble(totalRating) / product.TotalReviews);
                        }

                        transaction.Commit();

                        response.Code = AutoBuildSystemCodes.Success;
                        response.GenericObject = product;
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
