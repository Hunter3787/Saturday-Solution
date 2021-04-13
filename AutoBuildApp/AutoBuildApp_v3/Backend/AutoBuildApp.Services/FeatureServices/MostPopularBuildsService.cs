using AutoBuildApp.DataAccess;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Services.FeatureServices
{
    /// <summary>
    /// This class will handle the data transferrance from the manager
    /// to the DAO entity object.
    /// </summary>
    public class MostPopularBuildsService
    {
        // Initializes the logger.
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;

        // Initialize a private DAO inside of the serice so that any method can call DAO methods.
        private readonly MostPopularBuildsDAO _mostPopularBuildsDAO;

        /// <summary>
        /// This will initialize the private DAO with the one that is passed in.
        /// </summary>
        /// <param name="mostPopularBuildsDAO">Takes in a MPB DAO to use for initlilization.</param>
        public MostPopularBuildsService(MostPopularBuildsDAO mostPopularBuildsDAO)
        {
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
            // Logs the event of the service publish method being called
            _logger.LogInformation($"Most Popular Builds Service Publish Build was called for User:{buildPost.Username}");

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
        public bool addLike(Like like)
        {
            // Logs the event of the service addLike method being called
            _logger.LogInformation($"Most Popular Builds Service addLike was called for user:{like.UserId} and post{like.PostId}");

            var likeEntity = new LikeEntity()
            {
                PostId = like.PostId,
                UserId = like.UserId
            };

            return _mostPopularBuildsDAO.AddLike(likeEntity);
        }
    }
}
