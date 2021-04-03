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

    /// <summary>
    /// the claims per role class set 
    /// predefined claims per Roles: admin, basic, unregistered, vendor and developer
    /// to allow consistency for the set of claims defined per user
    /// </summary>
    public sealed class ClaimsPerRoles : IEquatable<ClaimsPerRoles>
    {
        public static readonly IList<Claims>
            claimsPerAdmin = new List<Claims>()
            {
            new Claims("all","all"),
            };
        public static readonly IList<Claims> 
            claimsPerVendor = new List<Claims>()
            {
            new Claims("Read Only","AutoBuild"),
                 new Claims("Delete","self"),
                 new Claims("Update","self"),
                 new Claims("Edit","self"),
                 new Claims("Create" ,"reviews"),
                 new Claims("Delete" ,"selfReview"),
                 new Claims("Update" ,"selfReview"),
                 new Claims("Create","Products"),
                new Claims("Update","VendorProducts"),
                new Claims("Delete","VendorProductsOnly"),
                new Claims("Update","VendorProducts")

            };
        public static readonly IList<Claims> 
            claimsPerBasic = new List<Claims>()
            {
                new Claims("Read Only","AutoBuild"),
                 new Claims("Delete","self"),
                 new Claims("Update","self"),
                 new Claims("Edit","self"),
                 new Claims("Create" ,"reviews"),
                 new Claims("Delete" ,"selfReview"),
                 new Claims("Update" ,"selfReview")
            };
        public static readonly IList<Claims> 
            claimsPerUnregistered = new List<Claims>()
            {
                new Claims("Read Only","AutoBuild")
            };
        public static readonly IList<Claims> 
            claimsPerDevolper = new List<Claims>()
            {
                new Claims("Read Only","AutoBuild")
            };

        /// <summary>
        /// here I define static claims per roles 
        /// this makes it easier
        /// </summary>
        public static readonly ClaimsPerRoles AdminUserClaims
            = new ClaimsPerRoles(claimsPerAdmin);
        public static readonly ClaimsPerRoles BasicUserClaims
            = new ClaimsPerRoles(claimsPerBasic);
        public static readonly ClaimsPerRoles VendorUserClaims
            = new ClaimsPerRoles(claimsPerVendor);
        public static readonly ClaimsPerRoles UnregisteredUserClaims
            = new ClaimsPerRoles(claimsPerUnregistered);
        public IEnumerable<Claims> Permissions { get; set; }

        private ClaimsPerRoles(IEnumerable<Claims> PermissionsPassed)
        {
            this.Permissions = PermissionsPassed;
        }

        /// <summary>
        /// the method is inherited from the IEquatable interface
        /// whcih returns true if two ClaimsPerRoles objects are equal and 
        /// false otherwise.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ClaimsPerRoles other)
        {
            if (other is null)
            {
                return false;
            }
            if(claimsPerAdmin.Equals(other.Permissions) || claimsPerBasic.Equals(other.Permissions) ||
                claimsPerDevolper.Equals(other.Permissions) || claimsPerUnregistered.Equals(other.Permissions) ||
                claimsPerVendor.Equals(other.Permissions)){

                return true;
            }
            return false;
        }
    }

}
