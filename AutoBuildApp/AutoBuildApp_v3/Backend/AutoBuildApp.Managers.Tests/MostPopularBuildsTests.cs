﻿using AutoBuildApp.DataAccess;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Managers;
using AutoBuildApp.Models.Enumerations;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Security.FactoryModels;
using AutoBuildApp.Security.Interfaces;
using AutoBuildApp.Security.Models;
using AutoBuildApp.Services.FeatureServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
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
        private const string testConnectionString = "Server = localhost; Database = TestDB; Trusted_Connection = True;";

        private readonly ClaimsPrincipal _basicClaimsPrincipal;
        private readonly ClaimsPrincipal _secondBasicClaimsPrincipal;

        public MostPopularBuildsTests()
        {
            UserIdentity userIdentity = new UserIdentity
            {
                Name = "SERGE",
                IsAuthenticated = true,
                AuthenticationType = "JWT"
            };

            UserIdentity adminIdentity = new UserIdentity
            {
                Name = "ADMIN USER",
                IsAuthenticated = true,
                AuthenticationType = "JWT"
            };

            ClaimsFactory claimsFactory = new ConcreteClaimsFactory();
            IClaims basicClaims = claimsFactory.GetClaims(RoleEnumType.BasicRole);
            IClaims secondBasicClaims = claimsFactory.GetClaims(RoleEnumType.BasicRole);

            ClaimsIdentity basicClaimsIdentity = new ClaimsIdentity
            (userIdentity, basicClaims.Claims(), userIdentity.AuthenticationType, userIdentity.Name, " ");
            
            ClaimsIdentity secondBasicClaimsIdentity = new ClaimsIdentity
            (adminIdentity, secondBasicClaims.Claims(), adminIdentity.AuthenticationType, adminIdentity.Name, " ");

            _basicClaimsPrincipal = new ClaimsPrincipal(basicClaimsIdentity);
            _secondBasicClaimsPrincipal = new ClaimsPrincipal(secondBasicClaimsIdentity);
        }

        /// <summary>
        /// This test will check if the publish build method will return false
        /// if the object passed through is null.
        /// </summary>
        [Test]
        public async Task MostPopularBuilds_PublishBuild_ReturnFalseIfObjectIsNull()
        {
            // Arrange
            Thread.CurrentPrincipal = _basicClaimsPrincipal;

            var mostPopularBuildsDAO = new MostPopularBuildsDAO(testConnectionString);
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
            Thread.CurrentPrincipal = _basicClaimsPrincipal;

            var mostPopularBuildsDAO = new MostPopularBuildsDAO(testConnectionString);
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
            Thread.CurrentPrincipal = _basicClaimsPrincipal;

            var mostPopularBuildsDAO = new MostPopularBuildsDAO(testConnectionString);
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
            Thread.CurrentPrincipal = _basicClaimsPrincipal;

            var mostPopularBuildsDAO = new MostPopularBuildsDAO(testConnectionString);
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
        /// This test will check if more than 3 posts can be made.
        /// </summary>
        [Test]
        public async Task MostPopularBuilds_PublishBuild_ReturnFalseIfMoreThanThreePostsAreMadeByTheSameUser()
        {
            // Arrange
            Thread.CurrentPrincipal = _basicClaimsPrincipal;

            var mostPopularBuildsDAO = new MostPopularBuildsDAO(testConnectionString);
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // Act
            var buildPost = new BuildPost()
            {
                Username = "SERGE",
                Title = "TestTitle",
                Description = "TestDescription",
                BuildType = BuildType.None,
            };

            await mostPopularBuildsManager.PublishBuild(buildPost);
            await mostPopularBuildsManager.PublishBuild(buildPost);
            await mostPopularBuildsManager.PublishBuild(buildPost);

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
            Thread.CurrentPrincipal = _secondBasicClaimsPrincipal;

            var mostPopularBuildsDAO = new MostPopularBuildsDAO(testConnectionString);
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
        /// This test will check if a non-query search is successful.
        /// </summary>
        [Test]
        public void MostPopularBuilds_GetBuildPosts_ReturnTrueIfNormalCallIsSuccessful()
        {
            // Arrange
            Thread.CurrentPrincipal = _basicClaimsPrincipal;

            var mostPopularBuildsDAO = new MostPopularBuildsDAO(testConnectionString);
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            var rowCount = mostPopularBuildsManager.GetBuildPosts(null, null).Count;

            // Initialize the ID to two less of the first DB entity ID.
            var id = 29998;

            var expectedResultEntityIds = new List<string>();

            for (var i = 0; i < rowCount; i++)
            {
                id += 2;

                expectedResultEntityIds.Add(id.ToString());
            }

            // Act
            var actualResultList = mostPopularBuildsManager.GetBuildPosts(null, null);

            List<string> actualResultEntityIds = new List<string>();

            foreach (var actualResult in actualResultList)
                actualResultEntityIds.Add(actualResult.EntityId);

            // Assert Assert.That(actualResult, Is.EquivalentTo(expectedResult));
            Assert.That(actualResultEntityIds, Is.EquivalentTo(expectedResultEntityIds));
        }

        /// <summary>
        /// This test will check if a queried search is made.
        /// </summary>
        [Test]
        public void MostPopularBuilds_GetBuildPosts_ReturnTrueIfSortedQueriesCallIsSuccessful()
        {
            // Arrange
            Thread.CurrentPrincipal = _basicClaimsPrincipal;

            var mostPopularBuildsDAO = new MostPopularBuildsDAO(testConnectionString);
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            var expectedResultList = mostPopularBuildsManager.GetBuildPosts(null, null);

            expectedResultList.RemoveAll(o => (int)o.BuildType != 2);

            expectedResultList.Reverse();

            var expectedListParsed = new List<(string, int)>();

            foreach (var item in expectedResultList)
            {
                expectedListParsed.Add((item.EntityId, (int)item.BuildType));
            }

            // Act
            var actualResultList = mostPopularBuildsManager.GetBuildPosts("AscendingLikes", "BuildType_Gaming");

            var actualListParsed = new List<(string, int)>();

            foreach (var item in actualResultList)
            {
                actualListParsed.Add((item.EntityId, (int)item.BuildType));
            }


            // Assert Assert.That(actualResult, Is.EquivalentTo(expectedResult));
            Assert.That(actualListParsed, Is.EquivalentTo(expectedListParsed));
        }

        /// <summary>
        /// This test will check if a particular build has been returned.
        /// </summary>
        [Test]
        public void MostPopularBuilds_GetBuildPost_ReturnTrueIfTheReturnedPostIsTheExpectedPost()
        {
            // Arrange
            Thread.CurrentPrincipal = _basicClaimsPrincipal;

            var mostPopularBuildsDAO = new MostPopularBuildsDAO(testConnectionString);
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            var expectedResult = "30000";

            // Act
            var actualResult = mostPopularBuildsManager.GetBuildPost(expectedResult);

            // Assert
            Assert.That(actualResult.EntityId, Is.EquivalentTo(expectedResult));
        }

        /// <summary>
        /// This test will check if a like has already been made by a user.
        /// </summary>
        [Test]
        public void MostPopularBuilds_AddLike_ReturnTrueForFirstLikeAndFalseForDuplicateLike()
        {
            // Arrange
            Thread.CurrentPrincipal = _basicClaimsPrincipal;

            var mostPopularBuildsDAO = new MostPopularBuildsDAO(testConnectionString);
            var mostPopularBuildsService = new MostPopularBuildsService(mostPopularBuildsDAO);
            var mostPopularBuildsManager = new MostPopularBuildsManager(mostPopularBuildsService);

            // Act
            var fakeUser = "fakeUser";

            var like = new Like()
            {
                PostId = "30000",
                UserId = fakeUser
            };

            // This first function creates a like.
            var firstLike = mostPopularBuildsManager.AddLike(like);

            // This second call creates a second like with the same credentials, returning false, because of duplicate entries.
            var secondLike = mostPopularBuildsManager.AddLike(like);

            // Assert
            Assert.IsTrue(firstLike);
            Assert.IsFalse(secondLike);
        }

    }
}
