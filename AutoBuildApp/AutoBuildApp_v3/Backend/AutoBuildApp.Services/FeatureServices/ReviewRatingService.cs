
using System.IO;
using System.Drawing;
using System;
using System.Collections.Generic;
using AutoBuildApp.DomainModels;
using AutoBuildApp.DomainModels.Enumerations;
using AutoBuildApp.DataAccess;
using AutoBuildApp.DataAccess.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutoBuildApp.Logging;

/// <summary>
/// References used from file: Solution Items/References.txt 
/// [1,16,17]
/// </summary>

namespace AutoBuildApp.Services
{
    /// <summary>
    /// This class acts as the intermediary for the review feature, will partake in the conversion
    /// of review ratings to review entities.
    /// </summary>
    public class ReviewRatingService
    {
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance; // This will get the logger so it can be used.
        private readonly ReviewRatingDAO _reviewRatingDAO; // this sets an instance of the DAO connection so that it can be used without starting a new connection every time.

        /// <summary>
        /// This will initialize the DAO with the same DAO that is passed in.
        /// </summary>
        /// <param name="reviewRatingDAO">Takes in a DAO object that will be used to communicate with DB.</param>
        public ReviewRatingService(ReviewRatingDAO reviewRatingDAO)
        {
            _reviewRatingDAO = reviewRatingDAO;
        }

        /// <summary>
        /// This method will be used to create a review rating entity to be sent to the DB.
        /// </summary>
        /// <param name="reviewRating">Takes in a ReviewRating object to be converted.</param>
        /// <returns>returns a success-state bool</returns>
        public async Task<bool> CreateReviewRating(ReviewRating reviewRating)
        {
            _logger.LogInformation($"Review Rating Service CreateReviewRating was called for User:{reviewRating.Username}");

            // this converts a ReviewRating object into a ReviewRatingEntity object.
            var reviewRatingEntity = new ReviewRatingEntity()
            {
                BuildId = reviewRating.BuildId,
                Username = reviewRating.Username,
                StarRatingValue = (int)reviewRating.StarRating, // casts the enum into a boolean for easier storage and query in DB.
                Message = reviewRating.Message,
                DateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:FFFFFFF"), // takes the current time and appends it to the object.
                ReviewImagePath = await UploadImage(reviewRating.Username, reviewRating.Image)
            };

            // returns the bool of the success of the DAO method.
            return _reviewRatingDAO.CreateReviewRatingRecord(reviewRatingEntity);
        }

        private async Task<string> UploadImage(string username, List<IFormFile> files)
        {
            string storeIn = " ";

            if (files == null)
                return storeIn;

            foreach (var item in files)
            {
                if (item.Length > 0)
                {
                    var currentDirectory = Directory.GetCurrentDirectory().ToString();

                    storeIn = $"/assets/images/Reviews/{username}_{ DateTime.UtcNow.ToString("yyyyMMdd_hh_mm_ss_ms")}.jpg";

                    var path = Path.GetFullPath(Path.Combine(currentDirectory, $@"..\..\FrontEnd{storeIn}"));

                    using (var stream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite))
                    {
                        await item.CopyToAsync(stream);
                    }
                }
            }
            return storeIn;
        }



        /// <summary>
        /// This method will get a single review from 
        /// the DB and store it as a ReviewRating
        /// </summary>
        /// <param name="reviewRating">takes in a review rating object that will be returned.</param>
        /// <returns></returns>
        public ReviewRating GetReviewsRatings(ReviewRating reviewRating)
        {
            _logger.LogInformation($"Review Rating Service GetReviewRating was called for ID:{reviewRating.EntityId}");

            // stores the entity retrieved locally so that it may be edited.
            var reviewEntity = _reviewRatingDAO.GetReviewsRatingsBy(reviewRating.EntityId);

            // this will store the entity values into the ReviewRating object.
            var reviewRatings = new ReviewRating()
            {
                EntityId = reviewEntity.EntityId,
                Username = reviewEntity.Username,
                StarRating = (StarType)reviewEntity.StarRatingValue,
                Message = reviewEntity.Message,
                DateTime = reviewEntity.DateTime,
                ImagePath = reviewEntity.ReviewImagePath
            };

            // returns the object
            return reviewRatings;
        }

