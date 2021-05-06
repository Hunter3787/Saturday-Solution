using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.Models;
using System.Collections.Generic;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Services;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers.Guards;
using System.Threading;
using System.Security.Claims;
using AutoBuildApp.Security;
using System;

/**
* User Garage Manager class that directs 
* operations with regards to the user garage.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Managers
{
    public class UserGarageManager
    {
        private BuildDAO _buildDAO;
        private ShelfDAO _shelfDAO;

        private List<string> _approvedRoles;
        private ShelfService _shelfService;
        private BuildManagementService _buildService;
        private readonly string _currentUser;

        public UserGarageManager(string connectionString)
        { 
            //ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            _approvedRoles = new List<string>()
            { RoleEnumType.BasicRole,RoleEnumType.DelegateAdmin,
            RoleEnumType.VendorRole, RoleEnumType.SystemAdmin};

            _buildDAO = new BuildDAO(connectionString);
            _shelfDAO = new ShelfDAO(connectionString);
            _shelfService = new ShelfService(_shelfDAO);
            _buildService = new BuildManagementService(_buildDAO);
            _currentUser = Thread.CurrentPrincipal.Identity.Name;


        }

        public CommonResponse AddBuild(IBuild build, string buildname)
        {
            // temp
            if (!AuthorizationCheck.IsAuthorized(_approvedRoles)) // added per Zee
            {
                return new CommonResponse()
                {
                    ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS,
                    ResponseBool = false
                };
            }
            NullGuard.IsNotNull(build);
            NullGuard.IsNotNullOrEmpty(buildname);

            CommonResponse response = _buildService.AddBuild(build, buildname, _currentUser);

            return response;
        }

        public CommonResponse CopyBuildToGarage(string buildID)
        {
            NullGuard.IsNotNullOrEmpty(buildID);

            CommonResponse response = _buildService.CopyBuild();

            return response;
        }

        public CommonResponse DeleteBuild(string buildID)
        {
            NullGuard.IsNotNullOrEmpty(buildID);

            CommonResponse response = _buildService.DeleteBuild();

            return response;
        }

        public List<IBuild> GetAllUserBuilds(string user, string sorting)
        {
            List<IBuild> outputList;
            var order = sorting;

            // TODO:Fix guards and parameters.
            NullGuard.IsNotNullOrEmpty(user);
            if (string.IsNullOrEmpty(sorting))
            {
                order = UserGarageGlobals.DEFAULT_SORT;
            }

            outputList = _buildService.GetAllUserBuilds(user, order);

            return outputList;
        }

        public CommonResponse PublishBuild(string buildID)
        {
            NullGuard.IsNotNullOrEmpty(buildID);

            CommonResponse response = _buildService.PublishBuild();

            return response;
        }

        public CommonResponse ModifyBuild(IBuild build, string oldName, string newName)
        {
            NullGuard.IsNotNull(build);

            CommonResponse response = _buildService.ModifyBuild();

            return response;
        }

        // Starting point
        public CommonResponse CreateShelf(string shelfName, string user)
        {
            NullGuard.IsNotNullOrEmpty(shelfName);
            NullGuard.IsNotNullOrEmpty(user);

            // TODO:Add input validation.
            CommonResponse response = _shelfService.CreateShelf(shelfName, user);

            return response;
        }

        public CommonResponse RenameShelf(string from, string to, string user)
        {
            // TODO:Add input validation.
            CommonResponse response = _shelfService.ChangeShelfName(from, to, user);

            return response;
        }

        public CommonResponse DeleteShelf(string shelfName, string user)
        {
            // Add Business rules

            // If(user == current)
            CommonResponse response = _shelfService.DeleteShelf(shelfName);

            return response;
        }

        public CommonResponse AddToShelf(IComponent item, string shelfName, string user)
        {
            // Add Business rules
            CommonResponse response = _shelfService.AddToShelf(item, shelfName, user);

            return response;
        }

        public CommonResponse RemoveFromShelf(int itemIndex, string shelfName)
        {
            // Add Business rules
            CommonResponse response = _shelfService.RemoveFromShelf(itemIndex, shelfName);

            return response;
        }

        public CommonResponse ModifyCount(int count, string itemID, string shelfName)
        {
            // Add Business rules
            CommonResponse response = _shelfService.ChangeQuantity(count, itemID, shelfName);

            return response;
        }

        public CommonResponse MoveItemOnShelf(int indexStart, int indexEnd, string user)
        {
            // Add business rules
            CommonResponse response = _shelfService.ModifyShelf(indexStart, indexEnd, user);

            return response;
        }

        public CommonResponseWithObject<Shelf> GetShelfByName(string shelfName, string username)
        {
            CommonResponseWithObject<Shelf> output = new CommonResponseWithObject<Shelf>();

            try
            {
                IsAuthorized();
                NullGuard.IsNotNullOrEmpty(shelfName);
            }
            catch (ArgumentNullException)
            {
                output.GenericObject = new Shelf();
                output.ResponseString = ResponseStringGlobals.INVALID_INPUT;
                output.ResponseBool = false;
                return output;

            }
            catch (UnauthorizedAccessException)
            {
                output.GenericObject = new Shelf();
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                output.ResponseBool = false;
                return output;
            }

            output = _shelfService.GetShelfByName(shelfName, username);

            return output;
        }

        // Out of Scope
        //public IComponent GetComponent(int index, string shelfName)
        //{
        //    // Add Business rules
        //    IComponent output = _shelfService.GetComponent(index, shelfName);

        //    return output;
        //}

        /// <summary>
        /// Private method to compare the current uesr to the
        /// user who is currently being viewed. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private void IsCurrentUser(string user)
        {
            if (_currentUser != user)
            {
                throw new UnauthorizedAccessException();
            }
        }

        private void IsAuthorized()
        {
            if (!AuthorizationCheck.IsAuthorized(_approvedRoles))
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
