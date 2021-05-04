using System;
//https://docs.microsoft.com/en-us/dotnet/api/microsoft.identitymodel.tokens.base64urlencoder?view=azure-dotnet 


using System.Security.Cryptography;
using System.Text.Json;

using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading;
/// <summary>
/// Reference: see /AuthReference.=
/// </summary>

namespace AutoBuildApp.Security.Models
{
    /// <summary>
    /// this class defines JWT: a JSON web token that
    /// the server side generates and contains a payload 
    /// of data. The payload contains information about
    /// the User and set of permissions the user has.
    /// </summary>
    public class JWT
    {
        /// <summary>
        /// Computes a Hash-based Message Authentication Code (HMAC)
        /// by using the SHA256 hash function
        /// </summary>
        private HMACSHA256 _SHA256;

        /// <summary>
        /// Represents an ASCII character encoding of Unicode characters.
        /// </summary>
        private ASCIIEncoding _encoding = new ASCIIEncoding();

        /// <summary>
        /// the payload and header are defined in seperate classes
        /// </summary>
        private JWTHeader Header { get; set; }
        private JWTPayload Payload { get; set; }

        public string JWTToken { get; set; }
        public string JWTSignature { get; set; }

        /// <summary>
        /// For the signiture, a secret key is defined along with 
        /// the algorithmic function
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// The JWT constructor, takes in the 
        /// secret key, the payload object and
        /// the header object for the JWT generation.
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="payload"></param>
        /// <param name="header"></param>
        public JWT(string secretKey, object payload, object header)
        {
            try
            {
                /// checking for null parameters first
                if (secretKey == null || payload == null || header == null)
                {
                    // catching any null objects passed.
                    var expectedParamName = "NULL OBJECT PROVIDED";
                    throw new ArgumentNullException(expectedParamName);
                }
                SecretKey = secretKey;
                this.Header = (JWTHeader)header;
                // here added the ifs should this class want to generate 
                // a JWT token based of a different algorithm
                if (this.Header.alg == Algorithm.HS256.AlgValue)
                {
                    // the hashing symmetric  algorithm our application uses
                    _SHA256 = new HMACSHA256(Encoding.ASCII.GetBytes(SecretKey));
                }
                if (this.Header.alg == Algorithm.RS256.AlgValue)
                {
                    // instantiate to use RS256 algorithm, the assymetric public/private
                    // key use (for future implementation)

                }
                // Finally instatiating the payload.
                this.Payload = (JWTPayload)payload;
            }
            catch (ArgumentNullException)
            {
                // catching any null objects passed.
                var expectedParamName = "NULL OBJECT PROVIDED";
                throw new ArgumentNullException(expectedParamName);

            }

        }

        /// <summary>
        /// serializing a C# object into a JSON formated object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ApplyJson(object obj)
        {
            return JsonSerializer.Serialize(obj);

        }

        /// <summary>
        /// https://www.jokecamp.com/blog/examples-of-creating-base64-hashes-using-hmac-sha256-in-different-languages/#csharp
        /// </summary>
        /// <returns></returns>
        public string GenerateJWTSignature()
        {
            //about ASCIIEncoding:  GetBytes(String)
            //Encodes all the characters in the specified
            //string into a sequence of bytes.
            byte[] header = Encoding.UTF8.GetBytes(ApplyJson(Header));
            byte[] payload = Encoding.UTF8.GetBytes(ApplyJson(Payload));

            // then we base 64 URL incode these bytes:
            string EncodedHeaderPayload =
                $"{Base64UrlEncoder.Encode(header)}" +
                $"." +
                $"{Base64UrlEncoder.Encode(payload)}";

            byte[] sig = Encoding.UTF8.GetBytes(EncodedHeaderPayload);
            string signature = Base64UrlEncoder.Encode(_SHA256.ComputeHash(sig));

            string jwt =
                $"{Base64UrlEncoder.Encode(header)}." +
                $"{Base64UrlEncoder.Encode(payload)}." +
                $"{signature}";

            JWTToken = jwt;
            JWTSignature = signature;
            return signature;
        }


        public override string ToString()
        {
            return ($"{JWTToken}");
        }

    }

