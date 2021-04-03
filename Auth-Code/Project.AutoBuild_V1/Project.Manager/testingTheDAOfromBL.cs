using Project.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Project.Manager
{
    public class testingTheDAOfromBL
    {

        DashboardDAO DashboardDAO;

        public testingTheDAOfromBL()
        {

            //utilization of the app config
            DashboardDAO = new DashboardDAO(GetConnectionStringByName("Connection"));

        }
        static string GetConnectionStringByName(string name)
        {
            string retVal = null;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            // If found, return the connection string.
            if (settings != null)
                retVal = settings.ConnectionString;
            Console.WriteLine("This is the connection string: " + retVal);

            return retVal;
        }
        public void getNumberOfUsers()
        {
            Console.WriteLine(DashboardDAO.GetNumberOfUsers());
        }

    }
}
