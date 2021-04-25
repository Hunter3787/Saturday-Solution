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
                  new Claim( PermissionEnumType.READ_ONLY  , ScopeEnumType.AUTOBUILD),
                  new Claim( PermissionEnumType.DELETE     , ScopeEnumType.SELF),
                  new Claim( PermissionEnumType.UPDATE     , ScopeEnumType.SELF),
                  new Claim( PermissionEnumType.EDIT       , ScopeEnumType.SELF),
                  new Claim( PermissionEnumType.CREATE     , ScopeEnumType.REVIEWS),
                  new Claim( PermissionEnumType.DELETE     , ScopeEnumType.SELF_REVIEWS),
                  new Claim( PermissionEnumType.UPDATE     , ScopeEnumType.SELF_REVIEWS),
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
    public class Admin : IClaims
    {
        public IEnumerable<Claim> Claims()
        {
            IEnumerable<Claim> adminUserClaims = new List<Claim>()
            {
                new Claim(PermissionEnumType.ALL , ScopeEnumType.ALL),
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
                new Claim( PermissionEnumType.READ_ONLY  , ScopeEnumType.AUTOBUILD ),
                new Claim( PermissionEnumType.DELETE     , ScopeEnumType.SELF),
                new Claim( PermissionEnumType.UPDATE     , ScopeEnumType.SELF),
                new Claim( PermissionEnumType.EDIT       , ScopeEnumType.SELF),
                new Claim( PermissionEnumType.CREATE     , ScopeEnumType.REVIEWS),
                new Claim( PermissionEnumType.DELETE     , ScopeEnumType.SELF_REVIEWS),
                new Claim( PermissionEnumType.UPDATE     , ScopeEnumType.SELF_REVIEWS),
                new Claim( PermissionEnumType.CREATE     , ScopeEnumType.PRODUCTS),
                new Claim( PermissionEnumType.UPDATE     , ScopeEnumType.VENDOR_PRODUCTS),
                new Claim( PermissionEnumType.DELETE     , ScopeEnumType.VENDOR_PRODUCTS),
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
                new Claim( PermissionEnumType.READ_ONLY,ScopeEnumType.AUTOBUILD),
                new Claim( PermissionEnumType.CREATE,ScopeEnumType.REGISTRATION)
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
                new Claim( PermissionEnumType.READ_ONLY,ScopeEnumType.AUTOBUILD),
                new Claim( PermissionEnumType.BLOCK,ScopeEnumType.REGISTRATION),
                new Claim( PermissionEnumType.BLOCK,ScopeEnumType.LOGIN)
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
