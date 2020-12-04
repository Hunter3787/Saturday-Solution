
using System;

namespace AutoBuildApp.Models
{
    /*
     *this is what we do to capture
     *each row to our table
     *UserName -> attribute in the database
     * 
     * 
     * this our data model
     * this is our bussiness object 
     */
    public class UserAccount
    {

        //there is more but this is for now.
        public int UserAccountID { get; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }

        public string role { get; set; }

        public UserAccount()
        {
            this.UserName = "";
            this.FirstName = "";
            this.LastName = "";
            this.role = "";
        }

        // created my little constructor... TAKING IN THE BASICs
        public UserAccount( string username, string fname, string lname, string email, string role)
        {
            this.UserName = username;
            this.FirstName = fname;
            this.LastName = lname;
            this.UserEmail = email;
            this.role = role.ToUpper();


        }

        public string UserAccountInfo
        {
            get
            {
                // this will return fname lname and (email)
                return $"{FirstName} {LastName} ({UserEmail} {role})";
            }
        }




        
       




    }
}
