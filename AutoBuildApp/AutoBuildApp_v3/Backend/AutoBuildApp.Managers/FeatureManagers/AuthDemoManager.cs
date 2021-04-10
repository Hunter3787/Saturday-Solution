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
        private ClaimsPrincipal _threadPrinciple;
        private ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
        IClaimsFactory unregistered;
        IClaimsFactory basic;
        public AuthDemoManager()
        {
            unregistered = claimsFactory.GetClaims(RoleEnumType.UNREGISTERED_ROLE);
            basic = claimsFactory.GetClaims(RoleEnumType.BASIC_ROLE);
        }


        public bool doWork()
        {

            return false;

        }


        public string getData()
        {

            _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            Console.WriteLine($"Within the AuthDemoManager get data:");
            Console.WriteLine($" wihtin the demo manager");
            string returnValue = "";
            foreach (var clm in _threadPrinciple.Claims)
            {
                returnValue += $" claim type: { clm.Type } claim value: {clm.Value} \n";
                // Console.WriteLine(returnValue);
            }

            Console.WriteLine($"End of  AuthDemoManager");


            // kk we are going to check if user authorized 

            AuthorizationService.print();
            string values = " ";


            if (AuthorizationService.checkPermissions(basic.Claims()))
            {

                values += $"" +
                    $"Here is the data you asked for" +
                    $"\n\tAuthorization output" +
                    $" {AuthorizationService.checkPermissions(basic.Claims())}";

            }
            else
            {
                values += "you are not authorized";
            }
            return values;
        }


    }
}
