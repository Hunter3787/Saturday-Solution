using AutoBuildApp.Services.SessionsService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBuildApp.Api.HelperFunctions
{
    /// <summary>
    /// this middleware is called after the jwt middleware (in order to assign a session to the thread)
    /// </summary>
    public class SessionsMiddleware
    {

        /// <summary>
        /// processes http requests.
        /// </summary>
        private readonly RequestDelegate _next;

        // i want to store this into the 
        // http.context.item["ClaimsPrincipal"] =.
        private ClaimsPrincipal _threadPrinciple;

        /// <summary>
        /// RequestDelegate is a function that can process an HTTP request.
        /// </summary>
        /// <param name="next"></param>
        public SessionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            
            Console.WriteLine($"IN THE SESSIONS MIDDLE WARE \n");

            SessionService sessionService = new SessionService();


            // STEP 1: GET THE CURRENT PINCIPLE ON THE THREAD
            // IN ORDER TO ADD THE SESSIONS TOKEN 

            _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            //https://stackoverflow.com/questions/24587414/how-to-update-a-claim-in-asp-net-identity 
            var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity; // getting the identity in order to add sessions claim


            //STEP 1: ISSUE THE SESSIONS TOKEN

            var SessiionsID = sessionService.GenerateUniqueSessionsIdentifierGUID();

            //https://www.uuidgenerator.net/dev-corner/c-sharp
            // will ask vong.
            identity.AddClaim(new Claim("SEESIONSID", SessiionsID.ToString() )); // not string value will be converted back into guid -> back into bigint

            // add that identity to the thread (will need to make adjustment with the authorization.
            _threadPrinciple.AddIdentity(identity); 
            Thread.CurrentPrincipal = _threadPrinciple;

            //STEP 2: HOW TO RETRIEVE THIS TOKEN:


            Console.WriteLine($"this is my SESSIONS ID: " +
               $"{_threadPrinciple.FindFirst("SEESIONSID").Value}\n");



            await _next(httpContext);
        }










    }
}
