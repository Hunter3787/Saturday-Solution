using System;
using System.Collections.Generic;
using System.Text;
using AutoBuildApp.Models;
using AutoBuildApp.DataAccess;
using System.Configuration;

namespace AutoBuildApp.ServiceLayer
{
    public class LoginService
    {

        {
            // establish a connection to DB
<<<<<<< Updated upstream
            _registrationDAO = new RegistrationDAO("Server = localhost; Database = Registration_Pack; Trusted_Connection = True;");
            _user = user;
=======
>>>>>>> Stashed changes
        }

        // login
        public string LoginUser(UserAccount user)
        {
        }
    }
}
