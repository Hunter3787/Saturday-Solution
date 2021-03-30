using System;
using System.Drawing;
using System.IO;

namespace APB.App.DomainModels
{
    public class ReviewRating : IEquatable<ReviewRating>
    {
        public string EntityId { get; set; }

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
                if (File.Exists(filePath))
                {
                    image = Image.FromFile(filePath);
                    return image;
                }

                return null;
            }
            set 
            {
                image = value;
            } 
        }

        public string DateTime { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ReviewRating);
        }

        public override int GetHashCode()
        {
            return EntityId.GetHashCode();
        }

        public bool Equals(ReviewRating other)
        {
            if (other is null)
            {
                return false;
            }

            return EntityId.Equals(other.EntityId);
        }

        public override string ToString()
        {
            return $"{nameof(ReviewRating)} {EntityId}";
        }
    }
}
