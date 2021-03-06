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
        private UserAccount _user;
        private RegistrationDAO _registrationDAO;

        public LoginService(UserAccount user)
        {
            // establish a connection to DB
            _registrationDAO = new RegistrationDAO("Server = localhost; Database = Registration_Pack; Trusted_Connection = True;");
            _user = user;
        }

        // login
        public string LoginUser(UserAccount user)
        {
            return _registrationDAO.Match(user.UserEmail, user.passHash);
        }

    }
}
