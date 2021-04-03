using System;



namespace AutoBuildApp.Security.Models
{
    public class JWTHeader : IEquatable<JWTHeader>
    {
        private static readonly JWTHeader headerDefault = new JWTHeader();
        public JWTHeader()
        {
            this.typ = "JWT";
            this.alg = Algorithm.HS256.AlgValue;
        }
        public JWTHeader(string Algorithm)
        {
            try
            {
                this.typ = "JWT";
                this.alg = Algorithm;
            }
            catch (ArgumentNullException)
            {
                var expectedParamName = "NULL OBJECT PROVIDED";
                throw new ArgumentNullException(expectedParamName);
            }
        }
        #region Properties to define a JWT HEADER.
        /// <summary>
        /// the security algorithm defines the "signing algorithm"
        ///  such as HMAC SHA256 or RSA.
        ///  **required
        /// </summary>
        public string alg
        {
            get; set;
        }
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
       
        #endregion
        public override string ToString()
        {
            return $"Alg: {alg} Token: {typ}";
        }
        #region IEquatable interface implementation
        public bool Equals(JWTHeader other)
        {
            if (other is null)
            {
                return false;
            }


            return this.typ == other.typ && this.alg == other.alg;
        }

        #endregion

    }

    /// <summary>
    /// Sealed classes are used to restrict 
    /// the inheritance feature of object oriented programming. Once a class is defined as a sealed class,
    /// this class cannot be inherited. 
    /// </summary>
    public sealed class Algorithm
    {


        /// <summary>
        ///Read-only modifier  readonly
        ///A static constructor executes 
        ///once per type, rather than 
        ///once per instance. 
        ///
        /// HS256 (HMAC with SHA-256)
        /// </summary>
        public static readonly Algorithm HS256 = new Algorithm("HS256");
        /// <summary>
        /// specifies the use of RS256 (RSA Signature with SHA-256):
        /// </summary>
        public static readonly Algorithm RS256 = new Algorithm("RS256");
        /// <summary>
        /// the Algorithm string value for JWT alg with a
        /// private set modifier on the setter
        /// </summary>
        public string AlgValue { get; private set; }

        private Algorithm(string algValue)
        {
            AlgValue = algValue;
        }
    }

    /*
     * references:
     * signing algorithms:
     * https://auth0.com/docs/tokens/signing-algorithms
     * 
     * 
     * 
     * 
     */

}