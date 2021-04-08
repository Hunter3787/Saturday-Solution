using AutoBuildApp.DomainModels;
using AutoBuildApp.DomainModels.Exceptions;
using AutoBuildApp.Services;
using AutoBuildApp.Services.FeatureServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AutoBuildApp.Managers
{
    /// <summary>
    /// This class will consist of the methods used to validate
    /// buisness requirements that can be seen in the BRD.
    /// </summary>
    public class MostPopularBuildsManager
    {
        // Initialize the logger service locally.
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;

        // creates a private variable of the service so its methods can be called.
        private readonly MostPopularBuildsService _mostPopularBuildsService;

        /// <summary>
        /// This default constructor to initalize the service.
        /// </summary>
        /// <param name="mostPopularBuildsService">Takes in a service as a parameter to intialize it.</param>
        public MostPopularBuildsManager(MostPopularBuildsService mostPopularBuildsService)
        {
            _mostPopularBuildsService = mostPopularBuildsService;
        }

        /// <summary>
        /// This method will be called to validate/invalidate BRD reqs.
        /// </summary>
        /// <param name="buildPost">takes in a build post object from the controller.</param>
        /// <returns>success state bool value.</returns>
        public bool PublishBuild(BuildPost buildPost)
        {
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
                Console.WriteLine(ex.Message);
                return false;
            }

            // This try/catch block checks for a null var in a BuildPost object.
            try
            {
                if (buildPost.Username == null || buildPost.Title == null || buildPost.Description == null ||
                    buildPost.BuildImagePath == null || buildPost.DateTime == null)
                {
                    throw new NullReferenceException("A null object variable was passed through the method parameters");
                }
            }
            catch (NullReferenceException ex)
            {
                _logger.LogWarning(ex.Message);
                Console.WriteLine(ex.Message);
                return false;
            }

            // Logs the calling event of the method.
            _logger.LogInformation($"Most Popular Builds Manager PublishBuild was called for User:{buildPost.Username}");

            // This var stores the maximum possible length of a title string.
            var maxTitleLength = 50;

            // This var stores the actual length of a title string.
            var actualTitleLength = buildPost.Title.Length;

            // This var stores the maximum possible length of a description string.
            var maxDescriptionLength = 10000;

            // The actual length of the build post description.
            var actualDescriptionLength = buildPost.Description.Length;

            // This sets the characters that will be checked to see if they are valid.
            var allowedChars = new Regex(@"^[a-zA-Z0-9 !]*$");

            // This will make sure that only the allowed characters are used.
            var allowed = allowedChars.Match(buildPost.Title);

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
                Console.WriteLine(ex.Message);
                return false;
            }

            // This try/catch block checks if the regex is less fulfilled.
            // if it's not, it will throw a custom exception, if does pass
            // then the method will continue.
            try
            {
                if(!allowed.Success)
                {
                    throw new InvalidUsernameException($"Build Post title: {buildPost.Title} contains invalid characters for User:{buildPost.Username}");
                }
            }
            catch(InvalidUsernameException ex)
            {
                _logger.LogWarning(ex.Message);
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
                return false;
            }

            // TODO: implement service return method.
            return _mostPopularBuildsService.PublishBuild(buildPost);
        }

        // TODO: create a manager method for retrieving builds from the DB, doing BRD validation checks
    }
}
