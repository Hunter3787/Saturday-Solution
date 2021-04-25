using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Managers;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Services.FeatureServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        public async Task MostPopularBuilds_PublishBuild_ReturnFalseIfObjectIsNull()
        {
            // Arrange
            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            BuildPost buildPost = null;

            // Act
            var result = await mostPopularBuildsManager.PublishBuild(buildPost);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// This test will check if the publish build method will return false
        /// if the object has any vars that are null.
        /// </summary>
        [Test]
        public async Task MostPopularBuilds_PublishBuild_ReturnFalseIfAnyNullVarsInBuildPostObject()
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

            var nullUsernameResultTask = await mostPopularBuildsManager.PublishBuild(nullUsernameBuildPost);
            var nullTitleResultTask = await mostPopularBuildsManager.PublishBuild(nullTitleBuildPost);
            var nullDescriptionResultTask = await mostPopularBuildsManager.PublishBuild(nullDescriptionBuildPost);

            if (!nullUsernameResultTask && !nullTitleResultTask && !nullDescriptionResultTask)
                result = true;

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// This test will check if the publish build method will return false
        /// if the title length string surpasses 50 characters.
        /// </summary>
        [Test]
        public async Task MostPopularBuilds_PublishBuild_ReturnFalseIfTitleCharsAreGreaterThan50()
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

            var result = await mostPopularBuildsManager.PublishBuild(buildPost);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// This test will check if the publish build method will return false
        /// if the description length string surpasses 10,000 characters.
        /// </summary>
        [Test]
        public async Task MostPopularBuilds_PublishBuild_ReturnFalseIfDescriptionCharsAreGreaterThan10k()
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

            var result = await mostPopularBuildsManager.PublishBuild(buildPost);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// This test will check if the publish build method will return false
        /// if the title string contains invalid characters.
        /// </summary>
        [Test]
        public async Task MostPopularBuilds_PublishBuild_ReturnFalseIfTitleContainsInvalidChars()
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

            var result = await mostPopularBuildsManager.PublishBuild(buildPost);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// This test will check if the publish build method will return true
        /// if all conditions are met
        /// </summary>
        [Test]
        public async Task MostPopularBuilds_PublishBuild_ReturnTrueIfAllConditionsAreMet()
        {
            // Arrange
            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // Act
            var buildPost = new BuildPost()
            {
                Username = "TestUsername",
                Title = "TestTitle",
                Description = "TestDescription",
                BuildType = BuildType.None,
            };

            var result = await mostPopularBuildsManager.PublishBuild(buildPost);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// This test will check if a queried search is made.
        /// </summary>
        [Test]
        public void MostPopularBuilds_GetBuildPosts_ReturnTrueIfSortedQueriesCallIsSuccessful()
        {
            // Arrange
            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // Act

            // Assert
        }

        /// <summary>
        /// This test will check if a non-queries search is successful.
        /// </summary>
        [Test]
        public void MostPopularBuilds_GetBuildPosts_ReturnTrueIfNormalCallIsSuccessful()
        {
            // Arrange
            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // Act

            // Assert
        }

        /// <summary>
        /// This test will check if a particular build has been returned.
        /// </summary>
        [Test]
        public void MostPopularBuilds_GetBuildPost_ReturnTrueIfTheReturnedPostIsTheExpectedPost()
        {
            // Arrange
            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // Act

            // Assert
        }

        /// <summary>
        /// This test will check if a like has been added to a build post.
        /// </summary>
        [Test]
        public void MostPopularBuilds_AddLike_ReturnTrueIfALikeWasAdded()
        {
            // Arrange
            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // Act

            // Assert
        }

        /// <summary>
        /// This test will check if a like has already been made by a user.
        /// </summary>
        [Test]
        public void MostPopularBuilds_AddLike_ReturnFalseIfALikeWasAddedButAlreadyExistsForUser()
        {
            // Arrange
            var mostPopularBuildsDAO = new MostPopularBuildsDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // Act

            // Assert
        }

    }
}
