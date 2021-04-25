using AutoBuildApp.DataAccess;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Security.Models;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;


namespace AutoBuildApp.Services.Auth_Services
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


        #region System.Security.Claims


        ClaimsPrincipal claimsPrincipal;

        // instantiation of security claims of 
        // type System.Security.Claims;
        private IList<Claim> _securityClaims = new List<Claim>();


        #endregion


        #region Data access objects

        /// <summary>
        /// the data access object to query for user permissions 
        /// should a user exist in the system
        /// </summary>
        private AuthUserDTO _authUserDTO = new AuthUserDTO();
        private CommonReponseAuth _responseAuth = new CommonReponseAuth();
        private AuthDAO _authDAO;

        #endregion

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
        public CommonReponseAuth AuthenticateUser(UserCredentials credentials) // RETURN CMRESPONSE
        {
            claimsPrincipal = (ClaimsPrincipal)Thread.CurrentPrincipal;
            /// retrieves the result from the query as an object
            /// then we cast it to type Common responde object -> Auth
            _responseAuth = _authDAO.RetrieveUserInformation(credentials);
            // an initial check for connection state:
            if (!_responseAuth.connectionState)
            {
                _responseAuth.isAuthenticated = false;
            }
            // return immediately cuz db is off  - do this - dNNY

            if (_responseAuth.IsUserExists == true)
            {

                _authUserDTO = _responseAuth.AuthUserDTO;
                _responseAuth.SuccessBool = true;
                _responseAuth.isAuthenticated = true;

                // conversion of userClaims to the built in claims 
                foreach (Claims claims in _authUserDTO.Claims)
                { // converting the claims in type System.Security.Claims
                    _securityClaims.Add(new Claim(claims.Permission, claims.scopeOfPermissions));
                }
                /// if the quthentication is a success then we
                /// add thos new claims to the claims principle
                UserIdentity userIdentity = new UserIdentity();
                userIdentity.Name = _authUserDTO.UserEmail;
                userIdentity.IsAuthenticated = true;


                ClaimsIdentity claimsIdentity = new ClaimsIdentity
                    (userIdentity,
                    _securityClaims,
                    userIdentity.AuthenticationType,
                     userIdentity.Name, "");
                claimsIdentity = new ClaimsIdentity
                  (userIdentity,
                  _securityClaims);


                claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                Console.WriteLine("\tChecking in the authentication service\n");
                foreach (var clm in claimsPrincipal.Claims)
                {
                    Console.WriteLine($" claim type: { clm.Type } claim value: {clm.Value} \n");
                }
                _responseAuth.JWTString = generateJWTToken(_authUserDTO);
            }
            else
            {
                _responseAuth.isAuthenticated = false;
                _responseAuth.SuccessBool = false;
            }

            Thread.CurrentPrincipal = claimsPrincipal;
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
            /*
            string jsonString = JsonSerializer.Serialize(_header);
            Console.WriteLine("Header\n" + jsonString + "\n\n");
            */

            // so the first thing is taking the auth dto and passing it to form the payload necessary. 
            _payload = new JWTPayload
                ("Autobuild User", "Autobuild", "US",
                AuthUserDTO.UserEmail,
                DateTimeOffset.UtcNow.AddDays(7),
                DateTimeOffset.UtcNow.AddDays(7),
                DateTimeOffset.UtcNow)
            {
                UserCLaims = AuthUserDTO.Claims
            };

            /*
            jsonString = JsonSerializer.Serialize(_payload);
            Console.WriteLine("PayLoad\n" + jsonString + "\n\n");
            */

            // instantiating the jwt class
            _jwt = new JWT(_key, _payload, _header);

            // call the signature JWT generater to return the signature
            string result = _jwt.GenerateJWTSignature();

            /*
            Console.WriteLine($"In authentication service to generateJWTToken: \n" +
                $" {_jwt.ToString()} \n\n");
            */

            // returns back the jwt token
            return _jwt.ToString();
        }
    }


}