    /*
     *  HMACSHA256  reference: 
     * https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hmacsha256?view=net-5.0
     * 
     * ASCIIEncoding
     * https://docs.microsoft.com/en-us/dotnet/api/system.text.asciiencoding?view=net-5.0
     * 
     * 
     * jwt validation reference: 
     * https://tools.ietf.org/html/rfc7519#section-7.2
     * 
     * for string parsing
     * https://docs.microsoft.com/en-us/dotnet/csharp/how-to/parse-strings-using-split#code-try-0
     * 
     * 
     *  Split(Char[]):
     *  
     *   https://docs.microsoft.com/en-us/dotnet/api/system.string.split?view=net-5.0
     *   
     *   
     *   
     */


    ///// <summary>
    ///// okay time to validate jwt tokens
    ///// https://tools.ietf.org/html/rfc7519#section-7.2
    ///// 
    ///// </summary>
    //public class JWTValidator
    //{
    //    //Represents an ASCII character encoding of Unicode characters.
    //    private ASCIIEncoding _encoding = new ASCIIEncoding();
    //    private JWTHeader _jwtHeaderDefault = new JWTHeader(Algorithm.HS256.AlgValue);
    //    private JWTPayload _jwtPayloadDefault = new JWTPayload();
    //    private JWT _jwt;


    //    #region claims priciple built in
    //    private IList<Claim> _securityClaims = new List<Claim>();

    //    ClaimsPrincipal _principalGenerated;

    //    #endregion


    //    public string JWT { get; set; }
    //    private string SecretKey { get { return "Secret"; } set { value = "Secret"; } }

    //    public JWTValidator()
    //    {
    //        this.JWT = " ";
    //    }
    //    public JWTValidator(string JWTToken)
    //    {
    //        this.JWT = JWTToken;
    //        //Console.WriteLine("Token pased: " + JWTToken);
    //    }


    //    public JWTPayload GetJWTPayload()
    //    {
    //        string[] parseJWT = JWT.Split('.');
    //        string payloadJSON = Base64UrlEncoder.Decode(parseJWT[1]);
    //        /// Console.WriteLine($"\n\nJson payload: {payloadJSON}");
    //        JWTPayload payload = JsonSerializer.Deserialize<JWTPayload>(payloadJSON);
    //        return payload;
    //    }

    //    public JWTHeader GetJWTHeader()
    //    {
    //        string[] parseJWT = JWT.Split('.');
    //        string headerJSON = Base64UrlEncoder.Decode(parseJWT[0]);
    //        JWTHeader header = JsonSerializer.Deserialize<JWTHeader>(headerJSON);
    //        return header;


    //    }

    //    #region Validating a JWT

    //    /// <summary>
    //    /// IsValidJWT runs all the checks for the jwt and returns true 
    //    /// upon success or false upon invalid jwt
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool IsValidJWT()
    //    {
    //        #region check the JWT state:

    //        if (IsValidJWTFormat() == false) { return false; }

    //        #endregion

    //        #region check the header data:
    //        if (IsValidHeader() == false) { return false; }
    //        #endregion

    //        #region check the payload data:
    //        if (IsValidPayload() == false) { return false; }
    //        #endregion

    //        #region check the signature 
    //        if (IsValidSignature() == false) { return false; }

    //        #endregion
    //        return true;
    //    }

    //    /// <summary>
    //    /// method isValidSignature() validates the JWT signature
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool IsValidSignature()
    //    {

    //        string[] parseJWT = JWT.Split('.');
    //        string headerJSON = Base64UrlEncoder.Decode(parseJWT[0]);
    //        JWTHeader header = JsonSerializer.Deserialize<JWTHeader>(headerJSON);

    //        string payloadJSON = Base64UrlEncoder.Decode(parseJWT[1]);
    //        ///Console.WriteLine($"\n\nJson payload: {payloadJSON}");
    //        JWTPayload payload = JsonSerializer.Deserialize<JWTPayload>(payloadJSON);
    //        ///Console.WriteLine("\n\nPaylaod ToString: " + payload.ToString());



    //        string signature = parseJWT[2];

    //        //  regenrate the signature and check if the generated signature matches the 
    //        // one passed 
    //        _jwt = new JWT(SecretKey, payload, header);
    //        string genratedJWT = _jwt.GenerateJWTSignature();

    //        if (genratedJWT.Equals(signature) == false)
    //        {
    //            return false;
    //        }

    //        return true;
    //    }

    //    /// <summary>
    //    /// checks the JWT form itself
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool IsValidJWTFormat()
    //    {

    //        //1. is the string empty?
    //        if (string.IsNullOrWhiteSpace(JWT) || string.IsNullOrEmpty(JWT))
    //        {
    //            //Console.WriteLine($"null JWT token"); 
    //            return false;
    //        }

