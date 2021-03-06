using System;
using AutoBuildApp.Models;
using AutoBuildApp.ServiceLayer;
namespace AutoBuildApp.BusinessLayer
{
    public class LoginManager
    {
        private LoginService _LogService;
<<<<<<< Updated upstream
        private UserAccount _userAccount;
       
        public LoginManager(LoginService LogService)
        {
            _LogService = LogService;
        }


        public bool ValidUserName(string username)
        {
            return !String.IsNullOrEmpty(username) && username.Length >= 4 && username.Length <= 20;
        }
        public bool IsPasswordValid(string password)
        {
            return !String.IsNullOrEmpty(password);
        }
        public bool IsInformationValid(UserAccount user)
        {
            return ValidUserName(user.UserName)
                && !IsPasswordValid(user.passHash);
         
        }


=======
        private String _cnnctString;
        public LoginManager(String _cnnctString)
        {
            this._cnnctString = _cnnctString;
            _LogService = new LoginService(_cnnctString);
        }


>>>>>>> Stashed changes
        public String DoesUserExist(UserAccount user)
        {
            if(  IsInformationValid( user) == false)
            {
                return "Information is invalid!";
            }
            else
                return _LogService.LoginUser(user);
        }


       

    }
}
