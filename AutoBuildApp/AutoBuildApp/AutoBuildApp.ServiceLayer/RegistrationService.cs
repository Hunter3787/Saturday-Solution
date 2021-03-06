using System;
using AutoBuildApp.Models;
using AutoBuildApp.DataAccess;
using System.Configuration;

namespace AutoBuildApp.ServiceLayer
{
    public class RegistrationService
    {
        private RegistrationDAO _registrationDOA;

        {

            // establish a connection to DB
<<<<<<< Updated upstream
            _registrationDOA = new RegistrationDAO("Server = localhost; Database = Registration_Pack; Trusted_Connection = True;");
            _user = user;
=======
>>>>>>> Stashed changes
        }
       
        // create user 
        public String IsRegistrationValid(UserAccount user)
        {
            return _registrationDOA.CreateUserRecord(user);
        }
    }
}
