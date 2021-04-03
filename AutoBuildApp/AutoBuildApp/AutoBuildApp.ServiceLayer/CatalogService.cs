using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using System;

namespace AutoBuildApp.ServiceLayer
{
    public class CatalogService
    {
        private CatalogGateway gateway;

        public CatalogService(string connectionString)
        {
            gateway = new CatalogGateway(connectionString);
        }
        public virtual String LoginUser(UserAccount user)
        {
            return gateway.LoginUser(user);
        }

        public String RegisterUser(UserAccount user)
        {
            return gateway.RegisterUser(user);
        }
    }
}
