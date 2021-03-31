/*using System;
using AutoBuildApp.Models;
using AutoBuildApp.DataAccess;
using System.Configuration;

namespace AutoBuildApp.ServiceLayer
{
    public class RegistrationService
    {
        private RegistrationDAO _registrationDOA;

        public RegistrationService(String CnnctString)
        {

            // establish a connection to DB

            _registrationDOA = new RegistrationDAO(CnnctString);
        }
       
        // create user 
        public String IsRegistrationValid(UserAccount user)
        {
            return _registrationDOA.CreateUserRecord(user);
        }
    }
}
*/