using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models.WebCrawler
{
    public class Review
    {
        public string ReviewerName { get; set; }
        public string StarRating { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }

        public Review(string reviewerName, string starRating, string content, string date)
        {
            ReviewerName = reviewerName;
            StarRating = starRating;
            Content = content;
            Date = date;
        }
    }
}
