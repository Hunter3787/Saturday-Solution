using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DataAccess.Entities
{
    public class UserCredentials
    {
        public long UserAccountID { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public DateTime modifiedAt { get; set; }



        public UserCredentials(string uname, string pass)
        {
            this.Username = uname;
            this.Password = pass;
        }
    }
}
