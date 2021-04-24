using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBuildApp.Api.HelperFunctions
{
    public static class Guest
    {

        public static ClaimsPrincipal DefaultClaimsPrinciple()
        {



            // setting a default principle object t=for the thread.
            #region Instantiating the Claims principle


            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaims unregistered = claimsFactory.GetClaims(RoleEnumType.UNREGISTERED_ROLE);
            /// NOTE: passed in the claims only to the claimsIdentity and not userIdentity 
            /// as "AutoBuild User" since that will trigger the read only value of is authenticated to 
            /// be True, when in fact the user is not
            /// authenticated.
            /// 

            //UserIdentity guestUser = new UserIdentity();
            // https://leastprivilege.com/2012/09/24/claimsidentity-isauthenticated-and-authenticationtype-in-net-4-5/ 
            ClaimsIdentity identity = new 
                ClaimsIdentity(unregistered.Claims());
            ClaimsPrincipal _principal = new ClaimsPrincipal(identity);
             //some printing
            Console.WriteLine($"IN THE GUEST");
            foreach (Claim c in _principal.Claims)
            {
                Console.WriteLine($"Permission:  {c.Type}, Scope: {c.Value} ");
            }
            
            #endregion

            #region NOT NECESSARY BUT WILL KEEP FOR NOW:
            /*
            UserIdentity guestUser = new UserIdentity();
            Console.WriteLine($" {guestUser.ToString()}");

            ClaimsIdentity guestIdentity =
                new ClaimsIdentity(unregistered.Claims()
                , authenticationType: guestUser.AuthenticationType
                , nameType: guestUser.Name, " ");

            Console.WriteLine($"Setting the default guest Principle: \n" +
                $" the Default user Identity: {guestUser.ToString() }\n " +
                $" the default claims: ");
            */

            #endregion

            return _principal; // returning the claimsPrinciple to be stored by the caller to the thread.

        }

    }
}
