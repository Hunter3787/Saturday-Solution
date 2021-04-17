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
            _RegistrationDAO = new RegistrationDAO(_cnnctString);
        }

        


        public String RegisterUser(UserAccount user)
        {
            if (!_inputValidityManager.IsInformationValid(user))
            {
                return "Invalid Input!";
            }
            user.passHash = BC.HashPassword(user.passHash, BC.GenerateSalt());
            return _RegistrationDAO.CreateUserRecord(user);
        }
    }
}
