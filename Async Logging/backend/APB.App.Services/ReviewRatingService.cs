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
                        img.Save($"C:\\Users\\Serge\\Desktop\\images\\{reviewEntities.Username}_{reviewEntities.EntityId}.jpg");
                        //img.Save($"C:\\Users\\Serge\\Code\\GitHub\\Saturday-Solution\\Async Logging\\backend\\APB.App.Apis\\wwwroot\\images\\{reviewEntities.Username}_{reviewEntities.EntityId}.jpg");
                        reviewRatings.Picture = img;

                        //reviewRatingEntity.EntityId = $"{ReviewTable}_{DateTime.UtcNow.ToString("yyyyMMdd_hh_mm_ss_ms")}"
                    }
                }
            }
            else
            {
                reviewRatings.Picture = null;
            }

            return reviewRatings;
        }

        public List<ReviewRating> GetAllReviewsRatings()
        {
            var reviewEntities = _reviewRatingDAO.GetAllReviewsRatings();

            var reviewRatingList = new List<ReviewRating>();
            foreach(ReviewRatingEntity reviewRatingEntity in reviewEntities)
            {
                var reviewRatings = new ReviewRating()
                {
                    EntityId = reviewRatingEntity.EntityId,
                    Username = reviewRatingEntity.Username,
                    StarRating = (StarType)reviewRatingEntity.StarRatingValue,
                    Message = reviewRatingEntity.Message,
                    DateTime = reviewRatingEntity.DateTime
                };

                if (reviewRatingEntity.ImageBuffer != null)
                {
                    using (var streamBitmap = new MemoryStream(reviewRatingEntity.ImageBuffer))
                    {

                        using (Image img = Image.FromStream(streamBitmap))
                        {
                            string filePath = $"C:\\Users\\Serge\\Code\\GitHub\\Saturday-Solution\\Async Logging\\backend\\APB.App.Apis\\wwwroot\\images\\{reviewRatingEntity.Username}_{reviewRatingEntity.EntityId}.jpg";
                            img.Save(filePath);
                            reviewRatings.Picture = img;
                            reviewRatings.FilePath = $"images/{ reviewRatingEntity.Username}_{ reviewRatingEntity.EntityId}.jpg";
                            //reviewRatingEntity.EntityId = $"{ReviewTable}_{DateTime.UtcNow.ToString("yyyyMMdd_hh_mm_ss_ms")}"
                        }
                    }
                }
                else
                {
                    reviewRatings.Picture = null;
                }
                reviewRatingList.Add(reviewRatings);
            }
            return reviewRatingList;
        }

        public bool DeleteReviewRating(string reviewId)
        {
            var reviewRatingEntity = new ReviewRatingEntity()
            {
                EntityId = reviewId
            };

            return _reviewRatingDAO.DeleteReviewRatingById(reviewRatingEntity.EntityId);
        }

        public bool EditReviewRating(ReviewRating reviewRating)
        {
            ImageConverter imageConverter = new ImageConverter();

            var reviewRatingEntity = new ReviewRatingEntity()
            {
                EntityId = reviewRating.EntityId,
                StarRatingValue = (int)reviewRating.StarRating,
                Message = reviewRating.Message,
                ImageBuffer = (byte[])imageConverter.ConvertTo(reviewRating.Picture, typeof(byte[])),
            };

            return _reviewRatingDAO.EditReviewRatingRecord(reviewRatingEntity);
        }
    }
}
