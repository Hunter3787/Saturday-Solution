
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
        public static void Print(IEnumerable<Claim> Y, IEnumerable<Claim> X)
        {

            #region PRINTING CHECK

            Console.WriteLine($"\n" +
                   $"In the authorizatioin service");
            Console.WriteLine($"The claims in the thread in AuthorizationService:");
            Console.WriteLine($"x");
            foreach (Claim c in X)
            {
                Console.WriteLine($" " +
                    $"claim type: { c.Type } claim value: {c.Value} ");

            }
            Console.WriteLine($"\n" +
                  $"Permissions passed in AuthorizationService:");
            Console.WriteLine($" " +
                   $"y:");
            foreach (Claim c in Y)
            {
                Console.WriteLine($" " +
                    $"claim type: { c.Type } claim value: {c.Value} ");

            }

            #endregion

        }

        /// <summary>
        /// check ther permissions passed to that wihtin the principle object 
        /// </summary>
        /// <returns></returns>
        public static bool CheckPermissions(IEnumerable<Claim> PermissionsRequired) // PASS INTO WHY STORE IT???????
        {
            ClaimsPrincipal threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            ///handle the null values
            if (PermissionsRequired is null) {

                //Console.WriteLine($"in THE AUTHORIZATION SERVICE RETURNING FALSE");
                return false; }

            /// how about ordering the claims first:
            /// http://csharphelper.com/blog/2018/04/determine-whether-two-lists-contain-the-same-sequences-of-objects-in-different-orders-in-c/

            //Console.WriteLine($"\nthe identitiy _name is Authorizatioin service: " + $"{threadPrinciple.Identity.Name}\n");

            var x =
                from Claim item in threadPrinciple.Claims
                    //where item.Type != "USERNAME"
                where item.Value != threadPrinciple.Identity.Name
                // added per email in claims - connor
                && item.Type != ClaimTypes.Email
                orderby item.Type
                select item;
            var y = from Claim item in PermissionsRequired
                    orderby item.Type
                    select item;


            ///http://csharphelper.com/blog/2018/04/determine-whether-two-lists-contain-the-same-sequences-of-objects-in-different-orders-in-c/
            ///
            bool outcome = Enumerable.SequenceEqual(x, y, new MyCustomComparer());

            //Console.WriteLine($"in THE AUTHORIZATION SERVICE RETURNING OUTCOME {outcome}");
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
        public bool Equals(Claim X, Claim Y)
        {
            return (X.Type == Y.Type && X.Value == Y.Value);
        }
        public int GetHashCode(Claim Obj)
        {
            throw new NotImplementedException();
            //return obj.ToString().GetHashCode();
        }
    }

}
