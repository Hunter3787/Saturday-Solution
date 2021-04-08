using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBuildSecure.WebApi.HelperFunctions
{
    public class DemoMiddleware
    {
        private readonly RequestDelegate _next;

        private ClaimsIdentity _claimsIdentity;
        private ClaimsPrincipal _principal;
        private IIdentity userIdentity = new UserIdentity();
        private ClaimsFactory claimsFactory = new ConcreteClaimsFactory();

        private JWTValidator _validateAuthorizationHeader;
        private IClaimsFactory unregistered;

        public DemoMiddleware(RequestDelegate next)
        {
            _next = next;
            unregistered = claimsFactory.GetClaims(RoleEnumType.UNREGISTERED_ROLE);
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            if (string.IsNullOrEmpty(httpContext.Request.Headers[HeaderNames.Authorization]))
            {

                var accessTokenReq = httpContext.Request.Headers[HeaderNames.Authorization];
                string accessToken = accessTokenReq;
                Console.WriteLine($"\nThe authorization header: { accessTokenReq}\n");

                // assigning as default claims principle
                _claimsIdentity = new ClaimsIdentity(unregistered.Claims(), userIdentity.AuthenticationType, userIdentity.Name, " ");

                _principal = new ClaimsPrincipal(_claimsIdentity);
                httpContext.User = _principal;
                Thread.CurrentPrincipal = _principal;
                await _next(httpContext);


            }

            if (!string.IsNullOrEmpty(httpContext.Request.Headers[HeaderNames.Authorization]))
            {
                var accessTokenReq = httpContext.Request.Headers[HeaderNames.Authorization];
                string accessToken = accessTokenReq;
                Console.WriteLine($"\nThe authorization header: { accessTokenReq}\n");


                string[] parse = accessToken.Split(' ');
                accessToken = parse[1];
                _validateAuthorizationHeader = new JWTValidator(accessToken);


                if (!string.IsNullOrEmpty(accessToken))
                {
                    if (!_validateAuthorizationHeader.IsValidJWT())
                    {
                        await httpContext.Response.WriteAsync(StatusCodes.Status500InternalServerError.ToString());
                    }
                    else
                    {
                        var userPrinciple = _validateAuthorizationHeader.ParseForUserPrinciple();
                        Console.WriteLine($" " +
                            $"PARSING FOR THE CLAIMS PRINCIPLE \n" +
                            $" {userPrinciple}");

                        _claimsIdentity = new ClaimsIdentity(userPrinciple.Claims);
                        _principal = userPrinciple;
                        httpContext.User = _principal;

                        Thread.CurrentPrincipal = _principal;

                        await _next(httpContext);
                    }
                }
            }

        }
    }
}
