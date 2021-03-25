using Microsoft.VisualStudio.TestTools.UnitTesting;
using APB.App.DomainModels;
using APB.App.Managers;

namespace ReviewsAndRatings.UnitTests
{
    [TestClass]
    public class ReviewRatingTests
    {
        [TestMethod]
        public void ReviewRating_NewReviewRating_ReturnsTrue()
        {
            // Arrange
            var reviewRating = new ReviewRatingManager();

            // Act
            var result = reviewRating.ReviewRating(new ReviewRating { Message = "Hello", StarRating = StarType.Four_Stars});
            
            // Assert
            Assert.IsTrue(result);
        }
    }
}
