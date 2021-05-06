using System;
using System.Collections.Generic;

using AutoBuildApp.Services;
using System.Text;
using AutoBuildApp.Models.Users;
using System.Linq;
using BC = BCrypt.Net.BCrypt;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers.UserManagers;

namespace AutoBuildApp.Managers
{
    public class RegistrationManager
    {
        private InputValidityManager _inputValidityManager;
        private RegistrationDAO _RegistrationDAO;

        public RegistrationManager(String _cnnctString)
        {
            _inputValidityManager = new InputValidityManager();
            _RegistrationDAO = new RegistrationDAO(_cnnctString);
        }

        


        public String RegisterUser(string username, string firstname, string lastname, string email, string password,
            string passwordCheck)
        {
            UserAccount user = new UserAccount();
            user.UserName = username;
            user.FirstName = firstname;
            user.LastName = lastname;
            user.UserEmail = email;
            user.passHash = password;
            if (!_inputValidityManager.IsInformationValid(user, passwordCheck))
            {
                return "Invalid Input!";
            }
            else
            {
                user.passHash = BC.HashPassword(user.passHash, BC.GenerateSalt());
                return _RegistrationDAO.RegisterAccount(user);
            }
        }
    }
}
