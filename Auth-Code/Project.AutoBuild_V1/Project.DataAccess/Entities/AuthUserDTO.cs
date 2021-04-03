using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DataAccess.Entities
{
    public class AuthUserDTO
    {
        // the list of credentials 
        public IList<UserPermissionsEntitiy> Claims;

        public AuthUserDTO()
        {
            this.UserEmail = "";
            this.Claims = new List<UserPermissionsEntitiy>();

        }
        // the email to identify the user
        public string UserEmail { get; set; }

    }
}
