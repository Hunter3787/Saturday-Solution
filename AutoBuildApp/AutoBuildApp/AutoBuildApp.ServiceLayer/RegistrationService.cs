using System;
using System.Collections.Generic;
using System.Text;
using AutoBuildApp.Models;
using AutoBuildApp.DataAccess;

namespace AutoBuildApp.ServiceLayer
{
    public class RegistrationService
    {
        private UserAccount _user;
        private RegistrationDAO _registrationDOA;

        public RegistrationService(UserAccount user)
        {

            // establish a connection to DB
            _registrationDOA = new RegistrationDAO("Server=localhost; Database=Registration_Pack;Trusted_Connection=True;");
            _user = user;
        }

        // create user 
        public String IsRegistrationValid(UserAccount user)
        {
            return _registrationDOA.CreateUserRecord(user);
        }
    }
}
