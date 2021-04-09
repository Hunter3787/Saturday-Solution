using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Managers;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Services.FeatureServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Manger.Tests
{
    /// <summary>
    /// This class will test all methods that have been created as well as verify that they do indeed work.
    /// and that the BRD reqs. are validated.
    /// </summary>
    [TestFixture]
    public class MostPopularBuildsTests
    {
        /// <summary>
        /// This test will check if the publish build method will return false
        /// if the object passed through is null.
        /// </summary>
        [Test]
        public void MostPopularBuilds_PublishBuild_ReturnFalseIfObjectIsNull()
        {
            // Arrange
            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            BuildPost buildPost = null;

            // Act
            var result = mostPopularBuildsManager.PublishBuild(buildPost);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// This test will check if the publish build method will return false
        /// if the object has any vars that are null.
        /// </summary>
        [Test]
        public void MostPopularBuilds_PublishBuild_ReturnFalseIfAnyNullVarsInBuildPostObject()
        {
            // Arrange
            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            bool result = false;

            // Act
            var nullUsernameBuildPost = new BuildPost()
            {
                Username = null,
                Title = "Test Title",
                Description = "Test Desctiption",
                LikeIncrementor = 0,
                BuildType = BuildType.None,
                BuildImagePath = @"C:\Users\Serge\Desktop\images\test.jpg",
                DateTime = "2021"
            };

            var nullUsernameResult = mostPopularBuildsManager.PublishBuild(nullUsernameBuildPost);

            var nullTitleBuildPost = new BuildPost()
            {
                Username = "Test Username",
                Title = null,
                Description = "Test Desctiption",
                LikeIncrementor = 0,
                BuildType = BuildType.None,
                BuildImagePath = @"C:\Users\Serge\Desktop\images\test.jpg",
                DateTime = "2021"
            };

            var nullTitleResult = mostPopularBuildsManager.PublishBuild(nullTitleBuildPost);

            var nullDescriptionBuildPost = new BuildPost()
            {
                Username = "Test Username",
                Title = "Test Title",
                Description = null,
                LikeIncrementor = 0,
                BuildType = BuildType.None,
                BuildImagePath = @"C:\Users\Serge\Desktop\images\test.jpg",
                DateTime = "2021"
            };

            var nullDescriptionResult = mostPopularBuildsManager.PublishBuild(nullDescriptionBuildPost);

            var nullImagePathBuildPost = new BuildPost()
            {
                Username = "Test Username",
                Title = "Test Title",
                Description = "Test Desctiption",
                LikeIncrementor = 0,
                BuildType = BuildType.None,
                BuildImagePath = null,
                DateTime = "2021"
            };

            var nullImagePathResult = mostPopularBuildsManager.PublishBuild(nullImagePathBuildPost);

            var nullDateTimeBuildPost = new BuildPost()
            {
                Username = "Test Username",
                Title = "Test Title",
                Description = "Test Desctiption",
                LikeIncrementor = 0,
                BuildType = BuildType.None,
                BuildImagePath = @"C:\Users\Serge\Desktop\images\test.jpg",
                DateTime = null
            };

            var nullDateTimeResult = mostPopularBuildsManager.PublishBuild(nullDateTimeBuildPost);

            if(!nullUsernameResult && !nullTitleResult && !nullDescriptionResult && 
               !nullImagePathResult && !nullDateTimeResult)
            {
                result = true;
            }

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// This test will check if the publish build method will return false
        /// if the title length string surpasses 50 characters.
        /// </summary>
        [Test]
        public void MostPopularBuilds_PublishBuild_ReturnFalseIfTitleCharsAreGreaterThan50()
        {
            // Arrange
            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            StringBuilder testTitleString = new StringBuilder();
            
            var surpassLength = 51;

            for(int i=0; i<surpassLength; i++)
            {
                testTitleString.Append('c');
            }

            // Act
            var buildPost = new BuildPost()
            {
                Username = "Test Username",
                Title = testTitleString.ToString(),
                Description = "Test Desctiption",
                LikeIncrementor = 0,
                BuildType = BuildType.None,
                BuildImagePath = @"C:\Users\Serge\Desktop\images\test.jpg",
                DateTime = "2021"
            };

            var result = mostPopularBuildsManager.PublishBuild(buildPost);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// This test will check if the publish build method will return false
        /// if the description length string surpasses 10,000 characters.
        /// </summary>
        [Test]
        public void MostPopularBuilds_PublishBuild_ReturnFalseIfDescriptionCharsAreGreaterThan10k()
        {
            // Arrange
            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            StringBuilder testDescriptionString = new StringBuilder();

            var surpassLength = 10001;

            for (int i = 0; i < surpassLength; i++)
            {
                testDescriptionString.Append('c');
            }

            // Act
            var buildPost = new BuildPost()
            {
                Username = "Test Username",
                Title = "Test Title",
                Description = testDescriptionString.ToString(),
                LikeIncrementor = 0,
                BuildType = BuildType.None,
                BuildImagePath = @"C:\Users\Serge\Desktop\images\test.jpg",
                DateTime = "2021"
            };

            var result = mostPopularBuildsManager.PublishBuild(buildPost);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// This test will check if the publish build method will return false
        /// if the title string contains invalid characters.
        /// </summary>
        [Test]
        public void MostPopularBuilds_PublishBuild_ReturnFalseIfTitleContainsInvalidChars()
        {
            // Arrange
            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            var invalidUsernameTest = "%^#TheGreatestTestTitle$#^#";

            // Act
            var buildPost = new BuildPost()
            {
                Username = "Test Username",
                Title = invalidUsernameTest,
                Description = "Test Description",
                LikeIncrementor = 0,
                BuildType = BuildType.None,
                BuildImagePath = @"C:\Users\Serge\Desktop\images\test.jpg",
                DateTime = "2021"
            };

            var result = mostPopularBuildsManager.PublishBuild(buildPost);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// This test will check if the publish build method will return true
        /// if all conditions are met
        /// </summary>
        [Test]
        public void MostPopularBuilds_PublishBuild_ReturnTrueIfAllConditionsAreMet()
        {
            // Arrange
            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // Act
            var buildPost = new BuildPost()
            {
                Username = "Test Username",
                Title = "Test Title",
                Description = "Test Description",
                LikeIncrementor = 0,
                BuildType = BuildType.None,
                BuildImagePath = @"C:\Users\Serge\Desktop\images\test.jpg",
                DateTime = "2021"
            };

            var result = mostPopularBuildsManager.PublishBuild(buildPost);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
