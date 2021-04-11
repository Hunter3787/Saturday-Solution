using AutoBuildApp.Security.Enumerations;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;


/// <summary>
/// https://www.dotnettricks.com/learn/designpatterns/factory-method-design-pattern-dotnet
/// 
/// </summary>
namespace AutoBuildApp.Security.Interfaces
{

    /// <summary>
    /// the 'Product' interface
    /// an interface for creating the objects
    /// </summary>
    public interface IClaimsFactory
    {
        IEnumerable<Claim> Claims();
        void PrintClaims();

    }


}

