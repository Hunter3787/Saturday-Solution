using AutoBuildApp.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;


namespace AutoBuildApp.Api.HelperFunctions
{
    /// <summary>
    /// Custom middleware into the request pipeline of ASP.NET Core application.
    /// </summary>
    public class JwtMiddleware
    {
        /// <summary>
        ///  in order to execute next middleware in a sequence, 
        ///  it should have RequestDelegate type parameter in the constructor. 
        /// </summary>
        private readonly RequestDelegate _next;

        private JwtValidator _validateAuthorizationHeader;

        // i want to store this into the 
        // http.context.item["ClaimsPrincipal"] =.
        private ClaimsPrincipal _threadPrinciple;

        /// <summary>
        /// RequestDelegate is a function that can process an HTTP request.
        /// </summary>
        /// <param name="next"></param>
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        ///   not asynchronous will block the 
        ///  thread till the time it completes the execution. 
        ///  So, make it asynchronous by using async and await
        ///  to improve performance and scalability. 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {

            Console.WriteLine($"\n\t " + $"IN THE JWT MIDDLE WARE \n");

            //STEP 1: EXTRACT THE TOKEN (IF EXISTS ANY)
            #region EXTRACT THE TOKEN (IF EXISTS ANY)
            var token = httpContext.Request
                .Headers[HeaderNames.Authorization]
                .FirstOrDefault()?
                .Split(" ")
                .Last();

            if (token != null)
            {
                Console.WriteLine($"Token to be validated : {token } \n");
            }

            var authHeader = httpContext.Request.Headers["Authorization"];//[0];

            if (AuthenticationHeaderValue.TryParse(authHeader, out var headerValue))
            {
                // we have a valid AuthenticationHeaderValue that has the following details:
                var scheme = headerValue.Scheme;

                var parameter = headerValue.Parameter;
                Console.WriteLine($"\n\t " + $"IN THE TRY PARSE \n" + $" scheme : { scheme}\n" + $" parameter :{ parameter }\n");

                // scheme will be "Bearer"
                // parmameter will be the token itself.
                token = parameter;
            }
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

            string ReturnUrl = Convert.ToString(httpContext.Request.QueryString.ToUriComponent());

            Console.WriteLine($"\n\t IN THE JWT_MIDDLEWARE \n" +
                $"INCOMING URL REFERAL TEST:   {Url}\n" +
                $"2: {Url_Two}\n" +
                $"3: {referer}\n" +
                $"4 query string: { ReturnUrl }\n");
            #endregion

            if (Url_Two.Contains("authentication"))
            {
                Console.WriteLine($"YOU ARE REQUESTING THE AUTHENTICATION URL");
            }
            else if (Url_Two.Contains("authdemo"))
            {

                Console.WriteLine($"YOU ARE REQUESTING THE AUTHDEMO URL\n");
            }

            /*
             * 2) user is NOT logged in and are not authorized -> 
             * *status code: 401.here comes in the REDIRECT URL.
             * They are directed to the login page and later
             * to the page intended(if they are aurthorized)
             *
             */

            if (token != null && token.Length != 0) // THERE EXISTS A JWT token 
            {
                _validateAuthorizationHeader = new JwtValidator(token); // validate the token 
                var result = ValidateTheToken(httpContext, token);
                if (result == false) //IF JWT NOT VALID
                {
                    //Console.WriteLine($"THE TOKEN PASSED IS NOT VALID JWT!");
                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return;

                }
            }
            else if (token == null || token == " ") // else set the default principle:
            {

                DefaultClaimsPrinciple();

                //if (Thread.CurrentPrincipal != null) // this check may be removed...
                //{
                //    Thread.CurrentPrincipal = (ClaimsPrincipal)Thread.CurrentPrincipal;
                //}

               
            }

            Console.WriteLine($"\n\t " +$"END OF THE JWT MIDDLE WARE \n");
            await _next(httpContext);
        }

        public bool ValidateTheToken(HttpContext httpContext, string token)
        {

            Console.WriteLine($"VALIDATING THE TOKEN METHOD.");
            ///https://dev.to/tjindapitak/better-way-of-storing-per-request-data-across-middlewares-in-asp-net-core-1m9k
            if (!_validateAuthorizationHeader.IsValidJWT()) // JWT IS NOT VALID, END CALL
            {

                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;//400; //Bad Request 
                return false;
            }
            else //THE JWT IS VALID, THEREFORE SET THE CLAIMS PRINCIPLE TO THREAD.
            {
                #region IF THE AUTH HEADER CONTAINS VALID TOKEN  -> SET CLAIMSPRINCIPAL TO THREAD
                JwtParser jwtParser = new JwtParser(token);

                var userPrinciple = jwtParser.ParseForClaimsPrinciple();
                _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal; // get the principle on the thread
                Console.WriteLine($"\nIN THE JWT MIDDLEWARE CHEWCKING THE PRINCIPLE NAME: {_threadPrinciple.Identity.Name}\n");
                Thread.CurrentPrincipal = _threadPrinciple; // SETTING THE PARSED TOKEN, TO THE THREAD.

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
