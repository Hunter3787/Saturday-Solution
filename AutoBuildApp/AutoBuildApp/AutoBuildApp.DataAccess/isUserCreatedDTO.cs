using System;
using System.Collections.Generic;
using System.Text;

using AutoBuildApp.Models;


namespace AutoBuildApp.DataAccess
{
    public class isUserCreatedDTO
    {
        private userAccountGateway userGateway;

        public isUserCreatedDTO(String connection)
        {
            userGateway = new userAccountGateway(connection);
        }
        public bool createUserinDB(UserAccount userA)
        {
            bool Flag = true;

            Flag = this.userGateway.verifyAccountExists(userA);
            if (Flag == true) 
            { 
                Console.WriteLine("user exists");
            }
            else 
            { 
                Console.WriteLine("user does not exist so creating..."); 
                userGateway.createUserAccountinDB(userA); 
            }
            return Flag;
        }
    }
}
