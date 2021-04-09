using AutoBuildApp.Security.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace AutoBuildApp.Security.Tests
{
    /// <summary>
    /// the authorization service unit test
    /// here I will testing on the following:
    /// 1) setting the thread with a claimsprinicple and 
    /// testing the ones passed if they match 
    /// 
    /// </summary>
    [TestClass]
    public class AuthorizationServiceTest
    {


        private static IEnumerable<object[]> getClaimsRequired()
        {
            return new List<object[]>()
            {             
                new object[]{ }
            };


        }

        [TestMethod]
        [DataTestMethod]
        public void checkPermissions_ReturnsBool()
        {



        }



    }
}
