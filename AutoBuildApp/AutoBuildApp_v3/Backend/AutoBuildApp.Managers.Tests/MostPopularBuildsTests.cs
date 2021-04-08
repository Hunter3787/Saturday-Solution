using AutoBuildApp.DomainModels;
using AutoBuildApp.Managers;
using AutoBuildApp.Models.Enumerations;
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
        /// if the title length string surpasses 50 characters.
        /// </summary>
        [Test]
        public void MostPopularBuilds_PublishBuild_ReturnFalseIfTitleCharsAreGreaterThan50()
        {
            // Arrange
            var mostPopularBuildsManager = new MostPopularBuildsManager();

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

            //Assert
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
            var mostPopularBuildsManager = new MostPopularBuildsManager();

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

            //Assert
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
            var mostPopularBuildsManager = new MostPopularBuildsManager();

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

            //Assert
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
            var mostPopularBuildsManager = new MostPopularBuildsManager();

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

            //Assert
            Assert.IsTrue(result);
        }
    }
}
