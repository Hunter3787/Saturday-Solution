using System;
using System.Collections.Generic;

using System.Text;
using AutoBuildApp.Models;
using AutoBuildApp.ServiceLayer;
using System.Linq;

namespace AutoBuildApp.BusinessLayer
{
    class LoginManager
    {
        private LoginService _LogService;
        private String _cnnctString;
        private UserAccount _userAccount;
        public LoginManager(LoginService LogService)
        {
            _cnnctString = "Server=localhost; Database=Registration_Pack;Trusted_Connection=True;";
            _LogService = LogService;
        }

        public String DoesUserExist(UserAccount user)
        {
            return _LogService.LoginUser(user);
        }


    }
}
