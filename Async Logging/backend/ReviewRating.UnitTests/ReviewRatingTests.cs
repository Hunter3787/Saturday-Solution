using Microsoft.VisualStudio.TestTools.UnitTesting;
using APB.App.DomainModels;
using APB.App.Managers;
using APB.App.Services;
using APB.App.DataAccess;

namespace ReviewsAndRatings.UnitTests
{
    [TestClass]
    public class ReviewRatingTests
    {
        [TestMethod]
        public void ReviewRating_NewReviewRating_ReturnsTrue()
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
    }
}
