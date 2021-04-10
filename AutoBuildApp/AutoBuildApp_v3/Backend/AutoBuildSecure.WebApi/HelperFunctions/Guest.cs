using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBuildSecure.WebApi.HelperFunctions
{
    public static class Guest
    {

        public static ClaimsPrincipal DefaultClaimsPrinciple()
        {



            // setting a default principle object t=for the thread.
            #region Instantiating the Claims principle

         

            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaimsFactory unregistered = claimsFactory.GetClaims(RoleEnumType.UNREGISTERED_ROLE);
            ClaimsIdentity identity = new ClaimsIdentity(unregistered.Claims());
            ClaimsPrincipal _principal = new ClaimsPrincipal(identity);
            Console.WriteLine($"iN THE GUEST");
            foreach (Claim c in _principal.Claims)
            {
                Console.WriteLine($"Permission:  {c.Type}, Scope: {c.Value} ");
            }

            // set it to th current thread
            //Thread.CurrentPrincipal = _principal;
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

            return _principal;

        }

    }
}
