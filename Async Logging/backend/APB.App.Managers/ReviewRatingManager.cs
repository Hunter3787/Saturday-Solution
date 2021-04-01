﻿using APB.App.DomainModels;
using APB.App.Services;
using System.Collections.Generic;

namespace APB.App.Managers
{
    public class ReviewRatingManager
    {
        ReviewRatingService _reviewRatingService;

        LoggingProducerService logger = LoggingProducerService.GetInstance;

        //private ReviewRating reviewRating;

        public ReviewRatingManager(ReviewRatingService reviewRatingService)
        {
            _reviewRatingService = reviewRatingService;
        }

        public bool CreateReviewRating(ReviewRating reviewRating)
        {
            //reviewRating = new ReviewRating();

            logger.LogInformation("a review and rating has been entered");

            //reviewRating.Message = message;
            //reviewRating.StarRating = starType;
            //reviewsRatingsObject.Img = image;
            return _reviewRatingService.CreateReviewRating(reviewRating);
        }

        public ReviewRating GetReviewsRatings(string reviewId)
        {
            var reviewRating = new ReviewRating()
            {
                EntityId = reviewId
            };

            return _reviewRatingService.GetReviewsRatings(reviewRating);
        }

        public List<ReviewRating> GetAllReviewsRatings()
        {
            return _reviewRatingService.GetAllReviewsRatings();
        }

        public bool DeleteReviewRating(string reviewId)
        {
            return _reviewRatingService.DeleteReviewRating(reviewId);
        }

        public bool EditReviewRating(ReviewRating reviewRating)
        {
            return _reviewRatingService.EditReviewRating(reviewRating);
        }
    }
}
