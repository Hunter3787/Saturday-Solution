using System.Drawing;

namespace APB.App.DomainModels
{
    public class ReviewRating
    {
        private string filePath;

        private Image image;

        public string Username { get; set; }

        public StarType StarRating { get; set; }

        public string Message { get; set; }

        public string FilePath 
        { 
            get { return filePath; } 
            set { filePath = value; }
        }

        public Image Picture 
        {
            get 
            {
                image = Image.FromFile(filePath);
                return image; 
            }
            set 
            {
                image = Image.FromFile(filePath);
                image = value;
            } 
        }
    }
}
