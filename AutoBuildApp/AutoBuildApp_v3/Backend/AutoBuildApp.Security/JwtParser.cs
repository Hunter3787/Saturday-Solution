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
            this.JWT = " ";
        }
        public JwtParser(string JWTToken)
        {
            this.JWT = JWTToken;
            this._jwtValidator = new JwtValidator(JWTToken);
            Console.WriteLine(_jwtValidator.GetJWTPayload());

            //Console.WriteLine("Token pased: " + JWTToken);
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
            Console.WriteLine($" user identity in the parser { userIdentity.ToString()}");

            /*
            ClaimsIdentity claimsIdentity = 
                new ClaimsIdentity(
                    userIdentity,
                    _securityClaims,
            userIdentity.AuthenticationType, userIdentity.Name, " ");
            _principalGenerated.AddIdentity(claimsIdentity);

            */

            //_securityClaims.Add(new Claim("USERNAME", userIdentity.Name));
            ClaimsIdentity claimsIdentity = new ClaimsIdentity
            (userIdentity, _securityClaims, userIdentity.AuthenticationType, userIdentity.Name, " ");

            //claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(userIdentity.Name));
            //_principalGenerated.FindFirst("USERNAME").Value

            _principalGenerated = new ClaimsPrincipal(claimsIdentity);

            Console.WriteLine($"\n" +
                $"In the jwt parser");
            foreach (Claim c in _principalGenerated.Claims)
            {
                Console.WriteLine($" " +
                    $"claim type: { c.Type } claim value: {c.Value} ");

            }

            Thread.CurrentPrincipal = _principalGenerated; //setting to the thread.
            return _principalGenerated;
        }

        #endregion


    }
}
