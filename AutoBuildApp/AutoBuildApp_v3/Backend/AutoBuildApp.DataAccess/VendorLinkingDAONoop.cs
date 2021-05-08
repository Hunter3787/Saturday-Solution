using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Models.VendorLinking;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class VendorLinkingDAONoop : VendorLinkingDAO
    {
        public VendorLinkingDAONoop(string connectionString) : base(connectionString)
        {

        }

        public override SystemCodeWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> PopulateVendorsProducts()
        {
            SystemCodeWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> response = new SystemCodeWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>>();
            response.Code = AutoBuildSystemCodes.Success;

            return response;
        }

        public override SystemCodeWithObject<List<string>> GetAllModelNumbers()
        {
            SystemCodeWithObject<List<string>> response = new SystemCodeWithObject<List<string>>();
            response.Code = AutoBuildSystemCodes.Success;
            response.GenericObject = new List<string>{ "test" };

            return response;
        }

        public override SystemCodeWithObject<List<AddProductDTO>> GetVendorProductsByFilter(ProductByFilterDTO product)
        {
            SystemCodeWithObject<List<AddProductDTO>> response = new SystemCodeWithObject<List<AddProductDTO>>();
            response.Code = AutoBuildSystemCodes.Success;

            return response;
        }

        public override SystemCodeWithObject<int> AddProductToVendorListOfProducts(AddProductDTO product)
        {
            SystemCodeWithObject<int> response = new SystemCodeWithObject<int>();
            response.Code = AutoBuildSystemCodes.Success;
            response.GenericObject = 1;

            return response;
        }

        public override SystemCodeWithObject<int> EditProductInVendorListOfProducts(AddProductDTO product)
        {
            SystemCodeWithObject<int> response = new SystemCodeWithObject<int>();
            response.Code = AutoBuildSystemCodes.Success;
            response.GenericObject = 1;

            return response;
        }

        public override SystemCodeWithObject<int> DeleteProductFromVendorList(string modelNumber)
        {
            SystemCodeWithObject<int> response = new SystemCodeWithObject<int>();
            response.Code = AutoBuildSystemCodes.Success;
            response.GenericObject = 0;

            return response;
        }
    }
}
