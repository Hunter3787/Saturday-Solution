using System;
using System.Collections.Generic;

using AutoBuildApp.ServiceLayer;
using System.Text;
using AutoBuildApp.Models;
using System.Linq;

namespace AutoBuildApp.BusinessLayer
{
    {

        private RegistrationService _RegService;
<<<<<<< Updated upstream
        public RegistrationManager(RegistrationService RegService)
        {
            _RegService = RegService;
        }


        // validate entry:
        // is the userAccount null
        // is the string empty
        // is the password of certain length -> min 8 char, upper and lower case REQUIRED, at least one digit


        public String RegisterUser(UserAccount user)
        {   
            if(!IsInformationValid(user))
            {
                return "Invalid Input!";
            }
            return _RegService.IsRegistrationValid(user);
        }

        public bool ValidEmail(string email)
        {
        }

        public bool ValidUserName(string username)
        {
            return !String.IsNullOrEmpty(username) && username.Length >= 4 && username.Length <= 20;
        }

        public bool IsPasswordValid(string password)
        {
            return !String.IsNullOrEmpty(password)
                && password.Length >= 8
                && password.Any(char.IsDigit)
                && password.Any(char.IsLower)
                && password.Any(char.IsUpper);
        }

        public bool IsInformationValid(UserAccount user)
        {
            return ValidEmail(user.UserEmail)
                && IsPasswordValid(user.passHash)
                && ValidUserName(user.UserName)
                && !String.IsNullOrEmpty(user.FirstName)
                && !String.IsNullOrEmpty(user.LastName)
                ;
        }


    }
}
