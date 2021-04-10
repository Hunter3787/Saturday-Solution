
using AutoBuildApp.Security.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;


namespace AutoBuildApp.Security
{
    public static class AuthorizationService
    {
        public static void print()
        {

            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            Console.WriteLine($"THE CLAIMS IN THE CURRENT THREAD: ");
            Console.WriteLine($" " +
                   $"In the authorizatioin service");
            foreach (Claim c in _threadPrinciple.Claims)
            {
                Console.WriteLine($" " +
                    $"claim type: { c.Type } claim value: {c.Value} ");

            }
        }

        /// <summary>
        /// check ther permissions passed to that wihtin the principle object 
        /// </summary>
        /// <returns></returns>
        public static bool checkPermissions(IEnumerable<Claim> permissionsRequired) // PASS INTO WHY STORE IT???????
        {
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            ///handle the null values
            if (permissionsRequired is null)
            {
                return false;
            }

            Console.WriteLine($" " +
                   $"In the authorizatioin service");
            Console.WriteLine($"The claims in the thead in AuthorizationService:");
            foreach (Claim c in _threadPrinciple.Claims)
            {
                Console.WriteLine($" " +
                    $"claim type: { c.Type } claim value: {c.Value} ");

            }

            Console.WriteLine($" " +
                   $"Permissions passed in AuthorizationService:");
            foreach (Claim c in _threadPrinciple.Claims)
            {
                Console.WriteLine($" " +
                    $"claim type: { c.Type } claim value: {c.Value} ");

            }

            bool outcome = Enumerable.SequenceEqual(_threadPrinciple.Claims, permissionsRequired, new MyCustomComparer());
            Console.WriteLine($" " +
                   $"The outcome: : { outcome}");
            outcome = _threadPrinciple.Claims.SequenceEqual( permissionsRequired, new MyCustomComparer());
            Console.WriteLine($" " +
                   $"The outcome2: : { outcome}");
            return outcome;
        }
        // nick: not a terrible idea :  make two different checks singular and 
        // FULL check


    }
    /// <summary>
    /// whaving trouble with the Ienum:
    /// https://dotnetcodr.com/2017/09/06/determine-if-two-sequences-are-equal-with-linq-c-2/
    /// 
    /// </summary>
    public class MyCustomComparer : IEqualityComparer<Claim>
    {
        public bool Equals(Claim x, Claim y)
        {
            return (x.Type == y.Type && x.Value == y.Value);
        }

        public int GetHashCode(Claim obj)
        {
            return obj.ToString().GetHashCode();
        }
    }

}
