using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Security
{
    public static class AuthorizationCheck
    {

        private static ClaimsFactory _claimsFactory = new ConcreteClaimsFactory();

        /// <summary>
        /// method to check permissions needed per authorization service.
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthorized(List<string> AllowedRoles)
        {
            foreach (string role in AllowedRoles)
            {
                IClaims claims = _claimsFactory.GetClaims(role);
                // FIRST LINE OF DEFENCE 

                if (AuthorizationService.CheckPermissions(claims.Claims()))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
