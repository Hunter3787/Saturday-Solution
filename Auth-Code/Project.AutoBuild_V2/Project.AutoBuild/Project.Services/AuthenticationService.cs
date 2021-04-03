using Project.DataAccess;
using Project.DataAccess.Entities;
using Project.Security.Models;

using System;
using System.Text.Json;
using System.Threading;

namespace Project.Services
{
    /// <summary>
    /// this class defines JWT: a JSON web token that the server side
    /// generates and contains a payload of data 
    /// The payload contains information about the User and set of permissions 
    /// the user has
    /// https://jwt.io/introduction
    /// https://auth0.com/docs/tokens/json-web-tokens/json-web-token-claims
    /// 
    /// </summary>
    public class AuthenticationService
    {

        #region JWT objects
        //the authentication service needs to use the JWT
        // classes  in the security folder.
        private JWT _jwt;
        private JWTPayload _payload;
        private JWTHeader _header;
        #endregion

        #region AuthDAO specific objects
        #endregion

        /// <summary>
        /// the data access object to query for user permissions 
        /// should a user exist in the system
        /// </summary>
        private AuthUserDTO _authUserDTO = new AuthUserDTO();
        private CommonReponseAuth _responseAuth = new CommonReponseAuth();
        private AuthDAO _authDAO;

        // this is to access the principleUser on the thread to update its identity and 
        // principle after they are authenticated 
        private UserPrinciple _threadPrinciple = (UserPrinciple)Thread.CurrentPrincipal;



        public AuthenticationService(AuthDAO authDAO)
        {
            try
            {
                if (authDAO == null)
                {

                    var expectedParamName = "NULL OBJECT PROVIDED";
                    throw new ArgumentNullException(expectedParamName);

                }

                this._authDAO = authDAO;
            }
            catch (ArgumentNullException)
            {
                var expectedParamName = "NULL OBJECT PROVIDED";
                throw new ArgumentNullException(expectedParamName);
            }

        }

        /// <summary>
        /// AutheniticateUser takes the users credentials and send it 
        /// to the backend for verification
        /// 
        /// </summary>
        /// <param name="credentials"></param>
        public object AuthenticateUser(UserCredentials credentials)
        {
            // retrieves the result from the query as an object
            // then we cast it to type Common responde object -> Auth
            _responseAuth = (CommonReponseAuth)_authDAO.RetrieveUserInformation(credentials);
            // an initial check for connection state:
            if (_responseAuth.connectionState == false)
            {
                _responseAuth.isAuthenticated = false;
                //return "Authentication Failed";
                return _responseAuth;
            }
            if (_responseAuth.IsUserExists == true)
            {
                _authUserDTO = _responseAuth.AuthUserDTO;
                _responseAuth.SuccessBool = true;
                _responseAuth.isAuthenticated = true;

                Console.WriteLine($"From Common Response: " +
                    $"{_responseAuth.SuccessString}");

                foreach (Claims claims in _authUserDTO.Claims)
                {
                    Console.WriteLine($" " +
                        $"userPermissions: {claims.Permission } " +
                        $"scope { claims.scopeOfPermissions}");
                }

                // setting the users claims retrieved to the 
                // user principle
                if(_threadPrinciple == null)
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


               _responseAuth.JWTString =  generateJWTToken(_authUserDTO);
               return _responseAuth;
            }
            else if (_responseAuth.IsUserExists == false)
            {
                _responseAuth.isAuthenticated = false;
                _responseAuth.SuccessBool = false;
                return _responseAuth;
            }
            // i think its enough to send back the JWT string to the controller
            return _responseAuth;
        }

        /// <summary>
        /// this method is responsible for sending back the jwt 
        /// token for authentication
        /// </summary>
        /// <returns></returns>
        public string generateJWTToken(AuthUserDTO AuthUserDTO)
        {
            ///instantiating  a header value
            _header = new JWTHeader();
            //_key = "a random, long, sequence of characters that only the server knows";
            string _key = "Secret";

            string jsonString = JsonSerializer.Serialize(_header);
            Console.WriteLine("Header\n" + jsonString + "\n\n");

            // so the first thing is taking the auth dto and passing it to form the payload necessary. 
            _payload = new JWTPayload
                ("Autobuild User", "Autobuild", "US",
                AuthUserDTO.UserEmail,
                DateTimeOffset.UtcNow.AddDays(7), 
                DateTimeOffset.UtcNow.AddDays(7),
                DateTimeOffset.UtcNow)
            {
                UserCLaims =  AuthUserDTO.Claims
            };
            
            jsonString = JsonSerializer.Serialize(_payload);
            Console.WriteLine("PayLoad\n" + jsonString + "\n\n");


            // instantiating the jwt class
            _jwt = new JWT(_key, _payload, _header);

            // call the signature JWT generater to return the signature
            string result = _jwt.GenerateJWTSignature();

            Console.WriteLine($"In authentication service to generateJWTToken: \n" +
                $" {_jwt.ToString()} \n\n");


            // returns back the jwt token
            return _jwt.ToString();
        }
    }


}

