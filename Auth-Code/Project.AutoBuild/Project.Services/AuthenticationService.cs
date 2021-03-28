using Project.DataAccess;
using Project.DataAccess.Entities;
using Project.Security.Models;
using System;
using System.Text.Json;

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
        //the authentication service needs to use the JWT class
        private JWT _jwt;
        private JWTPayload _payload;
        private JWTHeader _header;


        private AuthDAO _authDAO;
        private CommonReponseAuth _responseAuth;
        public AuthenticationService(AuthDAO authDAO)
        {
            this._authDAO = authDAO;
        }



        /// <summary>
        /// AutheniticateUser takes the users credentials and send it 
        /// to the backend for verification
        /// 
        /// </summary>
        /// <param name="credentials"></param>
        public bool AuthenticateUser(UserCredentials credentials)
        {
            var userInformation = _authDAO.RetrieveUserPermissions(credentials);
            _responseAuth = (CommonReponseAuth)userInformation;

            // an initial check for connection state:
            if (_responseAuth.connectionState == false)
            {
                Console.WriteLine($"Message from commonresponse: {_responseAuth.Failure} ");
                // I believ the business layer need a common response oject as well
                return false;
            }
            if (_responseAuth.SuccessBool == true)
            {
                Console.WriteLine($"From Common Response: {_responseAuth.Success}");
                foreach (UserPermissionsEntitiy claims in _responseAuth.AuthUserDTO.Claims)
                {
                    Console.WriteLine($" userPermissions: {claims.Permission } scope { claims.scopeOfPermissions}");
                }


                generateJWTToken((AuthUserDTO)_responseAuth.AuthUserDTO);
                return true;
            }
            else
            {
                Console.WriteLine($"From Common Response: {_responseAuth.Failure}");
            }
            return false;
        }


        /// <summary>
        /// this method is responsible for sending back the jwt 
        /// token for authentication
        /// </summary>
        /// <returns></returns>
        public string generateJWTToken(AuthUserDTO AuthUserDTO)
        {
            Console.WriteLine("WE ARE IN THE AUTHENTICATION SERVICE");
            // the jwt token generator will take the object from the database upon authentication 
            // and return a jwt token 
            _header = new JWTHeader();
            string _key = "secret";
            Console.WriteLine(_header.ToString());
            string jsonString = JsonSerializer.Serialize(_header);
            Console.WriteLine("Header\n" + jsonString + "\n\n");
            // so the first thing is taking the auth dto and passing it to form the payload necessary. 

            _payload = new JWTPayload()
            {
                Iss = "25374",
                UserName = "zee",
                Aud = "zee",
                Subject = "Autobuild",
                iat = "1594209600",
                exp = "1594209600",
                nbf = "1594209600",
                UserCLaims = AuthUserDTO.Claims
            };
            jsonString = JsonSerializer.Serialize(_payload);
            Console.WriteLine("PayLoad\n" + jsonString + "\n\n");


            _jwt = new JWT(_key, _payload, _header);
            Console.WriteLine($"{_jwt.ToString()} \n\n");


            // should the user not be authenticated then they retrieve minimum permissions (need to encorporate into design 
            // and show vong.




            return " ";
        }
        /// now that the user is authenticationed depecding on the 
        /// result the jwt token is generated
        /// 

    }


}

