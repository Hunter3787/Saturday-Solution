using System;

//this is a test
/*
 * this "Microsoft.Data.SqlClient" is a swap 
 * out for the existing "System.Data.SqlClient" namespace. since i started using the latter
 * https://www.connectionstrings.com/the-new-microsoft-data-sqlclient-explained/
 * https://devblogs.microsoft.com/dotnet/introducing-the-new-microsoftdatasqlclient/
 */
using AutoBuildApp.Models.Users;
using AutoBuildApp.Models.Catalog;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using AutoBuildApp.Managers;

namespace AutoBuildApp.DataAccess
{
    public class CatalogGateway
    {
        private String _connection;
        /*
         * "Represents a set of data commands and a database connection 
         * that are used to fill the DataSet and update a SQL Server database"
         * https://docs.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqldataadapter?view=sqlclient-dotnet-core-2.1
         * 
         * the adapter is essentially the "gateway" to the actual database side
         */
        private SqlDataAdapter adapter = new SqlDataAdapter();
        //Represents a Transact-SQL transaction to be made in a SQL Server database
        public CatalogGateway(String connectionString)
        {
            // instantiation of the connections string via a constructor to avoid any hardcoding
            _connection = connectionString;
        }

        public ISet<IResult> SearchForProduct(string product, string resultType) 
        { 
            return 
        }

        public bool SaveComponentToUser(string component, UserAccount user) 
        {
            return true;
        }

        public ProductDetails GetProductDetails(string component) 
        {
            // uses var connection and will automatically close once the using block has reached the end.
            using (var conn = new SqlConnection(_connection))
            {
                // Open the connection to the database.
                conn.Open();

                // Uses the var command and will only use the command within this block.
                using (var command = new SqlCommand())
                {
                    command.Transaction = conn.BeginTransaction(); 
                    command.Connection = conn; 
                    command.CommandTimeout = TimeSpan.FromSeconds(60).Seconds; 
                    command.CommandType = CommandType.Text;

                    command.CommandText =
                       "SELECT componentName, componentImage" +
                       "WHERE name = @componentName";
                }

                
            }
        }
    }
}
