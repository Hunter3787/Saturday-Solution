using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace AutoBuildApp.Security.FactoryModels
{

    /// <summary>
    /// This is a class which implements the Product interface.
    /// </summary>
    public class Basic : IClaims
    {
        public IEnumerable<Claim> Claims()
        {

            IEnumerable<Claim> basicUserClaims = new List<Claim>()
            {
                  new Claim( PermissionEnumType.ReadOnly  , ScopeEnumType.AutoBuild),
                  new Claim( PermissionEnumType.Delete     , ScopeEnumType.Self),
                  new Claim( PermissionEnumType.Update     , ScopeEnumType.Self),
                  new Claim( PermissionEnumType.Edit       , ScopeEnumType.Self),
                  new Claim( PermissionEnumType.Create     , ScopeEnumType.Reviews),
                  new Claim( PermissionEnumType.Delete     , ScopeEnumType.SelfReviews),
                  new Claim( PermissionEnumType.Update     , ScopeEnumType.SelfReviews),
            };


            return basicUserClaims;
        }
        public void PrintClaims()
        {
            foreach (Claim c in Claims())
            {
                Console.WriteLine($" claim type: { c.Type } claim value: {c.Value} ");

            }
        }

    }
    /// <summary>
    /// This is a class which implements the Product interface.
    /// </summary>
    public class SysAdmin : IClaims
    {
        public IEnumerable<Claim> Claims()
        {
            IEnumerable<Claim> adminUserClaims = new List<Claim>()
            {
                new Claim(PermissionEnumType.All , ScopeEnumType.All),
            };

            return adminUserClaims;
        }
        public void PrintClaims()
        {
            foreach (Claim c in Claims())
            {
                Console.WriteLine($" claim type: { c.Type } claim value: {c.Value} ");

            }
        }
    }

    public class DelAdmin : IClaims
    {
        public IEnumerable<Claim> Claims()
        {
            IEnumerable<Claim> adminUserClaims = new List<Claim>()
            {
                new Claim(PermissionEnumType.All , ScopeEnumType.All),
            };

            return adminUserClaims;
        }
        public void PrintClaims()
        {
            foreach (Claim c in Claims())
            {
                Console.WriteLine($" claim type: { c.Type } claim value: {c.Value} ");

            }
        }

    }


    /// <summary>
    /// This is a class which implements the Product interface.
    /// </summary>
    public class Vendor : IClaims
    {
        public IEnumerable<Claim> Claims()
        {
            IEnumerable<Claim> vendorUserClaims = new List<Claim>()
            {
                new Claim( PermissionEnumType.ReadOnly  , ScopeEnumType.AutoBuild ),
                new Claim( PermissionEnumType.Delete     , ScopeEnumType.Self),
                new Claim( PermissionEnumType.Update     , ScopeEnumType.Self),
                new Claim( PermissionEnumType.Edit       , ScopeEnumType.Self),
                new Claim( PermissionEnumType.Create     , ScopeEnumType.Reviews),
                new Claim( PermissionEnumType.Delete     , ScopeEnumType.SelfReviews),
                new Claim( PermissionEnumType.Update     , ScopeEnumType.SelfReviews),
                new Claim( PermissionEnumType.Create     , ScopeEnumType.Products),
                new Claim( PermissionEnumType.Update     , ScopeEnumType.VendorProducts),
                new Claim( PermissionEnumType.Delete     , ScopeEnumType.VendorProducts),
            };

            return vendorUserClaims;
        }
        public void PrintClaims()
        {
            foreach (Claim c in Claims())
            {
                Console.WriteLine($" claim type: { c.Type } claim value: {c.Value} ");

            }
        }

    }


    /// <summary>
    /// This is a class which implements the Product interface.
    /// </summary>
    public class Unregistered : IClaims
    {

        public IEnumerable<Claim> Claims()
        {
            IEnumerable<Claim> UnregisteredUserClaims = new List<Claim>()
            {
                new Claim( PermissionEnumType.ReadOnly,ScopeEnumType.AutoBuild),
                new Claim( PermissionEnumType.Create,ScopeEnumType.Registration)
            };

            return UnregisteredUserClaims;
        }
        public void PrintClaims()
        {
            foreach (Claim c in Claims())
            {
                Console.WriteLine($" claim type: { c.Type } claim value: {c.Value} ");

            }
        }

    }

    public class Locked : IClaims
    {
        public IEnumerable<Claim> Claims()
        {
            IEnumerable<Claim> LockedUserClaims = new List<Claim>()
            {
                new Claim( PermissionEnumType.ReadOnly,ScopeEnumType.AutoBuild),
                new Claim( PermissionEnumType.Block,ScopeEnumType.Registration),
                new Claim( PermissionEnumType.Block,ScopeEnumType.Login)
            };

            return LockedUserClaims;
        }
        public void PrintClaims()
        {
            foreach (Claim c in Claims())
            {
                Console.WriteLine($" claim type: { c.Type } claim value: {c.Value} ");

            }
        }
    }

}
