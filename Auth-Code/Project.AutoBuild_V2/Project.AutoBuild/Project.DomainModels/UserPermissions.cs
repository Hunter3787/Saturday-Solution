using Project.Security.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Project.DomainModels
{
    class UserPermissions : IUserClaim, IEquatable<UserPermissions>
    {

        public UserPermissions(string claims, string scope)
        {
            this.Permission = claims;
            this.scopeOfPermissions = scope;
        }
        public string Permission { get; set; }
        public string scopeOfPermissions { get; set; }
        public bool Equals(UserPermissions other)
        {
            if (other is null)
            {
                return false;
            }


            return this.Permission == other.Permission && this.scopeOfPermissions == other.scopeOfPermissions;
        }



    }

}