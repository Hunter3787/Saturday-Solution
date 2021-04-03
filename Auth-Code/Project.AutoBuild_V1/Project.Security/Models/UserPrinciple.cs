using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Project.Security.Models
{
    //Principal Object A Principal object is a holder 
    //for all the roles the user belongs to(according to the active authentication mechanism).
    public class UserPrinciple : IPrincipal
    {

        public UserPrinciple(IIdentity identity)
        {
            // the user account essentially
            Identity = identity;
        }
        /// <summary>
        /// an Identity property that is there
        /// in order to implement the IPrincipal interface
        /// </summary>
        public IIdentity Identity { get; }

        /// <summary>
        /// a single ClaimsPrincipal can consist of multiple Identities. 
        /// /// <summary>
        /// Claims which consists of all 
        /// the claims associated with an identity. 
        /// </summary>
        /// </summary>
        public IEnumerable<IUserClaim> Permissions { get; }

        /// <summary>
        ///  IsInRole will be generally unneeded 
        ///  if you adhere to the 
        ///  claims-based authentication emphasised
        ///  in ASP.NET Core
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
