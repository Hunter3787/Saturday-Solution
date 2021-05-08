using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers.FeatureManagers;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using AutoBuildApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace AutoBuildApp.Manger.Tests
{
    [TestClass]
    public class VendorLinkingManagerTests
    {
        private VendorLinkingDAONoop _mockVendorLinkingDAO;
        private VendorLinkingServiceNoop _mockVendorLinkingService;
        private VendorLinkingManager _vendorLinkingManager;

        private readonly ClaimsPrincipal _claimsPrincipal;

        #region Vendor linking constructor
        public VendorLinkingManagerTests()
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
            // Initialize the mocked Vendor Linking Service
            _mockVendorLinkingService = new VendorLinkingServiceNoop(_mockVendorLinkingDAO);
            // Initialize the manager with the mocked Vendor Linking Service
            _vendorLinkingManager = new VendorLinkingManager(_mockVendorLinkingService);
        }
        #endregion

        #region VendorLinkingManager_AuthorizationReturnsFalse
        [TestMethod]
        public void VendorLinkingManager_AuthorizationReturnsFalse()
        {
            // Arrange
            CommonResponseWithObject<ProductByFilterDTO> result;
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
            result = _vendorLinkingManager.ConvertToGetProductByFilterDTO("", "");

            // Assert
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.ResponseString.Equals("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString()));
        }
        #endregion

        #region VendorLinkingManager_ConvertFormToProduct_ReturnCommonResponseWithTrueBooleanValue
        [DataTestMethod]
        [DataRow("modelNumber", "name", "55.5", "testUrl")]

        public void VendorLinkingManager_ConvertFormToProduct_ReturnCommonResponseWithTrueBooleanValue(string modelNumber, string name, string price, string url)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            IFormCollection formData = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>{
                { "modelNumber", modelNumber },
                {"name", name },
                {"price", price },
                {"url", url },
                {"imageUrl", "imageUrl" },
                {"availability", "true" }
            });

            // Act
            CommonResponseWithObject<AddProductDTO> result = _vendorLinkingManager.ConvertFormToProduct(formData);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }
        #endregion

        #region VendorLinkingManager_ConvertFormToProduct_ReturnCommonResponseWithFalseBooleanValue
        [DataTestMethod]
        [DataRow("hey", "hey", "test", "hey")]
        [DataRow("ModelNumber", "", "", "")]
        [DataRow("", "", "", "")]
        [DataRow("test", "", "", "")]
        [DataRow("test", "test", "", "")]
        [DataRow("test", "test", "test", "")]

        public void VendorLinkingManager_ConvertFormToProduct_ReturnCommonResponseWithFalseBooleanValue(string modelNumber, string name, string price, string url)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            IFormCollection formData = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>{
                { "modelNumber", modelNumber },
                {"name", name },
                {"price", price },
                {"url", url },
                {"imageUrl", "imageUrl" },
                {"availability", "true" }
            });

            // Act
            CommonResponseWithObject<AddProductDTO> result = _vendorLinkingManager.ConvertFormToProduct(formData);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
        }
        #endregion

        #region VendorLinkingManager_ConvertToGetProductByFilterDTO_ReturnCommonResponseWithFalseBooleanValue
        [DataTestMethod]
        [DataRow(null, "")]
        public void VendorLinkingManager_ConvertToGetProductByFilterDTO_ReturnCommonResponseWithFalseBooleanValue(string filtersString, string order)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            CommonResponseWithObject<ProductByFilterDTO> result;

            // Act
            result = _vendorLinkingManager.ConvertToGetProductByFilterDTO(filtersString, order);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
        }
        #endregion

        #region VendorLinkingManager_ConvertToGetProductByFilterDTO_ReturnCommonResponseWithTrueBooleanValue
        [DataTestMethod]
        [DataRow("?filtersString=motherboard,cpu","price_asc")]
        public void VendorLinkingManager_ConvertToGetProductByFilterDTO_ReturnCommonResponseWithTrueBooleanValue(string filtersString, string order)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            CommonResponseWithObject<ProductByFilterDTO> result;
            Dictionary<string, bool> expectedDictionary = new Dictionary<string, bool>
            {
                { "motherboard", true },
                { "cpu", true }
            };

            // Act
            result = _vendorLinkingManager.ConvertToGetProductByFilterDTO(filtersString, order);

            // Assert

            Assert.IsTrue(result.IsSuccessful);
            // Checks if both dictionaries are equal by ordering the keys then comparing
            Assert.IsTrue(expectedDictionary.OrderBy(pair => pair.Key).SequenceEqual(result.GenericObject.FilteredListOfProducts.OrderBy(pair => pair.Key)));
        }
        #endregion

        #region VendorLinkingManager_ConvertToGetProductByFilterDTO_SetOrderToDefaultValueIfOrderIsNull
        [DataTestMethod]
        [DataRow("", null)]
        public void VendorLinkingManager_ConvertToGetProductByFilterDTO_SetOrderToDefaultValueIfOrderIsNull(string filtersString, string order)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            CommonResponseWithObject<ProductByFilterDTO> result;
            var expected = "price_asc";

            // Act
            result = _vendorLinkingManager.ConvertToGetProductByFilterDTO(filtersString, order);

            // Assert
            Assert.AreEqual(expected, result.GenericObject.PriceOrder);
        }
        #endregion

        #region VendorLinkingManager_AddProductToVendorListOfProducts_ReturnCommonResponseWithTrueBooleanValue
        [DataTestMethod]
        [DynamicData(nameof(GetAddProductDTOAndPhoto), DynamicDataSourceType.Method)]
        public async System.Threading.Tasks.Task VendorLinkingManager_AddProductToVendorListOfProducts_ReturnCommonResponseWithTrueBooleanValueAsync(AddProductDTO product, IFormFile photo)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            CommonResponse result;

            // Act
            result = await _vendorLinkingManager.AddProductToVendorListOfProducts(product, photo);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }
        #endregion

        #region VendorLinkingManager_AddProductToVendorListOfProducts_PhotoIsNull_ReturnCommonResponseWithFalseBooleanValueAsync
        [DataTestMethod]
        [DynamicData(nameof(GetAddProductDTOAndNullPhoto), DynamicDataSourceType.Method)]
        public async System.Threading.Tasks.Task VendorLinkingManager_AddProductToVendorListOfProducts_PhotoIsNull_ReturnCommonResponseWithFalseBooleanValueAsync(AddProductDTO product, IFormFile photo)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal; 
            CommonResponse result;

            // Act
            result = await _vendorLinkingManager.AddProductToVendorListOfProducts(product, photo);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
        }
        #endregion

        #region VendorLinkingManager_EditProductInVendorListOfProducts_ReturnCommonResponseWithTrueBooleanValue
        [DataTestMethod]
        [DynamicData(nameof(GetAddProductDTOAndPhoto), DynamicDataSourceType.Method)]
        public async System.Threading.Tasks.Task VendorLinkingManager_EditProductInVendorListOfProducts_ReturnCommonResponseWithTrueBooleanValueAsync(AddProductDTO product, IFormFile photo)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            CommonResponse result;

            // Act
            result = await _vendorLinkingManager.EditProductInVendorListOfProducts(product, photo);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }
        #endregion

        #region VendorLinkingManager_EditProductInVendorListOfProducts_PhotoIsNull_ReturnCommonResponseWithFalseBooleanValueAsync
        [DataTestMethod]
        [DynamicData(nameof(GetAddProductDTOAndNullPhoto), DynamicDataSourceType.Method)]
        public async System.Threading.Tasks.Task VendorLinkingManager_EditProductInVendorListOfProducts_PhotoIsNull_ReturnCommonResponseWithFalseBooleanValueAsync(AddProductDTO product, IFormFile photo)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            CommonResponse result;

            // Act
            result = await _vendorLinkingManager.EditProductInVendorListOfProducts(product, photo);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }
        #endregion

        #region VendorLinkingManager_DeleteProductFromVendorList_ReturnCommonResponseWithTrueBooleanValue
        [DataTestMethod]
        [DataRow("modelNumber")]
        public void VendorLinkingManager_DeleteProductFromVendorList_ReturnCommonResponseWithTrueBooleanValue(string modelNumber)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            CommonResponse result;

            // Act
            result = _vendorLinkingManager.DeleteProductFromVendorList(modelNumber);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }
        #endregion

        #region VendorLinkingManager_DeleteProductFromVendorList_ReturnCommonResponseWithFalseBooleanValue
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void VendorLinkingManager_DeleteProductFromVendorList_ReturnCommonResponseWithFalseBooleanValue(string modelNumber)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            CommonResponse result;

            // Act
            result = _vendorLinkingManager.DeleteProductFromVendorList(modelNumber);

            // Assert
            Assert.IsFalse(result.IsSuccessful);
        }
        #endregion

        #region VendorLinkingManager_GetAllProductsByVendor_ReturnCommonResponseWithTrueBooleanValue
        [DataTestMethod]
        [DataRow(null)]
        public void VendorLinkingManager_GetAllProductsByVendor_ReturnCommonResponseWithTrueBooleanValue(ProductByFilterDTO filters)
        {
            // Arrange
            Thread.CurrentPrincipal = _claimsPrincipal;
            CommonResponse result;

            // Act
            result = _vendorLinkingManager.GetAllProductsByVendor(filters);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }
        #endregion

        private static IEnumerable<object[]> GetAddProductDTOAndPhoto()
        {
            AddProductDTO addProductDTO= new AddProductDTO
            {
                Name = "Name",
                ImageUrl = "ImageUrl",
                Availability = true,
                Company = "new egg",
                Url = "Url",
                ModelNumber = "ModelNumber",
                Price = 55.55
            };

            IFormFile photo = new FormFile(null, 1, 1, "", "");

            return new List<object[]>() {
                new object[]{addProductDTO, photo }
            };
        }

        private static IEnumerable<object[]> GetAddProductDTOAndNullPhoto()
        {
            AddProductDTO addProductDTO = new AddProductDTO
            {
                Name = "Name",
                ImageUrl = "ImageUrl",
                Availability = true,
                Company = "new egg",
                Url = "Url",
                ModelNumber = "ModelNumber",
                Price = 55.55
            };

            IFormFile photo = null;

            return new List<object[]>() {
                new object[]{addProductDTO, photo }
            };
        }
    }
}
