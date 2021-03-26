using APB.App.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace APB.App.DataAccess.Interfaces
{
    public interface IReviewsRatingsDAO
    {
        ISet<ReviewRatingEntity> GetReviewsRatingsBy(string reviewId);
    }
}
