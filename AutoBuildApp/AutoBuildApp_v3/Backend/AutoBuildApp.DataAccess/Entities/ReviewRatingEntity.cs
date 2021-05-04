﻿/// <summary>
/// References used from file: Solution Items/References.txt 
/// [1]
/// </summary>
namespace AutoBuildApp.DataAccess.Entities
{
    /// <summary>
    /// This is a class that the DAO can access in order to send data to the datastore.
    /// It is used as a DTO to transfer data across classes to eliminate cicular dependencies.
    /// </summary>
    public class ReviewRatingEntity
    {
        // Unique identifier for each review, auto-generated by the database.
        public string EntityId { get; set; }

        // String username that uniquely identifies a user account.
        public string Username { get; set; }

        // Gets the enum value of a star and stores it as an integer.
        public int StarRatingValue { get; set; }

        // This is the review that a user can write, it is stored in the database.
        public string Message { get; set; }

        // This is a byte array, which acts as a buffer to store the byte value of an image.
        public byte[] ImageBuffer { get; set; }

        // This is the file path which store the file path of the image.
        public string FilePath { get; set; }

        // This is the datetime that is appended in the ReviewRating service layer, as a means to display when a review was effectively made.
        public string DateTime { get; set; }
    }
}
