using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.VendorLinking;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoBuildApp.Services
{
    public class VendorLinkingServiceNoop : VendorLinkingService
    {
        public VendorLinkingServiceNoop(VendorLinkingDAO vendorLinkingDAO) : base(vendorLinkingDAO)
        {

        }

        public override CommonResponseWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> PopulateVendorsProducts()
        {
            CommonResponseWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>> response = new CommonResponseWithObject<ConcurrentDictionary<string, ConcurrentDictionary<string, byte>>>();
            response.IsSuccessful = true;

            return response;
        }

        public override CommonResponse AddProductToVendorListOfProducts(AddProductDTO product)
        {
            CommonResponse response = new CommonResponse();
            response.IsSuccessful = true;

            return response;
        }

        public override CommonResponse EditProductInVendorListOfProducts(AddProductDTO product)
        {
            CommonResponse response = new CommonResponse();
            response.IsSuccessful = true;

            return response;
        }

        public override CommonResponse DeleteProductFromVendorList(string modelNumber)
        {
            CommonResponse response = new CommonResponse();
            response.IsSuccessful = true;

            return response;
        }

        public override CommonResponseWithObject<List<AddProductDTO>> GetAllProductsByVendor(ProductByFilterDTO filters)
        {
            CommonResponseWithObject<List<AddProductDTO>> response = new CommonResponseWithObject<List<AddProductDTO>>();
            response.IsSuccessful = true;

            return response;
        }

        public override CommonResponseWithObject<List<string>> GetAllModelNumbers()
        {
            CommonResponseWithObject<List<string>> response = new CommonResponseWithObject<List<string>>();
            response.IsSuccessful = true;

            return response;
        }

        public override async Task<string> UploadImage(string username, IFormFile file)
        {
            return "";
        }
    }
}
