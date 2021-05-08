using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Services.FeatureServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Services
{
    public class ProductDetailsServiceNoop : ProductDetailsService
    {
        public ProductDetailsServiceNoop(ProductDetailsDAO productDetailsDAO) : base(productDetailsDAO)
        {

        }

        public override CommonResponseWithObject<ProductDetailsDTO> GetProductByModelNumber(string modelNUmber)
        {
            CommonResponseWithObject<ProductDetailsDTO> response = new CommonResponseWithObject<ProductDetailsDTO>();
            response.IsSuccessful = true;

            return response;
        }

        public override CommonResponse AddEmailToEmailListForProduct(string modelNumber)
        {
            CommonResponse response = new CommonResponse();
            response.IsSuccessful = true;

            return response;
        }
    }
}
