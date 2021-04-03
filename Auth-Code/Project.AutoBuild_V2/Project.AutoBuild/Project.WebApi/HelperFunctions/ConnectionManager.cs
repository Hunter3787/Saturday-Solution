using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Project.WebApi.HelperFunctions
{
    /// <summary>
    /// referenced a Youtibe tutorial on the implementation of 
    /// reading connection strings for the appsetting.json file
    /// see 
    /// </summary>
    public sealed class ConnectionManager
    {

        public static readonly ConnectionManager connectionManager = new ConnectionManager();

        public string ConnectionString { get; set; }
        public string GetConnectionStringByName(string name)
        {
            if (name == null)
            {
                // if the connection string is null then 
                // assign it empty string
                name = " ";
            }
            // makes call to the appsetting json.
            var configuration = GetConfiguration();
            string connectionString = configuration.GetSection("ConnectionStrings").GetSection(name).Value;
            if (connectionString != null)
            {
                ConnectionString = connectionString;
                return connectionString;
            }
            else{connectionString = " "; }
            return connectionString;
        } 

        /// <summary>
        /// essentially When using the App.config and Web.config there 
        /// was a conflict in reading the connection string name 
        /// so this addressed the conflict and adding the json file to 
        /// the base path.
        /// </summary>
        /// <returns></returns>
        private  IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                . SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }


    }
}
