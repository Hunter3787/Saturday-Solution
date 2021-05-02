using AutoBuildApp.Models.Interfaces;
using AutoBuildApp.DataAccess;
using System.Collections.Generic;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Services;
using AutoBuildApp.DataAccess.Abstractions;
using AutoBuildApp.Models;
using AutoBuildApp.Managers.Guards;
using System.Threading;
using System.Security.Claims;
using AutoBuildApp.Security;

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
        private readonly IClaims _basic, _vendor, _developer, _admin;
        private ShelfService _shelfService;
        private BuildManagementService _buildService;
        private readonly string _currentUser;

        public UserGarageManager(string connectionString)
        {
            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            _basic = claimsFactory.GetClaims(RoleEnumType.BASIC_ROLE);
            _vendor = claimsFactory.GetClaims(RoleEnumType.VENDOR_ROLE);
            _admin = claimsFactory.GetClaims(RoleEnumType.DELEGATE_ADMIN);

            _buildDAO = new BuildDAO(connectionString);
            _shelfDAO = new ShelfDAO(connectionString);
            _shelfService = new ShelfService(_shelfDAO);
            _buildService = new BuildManagementService(_buildDAO);
            _currentUser = Thread.CurrentPrincipal.Identity.Name;
        }

        public IMessageResponse AddBuild(IBuild build, string buildname)
        {
            // temp
            if (!IsAuthorized())
            {
                return new StringBoolResponse()
                {
                    MessageString = ResponseStringGlobals.UNAUTHORIZED_ACCESS,
                    SuccessBool = false
                };
            }
            NullGuard.IsNotNull(build);
            NullGuard.IsNotNullOrEmpty(buildname);

            IMessageResponse response = _buildService.AddBuild(build, buildname, _currentUser);

            return response;
        }

        public IMessageResponse CopyBuildToGarage(string buildID)
        {
            NullGuard.IsNotNullOrEmpty(buildID);

            IMessageResponse response = _buildService.CopyBuild();

            return response;
        }

        public IMessageResponse DeleteBuild(string buildID)
        {
            NullGuard.IsNotNullOrEmpty(buildID);

            IMessageResponse response = _buildService.DeleteBuild();

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

        public IMessageResponse PublishBuild(string buildID)
        {
            NullGuard.IsNotNullOrEmpty(buildID);

            IMessageResponse response = _buildService.PublishBuild();

            return response;
        }

        public IMessageResponse ModifyBuild(IBuild build, string oldName, string newName)
        {
            NullGuard.IsNotNull(build);

            IMessageResponse response = _buildService.ModifyBuild();

            return response;
        }

        // Starting point
        public IMessageResponse CreateShelf(string shelfName, string user)
        {
            NullGuard.IsNotNullOrEmpty(shelfName);
            NullGuard.IsNotNullOrEmpty(user);

            // TODO:Add input validation.
            IMessageResponse response = _shelfService.CreateShelf(shelfName, user);

            return response;
        }

        public IMessageResponse RenameShelf(string from, string to, string user)
        {
            // TODO:Add input validation.
            IMessageResponse response = _shelfService.ChangeShelfName(from, to, user);

            return response;
        }

        public IMessageResponse DeleteShelf(string shelfName, string user)
        {
            // Add Business rules

            // If(user == current)
            IMessageResponse response = _shelfService.DeleteShelf(shelfName);

            return response;
        }

        public IMessageResponse AddToShelf(IComponent item, string shelfName, string user)
        {
            // Add Business rules
            IMessageResponse response = _shelfService.AddToShelf(item, shelfName, user);

            return response;
        }

        public IMessageResponse RemoveFromShelf(int itemIndex, string shelfName)
        {
            // Add Business rules
            IMessageResponse response = _shelfService.RemoveFromShelf(itemIndex, shelfName);

            return response;
        }

        public IMessageResponse ModifyCount(int count, string itemID, string shelfName)
        {
            // Add Business rules
            IMessageResponse response = _shelfService.ChangeQuantity(count, itemID, shelfName);

            return response;
        }

        public IMessageResponse MoveItemOnShelf(int indexStart, int indexEnd, string user)
        {
            // Add business rules
            IMessageResponse response = _shelfService.ModifyShelf(indexStart, indexEnd, user);

            return response;
        }

        public List<IComponent> GetShelf(string shelfName)
        {
            // Add Business rules
            List<IComponent> outputList = _shelfService.GetShelf(shelfName);

            return outputList;
        }

        public IComponent GetComponent(int index, string shelfName)
        {
            // Add Business rules
            IComponent output = _shelfService.GetComponent(index, shelfName);

            return output;
        }

        /// <summary>
        /// Private method to compare the current uesr to the
        /// user who is currently being viewed. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private bool IsCurrentUser(string user)
        {
            var valid = false;

            if (_currentUser == user)
            {
                valid = true;
            }

            return valid;
        }

        private bool IsAuthorized()
        {
            return AuthorizationService.checkPermissions(_basic.Claims())
                || AuthorizationService.checkPermissions(_vendor.Claims())
                || AuthorizationService.checkPermissions(_developer.Claims())
                || AuthorizationService.checkPermissions(_admin.Claims());
        }
    }
}
