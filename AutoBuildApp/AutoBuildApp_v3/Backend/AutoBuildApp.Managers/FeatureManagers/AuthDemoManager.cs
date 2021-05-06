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
        private ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
        IClaims unregistered;
        IClaims basic;
        public AuthDemoManager()
        {
            unregistered = claimsFactory.GetClaims(RoleEnumType.UnregisteredRole);
            basic = claimsFactory.GetClaims(RoleEnumType.BasicRole);

        }

        public string getData()
        {
            string values = "";

            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            Console.WriteLine($"\n  AuthDemoManager" +
                $"\nThe username:\n {_threadPrinciple.Identity.Name}");
            Console.WriteLine("\nprinciple claims: \n");
            foreach (var clm in _threadPrinciple.Claims)
            {
                Console.WriteLine($" claim type: { clm.Type } claim value: {clm.Value} \n");
            }

            if (AuthorizationService.CheckPermissions(basic.Claims()))
            {

                values += $"" +
                    $"Here is the data you asked for" +
                    $"\n\tAuthorization output" +
                    $" {AuthorizationService.CheckPermissions(basic.Claims())}";

            }
            else
            {
                values += "you are not authorized";
            }
            return values;
        }


        // follow naming convention -> clean up. 
        public string LogOut()
        {


            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            Console.WriteLine($"\n  AuthDemoManager" +
             $"\nThe username:\n {_threadPrinciple.Identity.Name}");
            Console.WriteLine("\nprinciple claims: \n");
            foreach (var clm in _threadPrinciple.Claims)
            {
                Console.WriteLine($" claim type: { clm.Type } claim value: {clm.Value} \n");
            }


            Console.WriteLine($"THESE ARE YOUR UPDATED PERMISSIONS:");
                // setting a default principle object t=for the thread.
                #region Instantiating the Claims principle
                ClaimsIdentity identity = new
                ClaimsIdentity(unregistered.Claims());
                _threadPrinciple= new ClaimsPrincipal(identity);
                foreach (var clm in _threadPrinciple.Claims)
                {
                Console.WriteLine($" claim type: { clm.Type } claim value: {clm.Value} \n");
                    // Console.WriteLine(returnValue);
                }

                Console.WriteLine($" Is authenticated? {_threadPrinciple.Identity.IsAuthenticated  } .");
                Thread.CurrentPrincipal = _threadPrinciple;
                #endregion

            return " " ; 

        }

    }
}
