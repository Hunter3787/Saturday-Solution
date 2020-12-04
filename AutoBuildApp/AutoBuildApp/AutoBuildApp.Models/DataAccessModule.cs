using System;
using System.Collections.Generic;
using System.Text;

//https://devblogs.microsoft.com/dotnet/introducing-the-new-microsoftdatasqlclient/ 

using Microsoft.Data.SqlClient;



namespace AutoBuildApp.Models
{
    /*
     * okay so im at a pause here I wanna see if I need to do a 
     * Repository or gateway
     */
    public class DataAccessModule
    {

        public List<UserAccount> RetrieveAccounts(string byLastName)
        {
            // this is like what vongster did
            // this llowed compilation without this thing 
            // ever being used 


            // this is essentially that new Microsoft.Data.SqlClient.Sqlconnection
            using (SqlConnection conn = new SqlConnection(ConnectionStringHelperClass.ConnectNow("AutoBuildDB")))
            {

                
                throw new NotImplementedException();

            }


            //lets see how to talk to SQL Server...


        }







    }
}
