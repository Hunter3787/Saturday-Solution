using AutoBuildApp.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoBuildApp.Managers.UserManagers
{
    public class InputValidityManager
    {

        public bool ValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".") && !String.IsNullOrEmpty(email);
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
