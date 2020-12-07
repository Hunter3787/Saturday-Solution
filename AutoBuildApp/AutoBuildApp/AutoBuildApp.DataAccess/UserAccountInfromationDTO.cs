using AutoBuildApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class UserAccountInfromationDTO
    {
        private userAccountGateway userGateway;
        // establishing the connection which is passed to the gateway
        public UserAccountInfromationDTO(String connection)
        {
            userGateway = new userAccountGateway(connection);
        }
        // gatewat is reponsible for querying that informationa and returing it here:
        public string DisplayInfoOnUser(UserAccount userA)
        {
            return userGateway.retrieveAccountInformation(userA); // userAccountGateway.retrieveAccountInformation(userA);

        }
    }
}
