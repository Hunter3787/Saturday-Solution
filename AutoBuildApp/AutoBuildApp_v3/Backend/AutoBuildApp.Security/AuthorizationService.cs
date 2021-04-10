
using AutoBuildApp.Security.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;


namespace AutoBuildApp.Security
{
    public static class AuthorizationService
    {
        private static ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
       
        public static void print()
        {
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

            ///handle the null values
            if (permissionsRequired is null)
            {
                return false;
            }
            Console.WriteLine($"THE CLAIMS IN THE CURRENT THREAD: ");
            Console.WriteLine($" " +
                   $"In the authorizatioin service");
            foreach (Claim c in _threadPrinciple.Claims)
            {
                Console.WriteLine($" " +
                    $"claim type: { c.Type } claim value: {c.Value} ");

            }

            Console.WriteLine($" " +
                   $"Permissions passed:");
            foreach (Claim c in _threadPrinciple.Claims)
            {
                Console.WriteLine($" " +
                    $"claim type: { c.Type } claim value: {c.Value} ");

            }

            bool outcome = Enumerable.SequenceEqual(_threadPrinciple.Claims, permissionsRequired, new MyCustomComparer());

            return outcome;


        }
        // nick: not a terrible idea :  make two different checks singular and 
        // FULL check


    }
    public class MyCustomComparer : IEqualityComparer<Claim>
    {
        public bool Equals(Claim x, Claim y)
        {
            if (x.Type == y.Type && x.Value == y.Value)
                return true;
            return false;
        }

        public int GetHashCode(Claim obj)
        {
            throw new NotImplementedException();
        }
    }

}
