

using AutoBuildApp.Security;
using AutoBuildApp.Security.FactoryModels; // for the claims factory
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;

using AutoBuildApp.Security.Enumerations;
using System.Security.Principal;
using AutoBuildApp.DataAccess;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.Services.Auth_Services;

namespace AutoBuildSecure.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            //ClaimsIdentity _claimsIdentity;

            //ClaimsPrincipal _principal;


            //Console.WriteLine("Hello World!");

            //UserIdentity userIdentity = new UserIdentity();

            //ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            //IClaimsFactory unregistered = claimsFactory.GetClaims("Unregistered");
            //_claimsIdentity = new ClaimsIdentity(userIdentity, unregistered.Claims());
            //Console.WriteLine($" identity name: {_claimsIdentity.Name} {_claimsIdentity.IsAuthenticated} ");
            //_principal = new ClaimsPrincipal(_claimsIdentity);

            //// set it to th current thread
            //Thread.CurrentPrincipal = _principal;


            //UserCredentials _userCredentials = new UserCredentials("Zeina", "PassHash");
            //AuthManager _loginManager = new AuthManager("Data Source=localhost;Initial Catalog=DB;Integrated Security=True;");
            //var JWTToken = _loginManager.AuthenticateUser(_userCredentials);
            //Console.WriteLine(JWTToken);
            //JWTValidator validator = new JWTValidator(JWTToken);
            //Console.WriteLine($"is Valid JWt: {validator.IsValidJWT()}");


            ClaimsIdentity _claimsIdentity;
            IIdentity userIdentity = new UserIdentity();

            #region creating a principle for this thread

            IEnumerable<Claim> claims = new List<Claim>() {
                new Claim(PermissionEnumType.READ_ONLY,ScopeEnumType.AUTOBUILD),

            };
            _claimsIdentity = new ClaimsIdentity(claims, userIdentity.AuthenticationType, userIdentity.Name, " ");

            ClaimsPrincipal _principal = new ClaimsPrincipal(_claimsIdentity);


            Console.WriteLine($"GETTING THE USERNAME ON CURRENT THREAD" +
                $"{_principal.Identity.Name}" +
                $"for rhe claimsidentity issues:" +
                $"{_claimsIdentity.NameClaimType}");
            // _principal.AddIdentity(_claimsIdentity);
            // set it to th current thread
            Thread.CurrentPrincipal = _principal;

            #endregion

         

            /// TAKLE OUT THE HARDCODING AND DO ENUMERATIOONS OF ROLES INSTAD OT ENUMERATIOSN 
            /// NO STING STATEMENTS IN SWITCH
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaimsFactory unregistered = claimsFactory.GetClaims(RoleEnumType.UNREGISTERED_ROLE);
      

            Console.WriteLine($"\n\tAuthorization output" +
                $" {AuthorizationService.checkPermissions(unregistered.Claims())}");

            UserCredentials credential1 = new UserCredentials("Zeina", "PassHash"); //user exists

            CommonReponseAuth _CRAuth = new CommonReponseAuth();
            AuthDAO _authDAO = new AuthDAO("Data Source=localhost;Initial Catalog=DB;Integrated Security=True");

            AuthenticationService authenticationService = new AuthenticationService(_authDAO);


        _CRAuth = authenticationService.AuthenticateUser(credential1);

            Console.WriteLine($"test method " +
                $"{ _CRAuth.ToString()}");


            UserIdentity testing = new UserIdentity();
            Console.WriteLine($"" +
                $"\n testing :" +
                $" { testing.ToString()}");


        }

        public void testing()
        {




            IList<Claim> c = new List<Claim>() {
                new Claim("Read Only","AutoBuild"),

                new Claim("Read Only2","AutoBuild2")
            };


            IEnumerable<Claim> claim2 = new List<Claim>() {
                new Claim("Read Only2","AutoBuild2"),
                new Claim("test2" , "all2")

            };



            UserIdentity userIdentity = new UserIdentity();
            userIdentity.Name = "dummyEmail";
            userIdentity.IsAuthenticated = true;

            // https://docs.microsoft.com/en-us/dotnet/api/system.security.claims.claimsidentity?view=net-5.0
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userIdentity, c);
            Console.WriteLine($" identity name: {claimsIdentity.Name} { claimsIdentity.IsAuthenticated} ");
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            Thread.CurrentPrincipal = principal;

            // principal.AddIdentity(new ClaimsIdentity(claim2));

            Console.WriteLine(principal.Claims.ToString());

            foreach (var clm in principal.Claims)
            {
                Console.WriteLine($" claim type: { clm.Type } claim value: {clm.Value} ");
            }
            CP test = new CP();
            test.testing();



            //------------------------------------------------------------------------

            Console.WriteLine($"this is a test for Ifactory for claims:");

            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();


            IClaimsFactory unregistered = claimsFactory.GetClaims("Unregistered");
            unregistered.PrintClaims();


            // Console.WriteLine( $"authorization output {authorization.checkPermissions(unregistered.Claims())}");
            IEnumerable<Claim> claims = new List<Claim>() {
                new Claim("Read Only","AutoBuild"),
            };
            Console.WriteLine($"authorization output {AuthorizationService.checkPermissions(claims)}");




        }
    }


}
