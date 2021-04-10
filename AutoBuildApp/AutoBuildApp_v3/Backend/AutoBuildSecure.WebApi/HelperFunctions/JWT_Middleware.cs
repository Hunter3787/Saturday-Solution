using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
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
        private ClaimsPrincipal _threadPrinciple;


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
          

            Console.WriteLine($"\n\t THIS IS WHATS IN THE AUTH HEADER: { token}  \n");


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
                $"INCOMING URL REFERAL TEST:   { Url}");
            if (token != null && token.Length != 0)
            {

                Console.WriteLine($"HITTING FIRST IF");
                _validateAuthorizationHeader = new JWTValidator(token);

                var result = ValidateTheToken(httpContext, token);
                if (result == false)
                {             
                    await httpContext.Response.WriteAsync(httpContext.Response.StatusCode.ToString());

                }

            }
            else if (token == null)
            { 


                Console.WriteLine($"HITTING 2ND ELSE IF");
                if (Thread.CurrentPrincipal != null)
                {
                    Thread.CurrentPrincipal = (ClaimsPrincipal)Thread.CurrentPrincipal;
                }
                else
                {

                    DefaultClaimsPrinciple();

                }
                //here we will set a default claims prinicple instead of returning that^.
            }
            await _next(httpContext);
        }

        public bool ValidateTheToken(HttpContext httpContext, string token)
        {
            
                ///https://dev.to/tjindapitak/better-way-of-storing-per-request-data-across-middlewares-in-asp-net-core-1m9k
                ///
                if (!_validateAuthorizationHeader.IsValidJWT())
                {
                    httpContext.Response.StatusCode = 400; //Bad Request   
                    return false;
                }
                else
                {
                    #region IF THE AUTH HEADER CONTAINS VALID TOKEN  -> SET CLAIMSPRINCIPAL TO THREAD
                   
                    var userPrinciple =
                        _validateAuthorizationHeader.ParseForClaimsPrinciple();
                    Console.WriteLine($" " +
                        $"THE USER CLAIMS PRINCIPLE if jwt it valid. in jwt middleware");
                    _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
                    Thread.CurrentPrincipal = _threadPrinciple;
                    foreach (Claim c in _threadPrinciple.Claims)
                    {
                        Console.WriteLine($"Permission:  {c.Type}, Scope: {c.Value} ");
                    }

                    #endregion
                }
           
            return true;


        }


        public void DefaultClaimsPrinciple()
        {
            // setting a default principle object t=for the thread.
            #region Instantiating the guest principle
            _threadPrinciple = Guest.DefaultClaimsPrinciple();
            Thread.CurrentPrincipal = _threadPrinciple;
            #endregion
        }

    }
}
