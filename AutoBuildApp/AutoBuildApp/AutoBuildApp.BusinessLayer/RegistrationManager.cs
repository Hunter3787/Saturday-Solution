using System;
using System.Collections.Generic;

using AutoBuildApp.ServiceLayer;
using System.Text;
using AutoBuildApp.Models;
using System.Linq;
using BC = BCrypt.Net.BCrypt;
using AutoBuildApp.DataAccess;

namespace AutoBuildApp.BusinessLayer
{
    public class RegistrationManager
    {

        private RegistrationDAO _RegistrationDAO;

        public RegistrationManager(String _cnnctString)
        {
            _RegistrationDAO = new RegistrationDAO(_cnnctString);
        }


        // validate entry:
        // is the userAccount null
        // is the string empty
        // is the password of certain length -> min 8 char, upper and lower case REQUIRED, at least one digit

        // check inputs, hash password
        public String RegisterUser(UserAccount user)
        {   
            if(!IsInformationValid(user))
            {
                return "Invalid Input!";
            }
            user.passHash = BC.HashPassword(user.passHash, BC.GenerateSalt());
            return _RegistrationDAO.CreateUserRecord(user);
        }

        // checks email
        public bool ValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".") && !String.IsNullOrEmpty(email);
        }

        // checks username
        public bool ValidUserName(string username)
        {
            return !String.IsNullOrEmpty(username) && username.Length >= 4 && username.Length <= 20;
        }

        // checks password
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
