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
        public static bool IsAuthorized(List<string> _allowedRoles)
        {
            foreach (string role in _allowedRoles)
            {
                IClaims _claims = _claimsFactory.GetClaims(role);
                // FIRST LINE OF DEFENCE 

                if (AuthorizationService.CheckPermissions(_claims.Claims()))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
