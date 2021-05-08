using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.WebCrawler;
using AutoBuildApp.Security.Enumerations;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading;

namespace AutoBuildApp.DataAccess
{
    /// <summary>
    /// This class is responsible for communicating with the database
    /// </summary>
    public class ProductDetailsDAO
    {
        private List<string> _allowedRoles;
        private string _connectionString;

        /// <summary>
        /// Establishes the connection with the connection string that is passed through and sets the allowed roles
        /// </summary>
        /// <param name="connectionString">sql database string to be able to connect to database.</param>
        public ProductDetailsDAO(string connectionString)
        {
            _connectionString = connectionString;
            _allowedRoles = new List<string>()
            {
                RoleEnumType.SystemAdmin,
                RoleEnumType.VendorRole
            };
        }

        /// <summary>
        /// This method will get the product by a specific model number
        /// </summary>
        /// <param name="modelNumber">takes in the model number of the product retrieved</param>
        /// <returns>returns a system code with a ProductDetailDTO.</returns>
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
                            // If the reader returns 0 rows, then the model number doesn't exist in our database
                            if(!reader.HasRows)
                            {
                                response.Code = AutoBuildSystemCodes.NullValue;
                                response.GenericObject = null;

                                return response;
                            }
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
                                var vendorName = (string)reader["vendorName"];

                                // Add an entry into dictionary with vendor name and an initialized productVendorDetailsDTO
                                vendorInformation.Add(vendorName, new ProductVendorDetailsDTO());

                                // Initialize list of reviews
                                vendorInformation[vendorName].Reviews = new List<Review>();
                            }

                            // Now get the product reviews per vendor
                            reader.NextResult();

                            while (reader.Read())
                            {
                                string vendorName = (string)reader["vendorName"];


                                // Instantiate review and populate it
                                Review review = new Review();
                                review.ReviewerName = (string)reader["reviewerName"];
                                review.StarRating = (string)reader["reviewStarRating"];
                                review.Content = (string)reader["reviewContent"];
                                review.Date = (string)reader["reviewDate"];

                                // Add the review to the vendor's list of reviews
                                vendorInformation[vendorName].Reviews.Add(review);

                                // Add to total rating
                                totalRating += Int32.Parse(review.StarRating);

                                // Increment total reviews
                                product.TotalReviews++;
                            }

                            // Now get the vendor product information
                            reader.NextResult();

                            while (reader.Read())
                            {
                                string vendorName = (string)reader["vendorName"];
                                ProductVendorDetailsDTO productVendorDetailsDTO = vendorInformation[vendorName];

                                // Set individual product vendor values  
                                productVendorDetailsDTO.Availability = (bool)reader["productStatus"];
                                productVendorDetailsDTO.ListingName = (string)reader["listingName"];
                                productVendorDetailsDTO.Url = (string)reader["vendorLinkUrl"];
                                productVendorDetailsDTO.Price = Decimal.ToDouble(reader["productPrice"] == DBNull.Value ? 0 : (decimal)reader["productPrice"]);
                            }

                            // Sets the average rating if the product has at least 1 review
                            if (product.TotalReviews > 0)
                            {
                                product.AverageRating = (Convert.ToDouble(totalRating) / product.TotalReviews);
                            }
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

        /// <summary>
        /// This method will add an email to the email list for a particular product
        /// </summary>
        /// <param name="modelNumber">takes in the model number of the product to be put on the email list with an email</param>
        /// <returns>returns a system code </returns>
        public AutoBuildSystemCodes AddEmailToEmailListForProduct(string modelNumber)
        {
            // Get the current principal on the thread
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string username = _threadPrinciple.Identity.Name;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();

                        string sql = "insert into emaillist (email, productID) values" +
                            "((select email from useraccounts where userid = (select userid from mappinghash where userhashid = (select userhashid from usercredentials where username = @username))), " +
                            "(select productid from products where modelnumber = @modelnumber));";
                            //"(select username from usercredentials where username = @USERNAME)), (select productID from products where modelNumber = @MODELNUMBER))";

                        adapter.InsertCommand = new SqlCommand(sql, connection, transaction);
                        adapter.InsertCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;
                        adapter.InsertCommand.Parameters.Add("@MODELNUMBER", SqlDbType.VarChar).Value = modelNumber;


                        adapter.InsertCommand.ExecuteNonQuery();
                        transaction.Commit();

                        return AutoBuildSystemCodes.Success;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();

                        // Passes the exception number to a handler which returns an AutoBuildSystemCode
                        return SqlExceptionHandler.GetCode(ex.Number);
                    }
                }
            }
        }
    }
}
