using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace AutoBuildApp.Services.Tests
{
    [TestClass]
    public class VendorLinkingServiceTests
    {
        private VendorLinkingDAONoop _mockVendorLinkingDAO;
        private VendorLinkingService _vendorLinkingService;

        private readonly ClaimsPrincipal _claimsPrincipal;

        #region Vendor linking constructor
        public VendorLinkingServiceTests()
        {
            //Prepare a ClaimsPrincipal object to use for the current thread
            #region Claims Principal preparation
            UserIdentity userIdentity = new UserIdentity
            {
                Name = "new egg",
                IsAuthenticated = true,
                AuthenticationType = "JWT"
            };
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaims vendorClaims = claimsFactory.GetClaims(RoleEnumType.VendorRole);
            ClaimsIdentity vendorClaimsIdentity = new ClaimsIdentity(userIdentity, vendorClaims.Claims(), userIdentity.AuthenticationType, userIdentity.Name, " ");

            _claimsPrincipal = new ClaimsPrincipal(vendorClaimsIdentity);
            #endregion
            Thread.CurrentPrincipal = _claimsPrincipal;

            // Initialize the mocked Vendor Linking DAO
            _mockVendorLinkingDAO = new VendorLinkingDAONoop("");
            // Initialize the Vendor Linking Service
            _vendorLinkingService = new VendorLinkingService(_mockVendorLinkingDAO);

        }
        #endregion

        #region VendorLinkingService_AuthorizationReturnsFalse
        [TestMethod]
        public void VendorLinkingService_AuthorizationReturnsFalse()
        {
            // Arrange
            CommonResponseWithObject<List<string>> result;
            UserIdentity userIdentity = new UserIdentity
            {
                Name = "new egg",
                IsAuthenticated = true,
                AuthenticationType = "JWT"
            };
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaims basicClaims = claimsFactory.GetClaims(RoleEnumType.BasicRole);
            ClaimsIdentity vendorClaimsIdentity = new ClaimsIdentity(userIdentity, basicClaims.Claims(), userIdentity.AuthenticationType, userIdentity.Name, " ");

            Thread.CurrentPrincipal = new ClaimsPrincipal(vendorClaimsIdentity);

            // Act
            result = _vendorLinkingService.GetAllModelNumbers();

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.ResponseString.Equals("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString()));
        }
        #endregion

        #region VendorLinkingService_ReturnSuccessfulResponseWithGenericObject
        [TestMethod]
        public void VendorLinkingService_ReturnSuccessfulResponseWithGenericObject()
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            CommonResponseWithObject<List<string>> result = new CommonResponseWithObject<List<string>>();
            List<string> expected = new List<string> { "test" };

            // Act
            result = _vendorLinkingService.GetAllModelNumbers();

            // Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsTrue(expected.SequenceEqual(result.GenericObject));
        }
        #endregion

        #region VendorLinkingService_ReturnSuccessfulCode
        [TestMethod]
        [DataRow(null)]
        public void VendorLinkingService_ReturnSuccessfulCode(AddProductDTO addProductDTO)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            CommonResponse result = new CommonResponse();

            // Act
            result = _vendorLinkingService.AddProductToVendorListOfProducts(addProductDTO);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsTrue(result.ResponseString.Equals(ResponseStringGlobals.SUCCESSFUL_ADDITION));
        }
        #endregion

        #region VendorLinkingService_ReturnNoChangeOccurredCode
        [TestMethod]
        [DataRow("test")]
        public void VendorLinkingService_ReturnNoChangeOccurredCode(string modelNumber)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            CommonResponse result = new CommonResponse();

            // Act
            result = _vendorLinkingService.DeleteProductFromVendorList(modelNumber);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsTrue(result.ResponseString.Equals(ResponseStringGlobals.NO_CHANGE_OCCURRED));
        }
        #endregion
    }
}
