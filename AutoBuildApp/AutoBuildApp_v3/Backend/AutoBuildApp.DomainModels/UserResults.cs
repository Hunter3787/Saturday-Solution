using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DomainModels
{
    /// <summary>
    /// This class will be a list of all registered users
    /// Only Admins can request this list
    /// </summary>
    public class UserResults
    {    
        // Unique identifier of user
        public string UserID { get; set; }

        // The username of a registered user
        public string UserName { get; set; }

        // The email of a registered user
        public string Email { get; set; }

        // The first name of a registered user
        public string FirstName { get; set; }

        // The last name of a registered user
        public string LastName { get; set; }

        // When the user's account was created
        public string CreatedAt { get; set; }

        // When the user's account was last modified
        public string ModifiedAt { get; set; }

        // Who last modified the user's account
        //public string ModifiedBy { get; set; }

        // The role of the user
        public string UserRole { get; set; }

    }
}
