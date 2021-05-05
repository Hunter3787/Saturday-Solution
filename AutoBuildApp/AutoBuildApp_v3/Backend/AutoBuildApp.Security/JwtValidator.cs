using AutoBuildApp.Security.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AutoBuildApp.Security
{

    ///// <summary>
    ///// okay time to validate jwt tokens
    ///// https://tools.ietf.org/html/rfc7519#section-7.2
    ///// 
    ///// </summary>
    public class JwtValidator
    {
        //Represents an ASCII character encoding of Unicode characters.
        private ASCIIEncoding _encoding = new ASCIIEncoding();
        private JWTHeader _jwtHeaderDefault = new JWTHeader(Algorithm.HS256.AlgValue);
        private JWTPayload _jwtPayloadDefault = new JWTPayload();
        private JWT _jwt;


        public string JWT { get; set; }
        private string SecretKey { get { return "Secret"; } set { value = "Secret"; } }

        public JwtValidator()
        {
            this.JWT = " ";
        }
        public JwtValidator(string JWTToken)
        {
            this.JWT = JWTToken;
            //Console.WriteLine("Token pased: " + JWTToken);
        }


        public JWTPayload GetJWTPayload()
        {
            string[] parseJWT = JWT.Split('.');
            string payloadJSON = Base64UrlEncoder.Decode(parseJWT[1]);
            Console.WriteLine($"\n\nJson payload in validator: {payloadJSON}");
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

            if (IsValidJWTFormat() == false) { return false; }

            #endregion

            #region check the header data:
            if (IsValidHeader() == false) { return false; }
            #endregion

            #region check the payload data:
            if (IsValidPayload() == false) { return false; }
            #endregion

            #region check the signature 
            if (IsValidSignature() == false) { return false; }

            #endregion
            return true;
        }

        /// <summary>
        /// method isValidSignature() validates the JWT signature
        /// </summary>
        /// <returns></returns>
        public bool IsValidSignature()
        {

            string[] parseJWT = JWT.Split('.');
            string headerJSON = Base64UrlEncoder.Decode(parseJWT[0]);
            JWTHeader header = JsonSerializer.Deserialize<JWTHeader>(headerJSON);

            string payloadJSON = Base64UrlEncoder.Decode(parseJWT[1]);
            ///Console.WriteLine($"\n\nJson payload: {payloadJSON}");
            JWTPayload payload = JsonSerializer.Deserialize<JWTPayload>(payloadJSON);
            ///Console.WriteLine("\n\nPaylaod ToString: " + payload.ToString());



            string signature = parseJWT[2];

            //  regenrate the signature and check if the generated signature matches the 
            // one passed 
            _jwt = new JWT(SecretKey, payload, header);
            string genratedJWT = _jwt.GenerateJWTSignature();

            if (genratedJWT.Equals(signature) == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// checks the JWT form itself
        /// </summary>
        /// <returns></returns>
        public bool IsValidJWTFormat()
        {

            //1. is the string empty?
            if (string.IsNullOrWhiteSpace(JWT) || string.IsNullOrEmpty(JWT))
            {
                //Console.WriteLine($"null JWT token"); 
                return false;
            }

            //2.Verify that the JWT contains at least one period ('.') character.
            if (JWT.Contains(".") == false)
            {
                //Console.WriteLine($"not valid JWT token, missing '.'"); 
                return false;
            }

            // I want to split to retrieve the header, payload and signature
            string[] parseJWT = JWT.Split('.');

            //3. id JWT split at 3?
            if (parseJWT.Length != 3)
            {
                //Console.WriteLine($" JWT length not valid.");
                return false;
            }

            //3.Let the Encoded JOSE Header be the portion of the JWT before the
            // first period('.') character.
            string headerAsJson = JsonSerializer.Serialize(_jwtHeaderDefault);

            // 4. Base64url decode check:
            if (headerAsJson.Equals(Base64UrlEncoder.Decode(parseJWT[0])) == false) { return false; }

            return true;
        }
        /// <summary>
        /// checks if the header values are in line with Autobuilds 
        /// choice of algorithm and type values
        /// </summary>
        /// <returns></returns>
        public bool IsValidHeader()
        {
            JWTHeader header = GetJWTHeader();
            if (header.typ != _jwtHeaderDefault.typ || header.alg != _jwtHeaderDefault.alg)
            {
                //Console.WriteLine($"not vaild type or algorithm");
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
        public bool IsValidPayload()
        {
            JWTPayload payload = GetJWTPayload();
            //Console.WriteLine("\n\nPaylaod ToString: " + payload.ToString());

            if (payload.exp <= payload.ToUnixTimestamp(DateTimeOffset.UtcNow))
            {
                //Console.WriteLine($"Not vaild experation time.");
                if (payload.nbf <= payload.ToUnixTimestamp(DateTimeOffset.UtcNow))
                {
                    // Console.WriteLine($"Not vaild not before time.");
                    return false;
                }
            }
            if (payload.Equals(_jwtPayloadDefault) == false)
            {
                //Console.WriteLine($"Payload not valid in JWT token");
                return false;
            }

            return true;
        }


        #endregion

    }
}
