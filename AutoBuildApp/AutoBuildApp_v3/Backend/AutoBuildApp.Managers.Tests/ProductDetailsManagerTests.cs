using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using AutoBuildApp.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace AutoBuildApp.Manger.Tests
{
    [TestClass]
    public class ProductDetailsManagerTests
    {
        private ProductDetailsDAONoop _mockProductDetailsDAO;
        private ProductDetailsServiceNoop _mockVendorLinkingService;
        private ProductDetailsManager _productDetailsManager;

        private readonly ClaimsPrincipal _claimsPrincipal;

        #region Product details constructor
        public ProductDetailsManagerTests()
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
            _mockProductDetailsDAO = new ProductDetailsDAONoop("");
            // Initialize the mocked Vendor Linking Service
            _mockVendorLinkingService = new ProductDetailsServiceNoop(_mockProductDetailsDAO);
            // Initialize the manager with the mocked Vendor Linking Service
            _productDetailsManager = new ProductDetailsManager(_mockVendorLinkingService);
        }
        #endregion

        #region ProductDetailsManager_ModelNumberIsNull_ReturnFalse
        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void ProductDetailsManager_ModelNumberIsNull_ReturnFalse(string modelNumber) 
        {
            // Act
            CommonResponseWithObject<ProductDetailsDTO> result = _productDetailsManager.GetProductByModelNumber(modelNumber);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.ResponseString.Equals("Model number is null."));
        }
        #endregion


        #region ProductDetailsManager_AddEmailToEmailListForProduct_ModelNumberIsNull_ReturnFalse
        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void ProductDetailsManager_AddEmailToEmailListForProduct_ModelNumberIsNull_ReturnFalse(string modelNumber)
        {
            // Act
            CommonResponse result = _productDetailsManager.AddEmailToEmailListForProduct(modelNumber);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.ResponseString.Equals("Model number is null."));
        }
        #endregion
    }
}