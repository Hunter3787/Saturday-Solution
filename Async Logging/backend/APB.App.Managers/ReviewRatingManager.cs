﻿using APB.App.DomainModels;
using APB.App.Services;
using System.Collections.Generic;

/// <summary>
/// References used from file: Solution Items/References.txt 
/// [1]
/// </summary>

namespace APB.App.Managers
{
    /// <summary>
    /// This class will consist of the methods that the controller can call to execute specific commands.
    /// </summary>
    public class ReviewRatingManager
    {
        private readonly ReviewRatingService _reviewRatingService; // this sets an instance of the DAO connection so that it can be used without starting a new connection every time.

        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance; // gets the logger instance so that it can be used.

        /// <summary>
        /// This method initialized the service.
        /// </summary>
        /// <param name="reviewRatingService">review rating service passed in so that it doesnt always have to be initialized.</param>
        public ReviewRatingManager(ReviewRatingService reviewRatingService)
        {
            _reviewRatingService = reviewRatingService;
        }

        /// <summary>
        /// This method will be used to communicate with the back end side of the application.
        /// </summary>
        /// <param name="reviewRating">ReviewRating object from controller to be sent to the service layer.</param>
        /// <returns>boolean success-state</returns>
        public bool CreateReviewRating(ReviewRating reviewRating)
        {
            _logger.LogInformation($"Review Rating Manager CreateReviewRating was called for User:{reviewRating.Username}");
            return _reviewRatingService.CreateReviewRating(reviewRating); // returns the boolean success state.
        }

        /// <summary>
        /// Creates a ReviewRating object and sets it to the ID.
        /// </summary>
        /// <param name="reviewId">string ID</param>
        /// <returns>returns the object of the string ID that is in the DB.</returns>
        public ReviewRating GetReviewsRatings(string reviewId)
        {
            _logger.LogInformation($"Review Rating Manager GetReviewRating was called for ID:{reviewId}");
            // creates a new ReviewRating with the id passed in.
            var reviewRating = new ReviewRating()
            {
                EntityId = reviewId
            };

            return _reviewRatingService.GetReviewsRatings(reviewRating); // returns the ReviewRating object
        }

        /// <summary>
        /// This methos is used to retrieve all reviews, called from the controller.
        /// </summary>
        /// <returns>returns the list ReviewRatings</returns>
        public List<ReviewRating> GetAllReviewsRatings()
        {
            _logger.LogInformation("Review Rating Manager GetAllReviews was called.");
            return _reviewRatingService.GetAllReviewsRatings(); // gets the list that is created in the service layer.
        }

        /// <summary>
        /// This method is used to pass in a string and delete the corresponding item.
        /// </summary>
        /// <param name="reviewId">string ID of the review to be deleted.</param>
        /// <returns>bool success-state</returns>
        public bool DeleteReviewRating(string reviewId)
        {
            _logger.LogInformation($"Review Rating Manager DeleteReviewRating was called for ID:{reviewId}");
            return _reviewRatingService.DeleteReviewRating(reviewId); // returns the bool of the service method.
        }

        /// <summary>
        /// This method will be used to edit an existing item in the DB
        /// </summary>
        /// <param name="reviewRating">ReviewRating object that will be sent through to the service layer</param>
        /// <returns>bool success state.</returns>
        public bool EditReviewRating(ReviewRating reviewRating)
        {
            _logger.LogInformation($"Review Rating Service EditReviewRating was called for ID:{reviewRating.EntityId}");
            return _reviewRatingService.EditReviewRating(reviewRating); // returns the boolean of the service method.
        }
    }
}
