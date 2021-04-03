using AutoBuildApp.Security.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Security.Tests
{
    [TestClass]
    public class JWTTest
    {
        private IList<Claims> claimPlaceHolder = new List<Claims>
        { new Claims("none", "no scope") };

        private static string SecurityKey = "Secret";

        private static JWTHeader _headerDefault = new JWTHeader();

        private static JWTPayload _payloadEmpty = new JWTPayload();

        private static JWTPayload _payloadOne = new JWTPayload
            ("Autobuild User", "Autobuild", "US",
            "Email", (long)1617759065, (long)1617759065, (long)1617154265)
        {
            UserCLaims = new List<Claims>
            {
                new Claims("none", "no scope")
            }

        };
        // these are from JWT.io
        private static string expectedJWTSignatureOne = "xZPE6hkhN8xfCqBvD2VZ69GtEz5OsVHYxMm0EgHnzUc";
        private static string expectedJWTSignatureDefault = "peD4NAb8LE098RGdReDzpdX2Hpl731GPn2YsZmwtmpc";


        private static IEnumerable<object[]> getJWTData()
        {
            return new List<object[]>()
            {
                // here I instatiate objects that carry the secret key, the JWTHeader object and the JWTPayload object
                new object[]{SecurityKey,_payloadOne, _headerDefault ,expectedJWTSignatureOne},
                new object[]{SecurityKey,_payloadEmpty, _headerDefault, expectedJWTSignatureDefault}
                };
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(getJWTData), DynamicDataSourceType.Method)]
        public void JWT_GenerateJWTSignature_ReturnsSignature(string secret, JWTPayload jWTPayload, JWTHeader jWTHeader, string expectedJWTSignature)
        {
            //Arrange:
            JWT jwtGenerator = new JWT(secret, jWTPayload, jWTHeader);
            //Act:
            jwtGenerator.GenerateJWTSignature();
            //Assert:
            string actualJWTSignature = jwtGenerator.JWTSignature;
            Assert.AreEqual(expectedJWTSignature, actualJWTSignature);

        }

        private static IEnumerable<object[]> getNullPayload()
        {
            JWTPayload jWTPayloadNull = null;

            JWTHeader jWTHeaderNull = null;
            return new List<object[]>()
            {
                // here I instatiate objects that carry the secret key, the JWTHeader object and the JWTPayload object
                new object[]{"Secret" , jWTPayloadNull,_headerDefault,"NULL OBJECT PROVIDED"},
                new object[]{"Sercret",_payloadEmpty,  jWTHeaderNull, "NULL OBJECT PROVIDED"},
                new object[]{null, jWTPayloadNull,  jWTHeaderNull, "NULL OBJECT PROVIDED"}

            };
        }

        [TestMethod]
        [DataTestMethod]
        [DynamicData(nameof(getNullPayload), DynamicDataSourceType.Method)]
        public void JWT_Null_Object_ReturnNullException
            (string secret, JWTPayload payload,
            JWTHeader header, string expectedParamName)
        {
            try
            {
                JWT jWT = new JWT(secret, payload, header);
            }
            //Assert:
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual(expectedParamName, ex.ParamName);
            }
        }
    }
}

/*
 * references:
 * https://docs.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2019
 * 
 * 
 * 
 * 
 */