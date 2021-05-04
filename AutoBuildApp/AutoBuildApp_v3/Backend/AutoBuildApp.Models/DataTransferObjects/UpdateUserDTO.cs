using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.DTO
{
    public class UpdateUserDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
        public string role { get; set; }

        public UpdateUserDTO(string username, string firstName, string lastName, string email, string role)
        {
            UserName = username;
            FirstName = firstName;
            LastName = lastName;
            UserEmail = email;
            this.role = role;
        }
    }
}
