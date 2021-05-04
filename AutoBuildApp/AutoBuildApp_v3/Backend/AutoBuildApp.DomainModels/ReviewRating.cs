using AutoBuildApp.DomainModels.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

/// <summary>
/// References used from file: Solution Items/References.txt 
/// [1,18]
/// </summary>

namespace AutoBuildApp.DomainModels
{
    /// <summary>
    /// This is a class that the DAO can access in order to send data to the datastore.
    /// It is used as a DTO to transfer data across classes to eliminate cicular dependencies.
    /// Implements equatable to override the equatable command.
    /// </summary>
    public class ReviewRating : IEquatable<ReviewRating>
    {
        // This list stores a list of form file objects.
        public List<IFormFile> Image { get; set; }

        private string _filePath;

        private Image _image;

        public string EntityId { get; set; }

        public string BuildId { get; set; }

        public string Username { get; set; }

        public StarType StarRating { get; set; }

        public string Message { get; set; }

        public string ImagePath { get; set; }

        public string DateTime { get; set; }

        /// <summary>
        /// overrrides the equals operator so that review rating objects can be
        /// equatable.
        /// </summary>
        /// <param name="obj">takes in an object</param>
        /// <returns>boolean of if they are equal or not, calls other equals operator</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ReviewRating);
        }

        /// <summary>
        /// This will get the hashcode of the ID.
        /// </summary>
        /// <returns>this will return the hashed ID</returns>
        public override int GetHashCode()
        {
            return EntityId.GetHashCode();
        }

        /// <summary>
        /// This method will see if two reviews are equal
        /// </summary>
        /// <param name="other">takes in a review rating object</param>
        /// <returns>will return boolean</returns>
        public bool Equals(ReviewRating other)
        {
            // if the object is true, will auto return false.
            if (other is null)
            {
                return false;
            }

            // else it will return the result of if the IDs are equal or not.
            return EntityId.Equals(other.EntityId);
        }

        /// <summary>
        /// this method will return the string of the name of the ReviewRating along with the ID.
        /// </summary>
        /// <returns>string of the nmae + ID of the object</returns>
        public override string ToString()
        {
            return $"{nameof(ReviewRating)} {EntityId}";
        }
    }
}
