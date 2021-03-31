
using System;


 // using BCrypt.Net-Next -> third party nugget package that supports our versioning 
//using BC = BCrypt.Net.BCrypt;


using System.Globalization; // this is for Iformamter in .public static DateTime ParseExact (string s, string format, IFormatProvider? provider);

namespace AutoBuildApp.Models
{
    /*
     *this is what we do to capture
     *each row to our table
     *UserName -> attribute in the database
     * 
     * 
     * this our data model
     * this is our business object 
     */
    public class Product
    {

        //there is more but this is for now.
        public int UserAccountID { get; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }

        public string passHash { get; set; }
        public string role { get; set; }

    }
}
