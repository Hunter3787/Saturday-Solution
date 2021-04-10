
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using AutoBuildApp.Security.Models;


namespace AutoBuildApp.Security
{
    public static class AuthorizationService
    {
        private static ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;


        public static void print()
        {
            Console.WriteLine($"THE CLAIMS IN THE CURRENT THREAD: ");

            foreach (Claim c in _threadPrinciple.Claims)
            {
                Console.WriteLine($" " +
                    $"In the authorizatioin service\n" +
                    $"claim type: { c.Type } claim value: {c.Value} ");

            }
        }

        /// <summary>
        /// check ther permissions passed to that wihtin the principle object 
        /// </summary>
        /// <returns></returns>
        public static bool checkPermissions(IEnumerable<Claim> permissionsRequired) // PASS INTO WHY STORE IT???????
        {
            ///handle the null values
            if(permissionsRequired is null)
            {
                return false; 
            }

            _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;

            foreach (Claim claims in _threadPrinciple.Claims)
            {
                foreach (Claim claimNeeded in permissionsRequired)
                {
                    /// FIX THISSS ITS DOING EQUALS WRONG
                    /// nicks opinion:  if defined as role based -> best to have matched
                    /// but easier to have a series of permissions 
                    if (!(claims.Type.Equals(claimNeeded.Type)
                        && claims.Value.Equals(claimNeeded.Value)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // nick: not a terrible idea :  make two different checks singular and 
        // FULL check


    }
}
