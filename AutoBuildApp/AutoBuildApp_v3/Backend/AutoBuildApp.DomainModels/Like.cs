using System;
using System.Collections.Generic;
using System.Text;


namespace AutoBuildApp.DomainModels
{
    /// <summary>
    /// This class represents a like object for when a user likes a post.
    /// </summary>
    public class Like
    {
        // Represents the unique identifier of a post.
        public string PostId { get; set; }

        // Represents the unique identifier of a user.
        public string UserId { get; set; }
    }
}
