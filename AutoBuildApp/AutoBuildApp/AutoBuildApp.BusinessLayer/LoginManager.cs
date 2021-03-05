using System;
using System.Collections.Generic;
using System.Text;
using AutoBuildApp.ServiceLayer;
namespace AutoBuildApp.BusinessLayer
{
    class LoginManager
    {
        private LoginService loginService;
        private String _cnnctString;
        public LoginManager()
        {
            _cnnctString = "Server=localhost; Database=Registration_Pack;Trusted_Connection=True;";
            loginService = new LoginService(_cnnctString);

        }




    }
}
