
using System;


 // using BCrypt.Net-Next -> third party nugget package that supports our versioning 
//using BC = BCrypt.Net.BCrypt;


using System.Globalization; // this is for Iformamter in .public static DateTime ParseExact (string s, string format, IFormatProvider? provider);

namespace AutoBuildApp.Models.Users
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

        public string passHash { get; set; }
        public string role { get; set; }

        public DateTime registrationDate { get; set; }
        public UserAccount()
        {
            this.UserName = "";
            this.FirstName = "";
            this.LastName = "";
            this.UserEmail = "";
            this.passHash = "";

            this.registrationDate = DateTime.MinValue;
            this.role = "";
        }

        // created my little constructor... TAKING IN THE BASICs
        public UserAccount( string username, string fname, string lname, string email, string role, string passHash, string regisDate)
        {
            this.UserName = username;
            this.FirstName = fname;
            this.LastName = lname;
            this.UserEmail = email;
            this.passHash = passHash;

            //The total length of the output that you will store in the database is always 60 bytes long from this Bcrypt hash just a note.
            //this.passHash = BC.HashPassword(passHash, BC.GenerateSalt());
            // BC.EnhancedHashPassword(passHash);
            
            //"yyyy-MM-dd HH:mm" :https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings?redirectedfrom=MSDN
            //                    http://blog.stevex.net/string-formatting-in-csharp/
            //CultureInfo.InvariantCulture : https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.invariantculture?view=net-5.0 
            this.registrationDate = DateTime.ParseExact(regisDate, "MM-dd-yyyy", CultureInfo.InvariantCulture,DateTimeStyles.None);

        
            this.role = role.ToUpper();

        }

        public string UserAccountInfo
        {
            get
            {
                // this will return fname lname and (email)
                return $"{FirstName} {LastName} ({UserEmail} {role}) {passHash}";
            }
        }




        
       




    }
}
