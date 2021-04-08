using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace AutoBuildApp.Managers.FeatureManagers
{
    /// <summary>
    /// this class is resposible for 
    /// demoing th eauthorization service 
    /// to retrict access to unauthorized users
    /// </summary>
    public class AuthDemoManager
    {
        private ClaimsPrincipal _principal;
        private ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
        IClaimsFactory unregistered;
        public AuthDemoManager()
        {
            _principal = (ClaimsPrincipal)Thread.CurrentPrincipal;
            Console.WriteLine($" wihtin the demo manager");
            foreach (var clm in _principal.Claims)
            {
                Console.WriteLine($" claim type: { clm.Type } claim value: {clm.Value} \n");
            }

            unregistered = claimsFactory.GetClaims(RoleEnumType.UNREGISTERED_ROLE);

        }


        public bool doWork()
        {

            return false;

        }


        public string getData()
        {

            Console.WriteLine($"Within the AuthDemoManager:");
            string returnValue = "";
            foreach (var clm in _principal.Claims)
            {
                returnValue += $" claim type: { clm.Type } claim value: {clm.Value} \n";
                // Console.WriteLine(returnValue);
            }

            Console.WriteLine($"End of  AuthDemoManager");


            // kk we are going to check if user authorized 

            AuthorizationService.print();
            string values = " ";


            if (AuthorizationService.checkPermissions(unregistered.Claims()))
            {

                values += $"here is the data you asked for" +
                    $"\n\tAuthorization output" +
                    $" {AuthorizationService.checkPermissions(unregistered.Claims())}";

            }
            else
            {
                values += "you are not authorized";
            }
            return values;
        }


    }
}