    //        //2.Verify that the JWT contains at least one period ('.') character.
    //        if (JWT.Contains(".") == false)
    //        {
    //            //Console.WriteLine($"not valid JWT token, missing '.'"); 
    //            return false;
    //        }

    //        // I want to split to retrieve the header, payload and signature
    //        string[] parseJWT = JWT.Split('.');

    //        //3. id JWT split at 3?
    //        if (parseJWT.Length != 3)
    //        {
    //            //Console.WriteLine($" JWT length not valid.");
    //            return false;
    //        }

    //        //3.Let the Encoded JOSE Header be the portion of the JWT before the
    //        // first period('.') character.
    //        string headerAsJson = JsonSerializer.Serialize(_jwtHeaderDefault);

    //        // 4. Base64url decode check:
    //        if (headerAsJson.Equals(Base64UrlEncoder.Decode(parseJWT[0])) == false) { return false; }

    //        return true;
    //    }
    //    /// <summary>
    //    /// checks if the header values are in line with Autobuilds 
    //    /// choice of algorithm and type values
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool IsValidHeader()
    //    {
    //        JWTHeader header = GetJWTHeader();
    //        if (header.typ != _jwtHeaderDefault.typ || header.alg != _jwtHeaderDefault.alg)
    //        {
    //            //Console.WriteLine($"not vaild type or algorithm");
    //            return false;
    //        }
    //        return true;
    //    }


    //    /// <summary>
    //    /// checks the JWT payload data such as 
    //    /// expiration of the JWT token, the not before time,
    //    /// and whether the default values by Autobuild match. 
    //    /// (being the auduence, issuer, and subject line)
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool IsValidPayload()
    //    {
    //        JWTPayload payload = GetJWTPayload();
    //        //Console.WriteLine("\n\nPaylaod ToString: " + payload.ToString());

    //        if (payload.exp <= payload.ToUnixTimestamp(DateTimeOffset.UtcNow))
    //        {
    //            //Console.WriteLine($"Not vaild experation time.");
    //            if (payload.nbf <= payload.ToUnixTimestamp(DateTimeOffset.UtcNow))
    //            {
    //                // Console.WriteLine($"Not vaild not before time.");
    //                return false;
    //            }
    //        }
    //        if (payload.Equals(_jwtPayloadDefault) == false)
    //        {
    //            //Console.WriteLine($"Payload not valid in JWT token");
    //            return false;
    //        }

    //        return true;
    //    }


    //    #endregion

    //    #region EXTRACT JWT TOKEN INTO CLAIMS PRINCIPAL

    //    /// <summary>
    //    /// returns the user Object to be stored to the thread 
    //    /// upon complete verfifcatons.
    //    /// </summary>
    //    /// <returns></returns>
    //    public ClaimsPrincipal ParseForClaimsPrinciple()
    //    {
    //        if (!IsValidJWT())
    //        {
    //            return null;
    //        }

    //        /// here we assign the user information 
    //        ///  from the JWT token to the UserPrinciple Object
    //        JWTPayload payload = GetJWTPayload();
    //        UserIdentity userIdentity = new UserIdentity
    //        {
    //            Name = payload.Username,
    //            IsAuthenticated = true,
    //            AuthenticationType = "JWT"
    //        };
    //        foreach (Claims claims in payload.UserCLaims)
    //        { // converting the claims in type System.Security.Claims
    //            _securityClaims.Add(new Claim(claims.Permission, claims.scopeOfPermissions));
    //        }

    //        /*
    //        ClaimsIdentity claimsIdentity = 
    //            new ClaimsIdentity(
    //                userIdentity,
    //                _securityClaims,
    //        userIdentity.AuthenticationType, userIdentity.Name, " ");
    //        _principalGenerated.AddIdentity(claimsIdentity);

    //        */

    //        //_securityClaims.Add(new Claim("USERNAME", userIdentity.Name));
    //        ClaimsIdentity claimsIdentity = new ClaimsIdentity
    //        (userIdentity, _securityClaims, userIdentity.AuthenticationType, userIdentity.Name, " ");

    //        //claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(userIdentity.Name));
    //        //_principalGenerated.FindFirst("USERNAME").Value

    //        _principalGenerated = new ClaimsPrincipal(claimsIdentity);


    //        Thread.CurrentPrincipal = _principalGenerated; //setting to the thread.
    //        return _principalGenerated;
    //    }

    //    #endregion


    //}

}
