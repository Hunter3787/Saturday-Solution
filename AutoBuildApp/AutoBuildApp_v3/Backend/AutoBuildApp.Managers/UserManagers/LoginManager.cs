using System;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.Users;
using AutoBuildApp.Services;

namespace AutoBuildApp.Managers
{
    public class LoginManager
    {
        private LoginDAO _LoginDAO;

        public LoginManager(String CnnctString)
        {
            // establish a connection to DB

            _LoginDAO = new LoginDAO(CnnctString);
        }


        //public String LoginUser(string username, string password)
        //{
        //    return _LoginDAO.LoginInformation();
        //}
    }
}
