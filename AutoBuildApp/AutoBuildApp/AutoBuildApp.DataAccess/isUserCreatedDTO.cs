using System;
using System.Collections.Generic;
using System.Text;

using AutoBuildApp.Models;


namespace AutoBuildApp.DataAccess
{
    public class isUserCreatedDTO
    {
        public static bool createUserinDB(UserAccount userA)
        {
            bool Flag = true;
            
            Flag = userAccountGateway.verifyAccountExists(userA);
            if(Flag == true)
            {
                userAccountGateway.createUserAccountinDB(userA);

            }
            return Flag;
        }
    }
}
