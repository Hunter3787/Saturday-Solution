using AutoBuildApp.Security.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace AutoBuildApp.Security
{
    /// <summary>
    /// okay time to validate jwt tokens
    /// https://tools.ietf.org/html/rfc7519#section-7.2
    /// 
    /// </summary>
    public class JwtParser
    {

        #region claims priciple built in
        private IList<Claim> _securityClaims = new List<Claim>();

        ClaimsPrincipal _principalGenerated;

        #endregion

        private JwtValidator _jwtValidator;
        public string JWT { get; set; }
        public JwtParser()
        {
            this.JWT = string.Empty;
        }
        public JwtParser(string JWTToken)
        {
            this.JWT = JWTToken;
            this._jwtValidator = new JwtValidator(JWTToken);
        }

        #region EXTRACT JWT TOKEN INTO CLAIMS PRINCIPAL

        /// <summary>
        /// returns the user Object to be stored to the thread 
        /// upon complete verfifcatons.
        /// </summary>
        /// <returns></returns>
        public ClaimsPrincipal ParseForClaimsPrinciple()
        {
            if (!_jwtValidator.IsValidJWT())
            {
                return null;
            }

            /// here we assign the user information 
            ///  from the JWT token to the UserPrinciple Object
            JWTPayload payload = _jwtValidator.GetJWTPayload();
            UserIdentity userIdentity = new UserIdentity
            {
                Name = payload.Username,
                IsAuthenticated = true,
                AuthenticationType = "JWT"
            };
            foreach (Claims claims in payload.UserCLaims)
            { // converting the claims in type System.Security.Claims
                _securityClaims.Add(new Claim(claims.Permission, claims.ScopeOfPermissions));
            }

           
            ClaimsIdentity claimsIdentity = new ClaimsIdentity
            (userIdentity, _securityClaims, userIdentity.AuthenticationType, userIdentity.Name, string.Empty);
            _principalGenerated = new ClaimsPrincipal(claimsIdentity);

            Thread.CurrentPrincipal = _principalGenerated; //setting to the thread.
            return _principalGenerated;
        }

        #endregion


    }
}
