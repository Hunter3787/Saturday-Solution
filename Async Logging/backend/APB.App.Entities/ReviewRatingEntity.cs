namespace APB.App.Entities
{
    public class ReviewRatingEntity
    {
        public string Username { get; set; }

        public int StarRatingValue { get; set; }

        public string Message { get; set; }

        public byte[] ImageBuffer { get; set; }

        public string DateTime { get; set; }
    }
}
