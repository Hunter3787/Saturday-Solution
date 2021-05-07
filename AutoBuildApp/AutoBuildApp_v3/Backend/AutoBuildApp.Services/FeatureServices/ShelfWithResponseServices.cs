using System;
using System.Collections.Generic;
using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Models.Enumerations;

/**
* The ShelfServices will provide the shelf specific operatons.
* Catches errors, prepares commmon response object.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Services
{
    public class ShelfWithResponseService
    {
        private ShelfDAO _dao;

        public ShelfWithResponseService(ShelfDAO shelfDAO)
        {
            _dao = shelfDAO;
        }

        public CommonResponse CreateShelf(string shelfName, string username)
        {
            CommonResponse output = new CommonResponse();
            var temp = _dao.InsertShelf(shelfName, username);

            output.ResponseBool = temp.GenericObject;

            // If successful add custom response bool for creation.
            if (!temp.GenericObject)
            {
                CodeToStringHandler(output, temp.Code);
            }
            else
            {
                output.ResponseString = ResponseStringGlobals.SUCCESSFUL_CREATION;
            }

            return output;
        }

        public CommonResponse DeleteShelf(string shelfName, string username)
        {
            CommonResponse output = new CommonResponse();
            var temp = _dao.DeleteShelf(shelfName, username);

            output.ResponseBool = temp.GenericObject;

            // If successful add custom response bool for creation.
            if (!temp.GenericObject)
            {
                CodeToStringHandler(output, temp.Code);
            }
            else
            {
                output.ResponseString = ResponseStringGlobals.SUCCESSFUL_DELETION;
            }

            return output;
        }

        public CommonResponse ChangeShelfName(string oldName, string newName, string username)
        {
            CommonResponse output = new CommonResponse();
            var temp = _dao.UpdateShelfName(oldName, newName, username);

            output.ResponseBool = temp.GenericObject;

            // If successful add custom response bool for creation.
            if (!temp.GenericObject)
            {
                CodeToStringHandler(output, temp.Code);
            }
            else
            {
                output.ResponseString = ResponseStringGlobals.SUCCESSFUL_MODIFICATION;
            }

            return output;
        }

        public CommonResponse AddToShelf(string modelNumber, int quantity, string shelfName, string username)
        {
            CommonResponse output = new CommonResponse();
            var temp = _dao.AddComponent(modelNumber, quantity, shelfName, username);

            output.ResponseBool = temp.GenericObject;

            // If successful add custom response bool for creation.
            if (!temp.GenericObject)
            {
                CodeToStringHandler(output, temp.Code);
            }
            else
            {
                output.ResponseString = ResponseStringGlobals.SUCCESSFUL_ADDITION;
            }

            return output;
        }

        public CommonResponse RemoveFromShelf(int itemIndex, string shelfName, string username)
        {
            CommonResponse output = new CommonResponse();
            var temp = _dao.RemoveComponent(itemIndex, shelfName, username);

            output.ResponseBool = temp.GenericObject;

            // If successful add custom response bool for creation.
            if (!temp.GenericObject)
            {
                CodeToStringHandler(output, temp.Code);
            }
            else
            {
                output.ResponseString = ResponseStringGlobals.SUCCESSFUL_REMOVAL;
            }

            return output;
        }

        public CommonResponse ReorderShelf(List<int> indicies, string shelfName, string username)
        {
            CommonResponse output = new CommonResponse();
            var temp = _dao.UpdateShelfOrder(indicies, shelfName, username);

            output.ResponseBool = temp.GenericObject;

            // If successful add custom response bool for creation.
            if (!temp.GenericObject)
            {
                CodeToStringHandler(output, temp.Code);
            }
            else
            {
                output.ResponseString = ResponseStringGlobals.SUCCESSFUL_MODIFICATION;
            }

            return output;
        }

        public CommonResponse ChangeQuantity(string modelNumber, int itemIndex, int count, string shelfName, string username)
        {
            CommonResponse output = new CommonResponse();


            return output;
        }

        public CommonResponseWithObject<Shelf> GetShelfByName(string shelfName, string username)
        {
            CommonResponseWithObject<Shelf> outputList = new CommonResponseWithObject<Shelf>();

            return outputList;
        }

        public CommonResponseWithObject<List<Shelf>> GetAllusernameShelves(string username)
        {
            CommonResponseWithObject<List<Shelf>> outputList = new CommonResponseWithObject<List<Shelf>>();

            return outputList;
        }

        public Component GetComponent(int index, string shelfName)
        {
            Component output = null;

            return output;
        }

        /// <summary>
        /// Helper function to insert the strings from each expected error code.
        /// </summary>
        private void CodeToStringHandler(CommonResponse commonResponse, AutoBuildSystemCodes code)
        {
            switch (code)
            {
                case AutoBuildSystemCodes.Success:
                    commonResponse.ResponseString = ResponseStringGlobals.SUCCESSFUL_RESPONSE;
                    break;
                case AutoBuildSystemCodes.Unauthorized:
                    commonResponse.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                    break;
                case AutoBuildSystemCodes.UndeclaredVariable:
                    commonResponse.ResponseString = ResponseStringGlobals.MISSING_ARGUEMENT;
                    break;
                case AutoBuildSystemCodes.DuplicateValue:
                    commonResponse.ResponseString = ResponseStringGlobals.DUPLICATE_VALUE;
                    break;
                case AutoBuildSystemCodes.InsertFailed:
                    commonResponse.ResponseString = ResponseStringGlobals.FAILED_ADDITION;
                    break;
                case AutoBuildSystemCodes.DeleteFailed:
                    commonResponse.ResponseString = ResponseStringGlobals.FAILED_DELETION;
                    break;
                case AutoBuildSystemCodes.ArguementNull:
                    commonResponse.ResponseString = ResponseStringGlobals.MISSING_ARGUEMENT;
                    break;
                case AutoBuildSystemCodes.FailedParse:
                    commonResponse.ResponseString = ResponseStringGlobals.SYSTEM_FAILURE;
                    break;
                case AutoBuildSystemCodes.DatabaseTimeout:
                    commonResponse.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;
                    break;
                case AutoBuildSystemCodes.ConnectionError:
                    commonResponse.ResponseString = ResponseStringGlobals.DATABASE_FAILURE;
                    break;
                default:
                    commonResponse.ResponseString = ResponseStringGlobals.DEFAULT_RESPONSE;
                    break;
            }
        }
    }
}