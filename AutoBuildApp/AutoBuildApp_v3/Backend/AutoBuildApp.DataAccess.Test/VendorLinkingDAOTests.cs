using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.VendorLinking;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace AutoBuildApp.DataAccess.Test
{
    [TestClass]
    public class VendorLinkingDAOTests
    {
        private string connectionString = "Server = localhost; Database = DBTest; Trusted_Connection = True;";
        private VendorLinkingDAO _vendorLinkingDAO;
        private readonly ClaimsPrincipal _claimsPrincipal;

        #region Vendor linking constructor
        public VendorLinkingDAOTests()
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

            // Initialize the Vendor Linking DAO
            _vendorLinkingDAO = new VendorLinkingDAO(connectionString);

        }
        #endregion

        #region VendorLinkingDAO_DeleteProductFromVendorList_SuccessfulDeletion
        [TestMethod]
        [DataRow("B450 PRO4")]
        public void VendorLinkingDAO_04_DeleteProductFromVendorList_SuccessfulDeletion(string modelNumber)
        {
            // Arrange
            SystemCodeWithObject<int> result;
            Thread.CurrentPrincipal = _claimsPrincipal;

            // Act
            result = _vendorLinkingDAO.DeleteProductFromVendorList(modelNumber);

            // Assert
            Assert.IsTrue(result.Code == AutoBuildSystemCodes.Success);
            Assert.IsTrue(result.GenericObject == 1);
        }
        #endregion

        #region VendorLinkingDAO_AddProductToVendorList_SuccessfulAddition
        [TestMethod]
        [DataRow("B450 PRO4")]
        public void VendorLinkingDAO_1_AddProductToVendorList_SuccessfulAddition(string modelNumber)
        {
            // Arrange
            SystemCodeWithObject<int> result;
            Thread.CurrentPrincipal = _claimsPrincipal;
            AddProductDTO addProductDTO = new AddProductDTO
            {
                Company = "new egg",
                Availability = true,
                ImageUrl = "ImageUrl",
                Name = "Name",
                ModelNumber = modelNumber,
                Price = 55.5,
                Url = "Url"
            };

            // Act
            result = _vendorLinkingDAO.AddProductToVendorListOfProducts(addProductDTO);

            // Assert
            Assert.IsTrue(result.Code == AutoBuildSystemCodes.Success);
            Assert.IsTrue(result.GenericObject == 1);
        }
        #endregion

        #region VendorLinkingDAO_AddProductToVendorList_DuplicateAddition
        [TestMethod]
        [DataRow("B450 PRO4")]
        public void VendorLinkingDAO_2_AddProductToVendorList_DuplicateAddition(string modelNumber)
        {
            // Arrange
            SystemCodeWithObject<int> result;
            Thread.CurrentPrincipal = _claimsPrincipal;
            AddProductDTO addProductDTO = new AddProductDTO
            {
                Company = "new egg",
                Availability = true,
                ImageUrl = "ImageUrl",
                Name = "Name",
                ModelNumber = modelNumber,
                Price = 55.5,
                Url = "Url"
            };

            // Act
            result = _vendorLinkingDAO.AddProductToVendorListOfProducts(addProductDTO);

            // Assert
            Assert.IsTrue(result.Code == AutoBuildSystemCodes.DuplicateValue);
            Assert.IsTrue(result.GenericObject == -1);
        }
        #endregion

        #region VendorLinkingDAO_EditProductInVendorListOfProducts_SuccesfulEdit
        [TestMethod]
        [DataRow("B450 PRO4")]
        public void VendorLinkingDAO_3_EditProductInVendorListOfProducts_SuccesfulEdit(string modelNumber)
        {
            // Arrange
            SystemCodeWithObject<int> result;
            Thread.CurrentPrincipal = _claimsPrincipal;
            AddProductDTO addProductDTO = new AddProductDTO
            {
                Company = "new egg",
                Availability = true,
                ImageUrl = "ImageUrl",
                Name = "Name",
                ModelNumber = modelNumber,
                Price = 55.5,
                Url = "Url"
            };

            // Act
            result = _vendorLinkingDAO.EditProductInVendorListOfProducts(addProductDTO);

            // Assert
            Assert.IsTrue(result.Code == AutoBuildSystemCodes.Success);
            Assert.IsTrue(result.GenericObject == 1);
        }
        #endregion


        #region VendorLinkingDAO_DeleteProductFromVendorList_ModelNumberDoesNotExist
        [TestMethod]
        [DataRow("gibberish")]
        public void VendorLinkingDAO_5_DeleteProductFromVendorList_ModelNumberDoesNotExist(string modelNumber)
        {
            // Arrange
            SystemCodeWithObject<int> result;
            Thread.CurrentPrincipal = _claimsPrincipal;

            // Act
            result = _vendorLinkingDAO.DeleteProductFromVendorList(modelNumber);

            // Assert
            Assert.IsTrue(result.Code == AutoBuildSystemCodes.Success);
            Assert.IsTrue(result.GenericObject == 0);
        }
        #endregion

        #region VendorLinkingDAO_AuthorizationReturnsFalse
        [TestMethod]
        public void VendorLinkingDAO_6_AuthorizationReturnsFalse()
        {
            // Arrange
            SystemCodeWithObject<List<string>> result;
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
            result = _vendorLinkingDAO.GetAllModelNumbers();

            // Assert
            Assert.IsTrue(result.Code == AutoBuildSystemCodes.Unauthorized);
        }
        #endregion

        #region VendorLinkingDAO_GetAllModelNumbers_Success
        [TestMethod]
        public void VendorLinkingDAO_7_GetAllModelNumbers_Success()
        {
            // Arrange
            SystemCodeWithObject<List<string>> result;
            Thread.CurrentPrincipal = _claimsPrincipal;

            // Act
            result = _vendorLinkingDAO.GetAllModelNumbers();

            // Assert
            Assert.IsTrue(result.Code == AutoBuildSystemCodes.Success);
            Assert.IsNotNull(result.GenericObject);
        }
        #endregion

        #region VendorLinkingDAO_PopulateVendorsProducts_ReturnsNotNullAndNotEmpty
        [TestMethod]
        public void VendorLinkingDAO_8_PopulateVendorsProducts_ReturnsNotNullAndNotEmpty()
        {
            // Arrange
            SystemCodeWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> result;
            Thread.CurrentPrincipal = _claimsPrincipal;

            // Act
            result = _vendorLinkingDAO.PopulateVendorsProducts();

            // Assert
            Assert.IsTrue(result.Code == AutoBuildSystemCodes.Success);
            Assert.IsNotNull(result.GenericObject);
            Assert.IsTrue(!result.GenericObject.IsEmpty);
        }
        #endregion

        #region VendorLinkingDAO_GetVendorProductsByFilter_ReturnListOfCPUs_OnlyCPUExistsInList
        [TestMethod]
        public void VendorLinkingDAO_9_GetVendorProductsByFilter_ReturnListOfCPUs_OnlyCPUExistsInList()
        {
            // Arrange
            SystemCodeWithObject<List<AddProductDTO>> result;
            Thread.CurrentPrincipal = _claimsPrincipal;
            ProductByFilterDTO productByFilterDTO = new ProductByFilterDTO
            {
                FilteredListOfProducts = new Dictionary<string, bool>
                {
                    {"GPU", true }
                },
                PriceOrder = "price_asc"
            };

            // Act
            result = _vendorLinkingDAO.GetVendorProductsByFilter(productByFilterDTO);

            Assert.IsNotNull(result.GenericObject);
            int length = result.GenericObject.Count;
            bool OnlyHasFilteredProductType = true;
            for(int i = 0; i < length; i++)
            {
                if(result.GenericObject[i].ProductType != "GPU")
                {
                    OnlyHasFilteredProductType = false;
                }
            }

            // Assert
            Assert.IsTrue(result.Code == AutoBuildSystemCodes.Success);
            Assert.IsTrue(OnlyHasFilteredProductType);
        }
        #endregion

        #region VendorLinkingDAO_GetVendorProductsByFilter_NoProductExistsWithSpecifiedFilter_ReturnEmpty
        [TestMethod]
        public void VendorLinkingDAO_9a_GetVendorProductsByFilter_NoProductExistsWithSpecifiedFilter_ReturnEmpty()
        {
            // Arrange
            SystemCodeWithObject<List<AddProductDTO>> result;
            Thread.CurrentPrincipal = _claimsPrincipal;
            ProductByFilterDTO productByFilterDTO = new ProductByFilterDTO
            {
                FilteredListOfProducts = new Dictionary<string, bool>
                {
                    {"gibberish", true }
                },
                PriceOrder = "price_asc"
            };

            // Act
            result = _vendorLinkingDAO.GetVendorProductsByFilter(productByFilterDTO);

            // Assert
            Assert.IsNotNull(result.GenericObject);
            Assert.IsTrue(result.GenericObject.Count == 0);
            Assert.IsTrue(result.Code == AutoBuildSystemCodes.Success);
        }
        #endregion
    }
}
