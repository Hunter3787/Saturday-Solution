using AutoBuildApp.Security.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace AutoBuildSecure.WebApi.HelperFunctions
{
    public class JWT_Middleware
    {
        /// <summary>
        /// processes http requests.
        /// </summary>
        private readonly RequestDelegate _next;

        private JWTValidator _validateAuthorizationHeader;

        // i want to store this into the 
        // http.context.item["ClaimsPrincipal"] =.
        private ClaimsPrincipal _principal;


        public JWT_Middleware(RequestDelegate next)
        {
            _next = next;

        }



        public async Task InvokeAsync(HttpContext httpContext)
        {
            /// first read the http authorization header 
            /// for the bearer token
            /// 

            /// postman isnt sending a request header containing aa referer....
            /// so:
            /// https://learning.postman.com/docs/sending-requests/requests/
            /// 
            ///

            string referer = "";
            var token = httpContext.Request
                .Headers[HeaderNames.Authorization]
                .FirstOrDefault()?
                .Split(" ")
                .Last();

            var Url =
                httpContext.Request
                .Headers[HeaderNames.Path]
                .FirstOrDefault();
            var test = httpContext.Request.Headers;


            var Url_Two =
                httpContext.Request.Path.ToUriComponent().ToString();


            referer = httpContext
                .Request
                .Headers["Referer"]
                .ToString();

            Console.WriteLine($"\n\t IN THE JWT_MIDDLEWARE \n" +
                $"INCOMING URL REFERAL TEST:   { Url}" +
                $" \nand referer string: {referer} " +
                $" \nand the referal by header name: {Url_Two}");
            if (token != null)
            {

                _validateAuthorizationHeader = new JWTValidator(token);
                ValidateTheToken(httpContext, token);
            }

            await _next(httpContext);
        }

        public void ValidateTheToken(HttpContext httpContext, string token)
        {
            try
            {
                ///https://dev.to/tjindapitak/better-way-of-storing-per-request-data-across-middlewares-in-asp-net-core-1m9k
                ///
                if (!_validateAuthorizationHeader.IsValidJWT())
                {
                    httpContext.Items["JWT_Result"] = "JWT failure";

                }
                else
                {
                    /// going to parse throu the 
                    /// the jwt token and extract the 
                    /// user principle ( claims and identity ) 
                    var userPrinciple =
                        _validateAuthorizationHeader.ParseForClaimsPrinciple();

                }

            }
            catch
            {


            }



        }
    }
}
