using System;
using System.Collections.Generic;


namespace AutoBuildApp.Security.Models
{

    /// <summary>
    /// defineds the claims set a user can have which is the permission and the scope of 
    /// that permission
    /// </summary>
    public class Claims : IUserClaim, IEquatable<Claims>
    {
        public Claims()
        {
            this.Permission = "";
            this.scopeOfPermissions = "";

        }
        
        public Claims(string claims, string scope)
        {
            this.Permission = claims;
            this.scopeOfPermissions = scope;
        }
        public string Permission { get; set; }
        public string scopeOfPermissions { get; set; }
        public bool Equals(Claims other)
        {
            if (other is null)
            {
                return false;
            }
            return 
                this.Permission == other.Permission 
                && this.scopeOfPermissions == other.scopeOfPermissions;
        }

    }


}
