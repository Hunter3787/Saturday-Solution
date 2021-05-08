using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;
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

namespace AutoBuildApp.DataAccess.Test
{
    [TestClass]
    public class ProductDetailsDAOTests
    {
        private string connectionString = "Server = localhost; Database = DBTest; Trusted_Connection = True;";
        private ProductDetailsDAO _productDetailsDAO;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public ProductDetailsDAOTests()
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

            _productDetailsDAO = new ProductDetailsDAO(connectionString);
        }

        #region ProductDetailsDAO_GetProductByModelNumber_ValidModelNumber_ReturnAddProductDTO
        [TestMethod]
        [DataRow("B450 PRO4")]
        public void ProductDetailsDAO_GetProductByModelNumber_ValidModelNumber_ReturnAddProductDTO(string modelNumber)
        {
            // Act
            SystemCodeWithObject<ProductDetailsDTO> response = _productDetailsDAO.GetProductByModelNumber(modelNumber);

            // Assert
            ProductDetailsDTO productDetailsDTO = response.GenericObject;
            Assert.IsTrue(response.Code == AutoBuildSystemCodes.Success);
            Assert.IsTrue(!String.IsNullOrEmpty(productDetailsDTO.ImageUrl));
            Assert.IsTrue(!String.IsNullOrEmpty(productDetailsDTO.ModelNumber));
            Assert.IsTrue(!String.IsNullOrEmpty(productDetailsDTO.ProductName));
            Assert.IsTrue(!String.IsNullOrEmpty(productDetailsDTO.ProductType));
            Assert.IsNotNull(productDetailsDTO.Specs);
            Assert.IsNotNull(productDetailsDTO.VendorInformation);

        }
        #endregion

        #region ProductDetailsDAO_GetProductByModelNumber_NullModelNumber_ReturnNullValue
        [TestMethod]
        [DataRow(null)]
        public void ProductDetailsDAO_GetProductByModelNumber_NullModelNumber_ReturnNullValue(string modelNumber)
        {
            // Act
            SystemCodeWithObject<ProductDetailsDTO> response = _productDetailsDAO.GetProductByModelNumber(modelNumber);

            // Assert
            Assert.IsTrue(response.Code == AutoBuildSystemCodes.NullValue);
            Assert.IsNull(response.GenericObject);

        }
        #endregion

        #region ProductDetailsDAO_GetProductByModelNumber_InvalidModelNumber_ReturnNoEntryFound
        [TestMethod]
        [DataRow("gibberish")]
        public void ProductDetailsDAO_GetProductByModelNumber_InvalidModelNumber_ReturnNoEntryFound(string modelNumber)
        {
            // Act
            SystemCodeWithObject<ProductDetailsDTO> response = _productDetailsDAO.GetProductByModelNumber(modelNumber);

            // Assert
            Assert.IsTrue(response.Code == AutoBuildSystemCodes.NoEntryFound);
            Assert.IsNull(response.GenericObject);

        }
        #endregion

        #region ProductDetailsDAO_AddEmailToEmailListForProduct_NullModelNumber_ReturnNullValue
        [TestMethod]
        [DataRow(null)]
        public void ProductDetailsDAO_AddEmailToEmailListForProduct_NullModelNumber_ReturnNullValue(string modelNumber)
        {
            // Act
            SystemCodeWithObject<int> response = _productDetailsDAO.AddEmailToEmailListForProduct(modelNumber);

            // Assert
            Assert.IsTrue(response.Code == AutoBuildSystemCodes.NullValue);
            Assert.IsTrue(response.GenericObject == -1);
        }
        #endregion

        #region ProductDetailsDAO_AddEmailToEmailListForProduct_ValidModelNumber_Return1Row
        [TestMethod]
        [DataRow("B450 PRO4")]
        public void ProductDetailsDAO_AddEmailToEmailListForProduct_ValidModelNumber_Return1Row(string modelNumber)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            // Act
            SystemCodeWithObject<int> response = _productDetailsDAO.AddEmailToEmailListForProduct(modelNumber);

            // Assert
            Assert.IsTrue(response.Code == AutoBuildSystemCodes.Success);
            Assert.IsTrue(response.GenericObject == 1);
        }
        #endregion

    }
}
