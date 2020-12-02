using System;
using System.Collections.Generic;
using System.Configuration; // for ConfigurationManager
using System.Text;

namespace AutoBuildApp.Models
{
    /*
     * i am trying to figure out a way to make 
     * connecting to a database easier 
     * where is can be used on any form 
     * of "Client"
     */
    public class ConnectionStringHelperClass
    {

        public static string ConnectNow(string name)
        {
            /*
             * what in the world is configuration manager!!
             * i love thins link it explanes its use towards 
             * connection strings
             * https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager.connectionstrings?view=dotnet-plat-ext-5.0
             * 
             * 
             */
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;

        }
         




    }
}
