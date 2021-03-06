using System;
using AutoBuildApp.Models;
using AutoBuildApp.ServiceLayer;
namespace AutoBuildApp.BusinessLayer
{
    public class LoginManager
    {
        private LoginService _LogService;

        private String _cnnctString;
        public LoginManager(String _cnnctString)
        {
            this._cnnctString = _cnnctString;
            _LogService = new LoginService(_cnnctString);
        }


        public String DoesUserExist(UserAccount user)
        {
                return _LogService.LoginUser(user);
        }
    }
}
