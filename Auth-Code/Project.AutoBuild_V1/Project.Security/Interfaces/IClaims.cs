using Project.Security.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Security.Interfaces
{  /// <summary>
   /// JWT payload
   /// https://tools.ietf.org/html/rfc7519#section-4.1
   /// Notice that the claim names are only three characters
   /// long as JWT is meant to be compact.
   /// </summary>
    public interface IClaims
    {
        /// <summary>
        /// The "iss" (issuer) claim identifies
        /// the principal that issued the JWT.
        /// generally application specific.
        /// The "iss" value is a case-sensitive 
        /// string containing a StringOrURI value.
        /// </summary>
        string Iss { get; set; }
        /// <summary>
        ///  The "sub" (subject) claim identifies 
        ///  the principal that is the subject of the JWT.
        /// The"sub" value is a case-sensitive string
        /// containing a StringOrURI
        /// value.Use of this claim is OPTIONAL.
        /// </summary>
        string Subject { get; set; }
        /// <summary>
        /// Aud (audience): Recipient for which the JWT is intended
        /// </summary>
        string Aud { get; set; }
        /// <summary>
        /// Exp (expiration time): Time after which the JWT expires
        /// </summary>
        DateTimeOffset Exp
        {
            get
            {
                return DateTimeOffset.UtcNow.AddDays(1);
            }
        }

        // I can add my own CUSTOM claims , which is the payload of the JWT:


        #region defining my custom JWT payload os UserName and Claims
        /// <summary>
        /// The username
        /// </summary>
        string UserName { get; set; } // zeinab@yahoo/alodbf

        /// <summary>
        /// the set of permissioons and scope a user has that my code will evaluate:
        /// "UserClaims": {
        ///     "create" : "admin and below"
        ///     "delete" : "all"
        /// }
        /// </summary>
        IEnumerable<IUserClaim> UserCLaims { get; set; }

        #endregion

    }
}
