using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models
{
    // with this class is just for passing that data (READ ONLY) 
    // this is what a DTO is..
    public class UserInformationDTO
    {

        private UserAccount info;
        // constructor?

        public UserInformationDTO( UserAccount user)
        {
            this.info = user;
        }

        public string UserAccountInformation
        {
            get
            {
                // this will return fname lname and (email)
                return $"{info.FirstName} {info.LastName} ({info.UserEmail})";
            }
        }


    }
}
