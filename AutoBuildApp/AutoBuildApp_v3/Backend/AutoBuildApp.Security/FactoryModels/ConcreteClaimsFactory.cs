using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.Interfaces;
using System;

namespace AutoBuildApp.Security.FactoryModels
{


    /// <summary>
    /// This is an abstract class and declares 
    /// the factory method, which returns an
    /// object of type Product.
    /// </summary>
    public abstract class ClaimsFactory
    {

        public abstract IClaims GetClaims(string type);

        public IClaims GetClaims(object sYSTEM_ADMIN)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// /This is a class which implements
    /// the Creator class and overrides
    /// the factory method to return an 
    /// instance of a ConcreteProduct.
    /// </summary>
    public class ConcreteClaimsFactory : ClaimsFactory
    {

        public override IClaims GetClaims(string type)
        {
            //type = type.ToUpper();
            switch (type)
            {
                case "BasicRole": return new Basic();
                case "SystemAdmin": return new SysAdmin();
                case "DelegateAdmin": return new DelAdmin();
                case "VendorRole": return new Vendor();
                case "UnregisteredRole": return new Unregistered(); // set to the default 
                //case "DEVELOPER": throw new NotImplementedException();
                case "Locked": return new Locked();
                default: return new Unregistered();
            }
        }

    }


}
