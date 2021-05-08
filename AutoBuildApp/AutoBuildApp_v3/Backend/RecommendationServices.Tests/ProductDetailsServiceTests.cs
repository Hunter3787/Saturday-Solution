using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace AutoBuildApp.Services.Tests
{
    public class ProductDetailsServiceTests
    {
        private ProductDetailsDAONoop _mockProductDetailsDAO;
        private ProductDetailsServiceNoop _mockProductDetailsService;

        #region Product Details constructor
        public ProductDetailsServiceTests()
        {
            // Initialize the mocked Product details DAO
            _mockProductDetailsDAO = new ProductDetailsDAONoop("");
            // Initialize the Vendor Linking Service
            _mockProductDetailsService = new ProductDetailsServiceNoop(_mockProductDetailsDAO);

        }
        #endregion

        #region ProductDetailsService_GetProductByModelNumber_ReturnTrue
        [TestMethod]
        public void ProductDetailsService_GetProductByModelNumber_ReturnTrue()
        {
            // Arrange
            CommonResponseWithObject<ProductDetailsDTO> result = new CommonResponseWithObject<ProductDetailsDTO>();

            // Act
            result = _mockProductDetailsService.GetProductByModelNumber(null);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsTrue(result.ResponseString.Equals(ResponseStringGlobals.SUCCESSFUL_RESPONSE));
        }
        #endregion

        #region ProductDetailsService_AddEmailToEmailListForProduct
        [TestMethod]
        public void ProductDetailsService_AddEmailToEmailListForProduct_ReturnTrue()
        {
            // Arrange
            CommonResponse result = new CommonResponse();

            // Act
            result = _mockProductDetailsService.AddEmailToEmailListForProduct(null);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
            Assert.IsTrue(result.ResponseString.Equals(ResponseStringGlobals.SUCCESSFUL_RESPONSE));

        }
        #endregion
    }
}
