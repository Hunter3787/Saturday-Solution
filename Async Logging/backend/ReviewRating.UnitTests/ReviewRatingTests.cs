using APB.App.DomainModels;
using APB.App.Managers;
using APB.App.Services;
using APB.App.DataAccess;
using NUnit.Framework;

namespace ReviewsAndRatings.UnitTests
{
    [TestFixture]
    public class ReviewRatingTests
    {
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
            Assert.That(result, Is.True);
        }

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

        [Test]
        public void ReviewRating_GetReviewRating_ReturnsReviewRatingObjectWithAnEntityId()
        {
            // Arrange
            var dataAccess = new ReviewRatingDAO("Server = localhost; Database = DB; Trusted_Connection = True;");
            var reviewService = new ReviewRatingService(dataAccess);
            var reviewRatingManager = new ReviewRatingManager(reviewService);

            // Act
            var result = reviewRatingManager.GetReviewsRatings("30000");

            // Assert
            Assert.AreEqual("30000", result.EntityId);
        }
    }
}
