using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Security.Models
{
    public class JWTHeader
    {
        private static readonly JWTHeader headerDefault = new JWTHeader();

        public JWTHeader()
        {
            this.typ = "JWT";
            this.alg = Algotithm.HS256.AlgValue;
        }

        #region Properties to define a JWT HEADER.
        /// <summary>
        /// The header is like the following:
        /// {
        /// "alg": "HS256"
        /// "typ": "JWT
        /// }
        ///  this JSON is Base64Url 
        ///  encoded to form the first part of the JWT.
        /// /// </summary>
        /// 

        public string typ
        {
            get; set;

        }
        /// <summary>
        /// the security algorithm defines the "signing algorithm"
        ///  such as HMAC SHA256 or RSA.
        ///  **required
        /// </summary>
        public string alg
        {
            get; set;
        }

        #endregion


        public override string ToString()
        {
            return $"Alg: {alg} Token: {typ}";
        }

    }
    /// <summary>
    /// Sealed classes are used to restrict 
    /// the inheritance feature of object oriented programming. Once a class is defined as a sealed class,
    /// this class cannot be inherited. 
    /// </summary>

    public sealed class Algotithm
    {


        /// <summary>
        ///Read-only modifier  readonly
        ///A static constructor executes 
        ///once per type, rather than once per instance. 
        /// </summary>
        public static readonly Algotithm HS256 = new Algotithm("HS256");
        /// <summary>
        /// the Algorithm string value for JWT alg with a
        /// private set modifier on the setter
        /// </summary>
        public string AlgValue { get; private set; }

        private Algotithm(string algValue)
        {
            AlgValue = algValue;
        }
    }


}