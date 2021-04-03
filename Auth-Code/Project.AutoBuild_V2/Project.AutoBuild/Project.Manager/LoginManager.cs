using Project.DataAccess;
using Project.DataAccess.Entities;
using Project.Security.Models;
using Project.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Project.Manager
{
    public class LoginManager
    {
        // this is to access the principleUser on the thread to update its identity and 
        // principle after they are authenticated 
        private UserPrinciple _threadPrinciple = (UserPrinciple)Thread.CurrentPrincipal;
        private AuthenticationService authenticationService;
        private AuthDAO _authDAO;
        private String _cnnctString;


        private AuthUserDTO _authUserDTO = new AuthUserDTO();

        public LoginManager(string _cnnctString)
        {
            Console.WriteLine($"connection string passed: { _cnnctString} ");
            // _cnnctString = "MyConnection";
            //this._cnnctString = _cnnctString;
            //string connectionString = _connectionManager.GetConnectionStringByName(_cnnctString);
            //connectionString = "Data Source=localhost;Initial Catalog=DB;Integrated Security=True;";
            _authDAO = new AuthDAO(_cnnctString);
            authenticationService = new AuthenticationService(_authDAO);
            //_LogService = new LoginService(_cnnctString);

        }

        public string AuthenticateUser(UserCredentials userCredentials)
        {

            var _CRAuth = (CommonReponseAuth)authenticationService.AuthenticateUser(userCredentials);
            if (_CRAuth.isAuthenticated == true)
            {
                // setting the users claims retrieved to the 
                // user principle
                if (_threadPrinciple == null)
                {
                    _threadPrinciple = new UserPrinciple();
                    _threadPrinciple.Permissions = _authUserDTO.Claims;
                    _threadPrinciple.myIdentity.UserEmail = _authUserDTO.UserEmail;
                    _threadPrinciple.myIdentity.IsAuthenticated = true;

                    Thread.CurrentPrincipal = _threadPrinciple;
                }
                else
                {
                    _threadPrinciple.Permissions = _authUserDTO.Claims;
                    _threadPrinciple.myIdentity.UserEmail = _authUserDTO.UserEmail;
                    _threadPrinciple.myIdentity.IsAuthenticated = true;
                }

            }
            else if (_CRAuth.isAuthenticated == false)
            {
                return "Authentication Failed, Username or Password Incorrect";
            }


            return _CRAuth.JWTString;

        }

    }
}
