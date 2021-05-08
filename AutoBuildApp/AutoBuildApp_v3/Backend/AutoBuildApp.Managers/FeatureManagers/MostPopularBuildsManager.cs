using AutoBuildApp.DomainModels;
using AutoBuildApp.DomainModels.Exceptions;
using AutoBuildApp.Logging;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Services;
using AutoBuildApp.Services.FeatureServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoBuildApp.Managers
{
    /// <summary>
    /// This class will consist of the methods used to validate
    /// buisness requirements that can be seen in the BRD.
    /// </summary>
    public class MostPopularBuildsManager
    {
        // Initialize the logger service locally.
        private readonly LoggingProducerService _logger;

        // creates a private variable of the service so its methods can be called.
        private readonly MostPopularBuildsService _mostPopularBuildsService;

        // Initialize a common response object
        private readonly CommonResponseWithObject<BuildPost> _commonResponse;

        // The registered user authorization check.
        private readonly List<string> _allowedRolesForViewing;

        // The unregistered user authorization check.
        private readonly List<string> _allowedRolesForPosting;

        /// <summary>
        /// This default constructor to initalize the service.
        /// </summary>
        /// <param name="mostPopularBuildsService">Takes in a service as a parameter to intialize it.</param>
        public MostPopularBuildsManager(MostPopularBuildsService mostPopularBuildsService)
        {
            _allowedRolesForViewing = new List<string>()
            {
                RoleEnumType.UnregisteredRole,
                RoleEnumType.BasicRole
            };

            _allowedRolesForPosting = new List<string>()
            {
                RoleEnumType.BasicRole
            };

            _logger = LoggingProducerService.GetInstance;
            _commonResponse = new CommonResponseWithObject<BuildPost>();
            _mostPopularBuildsService = mostPopularBuildsService;
        }

        /// <summary>
        /// This method will convert Forms to a BuildPost object.
        /// </summary>
        /// <param name="data">Takes in the data from the form.</param>
        /// <param name="image">Takes in a FormFile file.</param>
        /// <returns>returns a common response object with bool</returns>
        public CommonResponseWithObject<BuildPost> ConvertFormToBuildPost(IFormCollection data, List<IFormFile> image)
        {
            // Check authorization
            if (!AuthorizationCheck.IsAuthorized(_allowedRolesForPosting))
            {
                _logger.LogInformation("VendorLinking " + AuthorizationResultType.NotAuthorized.ToString());
                _commonResponse.ResponseString = "VendorLinking " + AuthorizationResultType.NotAuthorized.ToString();
                _commonResponse.IsSuccessful = false;

                return _commonResponse;
            }

            // This try catch catches a format exception or a null reference exception
            try
            {
                // Initialize a local BuildPost Object to store data into and then pass to the manager.
                var buildPost = new BuildPost()
                {
                    Username = data["username"],
                    Title = data["title"],
                    Description = data["description"],
                    BuildType = (BuildType)int.Parse(data["buildType"]),
                    BuildImagePath = data["buildImagePath"],
                    Image = image
                };

                _commonResponse.GenericObject = buildPost;
                _commonResponse.IsSuccessful = true;
                _commonResponse.ResponseString = "Successfully created a Build Post.";

                return _commonResponse;
            }

            // Catches common exceptions and returns generic response.
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    _logger.LogWarning(ex.Message);
                    _commonResponse.ResponseString = "One or more fields were empty.";
                }

                else if (ex is NullReferenceException)
                {
                    _logger.LogWarning(ex.Message);
                    _commonResponse.ResponseString = "Parameter was null.";
                }
                else
                {
                    _logger.LogWarning("An error occurred in vendor linking manager.");
                    _commonResponse.ResponseString = "An error occurred.";
                }

                _commonResponse.IsSuccessful = false;

                return _commonResponse;
            }
        }

        /// <summary>
        /// This method will be called to validate/invalidate BRD reqs.
        /// </summary>
        /// <param name="buildPost">takes in a build post object from the controller.</param>
        /// <returns>success state bool value.</returns>
        public async Task<bool> PublishBuild(BuildPost buildPost)
        {
            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRolesForPosting))
            {
                return false;
            }

            // This try/catch block checks for a null BuildPost object.
            try
            {
                if (buildPost == null)
                {
                    throw new ArgumentNullException("A null argument was passed through the method parameters");
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }

            // Logs the calling event of the method.
            _logger.LogInformation($"Most Popular Builds Manager PublishBuild was called for User:{buildPost.Username}");

            // This try/catch block checks for a null var in a BuildPost object.
            try
            {
                if (buildPost.Username == null || buildPost.Title == null || buildPost.Description == null)
                {
                    throw new NullReferenceException("A null object variable was passed through the method parameters");
                }
            }
            catch (NullReferenceException ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }

            // This var stores the maximum possible length of a title string.
            var maxTitleLength = 50;

            // This var stores the actual length of a title string.
            var actualTitleLength = buildPost.Title.Length;

            // This var stores the maximum possible length of a description string.
            var maxDescriptionLength = 10000;

            // The actual length of the build post description.
            var actualDescriptionLength = buildPost.Description.Length;

            // This try/catch block checks if the description length is greater than 10,000 characters,
            // if it is greater than 10,000 characters, it will throw a custom exception, if it is less
            // than, the method will continue.
            try
            {
                if (actualDescriptionLength > maxDescriptionLength)
                {
                    throw new StringTooLongException($"Build Post description length: {actualDescriptionLength} exceeds {maxDescriptionLength} for User:{buildPost.Username}");
                }
            }
            // Exception is caught from above and is returned false to the controller.
            catch (StringTooLongException ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }

            // This try/catch block checks if the title length is greater than 50 characters
            // if it is greater than 50 characters, it will throw a custom exception, if it is less
            // than, the method will continue.
            try
            {
                if (actualTitleLength > maxTitleLength)
                {
                    throw new StringTooLongException($"Build Post title length: {actualTitleLength} exceeds {maxTitleLength} for User:{buildPost.Username}");
                }
            }
            catch (StringTooLongException ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }

            buildPost.LikeIncrementor = 0; // A new post should have a start of 0 likes. This ensures that.

            buildPost.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // sets the current date time to this variable.

            // async call to save an image to a filepath.
            buildPost.BuildImagePath = await _mostPopularBuildsService.UploadImage(buildPost.Username, buildPost.Image);

            // TODO: implement service return method.
            return _mostPopularBuildsService.PublishBuild(buildPost);
        }

        /// <summary>
        /// This method returns a single build post from manager
        /// </summary>
        /// <param name="buildId">takes in an id string</param>
        /// <returns>returns a build post.</returns>
        public BuildPost GetBuildPost(string buildId)
        {
            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRolesForViewing))
            {
                return null;
            }

            // Log the manager get build posts being called
            _logger.LogInformation("Most Popular Builds Manager GetBuildPost was called.");

            // This try/catch block checks for a null string.
            try
            {
                if (buildId == null)
                {
                    throw new ArgumentNullException("A null argument was passed through the method parameters");
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex.Message);
                return null;
            }

            return _mostPopularBuildsService.GetBuildPost(buildId);
        }

        /// <summary>
        /// This method will be called to validate/invalidate BRD reqs.
        /// </summary>
        /// <param name="orderLikes">query string to determing which order to list elements by likes</param>
        /// <param name="buildType">query string to determinw which order to list builds</param>
        /// <returns>retruns a list of Build Posts.</returns>
        public List<BuildPost> GetBuildPosts(string orderLikes, string buildType)
        {
            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRolesForViewing))
            {
                return null;
            }

            // Log the manager get build posts being called
            _logger.LogInformation("Most Popular Builds Manager GetBuildPosts was called.");

            var defaultOrderLikes = "";
            var defaultBuildType = "";

            // The following if conditions will check all possible conditions and assign values accordingly,
            // else it will insert the default parameters.
            // conditions: GA and ASC, G and ASC, WP and ASC
            //             GA and DESC, G and DESC, WP and DESC
            //             GA, G, WP
            //             ASC
            // default:    DESC

            if ((buildType == "BuildType_GraphicArtist" && (orderLikes == "AscendingLikes" || orderLikes == "DescendingLikes"))|| 
                (buildType == "BuildType_Gaming" && (orderLikes == "AscendingLikes" || orderLikes == "DescendingLikes")) || 
                (buildType == "BuildType_WordProcessing" && (orderLikes == "AscendingLikes" || orderLikes == "DescendingLikes")))
            {
                return _mostPopularBuildsService.GetBuildPosts(orderLikes, buildType);
            }

            if (buildType == "BuildType_GraphicArtist" || buildType == "BuildType_Gaming" || buildType == "BuildType_WordProcessing")
            {
                return _mostPopularBuildsService.GetBuildPosts(defaultOrderLikes, buildType);
            }

            if (orderLikes == "AscendingLikes")
            {
                return _mostPopularBuildsService.GetBuildPosts(orderLikes, defaultBuildType);
            }

            return _mostPopularBuildsService.GetBuildPosts(defaultOrderLikes, defaultBuildType);
        }

        /// <summary>
        /// This is the manager method that calls the add like service function.
        /// </summary>
        /// <param name="like">takes in a like object to send to service.</param>
        /// <returns>returns a success state bool.</returns>
        public bool AddLike(Like like)
        {
            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRolesForPosting))
            {
                return false;
            }

            // This try/catch block checks for a null BuildPost object.
            try
            {
                if (like == null)
                {
                    throw new ArgumentNullException("A null argument was passed through the method parameters");
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }

            // This try/catch block checks for a null var in a BuildPost object.
            try
            {
                if (like.UserId == null || like.PostId == null)
                {
                    throw new NullReferenceException("A null object variable was passed through the method parameters");
                }
            }
            catch (NullReferenceException ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }

            // logs when the method is called.
            _logger.LogInformation($"Most Popular Builds Manager addLike was called for user:{like.UserId} and post:{like.PostId}");
            return _mostPopularBuildsService.AddLike(like);
        }
    }
}
