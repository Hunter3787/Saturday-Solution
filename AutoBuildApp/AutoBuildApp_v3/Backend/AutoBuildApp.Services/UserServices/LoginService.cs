/*using System;
using System.Collections.Generic;
using System.Text;
using AutoBuildApp.Models;
using AutoBuildApp.DataAccess;
using System.Configuration;

namespace AutoBuildApp.ServiceLayer
{
    public class LoginService
    {
        private LoginDAO _LoginDAO;

        public LoginService(String CnnctString)
        {
            // establish a connection to DB

            _LoginDAO = new LoginDAO(CnnctString);
        }

        // login
        public string LoginUser(UserAccount user)
        {
            return _LoginDAO.Match(user.UserName, user.passHash);
        }
    }
}
*/