using AutoBuildApp.Models;
using System.Collections.Generic;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Services;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers.Guards;
using System.Threading;
using AutoBuildApp.Security;
using System;
using AutoBuildApp.Models.Builds;
using AutoBuildApp.Models.Products;
using AutoBuildApp.Logging;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.DomainModels;

/**
* User Garage Manager class that directs 
* operations with regards to the user garage.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Managers
{
    public class UserGarageManager
    {
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;
        private readonly BuildDAO _buildDAO;
        private readonly ShelfDAO _shelfDAO;
        private readonly List<string> _approvedRoles;
        private readonly ShelfWithResponseService _shelfService;
        private readonly BuildWithResponseService _buildService;
        private readonly string _currentUser;

        public UserGarageManager(string connectionString)
        {
            _approvedRoles = new List<string>()
            {
                RoleEnumType.BasicRole,
                RoleEnumType.DelegateAdmin,
                RoleEnumType.VendorRole,
                RoleEnumType.SystemAdmin
            };

            _buildDAO = new BuildDAO(connectionString);
            _shelfDAO = new ShelfDAO(connectionString);
            _shelfService = new ShelfWithResponseService(_shelfDAO);
            _buildService = new BuildWithResponseService(_buildDAO);
            _currentUser = Thread.CurrentPrincipal.Identity.Name;
        }

        public CommonResponse AddBuild(Build build, string buildname)
        {
            CommonResponse output = new CommonResponse();
            try
            {
                NullGuard.IsNotNull(build);
                NullGuard.IsNotNullOrEmpty(buildname);
                IsAuthorized();
            }
            catch (UnauthorizedAccessException)
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                output.IsSuccessful = false;
                return output;
            }
            catch (ArgumentNullException)
            {

                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                output.IsSuccessful = false;
                return output;
            }

            CommonResponse response = _buildService.AddBuild(build, buildname, _currentUser);

            return response;
        }


        public CommonResponse CopyBuildToGarage(string buildName)
        {
            CommonResponse output = new CommonResponse();
            try
            {
                NullGuard.IsNotNullOrEmpty(buildName);
                IsAuthorized();
            }
            catch (UnauthorizedAccessException)
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                output.IsSuccessful = false;
                return output;
            }
            catch (ArgumentNullException)
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                output.IsSuccessful = false;
                return output;
            }

            CommonResponse response = _buildService.CopyBuild();

            return response;
        }

        //public CommonResponse DeleteBuild(string buildID)
        //{
        //    NullGuard.IsNotNullOrEmpty(buildID);

        //    CommonResponse response = _buildService.DeleteBuild();

        //    return response;
        //}
        public CommonResponse DeleteBuild(string buildName)
        {
            NullGuard.IsNotNullOrEmpty(buildName);

            CommonResponse response = new CommonResponse();
             response = _buildService.DeleteBuild(buildName);

            return response;
        }



        public List<Build> GetAllUserBuilds(string user, string sorting)
        {
            List<Build> outputList;
            var order = sorting;

            // TODO:Fix guards and parameters.
            NullGuard.IsNotNullOrEmpty(user);
            if (string.IsNullOrEmpty(sorting))
            {
                order = UserGarageGlobals.DEFAULT_SORT;
            }

            outputList = _buildService.GetAllUserBuilds( order);

            return outputList;
        }

        public CommonResponse PublishBuild(BuildPost BuildPost)
        {
            // let us cast into enitity


            CommonResponse response = _buildService.PublishBuild(BuildPost);

            return response;
        }


        public CommonResponse AddRecomendedBuild
            (IList<string> modelNumbers, string buildName)
        {
            CommonResponse response = new CommonResponse();
            try
            {

                NullGuard.IsNotNullOrEmpty(buildName);
                response = _buildService.AddRecomendedBuild(modelNumbers, buildName);

                return response;
            }
            catch(ArgumentNullException ex)
            {
                response.ResponseString = ResponseStringGlobals.INVALID_INPUT;
                return response;
            }
        }




        public CommonResponse ModifyBuild(Build build, string oldName, string newName)
        {
            NullGuard.IsNotNull(build);

            CommonResponse response = _buildService.ModifyBuild();

            return response;
        }
        public CommonResponse UpdateProductsInBuild(string buildName, string modleNumber, int quantity)
        {
            NullGuard.IsNotNull(buildName);
            NullGuard.IsNotNull(modleNumber);
            NullGuard.IsNotNull(quantity);

            CommonResponse response = _buildService.ModifyProductsInBuild(buildName, modleNumber, quantity);

            return response;
        }

        /// <summary>
        /// Create shelf under current user's garage.
        /// </summary>
        /// <param name="shelfName"></param>
        /// <returns></returns>
        public CommonResponse CreateShelf(string shelfName)
        {
            CommonResponse output = new CommonResponse()
            {
                IsSuccessful = false,
            };

            try
            {
                IsAuthorized();
                NullGuard.IsNotNullOrEmpty(shelfName);
            }
            catch (ArgumentNullException)
            {
                output.ResponseString = ResponseStringGlobals.INVALID_INPUT;
                return output;

            }
            catch (UnauthorizedAccessException)
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                return output;
            }

            output = _shelfService.CreateShelf(shelfName.Trim(), _currentUser);

            return output;
        }

        /// <summary>
        /// Rename a shelf in a user's garage.
        /// </summary>
        /// <param name="oldShelfName"></param>
        /// <param name="newShelfName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public CommonResponse RenameShelf(string oldShelfName, string newShelfName)
        {
            CommonResponse output = new CommonResponse()
            {
                IsSuccessful = false,
            };

            try
            {
                IsAuthorized();
                NullGuard.IsNotNullOrEmpty(oldShelfName);
                NullGuard.IsNotNullOrEmpty(newShelfName);
            }
            catch (ArgumentNullException)
            {
                output.ResponseString = ResponseStringGlobals.INVALID_INPUT;
                return output;

            }
            catch (UnauthorizedAccessException)
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                return output;
            }

            // Checks to see if the new name matches the old name.
            if(oldShelfName == newShelfName.Trim())
            {
                output.ResponseString = ResponseStringGlobals.DUPLICATE_VALUE;
                return output;
            }

            // Businesss rule to keep names
            // from being repeated by adding spaces.
            output = _shelfService.ChangeShelfName(
                oldShelfName,
                newShelfName.Trim(),
                _currentUser);

            return output;
        }

        /// <summary>
        /// Delete a shelf from a user's account.
        /// </summary>
        /// <param name="shelfName"></param>
        /// <returns></returns>
        public CommonResponse DeleteShelf(string shelfName)
        {
            CommonResponse output = new CommonResponse()
            {
                IsSuccessful = false,
            };

            try
            {
                IsAuthorized();
                NullGuard.IsNotNullOrEmpty(shelfName);
            }
            catch (ArgumentNullException)
            {
                output.ResponseString = ResponseStringGlobals.INVALID_INPUT;
                return output;

            }
            catch (UnauthorizedAccessException)
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                return output;
            }

            CommonResponse response = _shelfService.DeleteShelf(shelfName,_currentUser);

            return response;
        }

        /// <summary>
        /// Add an item to a user's shelf.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="shelfName"></param>
        /// <returns></returns>
        public CommonResponse AddToShelf(Component component, string shelfName)
        {
            CommonResponse output = new CommonResponse()
            {
                IsSuccessful = false,
            };

            try
            {
                IsAuthorized();
                NullGuard.IsNotNullOrEmpty(shelfName);
                NullGuard.IsNotNull(component);
            }
            catch (ArgumentNullException)
            {
                output.ResponseString = ResponseStringGlobals.INVALID_INPUT;
                return output;

            }
            catch (UnauthorizedAccessException)
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                return output;
            }

            // Business rule, prevent negative quantity.
            if (component.Quantity < UserGarageGlobals.MIN_INDEX)
            {
                output.ResponseString = ResponseStringGlobals.INVALID_INPUT;
                return output;
            }

            // Add Business rules
            output = _shelfService.AddToShelf(
                component.ModelNumber,
                component.Quantity,
                shelfName,
                _currentUser);

            return output;
        }

        /// <summary>
        /// Remove an item from the user's shelf.
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="shelfName"></param>
        /// <returns></returns>
        public CommonResponse RemoveFromShelf(int itemIndex, string shelfName)
        {
            CommonResponse output = new CommonResponse()
            {
                IsSuccessful = false,
            };

            try
            {
                IsAuthorized();
                NullGuard.IsNotNullOrEmpty(shelfName);
                NullGuard.IsNotNull(itemIndex);
            }
            catch (ArgumentNullException)
            {
                output.ResponseString = ResponseStringGlobals.INVALID_INPUT;
                return output;

            }
            catch (UnauthorizedAccessException)
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                return output;
            }

            // Business rule, prevent negative indices.
            if(itemIndex < UserGarageGlobals.MIN_INDEX)
            {
                output.ResponseString = ResponseStringGlobals.INVALID_INPUT;
                return output;
            }

            output= _shelfService.RemoveFromShelf(itemIndex, shelfName, _currentUser);

            return output;
        }

        /// <summary>
        /// Update the quantity of a product on the user's shelf.
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="quantity"></param>
        /// <param name="shelfName"></param>
        /// <returns></returns>
        public CommonResponse UpdateQuantity(int itemIndex, int quantity, string shelfName)
        {
            CommonResponse output = new CommonResponse()
            {
                IsSuccessful = false,
            };

            try
            {
                IsAuthorized();
                NullGuard.IsNotNullOrEmpty(shelfName);
                NullGuard.IsNotNull(itemIndex);
                NullGuard.IsNotNull(quantity);
            }
            catch (ArgumentNullException)
            {
                output.ResponseString = ResponseStringGlobals.INVALID_INPUT;
                return output;

            }
            catch (UnauthorizedAccessException)
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                return output;
            }

            // Quantity must be at least 1.
            if(quantity <= UserGarageGlobals.MIN_INTEGER_VALUE)
            {
                output.ResponseString = ResponseStringGlobals.INVALID_INPUT;
                return output;
            }

            output = _shelfService.UpdateQuantity(itemIndex, quantity, shelfName, _currentUser);

            return output;
        }

        public CommonResponse MoveItemOnShelf(
            List<int> indices,
            string shelfName,
            string username)
        {
            // Add business rules
            CommonResponse response = _shelfService.ReorderShelf(indices, shelfName, username);
            // TODO
            return response;
        }

        /// <summary>
        /// Requests all shelves based on a username.
        /// </summary>
        /// <param name="shelfName"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public CommonResponseWithObject<Shelf> GetShelfByName(string shelfName)
        {
            CommonResponseWithObject<Shelf> output = new CommonResponseWithObject<Shelf>()
            {
                IsSuccessful = false,
                GenericObject = new Shelf()
            };

            try
            {
                IsAuthorized();
                NullGuard.IsNotNullOrEmpty(shelfName);
            }
            catch (ArgumentNullException)
            {
                output.ResponseString = ResponseStringGlobals.INVALID_INPUT; 
                return output;

            }
            catch (UnauthorizedAccessException)
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                return output;
            }

            output = _shelfService.GetShelfByName(shelfName, Thread.CurrentPrincipal.Identity.Name);

            return output;
        }

        /// <summary>
        /// Requests all shelves based on a username.
        /// </summary>
        /// <param name="shelfName"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public CommonResponseWithObject<List<Shelf>> GetShelvesByUser()
        {
            CommonResponseWithObject<List<Shelf>> output = new CommonResponseWithObject<List<Shelf>>()
            {
                IsSuccessful = false,
                GenericObject = new List<Shelf>()
            };

            try
            {
                IsAuthorized();
            }
            catch (ArgumentNullException)
            {
                output.ResponseString = ResponseStringGlobals.INVALID_INPUT;
                return output;

            }
            catch (UnauthorizedAccessException)
            {
                output.ResponseString = ResponseStringGlobals.UNAUTHORIZED_ACCESS;
                return output;
            }

            output = _shelfService.GetShelvesByUser(Thread.CurrentPrincipal.Identity.Name);

            return output;
        }

        // Out of Scope
        //public IComponent GetComponent(int index, string shelfName)
        //{
        //    // Add Business rules
        //    IComponent output = _shelfService.GetComponent(index, shelfName);

        //    return output;
        //}

        // Unused at this time.
        ///// <summary>
        ///// Private method to compare the current uesr to the
        ///// user who is currently being viewed. 
        ///// </summary>
        ///// <param name="user"></param>
        ///// <returns></returns>
        //private void IsCurrentUser(string user)
        //{
        //    if (_currentUser != user)
        //    {
        //        throw new UnauthorizedAccessException();
        //    }
        //}

        /// <summary>
        /// Helper to perform and throw exception
        /// to be handled if user is unauthorized.
        /// </summary>
        private void IsAuthorized()
        {
            if (!AuthorizationCheck.IsAuthorized(_approvedRoles))
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
