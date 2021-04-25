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
            type = type.ToUpper();
            switch (type)
            {
                case "BASIC_ROLE": return new Basic();
                case "SENIOR_ADMIN": return new Admin();
                case "VENDOR_ROLE": return new Vendor();
                case "UNREGISTERED_ROLE": return new Unregistered(); // set to the default 
                case "DEVELOPER": throw new NotImplementedException();
                case "LOCKED": return new Locked();
                default: return new Unregistered();
            }
        }

    }


}
