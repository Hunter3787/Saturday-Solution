﻿using AutoBuildApp.DataAccess;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Logging;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Security;
using AutoBuildApp.Security.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoBuildApp.Services.FeatureServices
{
    /// <summary>
    /// This class will handle the data transferrance from the manager
    /// to the DAO entity object.
    /// </summary>
    public class MostPopularBuildsService
    {
        // Initializes the logger.
        private readonly LoggingProducerService _logger;

        // Initialize a private DAO inside of the serice so that any method can call DAO methods.
        private readonly MostPopularBuildsDAO _mostPopularBuildsDAO;

        // Initialize a common response object
        private readonly CommonResponseWithObject<BuildPost> _commonResponse;

        // The registered user authorization check.
        private readonly List<string> _allowedRolesForViewing;

        // The unregistered user authorization check.
        private readonly List<string> _allowedRolesForPosting;

        /// <summary>
        /// This will initialize the private DAO with the one that is passed in.
        /// </summary>
        /// <param name="mostPopularBuildsDAO">Takes in a MPB DAO to use for initlilization.</param>
        public MostPopularBuildsService(MostPopularBuildsDAO mostPopularBuildsDAO)
        {
            _allowedRolesForViewing = new List<string>()
            {
                RoleEnumType.UnregisteredRole,
                RoleEnumType.BasicRole
            };

            _allowedRolesForPosting = new List<string>()
            {
                RoleEnumType.BasicRole,
                RoleEnumType.DelegateAdmin,
                RoleEnumType.SystemAdmin,
                RoleEnumType.VendorRole
            };

            _logger = LoggingProducerService.GetInstance;
            _commonResponse = new CommonResponseWithObject<BuildPost>();
            _mostPopularBuildsDAO = mostPopularBuildsDAO;
        }

        /// <summary>
        /// This method is the data transference service from
        /// domain model objects to entity objects.
        /// </summary>
        /// <param name="buildPost">takes in a BuildPost object from the manager</param>
        /// <returns>returns a bool marking its success or failure.</returns>
        public bool PublishBuild(BuildPost buildPost)
        {

            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRolesForPosting))
            {
                return false;
            }

            // Logs the event of the service publish method being called
            _logger.LogInformation($"Most Popular Builds Service Publish Build was called"); //replace this with username

            var buildPostEntity = new BuildPostEntity()
            {
                Username = buildPost.Username,
                Title = buildPost.Title,
                Description = buildPost.Description,
                LikeIncrementor = buildPost.LikeIncrementor,
                BuildTypeValue = (int)buildPost.BuildType,
                BuildImagePath = buildPost.BuildImagePath,
                DateTime = buildPost.DateTime
            };

            return _mostPopularBuildsDAO.PublishBuildRecord(buildPostEntity);
            //return _mostPopularBuildsDAO.PublishBuildRecord(buildPostEntity, "mostpopularbuilds"); // This is the reflections method of the DAO (not currently optimized).
        }

        /// <summary>
        /// This method retrieves and parses DB object data to Domain Model objects.
        /// </summary>
        /// <param name="orderLikes">takes in the query condition for the likes.</param>
        /// <param name="buildType">takes in the query condition for the build type.</param>
        /// <returns>returns a list of build posts</returns>
        public List<BuildPost> GetBuildPosts(string orderLikes, string buildType)
        {


            // Logs the event of getting build posts in the service layer.
            _logger.LogInformation("Most Popular Builds Service GetBuildPosts was called.");

            // stores the list retreived from the DB into a local List var.
            var buildPostEntities = _mostPopularBuildsDAO.GetAllBuildPostRecordsByQuery(orderLikes, buildType);

            // create a list of build posts that will be appended to and returned to the manager.
            var buildPosts = new List<BuildPost>();

            // This for loop will iterate through the list of entities and incrememntally transfer
            // its data to a new list of build posts.
            foreach (BuildPostEntity buildPostEntity in buildPostEntities)
            {
                var buildPost = new BuildPost()
                {
                    EntityId = buildPostEntity.EntityId,
                    Username = buildPostEntity.Username,
                    Title = buildPostEntity.Title,
                    Description = buildPostEntity.Description,
                    LikeIncrementor = buildPostEntity.LikeIncrementor,
                    BuildType = (BuildType)buildPostEntity.BuildTypeValue,
                    BuildImagePath = buildPostEntity.BuildImagePath,
                    DateTime = buildPostEntity.DateTime
                };
                buildPosts.Add(buildPost);
            }

            // TODO: Check for false or null values??

            return buildPosts;
        }

        /// <summary>
        /// This method transfers data from a domain to a entity.
        /// </summary>
        /// <param name="like">takes in a log object</param>
        /// <returns>returns a success state bool.</returns>
        public bool AddLike(Like like)
        {
            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRolesForPosting))
            {
                return false;
            }

            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string username = _threadPrinciple.Identity.Name;

            // Logs the event of the service addLike method being called
            _logger.LogInformation($"Most Popular Builds Service addLike was called for user:{like.UserId} and post{like.PostId}");

            var likeEntity = new LikeEntity()
            {
                PostId = like.PostId,
                UserId = username
            };

            return _mostPopularBuildsDAO.AddLike(likeEntity);
        }

        /// <summary>
        /// This method returns a build post from the DAO.
        /// </summary>
        /// <param name="buildId">takes in an ID.</param>
        /// <returns>retruns a build post object.</returns>
        public BuildPost GetBuildPost(string buildId)
        {


            // Logs the event of getting build posts in the service layer.
            _logger.LogInformation("Most Popular Builds Service GetBuildPost was called.");

            // stores the list retreived from the DB into a local List var.
            var buildPostEntity = _mostPopularBuildsDAO.GetBuildPostRecord(buildId);

            // This will translate entities back into domain models.
            var buildPost = new BuildPost()
            {
                EntityId = buildPostEntity.EntityId,
                Username = buildPostEntity.Username,
                Title = buildPostEntity.Title,
                Description = buildPostEntity.Description,
                LikeIncrementor = buildPostEntity.LikeIncrementor,
                BuildType = (BuildType)buildPostEntity.BuildTypeValue,
                BuildImagePath = buildPostEntity.BuildImagePath,
                DateTime = buildPostEntity.DateTime
            };

            return buildPost;
        }

        /// <summary>
        /// This method takes in a list of files and uploads it to a folder.
        /// </summary>
        /// <param name="username">takes in a username string</param>
        /// <param name="files">takes in a list of IFormFile</param>
        /// <returns>returns boolean success state.</returns>
        public async Task<string> UploadImage(string username, List<IFormFile> files)
        {
            string storeIn = " ";

            // Authorization check
            if (!AuthorizationCheck.IsAuthorized(_allowedRolesForPosting))
            {
                return null;
            }

            if (files == null)
                return storeIn;

            foreach (var item in files)
            {
                if (item.Length > 0)
                {
                    var currentDirectory = Directory.GetCurrentDirectory().ToString();

                    _logger.LogWarning(currentDirectory);

                    storeIn = $"/assets/images/MPB/{username}_{ DateTime.UtcNow.ToString("yyyyMMdd_hh_mm_ss_ms")}.jpg";

                    var path = Path.GetFullPath(Path.Combine(currentDirectory, $@"..\..\FrontEnd{storeIn}"));

                    _logger.LogWarning(path);

                    try
                    {
                        using (var stream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite))
                        {
                            await item.CopyToAsync(stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex.Message);
                    }

                }
            }
            return storeIn;
        }
    }
}
