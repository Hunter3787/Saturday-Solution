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
            this.ScopeOfPermissions = "";

        }
        
        public Claims(string Claims, string Scope)
        {
            this.Permission = Claims;
            this.ScopeOfPermissions = Scope;
        }
        public string Permission { get; set; }
        public string ScopeOfPermissions { get; set; }
        public bool Equals(Claims Other)
        {
            if (Other is null)
            {
                return false;
            }
            return 
                this.Permission == Other.Permission 
                && this.ScopeOfPermissions == Other.ScopeOfPermissions;
        }

    }


}
