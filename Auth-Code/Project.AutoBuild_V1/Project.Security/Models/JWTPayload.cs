using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Security.Models
{
    public class JWTPayload
    {

        #region The JWT Paylod containing the claims Registered Claim Names
        /// <summary>
        /// JWS payload (set of claims): contains verifiable security statements, such as the identity 
        /// of the user and the permissions they are allowed.
        /// </summary>

        //*Autubuild.com
        public string Iss { get; set; }
        public string Subject { get; set; }

        /// <summary>
        /// who is this token for? 
        /// </summary>
        public string Aud { get; set; }
        public string UserName { get; set; }

        DateTimeOffset Exp
        {
            get
            {
                return DateTimeOffset.UtcNow.AddDays(1);
            }
        }
        public IEnumerable<IUserClaim> UserCLaims { get; set; }


        /// <summary>
        ///    The "iat" (issued at) claim 
        ///    identifies the time at which the JWT was
        ///    issued.
        /// </summary>
        public string iat { get; set; }

        /// <summary>
        ///  The "exp" (expiration time) claim identifies the expiration time on
        ///  or after which the JWT MUST NOT be accepted for processing.
        /// </summary>
        public string exp { get; set; }

        /// <summary>
        /// "nbf" (Not Before) Claim
        /// The "nbf" (not before) claim identifies the time before which the JWT
        /// MUST NOT be accepted for processing.The processing of the "nb
        /// claim requires that the current date/time MUST be after or equal to
        /// the not-before date/time listed in the "nbf" claim.
        /// </summary>

        public string nbf { get; set; }

        #endregion

    }
}
