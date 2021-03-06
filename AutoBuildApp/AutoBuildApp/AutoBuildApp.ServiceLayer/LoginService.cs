﻿using System;
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
            _registrationDAO = new RegistrationDAO(GetConnectionStringByName("ZeeConnection"));
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

        // login
        public string LoginUser(UserAccount user)
        {
            return _registrationDAO.Match(user.UserEmail, user.passHash);
        }

    }
}
