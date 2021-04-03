using NUnit.Framework;
using System.Collections.Generic;
using AutoBuildApp.DomainModels;
using AutoBuildApp.DomainModels.Enumerations;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers;
using AutoBuildApp.Services;

/// <summary>
/// References used from file: Solution Items/References.txt 
/// [14]
/// </summary>

namespace ReviewsAndRatings.UnitTests
{
    /// <summary>
    /// This class will test all methods that have been created and verify that they do indeed work.
    /// Database must be cleared and fresh before testing commences.
    /// </summary>
    [TestFixture]
    public class ReviewRatingTests
    {
        /// <summary>
        /// This test will check if the equals operator comparison for two review ratings objectd are equal.
        /// </summary>
        [Test]
        public void ReviewRating_Equals_TwoReviewRatingObjectsAreEqual()
        {
            // Arrange
            var reviewRating = new ReviewRating
            {
                Username = "Zee",
                Message = "Hello",
                StarRating = StarType.Four_Stars,
                FilePath = "C:/Users/Serge/Desktop/images/5.jpg"
            };

            // Act
            var reviewRatingCopy = reviewRating;

            // Assert
            Assert.AreEqual(reviewRating, reviewRatingCopy);
        }

        /// <summary>
        /// This test will check if a review has been created, it will pass if it returns true.
        /// </summary>
        [Test]
        public void ReviewRating_CreateReviewRating_ReturnsTrue()
        {
            // Arrange
            var dataAccess = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var reviewService = new ReviewRatingService(dataAccess);
            var reviewRating = new ReviewRatingManager(reviewService);

            // Act
            var result = reviewRating.CreateReviewRating(new ReviewRating { 
                Username = "Zee",
                Message = "Hello",
                StarRating = StarType.Four_Stars,
                FilePath = "C:/Users/Serge/Desktop/images/5.jpg"
            });

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// This test will return a review rating object, if it does, then it passes.
        /// </summary>
        [Test]
        public void ReviewRating_GetReviewRating_ReturnsReviewRatingObject()
        {
            // Arrange
            var reviewRatingDAO = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var reviewRatingService = new ReviewRatingService(reviewRatingDAO);
            var reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            var reviewRating = new ReviewRating();

            // Act
            var result = reviewRatingManager.GetReviewsRatings("30000");

            // Assert
            Assert.AreEqual(reviewRating.GetType(), result.GetType());
        }

        /// <summary>
        /// This test will return a list of review rating objects, if it does, then it passes.
        /// </summary>
        [Test]
        public void ReviewRating_GetAllReviewsRatings_ReturnsListOfReviewRatingObjects()
        {
            // Arrange
            var reviewRatingDAO = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var reviewRatingService = new ReviewRatingService(reviewRatingDAO);
            var reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            var reviewRatingList = new List<ReviewRating>();

            // Act
            var result = reviewRatingManager.GetAllReviewsRatings();

            // Assert
            Assert.AreEqual(reviewRatingList.GetType(), result.GetType());
        }

        /// <summary>
        /// This test will return a review rating object with a specified ID, it will check if IDs are equal.
        /// If IDs are equal, then it will return true.
        /// </summary>
        [Test]
        public void ReviewRating_GetReviewRating_ReturnsReviewRatingObjectWithAnEntityId()
        {
            // Arrange
            var reviewRatingDAO = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var reviewRatingService = new ReviewRatingService(reviewRatingDAO);
            var reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            // Act
            var result = reviewRatingManager.GetReviewsRatings("30000");

            // Assert
            Assert.AreEqual("30000", result.EntityId);
        }

        /// <summary>
        /// This test will test if a review has been deleted, it will return true and pass if it has been deleted.
        /// </summary>
        [Test]
        public void ReviewRating_DeleteReviewRating_ReturnsTrue()
        {
            // Arrange
            var reviewRatingDAO = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var reviewRatingService = new ReviewRatingService(reviewRatingDAO);
            var reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            reviewRatingManager.CreateReviewRating(new ReviewRating
            {
                EntityId = "80000",
                Username = "Zee",
                Message = "Hello",
                StarRating = StarType.Four_Stars,
                FilePath = "C:/Users/Serge/Desktop/images/5.jpg"
            });

            // Act
            var result = reviewRatingManager.DeleteReviewRating("30002");

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// This test will check if a review has been edited, it will return true if it has been edited.
        /// </summary>
        [Test]
        public void ReviewRating_EditReviewRating_ReturnsTrue()
        {
            // Arrange
            var reviewRatingDAO = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var reviewRatingService = new ReviewRatingService(reviewRatingDAO);
            var reviewRatingManager = new ReviewRatingManager(reviewRatingService);

            var reviewRating = new ReviewRating
            {
                EntityId = "30000",
                Message = "Edited Review",
                StarRating = StarType.One_Star,
                FilePath = "C:/Users/Serge/Desktop/images/2.jpg"
            };

            // Act
            var result = reviewRatingManager.EditReviewRating(reviewRating);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
