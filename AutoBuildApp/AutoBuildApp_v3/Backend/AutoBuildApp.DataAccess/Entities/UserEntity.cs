using AutoBuildApp.DataAccess.Reflections;
using AutoBuildApp.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess.Entities
{/// <summary>
/// This class contains entity objects that will be 
/// transferred to the DAO for DB interactions
/// </summary>
    public class UserEntity
    {
        // Unique identifier of user
        public string UserID { get; set; }

        // The username of a registered user
        public string UserName { [DAO("username", typeof(string),false)] get; set; }

        // The email of a registered user
        public string Email { [DAO("email", typeof(string), false)] get; set; }

        // The first name of a registered user
        public string FirstName { [DAO("firstname", typeof(string), false)] get; set; }

        // The last name of a registered user
        public string LastName { [DAO("lastname", typeof(string), false)] get; set; }

        // When the user's account was created
        public string CreatedAt { [DAO("createdat", typeof(string), false)] get; set; }

        // When the user's account was last modified
        public string ModifiedAt { [DAO("modifiedat", typeof(string), false)] get; set; }

        // Who last modified the user's account
        public string ModifiedBy { [DAO("modifiedby", typeof(string), false)] get; set; }
    }
}
