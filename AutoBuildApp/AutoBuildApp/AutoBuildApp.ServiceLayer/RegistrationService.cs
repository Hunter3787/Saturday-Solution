using System;
using AutoBuildApp.Models;
using AutoBuildApp.DataAccess;
using System.Configuration;

namespace AutoBuildApp.ServiceLayer
{
    public class RegistrationService
    {
        private UserAccount _user;
        private RegistrationDAO _registrationDOA;

        public RegistrationService(UserAccount user)
        {

            // establish a connection to DB
            _registrationDOA = new RegistrationDAO(GetConnectionStringByName("ZeeConnection"));
            _user = user;
        }
        static string GetConnectionStringByName(string name)
        {
            string retVal = null;

            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            // If found, return the connection string.
            if (settings != null)
                retVal = settings.ConnectionString;

            return retVal;
        }
        // create user 
        public String IsRegistrationValid(UserAccount user)
        {
            return _registrationDOA.CreateUserRecord(user);
        }
    }
}
