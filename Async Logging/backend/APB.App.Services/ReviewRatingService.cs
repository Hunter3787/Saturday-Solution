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
            //        img.Save("C:\\Users\\Serge\\Desktop\\images\\testttt.jpg");
            //    }
            //}

            logger.LogInformation("A new Review and Rating record has been created in the database");

            return _reviewRatingDAO.CreateReviewRatingRecord(reviewRatingEntity);
        }

        public ISet<ReviewRating> GetReviewsRatings(ReviewRating reviewRating)
        {
            var reviewEntities = _reviewRatingDAO.GetReviewsRatingsBy(reviewRating.EntityId);

            return null;
        }
    }
}
