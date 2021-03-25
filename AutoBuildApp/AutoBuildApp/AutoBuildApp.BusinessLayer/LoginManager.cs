using System;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.ServiceLayer;
namespace AutoBuildApp.BusinessLayer
{
    public class LoginManager
    {
        private LoginDAO _LoginDAO;

        public LoginManager(String CnnctString)
        {
            // establish a connection to DB

            _LoginDAO = new LoginDAO(CnnctString);
        }


        public String LoginUser(UserAccount user)
        {
                return _LoginDAO.MatchData(user.UserName, user.passHash);
        }
    }
}
