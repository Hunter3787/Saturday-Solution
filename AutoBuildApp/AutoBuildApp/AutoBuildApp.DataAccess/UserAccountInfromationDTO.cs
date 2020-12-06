using AutoBuildApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class UserAccountInfromationDTO
    {
        public static string DisplayInfoOnUser(UserAccount userA)
        {
            return userAccountGateway.retrieveAccountInformation(userA);

        }
    }
}
