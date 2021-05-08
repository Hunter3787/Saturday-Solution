using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.DataAccess
{
    public class ProductDetailsDAONoop : ProductDetailsDAO
    {
        public ProductDetailsDAONoop(string connectionString) : base(connectionString)
        {

        }

        public override SystemCodeWithObject<ProductDetailsDTO> GetProductByModelNumber(string modelNumber)
        {
            SystemCodeWithObject<ProductDetailsDTO> response = new SystemCodeWithObject<ProductDetailsDTO>();
            response.Code = AutoBuildSystemCodes.Success;

            return response;
        }

        public override SystemCodeWithObject<int> AddEmailToEmailListForProduct(string modelNumber)
        {
            SystemCodeWithObject<int> response = new SystemCodeWithObject<int>();
            response.Code = AutoBuildSystemCodes.Success;

            return response;
        }
    }
}
