using System;
using System.Collections.Generic;
using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security;

/**
* The ShelfWithResponseService will provide the shelf specific operatons
* and handles translating the DAO returns into user friendly responses.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Services
{
    public class ShelfWithResponseService
    {
        private readonly ShelfDAO _dao;
        private readonly List<string> _approvedRoles;

        public ShelfWithResponseService(ShelfDAO shelfDAO)
        {
            _approvedRoles = new List<string>()
            {
                RoleEnumType.BasicRole,
                RoleEnumType.DelegateAdmin,
                RoleEnumType.VendorRole,
                RoleEnumType.SystemAdmin
            };

            _dao = shelfDAO;
        }

        /// <summary>
        /// Create a Shelf.
        /// </summary>
        /// <param name="shelfName">Shelf name to be added.</param>
        /// <param name="username">User the shelf is to be associated with.</param>
        /// <returns>True or false with a response string.</returns>
        public CommonResponse CreateShelf(string shelfName, string username)
        {
            CommonResponse output = new CommonResponse();

            if (!AuthorizationCheck.IsAuthorized(_approvedRoles))
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                output.ResponseBool = false;
                return output;
            }

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

        /// <summary>
        /// Delete shelf from a users account.
        /// </summary>
        /// <param name="shelfName">Name of shelf to be deleted.</param>
        /// <param name="username">Name of user the shelf is associated with.</param>
        /// <returns>True or false with a response string.</returns>
        public CommonResponse DeleteShelf(string shelfName, string username)
        {
            CommonResponse output = new CommonResponse();

            if (!AuthorizationCheck.IsAuthorized(_approvedRoles))
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                output.ResponseBool = false;
                return output;
            }

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

        /// <summary>
        /// Change the name of a shelf.
        /// </summary>
        /// <param name="oldName">Current name of the shelf.</param>
        /// <param name="newName">Name the shelf shall be changed to.</param>
        /// <param name="username">Name of the user associated with the shelf to change.</param>
        /// <returns>True or false with a response string.</returns>
        public CommonResponse ChangeShelfName(string oldName, string newName, string username)
        {
            CommonResponse output = new CommonResponse();

            if (!AuthorizationCheck.IsAuthorized(_approvedRoles))
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                output.ResponseBool = false;
                return output;
            }

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

        /// <summary>
        /// Add an item to a shelf.
        /// </summary>
        /// <param name="modelNumber">Model number of the item to be added.</param>
        /// <param name="quantity">The amount of items to add.</param>
        /// <param name="shelfName">The name of the shelf to add to.</param>
        /// <param name="username">Name of the user the shelf is associated with.</param>
        /// <returns>True or false with a response string.</returns>
        public CommonResponse AddToShelf(string modelNumber, int quantity, string shelfName, string username)
        {
            CommonResponse output = new CommonResponse();

            if (!AuthorizationCheck.IsAuthorized(_approvedRoles))
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                output.ResponseBool = false;
                return output;
            }

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

        /// <summary>
        /// Remove an item from the shelf.
        /// </summary>
        /// <param name="itemIndex">Location of the item on the shelf to be removed.</param>
        /// <param name="shelfName">Name of the shelf to remove from.</param>
        /// <param name="username">Name of the user associated with the shelf to be added to.</param>
        /// <returns>True or false with a response string.</returns>
        public CommonResponse RemoveFromShelf(int itemIndex, string shelfName, string username)
        {
            CommonResponse output = new CommonResponse();

            if (!AuthorizationCheck.IsAuthorized(_approvedRoles))
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                output.ResponseBool = false;
                return output;
            }

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

        /// <summary>
        /// Re-order the location of the items on the shelf.
        /// </summary>
        /// <param name="indicies">List of indicies to represent the current location of the items.</param>
        /// <param name="shelfName">Name of the shelf to modify.</param>
        /// <param name="username">Name of the user the shelf is associated wih.</param>
        /// <returns>True or false with a response string.</returns>
        public CommonResponse ReorderShelf(List<int> indicies, string shelfName, string username)
        {
            CommonResponse output = new CommonResponse();

            if (!AuthorizationCheck.IsAuthorized(_approvedRoles))
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                output.ResponseBool = false;
                return output;
            }

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

        /// <summary>
        /// Update the amount of an item that is at a particular index. 
        /// </summary>
        /// <param name="itemIndex">The index of the item to update.</param>
        /// <param name="quantity">The quantity to update to.</param>
        /// <param name="shelfName">The name of the shelf the item resides on.</param>
        /// <param name="username">The name of the user the shelf is associated with.</param>
        /// <returns>True or false with a response string.</returns>
        public CommonResponse UpdateQuantity(int itemIndex, int quantity, string shelfName, string username)
        {
            CommonResponse output = new CommonResponse();

            if (!AuthorizationCheck.IsAuthorized(_approvedRoles))
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                output.ResponseBool = false;
                return output;
            }

            var temp = _dao.UpdateQuantity(itemIndex, quantity, shelfName, username);

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

        /// <summary>
        /// Get a shelf by the user name and shelf name.
        /// </summary>
        /// <param name="shelfName">Name of the shelf to retrieve.</param>
        /// <param name="username">User the shelf is associated with.</param>
        /// <returns></returns>
        public CommonResponseWithObject<Shelf> GetShelfByName(string shelfName, string username)
        {
            CommonResponseWithObject<Shelf> output = new CommonResponseWithObject<Shelf>()
            {
                ResponseBool = false
            };

            if (!AuthorizationCheck.IsAuthorized(_approvedRoles))
            {
                output.GenericObject = new Shelf();
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                return output;
            }

            var temp = _dao.GetShelfByName(shelfName, username);
            output.GenericObject = temp.GenericObject;

            CodeToStringHandler(output, temp.Code);

            if(temp.Code == AutoBuildSystemCodes.Success)
            {
                output.ResponseBool = true;
            }

            return output;
        }

        /// <summary>
        /// Get all shelves based on user name.
        /// </summary>
        /// <param name="username">Name associated with the account to retrieve from.</param>
        /// <returns></returns>
        public CommonResponseWithObject<List<Shelf>> GetShelvesByUser(string username)
        {
            CommonResponseWithObject<List<Shelf>> output = new CommonResponseWithObject<List<Shelf>>()
            {
                ResponseBool = false
            };

            if (!AuthorizationCheck.IsAuthorized(_approvedRoles))
            {
                output.GenericObject = new List<Shelf>();
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                return output;
            }

            var temp = _dao.GetShelvesByUser(username);
            output.GenericObject = temp.GenericObject;

            CodeToStringHandler(output, temp.Code);

            if(temp.Code == AutoBuildSystemCodes.Success)
            {
                output.ResponseBool = true;
            }

            return output;
        }

        // Out of scope.
        //public Component GetComponent(int index, string shelfName)
        //{
        //    Component output = null;

        //    return output;
        //}

        #region Private Helper Methods
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
        #endregion
    }
}