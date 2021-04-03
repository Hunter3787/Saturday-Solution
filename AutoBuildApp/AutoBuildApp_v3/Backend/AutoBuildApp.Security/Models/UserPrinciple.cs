using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;


namespace AutoBuildApp.Security.Models
{
    //Principal Object A Principal object is a holder 
    //for all the roles the user belongs to(according to the active authentication mechanism).
    public class UserPrinciple : IPrincipal
    {
        private UserIdentity defaultIdentity;
        public UserPrinciple()
        {
            defaultIdentity = new UserIdentity()
            {
                AuthenticationType = "Autobuild Authentication Bearer",
                IsAuthenticated = false,
                Name = "UNREGISTERED"
            };
            IList<Claims> DefaultClaims = new List<Claims>()
            {
                new Claims("Read Only", "AutoBuild")
            };
            this.Permissions = DefaultClaims;
            this.myIdentity = defaultIdentity;
        }



        public UserPrinciple(UserIdentity identity)
        {
            // the user account essentially
            myIdentity = identity;
        }
        /// <summary>
        /// an Identity property that is there
        /// in order to implement the IPrincipal interface
        /// </summary>
        public UserIdentity myIdentity { get; set; }

        /// <summary>
        /// a single ClaimsPrincipal can consist of multiple Identities. 
        /// /// <summary>
        /// Claims which consists of all 
        /// the claims associated with an identity. 
        /// </summary>
        /// </summary>
        public IEnumerable<Claims> Permissions { get; set; }

        IIdentity IPrincipal.Identity { get { return this.myIdentity; } }

        /// <summary>
        ///  IsInRole will be generally unneeded 
        ///  if you adhere to the 
        ///  claims-based authentication emphasised
        ///  in ASP.NET Core
        ///  NOT BEING UTILIZED BUT PART 
        ///  OF THE INTERFACE
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            return false;
        }


        public string getPermissions()
        {
            string ret = "";
            foreach( Claims claims in this.Permissions)
            {
                ret += $" {claims.Permission}, {claims.scopeOfPermissions }\n";

            }
            return ret;
        }


        public override string ToString()
        {
            string ret = "";
            foreach (Claims claims in this.Permissions)
            {
                ret += $" {claims.Permission}, {claims.scopeOfPermissions } \n";

            }
            return ret;
        }

    }
}
