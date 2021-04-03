
using AutoBuildApp.Security.Models;
using System.Collections.Generic;


namespace AutoBuildApp.DataAccess.Entities
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
        public string UserEmail { get; set; }

        public AuthUserDTO()
        {
            this.UserEmail = "";
            this.Claims = new List<Claims>();

        }
        // the email to identify the user

    }
}
