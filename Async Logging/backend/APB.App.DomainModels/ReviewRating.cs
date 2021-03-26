using System.Drawing;

namespace APB.App.DomainModels
{
    public class ReviewRating
    {
        public StarType StarRating { get; set; }

        public string Message { get; set; }

        public Image Picture { get; set; }
    }
}
