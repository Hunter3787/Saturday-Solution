using System;
using System.Collections.Generic;

using AutoBuildApp.ServiceLayer;
using System.Text;
using AutoBuildApp.Models;

namespace AutoBuildApp.BusinessLayer
{
    class RegistrationManager
    {

        private RegistrationService _RegService;
        private String _cnnctString;
        private UserAccount _userAccount;
        public RegistrationManager(RegistrationService RegService)
        {
            _cnnctString = "Server=localhost; Database=Registration_Pack;Trusted_Connection=True;";
            _RegService = RegService;
        }


        // validate entry:
        // is the userAccount null
        // is the string empty
        // is the password of certain length -> min 8 char, upper and lower case REQUIRED, at least one digit
        /// is passowrd empty 
        /// is password ->


        public String RegisterUser(UserAccount user)
        {
            String ret = " ";
            bool result = ValidEmail(user.UserEmail);
            result = ValidUserName(user.UserName);

          

            return _RegService.IsRegistrationValid(user);
        }
        public bool ValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        public bool ValidUserName(string username)
        {
            return !String.IsNullOrEmpty(username) && username.Length >= 4 && username.Length <= 12;
        }
        // apply password
        public bool IsInformationValid(UserAccount user)
        {
            return ValidEmail(user.UserEmail)
                && ValidUserName(user.UserName)
                && !String.IsNullOrEmpty(user.FirstName)
                && !String.IsNullOrEmpty(user.LastName)
                ;
        }


    }
}
