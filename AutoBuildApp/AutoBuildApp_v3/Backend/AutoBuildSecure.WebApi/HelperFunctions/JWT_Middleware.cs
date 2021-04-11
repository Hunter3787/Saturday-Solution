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

        /// <summary>
        /// RequestDelegate is a function that can process an HTTP request.
        /// </summary>
        /// <param name="next"></param>
        public JWT_Middleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            /// postman isnt sending a request header containing aa referer....
            /// so:
            /// https://learning.postman.com/docs/sending-requests/requests/
            /// 
            ///

            //STEP 1: EXTRACT THE TOKEN (IF EXISTS ANY)
            #region EXTRACT THE TOKEN (IF EXISTS ANY)
            var token = httpContext.Request
                .Headers[HeaderNames.Authorization]
                .FirstOrDefault()?
                .Split(" ")
                .Last();
            Console.WriteLine($"\n\t THIS IS WHATS IN THE AUTH HEADER: { token}  \n");
            #endregion
            //STEP 2: EXTRACT THE REFERER
            #region EXTRACT THE REFERER
            var Url =
                httpContext.Request
                .Headers[HeaderNames.Path]
                .FirstOrDefault();
            var Url_Two =
                httpContext.Request.Path.ToUriComponent().ToString();
            string referer = httpContext
                .Request
                .Headers["Referer"]
                .ToString();

            Console.WriteLine($"\n\t IN THE JWT_MIDDLEWARE \n" +
                $"INCOMING URL REFERAL TEST:   {Url}\n" +
                $"2: {Url_Two}\n" +
                $"3: {referer}\n");
            #endregion
            if(Url_Two.Contains("authentication"))
            {
                Console.WriteLine($"YOU ARE REQUESTING THE AUTHENTICATION URL");
            }
            else if (Url_Two.Contains("authdemo"))
            {

                Console.WriteLine($"YOU ARE REQUESTING THE AUTHDEMO URL");
            }


            if (token != null && token.Length != 0) // there is a JWT token 
            {
                _validateAuthorizationHeader = new JWTValidator(token); // validate the token 
                var result = ValidateTheToken(httpContext, token);
                if (result == false) //IF JWT NOT VALID
                {             
                    await httpContext
                        .Response
                        .WriteAsync(httpContext.Response.StatusCode.ToString());

                }
            }
            else if (token == null) // else set the default principle:
            { 
                if (Thread.CurrentPrincipal != null) // this check may be removed...
                {
                    Thread.CurrentPrincipal = (ClaimsPrincipal)Thread.CurrentPrincipal;
                }
                else{ DefaultClaimsPrinciple();}
            }
            await _next(httpContext);
        }

        public bool ValidateTheToken(HttpContext httpContext, string token)
        {
            
                ///https://dev.to/tjindapitak/better-way-of-storing-per-request-data-across-middlewares-in-asp-net-core-1m9k
                ///
                if (!_validateAuthorizationHeader.IsValidJWT()) // JWT IS NOT VALID, END CALL
                {
                    httpContext.Response.StatusCode = 400; //Bad Request   
                    return false;
                }
                else //THE JWT IS VALID, THEREFORE SET THE CLAIMS PRINCIPLE TO THREAD.
                {
                    #region IF THE AUTH HEADER CONTAINS VALID TOKEN  -> SET CLAIMSPRINCIPAL TO THREAD
                   
                    var userPrinciple =
                        _validateAuthorizationHeader.ParseForClaimsPrinciple();
                    _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
                    Thread.CurrentPrincipal = _threadPrinciple; // SETTING THE PARSED TOKEN, TO THE THREAD.

                /*
                Console.WriteLine($" " +
                    $"THE USER CLAIMS PRINCIPLE if jwt it valid. in jwt middleware");
                foreach (Claim c in _threadPrinciple.Claims)
                    {
                        Console.WriteLine($"Permission:  {c.Type}, Scope: {c.Value} ");
                    }
                */
                    #endregion
                }
           
            return true;


        }
        /// <summary>
        /// CREATED A HELPER CLASS THAT HANDLES THE DEFAULT VALUES
        /// FOR A GUEST REQUEST. (EMPTY TOKEN).
        /// </summary>
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
