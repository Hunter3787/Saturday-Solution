
using AutoBuildApp.Security.Models;
using System.Collections.Generic;


namespace AutoBuildApp.Models.Entities
{
    /// <summary>
    /// the AuthDTO is responsible for sending the data to the authentication service 
    /// about the user encompassing  user email and set of permissions
    /// </summary>
    public class AuthUserDTO
    {
        // the list of credentials 
        public IList<Claims> Claims;
        // the users Email to identify the user.
        public string UserName { get; set; }

        public AuthUserDTO()
        {
            this.UserName = "";
            this.Claims = new List<Claims>();

        }
        // the email to identify the user
        public override string ToString()
        {
            string retVal = " ";
            retVal += UserName;
            foreach (Claims claims in Claims)
            {
                retVal += $" " +
                    $"userPermissions: {claims.Permission } " +
                    $"scope { claims.scopeOfPermissions}";
            }


            return retVal;
        }

    }
}
