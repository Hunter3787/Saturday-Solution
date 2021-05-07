using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models;
using AutoBuildApp.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Services.FeatureServices
{
    public class CommonResponseService
    {
        public void SetCommonResponse(AutoBuildSystemCodes code, CommonResponse response)
        {
            switch (code)
            {
                case AutoBuildSystemCodes.Success:
                    response.IsSuccessful = true;
                    response.ResponseString = ResponseStringGlobals.SUCCESSFUL_RESPONSE;

                    break;
                case AutoBuildSystemCodes.DatabaseTimeout:
                    response.IsSuccessful = false;
                    response.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;

                    break;
                case AutoBuildSystemCodes.DuplicateValue:
                    response.IsSuccessful = false;
                    response.ResponseString = ResponseStringGlobals.DUPLICATE_VALUE;

                    break;
                default:
                    response.IsSuccessful = false;
                    response.ResponseString = ResponseStringGlobals.DEFAULT_RESPONSE;

                    break;
            }
        }

        public void SetCommonResponse(AutoBuildSystemCodes code, CommonResponse response, string customSuccessString)
        {
            SetCommonResponse(code, response);
            
            if(code == AutoBuildSystemCodes.Success)
            {
                response.ResponseString = customSuccessString;
            }
        }
    }
}
