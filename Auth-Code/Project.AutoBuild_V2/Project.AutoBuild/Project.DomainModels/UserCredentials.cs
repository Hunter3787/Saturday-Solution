using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DomainModels
{
    /// <summary>
    /// this class models the Credentials of the user
    /// that is passed 
    /// </summary>
    class UserCredentials
    {
        public long UserAccountID { get; set; }
        public string Username { get; set; } // the username
        public string Password { get; set; } // the password
        public DateTime modifiedAt { get; set; } // the dataModified
        public UserCredentials(string uname, string pass)
        {
            this.Username = uname;
            this.Password = pass;
        }


    }
}
