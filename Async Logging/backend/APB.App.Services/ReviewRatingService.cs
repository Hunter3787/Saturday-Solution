using APB.App.DataAccess;
using APB.App.Entities;
using APB.App.DomainModels;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.Collections.Generic;

namespace APB.App.Services
{
    public class ReviewRatingService
    {
        LoggingProducerService logger = LoggingProducerService.GetInstance;
        ReviewRatingDAO _reviewRatingDAO;

        public ReviewRatingService(ReviewRatingDAO reviewRatingDAO)
        {
            _reviewRatingDAO = reviewRatingDAO;
        }

        public bool CreateReviewRating(ReviewRating reviewRating)
        {
            ImageConverter imageConverter = new ImageConverter();

            var reviewRatingEntity = new ReviewRatingEntity()
            {
                Username = reviewRating.Username,
                StarRatingValue = (int)reviewRating.StarRating,
                Message = reviewRating.Message,
                ImageBuffer = (byte[])imageConverter.ConvertTo(reviewRating.Picture, typeof(byte[])),
                DateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:FFFFFFF")
            };

            //using (var streamBitmap = new MemoryStream(reviewRatingEntity.ImageBuffer))
            //{
            //    using (Image img = Image.FromStream(streamBitmap))
            //    {
            //        img.Save("C:\\Users\\Serge\\Desktop\\images\\twooost.jpg");
            //    }
            //}

            logger.LogInformation("A new Review and Rating record has been created in the database");

            return _reviewRatingDAO.CreateReviewRatingRecord(reviewRatingEntity);
        }

        public ReviewRating GetReviewsRatings(ReviewRating reviewRating)
        {
            var reviewEntities = _reviewRatingDAO.GetReviewsRatingsBy(reviewRating.EntityId);

            var reviewRatings = new ReviewRating()
            {
                EntityId = reviewEntities.EntityId,
                Username = reviewEntities.Username,
                StarRating = (StarType)reviewEntities.StarRatingValue,
                Message = reviewEntities.Message,
                DateTime = reviewEntities.DateTime
            };

            if (reviewEntities.ImageBuffer != null)
            {
                using (var streamBitmap = new MemoryStream(reviewEntities.ImageBuffer))
                {

                    using (Image img = Image.FromStream(streamBitmap))
                    {
                        img.Save("C:\\Users\\Serge\\Desktop\\images\\testttttt.jpg");
                        reviewRatings.Picture = img;
                    }
                }
            }
            else
            {
                reviewRatings.Picture = null;
            }

            return reviewRatings;
        }
    }
}