        public List<ReviewRating> GetAllReviewsRatingsByBuildId(string buildId)
        {
            var reviewEntities = _reviewRatingDAO.GetAllReviewsRatingsByBuildId(buildId);

            var reviewRatingList = new List<ReviewRating>(); // initilaize a new list that will be used to append values to.

            foreach (ReviewRatingEntity reviewRatingEntity in reviewEntities)
            {
                // add entity object values into model object values.
                var reviewRatings = new ReviewRating()
                {
                    EntityId = reviewRatingEntity.EntityId,
                    BuildId = reviewRatingEntity.BuildId,
                    Username = reviewRatingEntity.Username,
                    StarRating = (StarType)reviewRatingEntity.StarRatingValue,
                    Message = reviewRatingEntity.Message,
                    DateTime = reviewRatingEntity.DateTime,
                    ImagePath = reviewRatingEntity.ReviewImagePath
                };

       

                reviewRatingList.Add(reviewRatings); // appends the review rating object to the list.
            }
            return reviewRatingList; // returns the list.
        }

        /// <summary>
        /// This method will fetch the entire DB of reviews
        /// </summary>
        /// <returns>Returns a list of ReviewRatings</returns>
        public List<ReviewRating> GetAllReviewsRatings()
        {
            _logger.LogInformation("Review Rating Service GetAllReviews was called.");

            // stores the entities retrieved locally so that it may be edited.
            var reviewEntities = _reviewRatingDAO.GetAllReviewsRatings();

            var reviewRatingList = new List<ReviewRating>(); // initilaize a new list that will be used to append values to.
            foreach(ReviewRatingEntity reviewRatingEntity in reviewEntities)
            {
                // add entity object values into model object values.
                var reviewRatings = new ReviewRating()
                {
                    EntityId = reviewRatingEntity.EntityId,
                    Username = reviewRatingEntity.Username,
                    StarRating = (StarType)reviewRatingEntity.StarRatingValue,
                    Message = reviewRatingEntity.Message,
                    DateTime = reviewRatingEntity.DateTime,
                    ImagePath = reviewRatingEntity.ReviewImagePath
                };

            
                reviewRatingList.Add(reviewRatings); // appends the review rating object to the list.
            }
            return reviewRatingList; // returns the list.
        }

        /// <summary>
        /// This methos will delete a review with the ID passed in.
        /// </summary>
        /// <param name="reviewId">string that will be used to identify an item in DB</param>
        /// <returns>This will return a success-state bool</returns>
        public bool DeleteReviewRating(string reviewId)
        {
            _logger.LogInformation($"Review Rating Service DeleteReviewRating was called for ID:{reviewId}");

            // store the entity id so that it may be passed to the DB.
            var reviewRatingEntity = new ReviewRatingEntity()
            {
                EntityId = reviewId
            };

            // Accesses the DAO and sends the ID through.
            return _reviewRatingDAO.DeleteReviewRatingById(reviewRatingEntity.EntityId);
        }

        /// <summary>
        /// Method that is used to pass in values to replace a DB item.
        /// </summary>
        /// <param name="reviewRating">takes in a review rating object</param>
        /// <returns>returns a success-state bool</returns>
        public async Task<bool> EditReviewRating(ReviewRating reviewRating)
        {
            _logger.LogInformation($"Review Rating Service EditReviewRating was called for ID:{reviewRating.EntityId}");

            // Create and entity with the values that are to be updated with.
            var reviewRatingEntity = new ReviewRatingEntity()
            {
                EntityId = reviewRating.EntityId,
                StarRatingValue = (int)reviewRating.StarRating,
                Message = reviewRating.Message,
                ReviewImagePath = await UploadImage(reviewRating.Username, reviewRating.Image)
            };

            // returns the bool of the success of the DAO method.
            return _reviewRatingDAO.EditReviewRatingRecord(reviewRatingEntity);
        }
    }
}
