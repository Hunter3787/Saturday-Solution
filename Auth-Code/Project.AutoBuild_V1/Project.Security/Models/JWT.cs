using System;
using Microsoft.IdentityModel.Tokens; //https://docs.microsoft.com/en-us/dotnet/api/microsoft.identitymodel.tokens.base64urlencoder?view=azure-dotnet 
using IdentityModel;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Project.Security.Models
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
    public class JWT
    {

        private HMACSHA256 hmac;
        private ASCIIEncoding encoding = new ASCIIEncoding();
        // the payload and header are defined in seperate classes
        private JWTHeader Header { get; set; }
        private  JWTPayload Payload { get; set; }

        /// <summary>
        /// for the signiture 
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// JWT constructor.
        /// </summary>
        /// <param name="secretKey"></param>
        public JWT(string secretKey, object payload, object header)
        {
            SecretKey = secretKey;
            this.Header = (JWTHeader)header;
            this.Payload = (JWTPayload)payload;
            hmac = new  HMACSHA256();

        }

        /*
         * note the only thing you will see in a base 64:
         * A-Z, a-z, 0-9, + / =
         * 
         * but for jwt we are using!!BASE 64 URL ENCODING (might see _)
         */

       
        public string ApplyJson(object headpay)
        { 
            return JsonSerializer.Serialize(headpay);

        }

        public string Base64Object(Object obj)
        {
            string jsonString = JsonSerializer.Serialize(obj);
            return jsonString;

        }

        /// <summary>
        /// https://www.jokecamp.com/blog/examples-of-creating-base64-hashes-using-hmac-sha256-in-different-languages/#csharp
        /// </summary>
        /// <returns></returns>
        public string GenerateJWTSignature()
        {
            //Convert.ToBase64String(
        
            byte[] header = Encoding.UTF8.GetBytes(ApplyJson(Header));
            byte[] payload = Encoding.UTF8.GetBytes(ApplyJson(Payload));
            byte[] secret = Encoding.UTF8.GetBytes(SecretKey);
            byte[] sig;
            string EncodedHeaderPayload =$"{Base64UrlEncoder.Encode(header)}.{Base64UrlEncoder.Encode(payload)}";
            hmac.Key= secret;
            sig = Encoding.UTF8.GetBytes(EncodedHeaderPayload);


            string ret = Base64UrlEncoder.Encode(hmac.ComputeHash(sig));

            string jwt = $"{Base64UrlEncoder.Encode(header)}.{Base64UrlEncoder.Encode(payload)}.{ret}";


            Console.WriteLine($" TESTING THE SIGNATURE: \n\n");
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(jwt);

            Console.WriteLine("decoded value \n" + decodedValue.ToString());




            Console.WriteLine($" TESTING THE SIGNATURE: PART 2 \n\n");
            string test = "ZsT2hOxY2bPmAiRAuKQwqzpDp6V5xwx8SbFWp3NScGs";
            jwt = $"{Base64UrlEncoder.Encode(header)}.{Base64UrlEncoder.Encode(payload)}.{test}";
            
            decodedValue = handler.ReadJwtToken(jwt);

            Console.WriteLine("decoded value " + decodedValue.ToString());


            return ret;

        }

        public override string ToString()
        {
            string Signature = "\n";
            Signature += $"JWT:{ Base64UrlEncoder.Encode(encoding.GetBytes(ApplyJson(Header)))}." +
                $"{Base64UrlEncoder.Encode(ApplyJson(Payload))}\n";

            Signature += $"The generated signature: {GenerateJWTSignature()}\n";


            return Signature;
        }

    }


    public class JWTValidator
    {

        /*
         * To validate a JWT, your application needs to:
         * Check that the JWT is well formed.
         * Check the signature.
         * Check the standard claims.
         * If any of these steps fail, then the 
         * associated request must be rejected
         * 
         * 
         * 
         */
        public bool CheckJWTForm(string jwt)
        {
            /*
            1) Verify that the JWT contains three segments, 
              separated by two period ('.') characters.
            2) parse th JWT to extract its three components

             The first segment is the Header, the second is 
            the Payload, and the third is the Signature.
            Each segment is base64url encoded.

            3) Base64url-decode the Header AND the payload, 
            ensuring that no line breaks, whitespace, 
            or other additional characters have been used, 
            and verify that the decoded Header is a valid JSON object.

             */
            //https://docs.microsoft.com/en-us/dotnet/api/system.identitymodel.tokens.jwt.jwtsecuritytokenhandler.canreadtoken?view=azure-dotnet

            bool canReadToken = false;


            return false;

        }
        /*
         * 
         * https://auth0.com/docs/tokens/json-web-tokens/validate-json-web-tokens#manually-implement-checks
         * 
         * 
         */
        public void checkSigniture()
        {


        }


        public void CheckClaims()
        {


        }

    }
}
