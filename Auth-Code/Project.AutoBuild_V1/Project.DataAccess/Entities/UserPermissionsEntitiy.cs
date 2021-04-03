using Project.Security.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DataAccess.Entities
{
    public class UserPermissionsEntitiy : IUserClaim
    {

        //there is more but this is for now.
        public long UserAccountID { get; set; }
        public string Permission { get; set; }
        public string scopeOfPermissions { get; set; }
     }

    public enum PermissionEnum
    {
        NONE,
        CREATE,
        UPDATE,
        DELETE,
        BLOCK
    }

}
