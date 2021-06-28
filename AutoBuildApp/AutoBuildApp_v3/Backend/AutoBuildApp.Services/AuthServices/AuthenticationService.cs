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


        #region System.Security.Claims


        ClaimsPrincipal claimsPrincipal;

        // instantiation of security claims of type System.Security.Claims;
        private IList<Claim> _securityClaims = new List<Claim>();


        #endregion


        #region Data access objects

        /// <summary>
        /// the data access object to query for user permissions 
        /// should a user exist in the system
        /// </summary>
        private AuthUserDTO _authUserDTO = new AuthUserDTO();
        private CommonReponseAuth _responseAuth = new CommonReponseAuth();
        //private AuthDAO _authDAO;
        private LoginDAO _loginDAO;

        #endregion

        public AuthenticationService(LoginDAO loginDAO)
        {
            try
            {
                if (loginDAO == null)
                {
                    var expectedParamName = "NULL OBJECT PROVIDED";
                    throw new ArgumentNullException(expectedParamName);

                }
                this._loginDAO = loginDAO;
                //this._authDAO = authDAO;
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
        /// </summary>
        /// <param name="credentials"></param>
        public CommonReponseAuth AuthenticateUser(UserCredentials credentials) // RETURN CMRESPONSE
        {

            //_responseAuth = _authDAO.RetrieveUserInformation(credentials);
            _responseAuth = _loginDAO.LoginInformation(credentials);
            // an initial check for connection state:


            if (!_responseAuth.connectionState)
            {
                _responseAuth.isAuthenticated = false;
                return _responseAuth;
            }

            if (!_responseAuth.IsUserExists)
            {

                _responseAuth.isAuthenticated = false;
                _responseAuth.IsSuccessful = false;
                return _responseAuth;
            }


            _authUserDTO = _responseAuth.AuthUserDTO;
            _responseAuth.IsSuccessful = true;
            _responseAuth.isAuthenticated = true;

            // conversion of userClaims to the .net built in claims 
            foreach (Claims claims in _authUserDTO.Claims)
            { // converting the claims in type System.Security.Claims
                _securityClaims.Add(new Claim(claims.Permission, claims.ScopeOfPermissions));
              }
            /// if the quthentication is a success then we new claims to the claims principle
            UserIdentity userIdentity = new UserIdentity()
            {
                Name = _authUserDTO.UserName,
                IsAuthenticated = true,
            };


            ClaimsIdentity claimsIdentity = new ClaimsIdentity
                (userIdentity,
                _securityClaims,
                userIdentity.AuthenticationType,
                 userIdentity.Name, "");

            // or
            claimsIdentity = new ClaimsIdentity
              (userIdentity,
              _securityClaims);

            claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


            _responseAuth.JWTString = generateJWTToken(_authUserDTO);

            Thread.CurrentPrincipal = claimsPrincipal; // set the authenticated user to the thread
            return _responseAuth;
        }

        /// <summary>
        /// this method is responsible for sending back the jwt 
        /// token for authentication
        /// </summary>
        /// <returns></returns>
        public string generateJWTToken(AuthUserDTO AuthUserDTO)
        {
            #region JWT objects
            //the authentication service needs to use the JWT
            // classes  in the security folder.
            JWT jwt;
            JWTPayload payload;
            JWTHeader header;
            #endregion



            ///instantiating  a header value
            header = new JWTHeader();

            string _key = "Secret"; //_key = "a random, long, sequence of characters that only the server knows"; 

            payload = new JWTPayload
                ("Autobuild User", "Autobuild", "US",
                AuthUserDTO.UserName, // username
                DateTimeOffset.UtcNow.AddDays(7),
                DateTimeOffset.UtcNow.AddDays(7),
                DateTimeOffset.UtcNow)
            {
                UserCLaims = AuthUserDTO.Claims
            };


            jwt = new JWT(_key, payload, header);

            // call the signature JWT generater to return the signature
            string result = jwt.GenerateJWTSignature();

            return jwt.ToString();
        }
    }


}

