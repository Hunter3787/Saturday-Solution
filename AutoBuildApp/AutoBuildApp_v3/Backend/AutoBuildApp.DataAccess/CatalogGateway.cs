
using System;

//this is a test
/*
 * this "Microsoft.Data.SqlClient" is a swap 
 * out for the existing "System.Data.SqlClient" namespace. since i started using the latter
 * https://www.connectionstrings.com/the-new-microsoft-data-sqlclient-explained/
 * https://devblogs.microsoft.com/dotnet/introducing-the-new-microsoftdatasqlclient/
 */
using AutoBuildApp.Models.Users;
using System.Data.SqlClient;
using AutoBuildApp.Models;

namespace AutoBuildApp.DataAccess
{
    public class CatalogGateway
    {
        private String connection;
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
            this.connection = connectionString;
        }

        public bool SaveComponentToUser(string component, UserAccount user) 
        { 
            
        }

        public ComponentDetails GetComponentDetails(string component) 
        { 
        
        }
    }
}
