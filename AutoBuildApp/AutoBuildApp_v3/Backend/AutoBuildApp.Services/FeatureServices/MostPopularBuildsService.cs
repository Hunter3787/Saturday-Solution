using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.DomainModels;
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
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance; // This will get the logger so it can be used.

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

            return true;
        }
    }
}
