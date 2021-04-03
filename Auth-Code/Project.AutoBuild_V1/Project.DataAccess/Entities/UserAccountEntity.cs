using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DataAccess.Entities
{
    public class UserAccountEntity
    {

        //there is more but this is for now.
        public long UserAccountID { get; }
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset createdAt { get; set; }
        public DateTimeOffset modifiedAt { get; set; }
        // public DateTimeOffset modifiedBy { get; set; }


        public UserAccountEntity()
        {

            this.FirstName = "";
            this.LastName = "";
            this.UserEmail = " ";
            this.createdAt = DateTimeOffset.MinValue.UtcDateTime;
            this.modifiedAt = DateTimeOffset.MinValue;

        }

        // created my little constructor... TAKING IN THE BASICs
        public UserAccountEntity(string username, string fname, string lname, string email, string regisDate)
        {

            this.FirstName = fname;
            this.LastName = lname;
            this.UserEmail = email;

        }

        public string UserAccountInfo
        {
            get
            {
                // this will return fname lname and (email)
                return $"{FirstName} {LastName} ({UserEmail}) ";
            }
        }



    }
}
