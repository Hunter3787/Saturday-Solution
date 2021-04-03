using System;
 //https://docs.microsoft.com/en-us/dotnet/api/microsoft.identitymodel.tokens.base64urlencoder?view=azure-dotnet 


using System.Security.Cryptography;
using System.Text.Json;

using System.Text;
using Microsoft.IdentityModel.Tokens;
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

            ///Console.WriteLine($" TESTING THE SIGNATURE: \n\n");
            ///var handler = new JwtSecurityTokenHandler();
            ///var decodedValue = handler.ReadToken(jwt);
            ///Console.WriteLine("Decoded value \n" + decodedValue.ToString());
            JWTToken = jwt;
            JWTSignature = signature;
            return signature;

        }
        public string JWTToken { get; set; }
        public string JWTSignature { get; set; }
        public  string ToString2()
        {
            return $"\nThe JWT token: {JWTToken}\n" +
                $"Header : {ApplyJson(Header)} \n" +
                $"Payload: {ApplyJson(Payload)} \n";
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


    /// <summary>
    /// okay time to validate jwt tokens
    /// https://tools.ietf.org/html/rfc7519#section-7.2
    /// 
    /// </summary>
    public class JWTValidator
    {
        //Represents an ASCII character encoding of Unicode characters.
        private ASCIIEncoding _encoding = new ASCIIEncoding();
        private JWTHeader _jwtHeaderDefault = new JWTHeader(Algorithm.HS256.AlgValue);
        private JWTPayload _jwtPayloadDefault = new JWTPayload();
        private JWT _jwt;


        public string JWT { get; set; }
        private string SecretKey { get { return "secret"; } set { value = "secret"; } }

        public JWTValidator()
        {
            this.JWT = " ";
        }
        public JWTValidator(string JWTToken)
        {
            this.JWT = JWTToken;
            Console.WriteLine("Token pased: " + JWTToken);
        }


        // so what I am thinking is that upon the 
        // successful JWT validation then the UserPrinciple object 
        // will be return to set it to the current thread.
        public UserPrinciple UserPrincipleObject { get; set; }


        public JWTPayload GetJWTPayload()
        {
            string[] parseJWT = JWT.Split('.');
            string payloadJSON = Base64UrlEncoder.Decode(parseJWT[1]);
            Console.WriteLine($"\n\nJson payload: {payloadJSON}");
            JWTPayload payload = JsonSerializer.Deserialize<JWTPayload>(payloadJSON);
            return payload;
        }

        public JWTHeader GetJWTHeader()
        {
            string[] parseJWT = JWT.Split('.');
            string headerJSON = Base64UrlEncoder.Decode(parseJWT[0]);
            JWTHeader header = JsonSerializer.Deserialize<JWTHeader>(headerJSON);
            return header;


        }

        #region Validating a JWT

        /// <summary>
        /// IsValidJWT runs all the checks for the jwt and returns true 
        /// upon success or false upon invalid jwt
        /// </summary>
        /// <returns></returns>
        public bool IsValidJWT()
        {
            #region check the JWT state:

            if (isValidJWTFormat() == false) { return false; }

            #endregion

            #region check the header data:
            if (isValidHeader() == false) { return false; }
            #endregion

            #region check the payload data:
            if (isValidPayload() == false) { return false; }
            #endregion

            #region check the signature 
            if (isValidSignature() == false) { return false; }

            #endregion
            return true;
        }

        /// <summary>
        /// method isValidSignature() validates the JWT signature
        /// </summary>
        /// <returns></returns>
        public bool isValidSignature()
        {

            string[] parseJWT = JWT.Split('.');
            string headerJSON = Base64UrlEncoder.Decode(parseJWT[0]);
            JWTHeader header = JsonSerializer.Deserialize<JWTHeader>(headerJSON);



            string payloadJSON = Base64UrlEncoder.Decode(parseJWT[1]);
            Console.WriteLine($"\n\nJson payload: {payloadJSON}");
            JWTPayload payload = JsonSerializer.Deserialize<JWTPayload>(payloadJSON);
            Console.WriteLine("\n\nPaylaod ToString: " + payload.ToString());



            string signature = parseJWT[2];
            Console.WriteLine($" the signature passed: {signature}");
            // so what i will do is regenrate the signature and 
            // check if the generated signature matches the 
            // one passed 
            _jwt = new JWT(SecretKey, payload, header);
            string genratedJWT = _jwt.GenerateJWTSignature();
            Console.WriteLine($"generated JWT signature: { genratedJWT}");

            if (genratedJWT.Equals(signature) == false)
            {
                Console.WriteLine($"the signature is not valid.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// checks the JWT form itself
        /// </summary>
        /// <returns></returns>
        public bool isValidJWTFormat()
        {

            //1. is the string empty?
            if (string.IsNullOrWhiteSpace(JWT) || string.IsNullOrEmpty(JWT)) { Console.WriteLine($"null JWT token"); return false; }
            //2.Verify that the JWT contains at least one period ('.') character.
            if (JWT.Contains(".") == false) { Console.WriteLine($"not valid JWT token, missing '.'"); return false; }
            // I want to split to retrieve the header, payload and signature
            string[] parseJWT = JWT.Split('.');

            if (parseJWT.Length != 3) { Console.WriteLine($" JWT length not valid."); return false; }
            //3.Let the Encoded JOSE Header be the portion of the JWT before the
            // first period('.') character.
            string headerAsJson = JsonSerializer.Serialize(_jwtHeaderDefault);
            Console.WriteLine($"Default header is: {headerAsJson}");
            Console.WriteLine($"Other header is: {Base64UrlEncoder.Decode(parseJWT[0])}");
            // 4. Base64url decode check:
            if (headerAsJson.Equals(Base64UrlEncoder.Decode(parseJWT[0])) == false) { return false; }

            return true;
        }
        /// <summary>
        /// checks if the header values are in line with Autobuilds 
        /// choice of algorithm and type values
        /// </summary>
        /// <returns></returns>
        public bool isValidHeader()
        {
            JWTHeader header = GetJWTHeader();
            if (header.typ != _jwtHeaderDefault.typ || header.alg != _jwtHeaderDefault.alg)
            {
                Console.WriteLine($"not vaild type or algorithm");
                return false;
            }
            return true;
        }


        /// <summary>
        /// checks the JWT payload data such as 
        /// expiration of the JWT token, the not before time,
        /// and whether the default values by Autobuild match. 
        /// (being the auduence, issuer, and subject line)
        /// </summary>
        /// <returns></returns>
        public bool isValidPayload()
        {
            JWTPayload payload = GetJWTPayload();
            Console.WriteLine("\n\nPaylaod ToString: " + payload.ToString());

            if (payload.exp <= payload.ToUnixTimestamp(DateTimeOffset.UtcNow))
            {
                Console.WriteLine($"not vaild experation time.");
                if (payload.nbf <= payload.ToUnixTimestamp(DateTimeOffset.UtcNow))
                {
                    Console.WriteLine($"not vaild not before time.");
                    return false;
                }
            }
            if (payload.Equals(_jwtPayloadDefault) == false)
            {
                Console.WriteLine($"payload not valid in JWT token");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Base64url decode the Encoded JOSE Header following the
        /// restriction that no line breaks, whitespace, or other additional
        /// characters have been used.
        /// </summary>
        /// <param name="input"></param>
        public bool checkBase64urlDecode(string encodedString)
        {
            /*
            * note the only thing you will see in a base 64:
            * A-Z, a-z, 0-9, + / =
            *  padded in the end with three: = to make it a length of mulitple four
            *  
            * but for jwt we are using!!BASE 64 URL ENCODING (might see _)
            * 
            * so i guess when you decode , - => + and _ => / and adding three === at the end.
            * (opposite of encoding)
            */
            var output = Base64UrlEncoder.Decode(encodedString);
            Console.WriteLine($"Decoded value: {output} ");

            string start = encodedString;

            start = start.Replace('_', '/');
            start = start.Replace('-', '+');

            // also the ecoded string is a length of multiple of four:
            switch (start.Length % 4) // goes throu the string by four
            {
                //length 0
                case 0:
                    break;
                //length 2
                case 2:
                    start += "=";
                    break;

                case 3:
                    start += "=+";
                    break;
                default:
                    return false;

            }
            Convert.FromBase64String(start);

            return true;

        }

        #endregion

        /// <summary>
        /// returns the user Object to be stored to the thread 
        /// upon complete verfifcatons.
        /// </summary>
        /// <returns></returns>
        public UserPrinciple ParseForUserPrinciple()
        {
            /// if the JWT check all pass then
            /// go ahead and parse for the user object information
            UserPrinciple userPrinciple = new UserPrinciple();
            if (IsValidJWT())
            {
                /// here we assign the user information 
                ///  from the JWT token to the UserPrinciple Object
                JWTPayload payload = GetJWTPayload();
                UserIdentity userIdentity = new UserIdentity
                {
                    Name = payload.Username,
                    IsAuthenticated = false,
                    AuthenticationType = "JWT"
                };
                userPrinciple = new UserPrinciple(userIdentity)
                {
                    Permissions = payload.UserCLaims

                };

            }
            this.UserPrincipleObject = userPrinciple;
            return userPrinciple;
        }



    }

}
